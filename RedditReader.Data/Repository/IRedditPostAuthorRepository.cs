using RedditReader.Business.Entities;
using RedditReader.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Data.Repository;
public interface IRedditPostAuthorRepository : IRepositoryBase<RedditPostAuthor>
{
}

public class RedditPostAuthorRepository(RedditDbContext redditDbContext) : RepositoryBase<RedditPostAuthor>(redditDbContext), IRedditPostAuthorRepository
{

}