using Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models.Identity
{
    public class BlazorAdventuresRole : IdentityRole, IAuditableEntity<string>
    {
        public string Description { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual ICollection<BlazorAdventuresRoleClaim> RoleClaims { get; set; }

        public BlazorAdventuresRole() : base()
        {
            RoleClaims = new HashSet<BlazorAdventuresRoleClaim>();
        }

        public BlazorAdventuresRole(string roleName, string roleDescription = null) : base(roleName)
        {
            RoleClaims = new HashSet<BlazorAdventuresRoleClaim>();
            Description = roleDescription;
        }
    }
}
