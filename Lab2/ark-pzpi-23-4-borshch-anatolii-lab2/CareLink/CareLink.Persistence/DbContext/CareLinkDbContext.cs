using CareLink.Domain.Entities;
using CareLink.Domain.Entities.SubEntities;
using Microsoft.EntityFrameworkCore;

namespace CareLink.Persistence.DbContext
{
    public class CareLinkDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public CareLinkDbContext(DbContextOptions<CareLinkDbContext> options) : base(options){}
        
        public DbSet<CognitiveExercise> CognitiveExercises { get; set; }
        public DbSet<CognitiveResult> CognitiveResults { get; set; }
        public DbSet<IoTReading> IotReadings { get; set; }
        public DbSet<IoTDevice> IotDevices { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Relatives> Relatives { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<User> Users { get; set; }
        
        public DbSet<CognitiveExerciseType> CognitiveExerciseTypes { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<NotificationType> NotificationTypes { get; set; }
        public DbSet<RelationType> RelationTypes { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SubscriptionPlanType> SubscriptionPlanTypes { get; set; }
        public DbSet<SubscriptionStatus> SubscriptionStatuses { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(CareLinkDbContext).Assembly);
            
            AddStartUpEntities(modelBuilder);
            
            base.OnModelCreating(modelBuilder);
        }

        private void AddStartUpEntities(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Name = "Administrator" },
                new Role { Id = 2, Name = "Moderator" },
                new Role { Id = 3, Name = "Doctor" },
                new Role { Id = 4, Name = "Guardian" },
                new Role { Id = 5, Name = "Relative" }
            );
        }
    }
}