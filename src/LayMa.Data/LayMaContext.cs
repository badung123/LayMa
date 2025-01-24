using Azure;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LayMa.Core.Domain.Identity;
using LayMa.Core.Domain.Link;
using LayMa.Core.Domain.Bank;
using LayMa.Core.Domain.Campain;
using LayMa.Core.Domain.Mission;
using LayMa.Core.Domain.Visitor;
using LayMa.Core.Domain.Transaction;
using LayMa.Core.Domain.Commment;

namespace LayMa.Data
{
    public class LayMaContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        public LayMaContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<ShortLink> ShortLinks { get; set; }
        public DbSet<KeySearch> KeySearches { get; set; }
        public DbSet<Code> Codes { get; set; }
        public DbSet<ViewDetail> ViewDetails { get; set; }

        public DbSet<TransactionBank> TransactionBanks { get; set; }
		public DbSet<Campain> Campains { get; set; }
		public DbSet<Mission> Missions { get; set; }
		public DbSet<Visitor> Visitors { get; set; }
		public DbSet<TransactionLog> TransactionLogs { get; set; }
		public DbSet<Commments> Comments { get; set; }
		public DbSet<Messages> Messages { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);

            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims")
            .HasKey(x => x.Id);

            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);

            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles")
            .HasKey(x => new { x.RoleId, x.UserId });

            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens")
               .HasKey(x => new { x.UserId });
        }

        //public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        //{
        //    var entries = ChangeTracker
        //        .Entries()
        //        .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        //    foreach (var entityEntry in entries)
        //    {
        //        var dateCreatedProp = entityEntry.Entity.GetType().GetProperty("DateCreated");
        //        if (entityEntry.State == EntityState.Added
        //            && dateCreatedProp != null)
        //        {
        //            dateCreatedProp.SetValue(entityEntry.Entity, DateTime.Now);
        //        }
        //        var modifiedDateProp = entityEntry.Entity.GetType().GetProperty("ModifiedDate");
        //        if (entityEntry.State == EntityState.Modified
        //            && modifiedDateProp != null)
        //        {
        //            modifiedDateProp.SetValue(entityEntry.Entity, DateTime.Now);
        //        }
        //    }
        //    return base.SaveChangesAsync(cancellationToken);
        //}
    }
}
