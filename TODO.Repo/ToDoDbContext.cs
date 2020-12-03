using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using TODO.Repo.Models;


namespace TODO.Repo
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<ToDo> Tasks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<User>()
                .Property(c => c.UserId)
                .ValueGeneratedOnAdd();

            builder.Entity<ToDo>()
                .Property(c => c.TaskId)
                .ValueGeneratedOnAdd();

            builder.Entity<User>()
                .HasData(
                    new User { UserName = "test", Password = "pwd123",UserId = 1},
                    new User { UserName = "test1", Password = "pwd345",UserId = 2}
                );
        }
    }
}
