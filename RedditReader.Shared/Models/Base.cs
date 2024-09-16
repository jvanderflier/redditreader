using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RedditReader.Shared.Models;
public abstract class Base
{
    [JsonPropertyName("kind")]
    public string Kind { get; set; } = string.Empty;
}
