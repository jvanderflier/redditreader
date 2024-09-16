using LanguageExt;
using LanguageExt.Common;
using RedditReader.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reddit.Business.Services;
public interface IRedditService
{
    Task<Option<RedditListingResponse>> GetRedditListing(string subreddit, string sort = "top", string time = "day", string limit = "");
}
