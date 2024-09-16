
using LanguageExt;
using Microsoft.AspNetCore.SignalR;
using Reddit.Business.Services;
using RedditReader.Business.Entities;
using RedditReader.Data;
using RedditReader.Data.Repository;
using RedditReader.Shared.Models;
using RedditReader.Web.Hubs;
namespace RedditReader.Web;

public class RedditFeedReader : BackgroundService
{
    private readonly ILogger<RedditFeedReader> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IConfiguration _configuration;
    private readonly IHubContext<RedditFeedHub, IRedditFeedClient> _hubContext;

    private readonly TimeSpan Period = TimeSpan.FromSeconds(2);

    public RedditFeedReader(ILogger<RedditFeedReader> logger, IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, IHubContext<RedditFeedHub, IRedditFeedClient> hubContext)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
        _configuration = configuration;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {

        try
        {
            using var timer = new PeriodicTimer(Period);

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                var redditService = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IRedditService>();
                var redditPostRepository = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IRedditPostRepository>();
                var redditPostAuthorRepository = _serviceScopeFactory.CreateScope().ServiceProvider.GetRequiredService<IRedditPostAuthorRepository>();

                var postListings = await redditService.GetRedditListing(_configuration["Reddit:SubReddit"]!, 
                    sort: _configuration["Reddit:Sort"]!, 
                    time: _configuration["Reddit:Time"]!, 
                    limit: _configuration["Reddit:Limit"]!);

                postListings.Match(
                    Some: async data =>
                    {

                        if (data.rateLimitRemaining == 0) //fail safe
                            Thread.Sleep(TimeSpan.FromSeconds(data.rateLimitReset));

                        var topPost = data.postData!.Children
                                            .OrderByDescending(x => x.Data.Ups)
                                            .Select(x => new TopPostDetails(x.Data.Id, x.Data.Title, x.Data.Ups, x.Data.URL))
                                            .FirstOrDefault();
                        var topAuthor = data.postData!.Children
                                                .GroupBy(x => x.Data.Author)
                                                .Select(grp => new { Author = grp.Key, Count = grp.Count() })
                                                .OrderByDescending(x => x.Count)
                                                .Select(x => new TopAuthor(x.Author, x.Count))
                                                .FirstOrDefault();


                        var post = redditPostRepository.Find(x => x.PostId == topPost.PostId).FirstOrDefault();
                        if(post is null)
                        {
                            redditPostRepository.Add(new RedditPost()
                            {
                                PostId = topPost.PostId,
                                PostTitle = topPost.PostTitle,
                                TotalVotes = topPost.Ups
                            });

                            await redditPostRepository.SaveAsync();
                        }
                        else
                        {
                            post.TotalVotes = topPost.Ups;
                            redditPostRepository.Update(post);

                            await redditPostRepository.SaveAsync();
                        }

                        var author = redditPostAuthorRepository.Find(x => x.Name == topAuthor.AuthorName).FirstOrDefault();
                        if (author is null)
                        {
                            redditPostAuthorRepository.Add(new RedditPostAuthor()
                            {
                                Name = topAuthor.AuthorName,
                                NumberOfPost = topAuthor.NumberOfPosts
                            });

                            await redditPostAuthorRepository.SaveAsync();
                        }
                        else
                        {
                            if(author.NumberOfPost != topAuthor.NumberOfPosts)
                            {
                                author.NumberOfPost = topAuthor.NumberOfPosts;
                                redditPostAuthorRepository.Update(author);

                                await redditPostRepository.SaveAsync();
                            }                          
                        }

                        await _hubContext.Clients.All.ReceivePost(new HubNotifcation(topPost!, topAuthor!));
                    },
                    None: () =>
                    {
                        _logger.LogWarning("No subbit data returned");
                    });
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing reddit post service");
        }
    }

}
