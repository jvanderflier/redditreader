using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Shared.HttpClients;
public sealed class RedditClient
{
    private readonly HttpClient _httpClient;

    public RedditClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}
