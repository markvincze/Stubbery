using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Stubbery.RequestMatching.Preconditions
{
    internal interface IPrecondition
    {
        Task<bool> Match(HttpContext context);
    }
}