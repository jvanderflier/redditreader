using Microsoft.EntityFrameworkCore;
using RedditReader.Business.Entities;

namespace RedditReader.Data;

public class RedditDbContext(DbContextOptions<RedditDbContext> options) : DbContext(options)
{
    public DbSet<RedditPost> RedditPosts { get; set; }
    public DbSet<RedditPostAuthor> RedditPostUsers { get; set; }

}
