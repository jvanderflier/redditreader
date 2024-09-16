using RedditReader.Business.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.Repository;
public interface IRedditPostRepository : IRepositoryBase<RedditPost>
{
}

public class RedditPostRepository(RedditDbContext redditDbContext) : RepositoryBase<RedditPost>(redditDbContext), IRedditPostRepository
{

}