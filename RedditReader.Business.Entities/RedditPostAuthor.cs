using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Business.Entities;
public class RedditPostAuthor
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int NumberOfPost { get; set; }
}
