using Microsoft.EntityFrameworkCore;
using Raceday.Api.Models;

namespace Raceday.Api.Data
{
    public class RaceDayDbContext : DbContext
    {
        public RaceDayDbContext(DbContextOptions<RaceDayDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Club> Clubs => Set<Club>();
        public DbSet<Event> Events => Set<Event>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Enrolment> Enrolments => Set<Enrolment>();
        public DbSet<Result> Results => Set<Result>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Explicitly map entities to match your exact SQL schema table names
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Club>().ToTable("Club");
            modelBuilder.Entity<Event>().ToTable("Event");
            modelBuilder.Entity<Category>().ToTable("Category");
            modelBuilder.Entity<Enrolment>().ToTable("Enrolment");
            modelBuilder.Entity<Result>().ToTable("Result");

            modelBuilder.Entity<User>(entity =>
            { 
                entity.HasIndex(u => u.email).IsUnique();
                entity.HasIndex(u => u.phone_number).IsUnique();
                entity.Property(u => u.userID).UseIdentityColumn(1401, 1);
                entity.HasOne(u => u.Club)
                      .WithMany(c => c.Users)
                      .HasForeignKey(u => u.clubID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Club>(entity =>
            {
                entity.HasIndex(c => c.club_name).IsUnique();
                entity.Property(c => c.clubID).UseIdentityColumn(15432, 1);
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.Property(e => e.eventID).UseIdentityColumn(1, 10);
                entity.HasOne(e => e.Organiser)
                      .WithMany(u => u.Events)
                      .HasForeignKey(e => e.organiserID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

             modelBuilder.Entity<Category>(entity =>
             {
                 entity.Property(c => c.categoryID).UseIdentityColumn(101, 100);
                 entity.HasOne(c => c.Event)
                       .WithMany(e => e.Categories)
                       .HasForeignKey(c => c.eventID)
                       .OnDelete(DeleteBehavior.Cascade);
             });

            modelBuilder.Entity<Enrolment>(entity =>
            {
              entity.Property(e => e.enrolmentID).UseIdentityColumn(20058, 20);
              entity.HasOne(e => e.Participant)
                    .WithMany(u => u.Enrolments)
                    .HasForeignKey(e => e.participantID)
                    .OnDelete(DeleteBehavior.NoAction);

              entity.HasOne(e => e.Category)
                    .WithMany(c => c.Enrolments)
                    .HasForeignKey(e => e.categoryID)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Result>(entity =>
            {
                entity.Property(r => r.resultID).UseIdentityColumn(1, 1
                    );
                entity.HasOne(r => r.Enrolment)
                      .WithMany(e => e.Results)
                      .HasForeignKey(r => r.enrolmentID)
                      .OnDelete(DeleteBehavior.Cascade);
            });

        } 

    }
}
