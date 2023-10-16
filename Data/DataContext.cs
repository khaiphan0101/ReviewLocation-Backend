using Microsoft.EntityFrameworkCore;
using review.Common.Entities;
using System.Text.RegularExpressions;

namespace review.Data
{
    public class DataContext : DbContext
    {

        public DbSet<AccountEntity> AccountEntitys { get; set; }

        public DbSet<ProfileEntity> ProfileEntitys { get; set; }

        public DbSet<RefeshTokenEntity> RefeshTokenEntitys { get; set; }

        public DbSet<ProfileFollowEntity> ProfileFollowEntitys { get; set; }

        public DbSet<ProvinceEntity> ProvinceEntitys { get; set; }
        public DbSet<CategoryEntity> CategoryEntitys { get; set; }
        public DbSet<ProvinceCategoryEntity> ProvinceCategoryEntitys { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options){}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProfileFollowEntity>(entity =>
            {
                //entity.Property(e => e.ID).ValueGeneratedNever();
                entity.HasOne(f => f.Follower)
                .WithMany(f => f.Followers)
                .HasForeignKey(f => f.FollowerID)
                .HasConstraintName("FK_ProfileFollow_Profile_FollowerID")
                .OnDelete(DeleteBehavior.ClientSetNull);
            });

            modelBuilder.Entity<ProfileFollowEntity>(entity =>
            {
                //entity.Property(e => e.ID).ValueGeneratedNever();
                entity.HasOne(f => f.Following)
                .WithMany(f => f.Followings)
                .HasForeignKey(f => f.FollowingID)
                .HasConstraintName("FK_ProfileFollow_Profile_FollowingID")
                .OnDelete(DeleteBehavior.ClientSetNull); ;
            });
        }

    }
}
