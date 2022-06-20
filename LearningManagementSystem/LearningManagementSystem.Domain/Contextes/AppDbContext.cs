using LearningManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LearningManagementSystem.Domain.Contextes
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<Course> Courses { get; set; } = null!;
        public DbSet<Subject> Subjects { get; set; } = null!;
        public DbSet<Teacher> Teachers { get; set; } = null!;
        public DbSet<Document> Documents { get; set; } = null!;
        public DbSet<HomeTask> HomeTasks { get; set; } = null!;
        public DbSet<TaskAnswer> TaskAnswers { get; set; } = null!;
        public DbSet<Topic> Topics { get; set; } = null!;
        public DbSet<Grade> Grades { get; set; } = null!;
        public DbSet<Test> Tests { get; set; } = null!;
        public DbSet<Question> Questions { get; set; } = null!;
        public DbSet<Answer> Answers { get; set; } = null!;
        public DbSet<StudentAnswer> StudentAnswers { get; set; } = null!;
        public DbSet<GroupChatMessage> GroupChatMessages { get; set; } = null!;

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(p => p.UserName)
                .IsRequired()
                .HasMaxLength(60);

            modelBuilder.Entity<User>()
                .Property(p => p.Email)
                .IsRequired();

            modelBuilder.Entity<Student>()
                .HasOne(o => o.Group)
                .WithMany(m => m.Students)
                .HasForeignKey(f => f.GroupId);

            modelBuilder.Entity<HomeTask>()
                .HasKey(k => k.TopicId);

            modelBuilder.Entity<Topic>()
                .HasOne(o => o.HomeTask)
                .WithOne(o => o.Topic)
                .HasForeignKey<HomeTask>(fk => fk.TopicId);

            modelBuilder.Entity<Grade>().HasKey(k => k.Id);

            modelBuilder.Entity<Student>()
                .HasOne(o => o.User)
                .WithOne()
                .HasForeignKey<Student>(fk => fk.Id);

            modelBuilder.Entity<Teacher>()
                .HasOne(o => o.User)
                .WithOne()
                .HasForeignKey<Teacher>(fk => fk.Id);

            modelBuilder.Entity<TaskAnswer>()
                .HasOne(o => o.Grade)
                .WithOne(o => o.TaskAnswer)
                .HasForeignKey<Grade>(fk => fk.Id);

            modelBuilder.Entity<StudentAnswer>()
                .HasOne(o => o.Answer)
                .WithOne()
                .HasForeignKey<StudentAnswer>(fk => fk.AnswerId)
                .OnDelete(DeleteBehavior.ClientNoAction);
        }
    }
}
