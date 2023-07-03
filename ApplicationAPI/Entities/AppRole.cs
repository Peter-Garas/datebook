

using Microsoft.AspNetCore.Identity;

namespace ApplicationAPI.Entities
{
    public class AppRole : IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; }
    }
}