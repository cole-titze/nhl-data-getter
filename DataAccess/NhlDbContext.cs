using System.Collections.Generic;
using Entities.DbModels;
using Microsoft.EntityFrameworkCore;

namespace DataAccess
{
    public partial class NhlDbContext : DbContext
    {
        private readonly string _connectionString;
        public NhlDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        public virtual DbSet<DbGame> Game { get; set; } = null!;
        public virtual DbSet<DbPlayer> PlayerValue { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbPlayer>()
                .HasKey(c => new { c.id, c.seasonStartYear });
        }
    }
}