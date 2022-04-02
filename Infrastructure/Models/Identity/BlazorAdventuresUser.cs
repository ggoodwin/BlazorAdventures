using System.ComponentModel.DataAnnotations.Schema;
using Application.Interfaces.Chat;
using Domain.Contracts;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Models.Identity
{
    public class BlazorAdventuresUser : IdentityUser<string>, IChatUser, IAuditableEntity<string>
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
        public string? CreatedBy { get; set; }

        [Column(TypeName = "text")]
        public string? ProfilePictureDataUrl { get; set; }

        public DateTime CreatedOn { get; set; }

        public string? LastModifiedBy { get; set; }

        public DateTime? LastModifiedOn { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }
        public bool IsActive { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        //public virtual ICollection<ChatHistory<BlazorAdventuresUser>> ChatHistoryFromUsers { get; set; }
        //public virtual ICollection<ChatHistory<BlazorAdventuresUser>> ChatHistoryToUsers { get; set; }

        //public BlazorAdventuresUser()
        //{
        //    ChatHistoryFromUsers = new HashSet<ChatHistory<BlazorAdventuresUser>>();
        //    ChatHistoryToUsers = new HashSet<ChatHistory<BlazorAdventuresUser>>();
        //}
    }
}
