using RedditReader.Shared.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reddit.Business.Services;
public interface IAuthService
{
    Task<Token?> GetAccessToken();
}
