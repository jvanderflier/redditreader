namespace RedditReader.Business.Entities;

public class RedditPost
{
    public int Id { get; set; }
    public string PostId { get; set; } = string.Empty;
    public string PostTitle { get; set; } = string.Empty;
    public int TotalVotes { get; set; }
}
