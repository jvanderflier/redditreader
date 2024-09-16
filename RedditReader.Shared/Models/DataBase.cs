using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RedditReader.Shared.Models;
public abstract class DataBase
{
    public string Modhash { get; set; } = string.Empty;
    public int? Dist { get; set; }
    public string After { get; set; } = string.Empty;
    public string Before { get; set; } = string.Empty;
}
