using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace AfterWorkMVCProject.Models.Entities
{
    public partial class MyContext : DbContext
    {
        public MyContext()
        {
        }

        public MyContext(DbContextOptions<MyContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetRoleClaim> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual DbSet<Awsession> Awsessions { get; set; }
        public virtual DbSet<Digikalja> Digikaljas { get; set; }
        public virtual DbSet<DigikaljaPlayer> DigikaljaPlayers { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<JoinedUser> JoinedUsers { get; set; }
        public virtual DbSet<UserInfo> UserInfos { get; set; }
        public virtual DbSet<Word> Words { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<AspNetRole>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName, "RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetRoleClaim>(entity =>
            {
                entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetUser>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaim>(entity =>
            {
                entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogin>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserToken>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<Awsession>(entity =>
            {
                entity.ToTable("AWSessions");

                entity.Property(e => e.Awname)
                    .IsRequired()
                    .HasColumnName("AWName");

                entity.Property(e => e.Code).IsRequired();

                entity.Property(e => e.CreatorId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Awsessions)
                    .HasForeignKey(d => d.CreatorId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__AWSession__Creat__52593CB8");
            });

            modelBuilder.Entity<Digikalja>(entity =>
            {
                entity.ToTable("Digikalja");

                entity.Property(e => e.AwsessionId).HasColumnName("AWSessionId");

                entity.Property(e => e.Code).IsRequired();

                entity.HasOne(d => d.Awsession)
                    .WithMany(p => p.Digikaljas)
                    .HasForeignKey(d => d.AwsessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Digikalja__AWSes__07C12930");

                entity.HasOne(d => d.Word)
                    .WithMany(p => p.Digikaljas)
                    .HasForeignKey(d => d.WordId)
                    .HasConstraintName("FK__Digikalja__WordI__06CD04F7");
            });

            modelBuilder.Entity<DigikaljaPlayer>(entity =>
            {
                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Digikalja)
                    .WithMany(p => p.DigikaljaPlayers)
                    .HasForeignKey(d => d.DigikaljaId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Digikalja__Digik__09A971A2");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.DigikaljaPlayers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Digikalja__UserI__08B54D69");
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.AltImgText).IsRequired();

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ImgUrl).HasColumnName("ImgURL");

                entity.Property(e => e.Name).IsRequired();
            });

            modelBuilder.Entity<JoinedUser>(entity =>
            {
                entity.Property(e => e.AwsessionId).HasColumnName("AWSessionId");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.Awsession)
                    .WithMany(p => p.JoinedUsers)
                    .HasForeignKey(d => d.AwsessionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__JoinedUse__AWSes__70DDC3D8");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.JoinedUsers)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__JoinedUse__UserI__6FE99F9F");
            });

            modelBuilder.Entity<UserInfo>(entity =>
            {
                entity.ToTable("UserInfo");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.UserId)
                    .IsRequired()
                    .HasMaxLength(450)
                    .HasColumnName("UserID");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserInfos)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserInfo__UserID__4CA06362");
            });

            modelBuilder.Entity<Word>(entity =>
            {
                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Word1)
                    .IsRequired()
                    .HasColumnName("Word");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
