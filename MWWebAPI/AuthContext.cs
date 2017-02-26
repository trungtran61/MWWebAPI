using Microsoft.AspNet.Identity.EntityFramework;

namespace MWWebAPI
{
    public class AuthContext : IdentityDbContext<IdentityUser>
    {
        public AuthContext()
            : base("AuthContext")
        {

        }
    }
}