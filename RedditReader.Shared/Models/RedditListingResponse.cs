namespace RedditReader.Shared.Models;
public record RedditListingResponse(int rateLimitUsed, double rateLimitRemaining, int rateLimitReset, PostData? postData);
