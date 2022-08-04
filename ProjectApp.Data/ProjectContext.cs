using Microsoft.EntityFrameworkCore;
using ProjectApp.Domain;

namespace ProjectApp.Data
{
    public partial class ProjectContext : DbContext
    {
        public ProjectContext()
        {
        }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
        }

        public virtual DbSet<Project>? Projects { get; set; }
        public virtual DbSet<User>? Users { get; set; }
        public virtual DbSet<Tag>? Tags { get; set; }
        public virtual DbSet<Comment>? Comments { get; set; }
        public virtual DbSet<Project_Assignee>? Assignees { get; set; }
        public virtual DbSet<Project_Tag>? ProjectTags { get; set; }
        public virtual DbSet<UserRefreshToken>? UserRefreshTokens { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(
                "Server = (localdb)\\mssqllocaldb; Database = ProjectAppData; Trusted_Connection = True; "
            );
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Project_Assignee>()
                    .HasOne(m => m.Assignee)
                    .WithMany(t => t.Assignees)
                    .HasForeignKey(m => m.AssigneeId)
                    .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Project>()
                    .HasOne(m => m.Creator)
                    .WithMany(t => t.Creators)
                    .HasForeignKey(m => m.CreatorId)
                    .OnDelete(DeleteBehavior.Restrict);

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
