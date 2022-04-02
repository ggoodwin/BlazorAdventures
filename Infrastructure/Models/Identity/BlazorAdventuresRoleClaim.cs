using Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models.Identity
{
    public class BlazorAdventuresRoleClaim : IdentityRoleClaim<string>, IAuditableEntity<int>
    {
        public string? Description { get; set; }
        public string? Group { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
        public virtual BlazorAdventuresRole Role { get; set; }

        public BlazorAdventuresRoleClaim() : base()
        {
        }

        public BlazorAdventuresRoleClaim(string roleClaimDescription = null, string roleClaimGroup = null) : base()
        {
            Description = roleClaimDescription;
            Group = roleClaimGroup;
        }
    }
}
