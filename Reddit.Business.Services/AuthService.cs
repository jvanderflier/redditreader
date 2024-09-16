using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RedditReader.Shared.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Reddit.Business.Services;
public sealed class AuthService : IAuthService
{
    private readonly ILogger<RedditService> _logger;
    private readonly IMemoryCache _memoryCache;
    private readonly IConfiguration _configuration;
    public AuthService(ILogger<RedditService> logger, IMemoryCache memoryCache, IConfiguration configuration)
    {
        _logger = logger;
        _memoryCache = memoryCache;
        _configuration = configuration;
    }

    public async Task<Token?> GetAccessToken()
    {
        try
        {
            var token = await _memoryCache.GetOrCreateAsync("token", async entry =>
            {
                var token = await RetrieveToken();
                if (token is not null)
                {
                    var expiration = DateTimeOffset.FromUnixTimeSeconds(token.ExpiresIn);
                    entry.SetAbsoluteExpiration(expiration);
                    return token;
                }
                else
                {
                    return null;
                }
            });

            return token;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error receiving access token");
            return null;
        }
    }

    async Task<Token?> RetrieveToken()
    {
        try
        {
            //TODO: real world store in key fault or environment variable

            var username = _configuration["Client:ClientId"];
            var password = _configuration["Client:ClientSecret"];
            var authenticationString = $"{username}:{password}";
            var base64EncodedAuthenticationString = Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(authenticationString));

            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("RedditAgent", "1.0.0"));

            var values = new List<KeyValuePair<string, string>>();
            values.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
            var content = new FormUrlEncodedContent(values);

            var response = await httpClient.PostAsync("https://www.reddit.com/api/v1/access_token", content);
            response.EnsureSuccessStatusCode();
            var reponseData = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<Token>(reponseData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error receiving access token");
            return null;
        }
    }
}
