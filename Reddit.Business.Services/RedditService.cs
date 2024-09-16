using LanguageExt;
using LanguageExt.Common;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RedditReader.Shared.Models;
using RedditReader.Shared.Extensions;
using System.Text.Json;

namespace Reddit.Business.Services;
public sealed class RedditService : IRedditService
{
    private readonly ILogger<RedditService> _logger;
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;
    public RedditService(ILogger<RedditService> logger,IConfiguration configuration, IHttpClientFactory httpClientFactory) 
    {
        _logger = logger;
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }


    public async Task<Option<RedditListingResponse>> GetRedditListing(string subreddit, string sort, string time, string limit)
    {
        using var httpClient = _httpClientFactory.CreateClient("reddit");
        var r = await httpClient.GetAsync($"r/{subreddit}/{sort}?t={time}&limit={limit}");

        if (r.IsSuccessStatusCode)
        {
            var options = new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            };

          
            var data = await JsonSerializer.DeserializeAsync<PostRoot>(await r.Content.ReadAsStreamAsync(), options);

            if (data is null)
            {
                return Option<RedditListingResponse>.None;
            }


            return Option<RedditListingResponse>.Some(new RedditListingResponse(
                r.Headers.GetValues("X-Ratelimit-Used").First().Parse<int>(),
                r.Headers.GetValues("X-Ratelimit-Remaining").First().Parse<double>(),
                r.Headers.GetValues("X-Ratelimit-Reset").First().Parse<int>(),
                postData: data.Data));

        }
        
        
        return Option<RedditListingResponse>.None;
    }
}
