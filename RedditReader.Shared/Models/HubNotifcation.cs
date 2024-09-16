using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Shared.Models;
public record HubNotifcation(TopPostDetails TopPost, TopAuthor TopAuthor);

public record TopPostDetails(string PostId, string PostTitle, int Ups, string Url);
public record TopAuthor(string AuthorName, int NumberOfPosts);
