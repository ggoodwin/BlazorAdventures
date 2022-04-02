using Application.Interfaces.Services;
using Domain.Contracts;
using Domain.Entities.ExtendedAttributes;
using Domain.Entities.Misc;
using Domain.Entities.Sms;
using Domain.Entities.TimeStamp;
using Infrastructure.Models.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Infrastructure.Contexts
{
    public class DataContext : AuditableContext
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IDateTimeService _dateTimeService;
        //private const string UserGuid = "48541c9d-0c17-4e32-8cc1-dd12de643003";

        public DataContext(DbContextOptions<DataContext> options, ICurrentUserService currentUserService, IDateTimeService dateTimeService) : base(options)
        {
            _currentUserService = currentUserService;
            _dateTimeService = dateTimeService;
        }

        public DbSet<Stamp> Stamps { get; set; }
        public DbSet<Sms> Smses { get; set; }
        public DbSet<Document> Documents { get; set; }
        public DbSet<DocumentType> DocumentTypes { get; set; }
        public DbSet<DocumentExtendedAttribute> DocumentExtendedAttributes { get; set; }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedOn = _dateTimeService.NowUtc;
                        entry.Entity.CreatedBy = _currentUserService.UserId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = _dateTimeService.NowUtc;
                        entry.Entity.LastModifiedBy = _currentUserService.UserId;
                        break;
                }
            }
            if (_currentUserService.UserId == null)
            {
                return await base.SaveChangesAsync(cancellationToken);
            }
            else
            {
                return await base.SaveChangesAsync(_currentUserService.UserId, cancellationToken);
            }
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            var keysProperties = builder.Model.GetEntityTypes().Select(x => x.FindPrimaryKey()).SelectMany(x => x.Properties);
            foreach (var property in keysProperties)
            {
                property.ValueGenerated = ValueGenerated.OnAdd;
            }

            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,2)");
            }

            foreach (var property in builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetProperties())
                .Where(p => p.Name is "LastModifiedBy" or "CreatedBy"))
            {
                property.SetColumnType("nvarchar(128)");
            }

            //base.OnModelCreating(builder);
            //builder.Entity<ChatHistory<BlazorAdventuresUser>>(entity =>
            //{
            //    entity.ToTable("ChatHistory");

            //    entity.HasOne(d => d.FromUser)
            //        .WithMany(p => p.ChatHistoryFromUsers)
            //        .HasForeignKey(d => d.FromUserId)
            //        .OnDelete(DeleteBehavior.ClientSetNull);

            //    entity.HasOne(d => d.ToUser)
            //        .WithMany(p => p.ChatHistoryToUsers)
            //        .HasForeignKey(d => d.ToUserId)
            //        .OnDelete(DeleteBehavior.ClientSetNull);
            //});
            //builder.Entity<BlazorAdventuresUser>(entity =>
            //{
            //    entity.ToTable(name: "Users", "Identity");
            //    entity.Property(e => e.Id).ValueGeneratedOnAdd();
            //});

            //builder.Entity<BlazorAdventuresRole>(entity =>
            //{
            //    entity.ToTable(name: "Roles", "Identity");
            //});
            //builder.Entity<IdentityUserRole<string>>(entity =>
            //{
            //    entity.ToTable("UserRoles", "Identity");
            //});

            //builder.Entity<IdentityUserClaim<string>>(entity =>
            //{
            //    entity.ToTable("UserClaims", "Identity");
            //});

            //builder.Entity<IdentityUserLogin<string>>(entity =>
            //{
            //    entity.ToTable("UserLogins", "Identity");
            //});

            //builder.Entity<BlazorAdventuresRoleClaim>(entity =>
            //{
            //    entity.ToTable(name: "RoleClaims", "Identity");

            //    entity.HasOne(d => d.Role)
            //        .WithMany(p => p.RoleClaims)
            //        .HasForeignKey(d => d.RoleId)
            //        .OnDelete(DeleteBehavior.Cascade);
            //});

            //builder.Entity<IdentityUserToken<string>>(entity =>
            //{
            //    entity.ToTable("UserTokens", "Identity");
            //});
        }
    }
}
