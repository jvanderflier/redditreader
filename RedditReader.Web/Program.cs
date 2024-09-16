using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Polly;
using Reddit.Business.Services;
using RedditReader.Business.Entities;
using RedditReader.Data;
using RedditReader.Data.Repository;
using RedditReader.Shared;
using RedditReader.Web;
using RedditReader.Web.Components;
using RedditReader.Web.Handlers;
using RedditReader.Web.Hubs;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMemoryCache();
builder.Services.AddDbContext<RedditDbContext>(options =>
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("DataContext")!)
 );

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IRedditService, RedditService>();

builder.Services.AddScoped(typeof(IRepositoryBase<>), typeof(RepositoryBase<>));
builder.Services.AddScoped<IRedditPostRepository, RedditPostRepository>();
builder.Services.AddScoped<IRedditPostAuthorRepository, RedditPostAuthorRepository>();


builder.Services.AddTransient<AuthenticationDelegatingHandler>();

builder.Services.AddHostedService<RedditFeedReader>();

var ratePolicy = 

builder.Services.AddHttpClient("reddit", httpClient =>
{
    httpClient.BaseAddress = new Uri("https://oauth.reddit.com/");
    httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("RedditAgent", "1.0.0"));

})
.AddPolicyHandler(Policy.RateLimitAsync<HttpResponseMessage>(100, TimeSpan.FromMinutes(1))) //Reddit Rate Limit - 100 queries per minute (QPM) per OAuth client id
.AddHttpMessageHandler<AuthenticationDelegatingHandler>();


builder.Services.AddSignalR();

builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseCors(policy => policy.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

app.UseHttpsRedirection();

app.MapHub<RedditFeedHub>("redditfeed");

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


app.Map("/toppost", Results<Ok<RedditPost>, BadRequest<Exception>> (ILoggerFactory loggerFactory, IRedditPostRepository repository) =>
{
    try
    {
        var post = repository.GetAll(true).OrderByDescending(x => x.TotalVotes).FirstOrDefault();
        return TypedResults.Ok(post);
    }
    catch (Exception ex)
    {
        loggerFactory.CreateLogger("Program").LogApiError(ex);
        return TypedResults.BadRequest(ex);
    }
});

app.Map("/topauthor", Results<Ok<RedditPostAuthor>, BadRequest<Exception>> (ILoggerFactory loggerFactory, IRedditPostAuthorRepository repository) =>
{
    try
    {
        var author = repository.GetAll(true).OrderByDescending(x => x.NumberOfPost).FirstOrDefault();
        return TypedResults.Ok(author);
    }
    catch (Exception ex)
    {
        loggerFactory.CreateLogger("Program").LogApiError(ex);
        return TypedResults.BadRequest(ex);
    }
});

app.Run();
