using Microsoft.AspNetCore.Authentication.JwtBearer;
using Reddit.Business.Services;
using System.Net.Http.Headers;

namespace RedditReader.Web.Handlers;

public class AuthenticationDelegatingHandler(IAuthService authService, ILogger<AuthenticationDelegatingHandler> logger) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(nameof(authService));

        HttpResponseMessage? httpResponseMessage = null;
        try
        {
            var authToken = await authService.GetAccessToken();

            if (authToken is null)
            {
                return await base.SendAsync(request, cancellationToken);
            }

            request.Headers.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, authToken.AccessToken);

            httpResponseMessage = await base.SendAsync(request, cancellationToken);

            httpResponseMessage.EnsureSuccessStatusCode();

            return httpResponseMessage;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error Executing Http Request");
            return httpResponseMessage;
        }
       
    }
}
