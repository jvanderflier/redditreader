using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace RedditReader.Shared.Models;
public class Post
{
    public string Subreddit { get; set; }
    public string Title { get; set; }

    [JsonPropertyName("subreddit_name_prefixed")]
    public string SubredditNamePrefixed { get; set; }
    public bool Hidden { get; set; }
    public int Downs { get; set; }

    [JsonPropertyName("hide_score")]
    public bool HideScore { get; set; }

    public string Name { get; set; }

    [JsonPropertyName("upvote_ratio")]
    public double UpvoteRatio { get; set; }

    [JsonPropertyName("subreddit_type")]
    public string SubredditType { get; set; }
    public int Ups { get; set; }
    public string Domain { get; set; }

    [JsonPropertyName("is_original_content")]
    public bool IsOriginalContent { get; set; }

    [JsonPropertyName("author_fullname")]
    public string AuthorFullname { get; set; }
    public string Category { get; set; }

    [JsonPropertyName("num_comments")]
    public int NumComments { get; set; }
    public int Score { get; set; }

    [JsonPropertyName("content_categories")]
    public object ContentCategories { get; set; }

    [JsonPropertyName("is_self")]
    public bool IsSelf { get; set; }
    public bool? Likes { get; set; }

    [JsonPropertyName("suggested_sort")]
    public string SuggestedSort { get; set; }

    [JsonPropertyName("view_count")]
    public int? ViewCount { get; set; }
    public bool Pinned { get; set; }
    public bool Over18 { get; set; }
    public string SubredditId { get; set; }
    public string Id { get; set; }
    public string Author { get; set; }
    public bool Approved { get; set; }
    public string URL { get; set; }

    //[JsonPropertyName("created_utc")]
    //[JsonConverter(typeof(UtcTimestampConverter))]
    //public DateTime CreatedUTC { get; set; }
}
