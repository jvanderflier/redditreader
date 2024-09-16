using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RedditReader.Shared.Models;
public class PostRoot : Base
{
    [JsonPropertyName("data")]
    public PostData Data { get; set; }
}
