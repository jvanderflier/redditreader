using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditReader.Shared;
public static partial class LoggerCommands
{
    [LoggerMessage(EventId = 0, Level = LogLevel.Error, Message = "An exception occured when retrieving api data")]
    public static partial void LogApiError(this ILogger logger, Exception? ex);
}
