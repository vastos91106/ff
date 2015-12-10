
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Web.Models
{
    public partial class ApplicationDbContext
    {

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Film>().HasKey(t => t.IdGuid);
            modelBuilder.Entity<ApplicationUser>().HasKey(t => t.Id);
            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<Film>().Property(t => t.AuthorId).IsRequired();
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
            modelBuilder.Entity<Film>().HasRequired(t => t.ApplicationUser).WithMany(t => t.UserFilms).HasForeignKey(t => t.AuthorId).WillCascadeOnDelete(false);
        }

        public virtual Film Film { get; set; }
    }
}
