using Microsoft.AspNetCore.Identity;

namespace Dotnet8Authentication.Database
{
    public class User: IdentityUser
    {
        public string ? Initials {  get; set; }
    }
}
