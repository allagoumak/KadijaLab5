using Lab5.Model;
using Microsoft.EntityFrameworkCore;

namespace Lab5.Data
{
    public class PredictionDataContext : DbContext
    {
        public PredictionDataContext(DbContextOptions<PredictionDataContext> options) : base(options)
        {
        }
        public DbSet<Prediction> Predictions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Prediction>().ToTable("Prediction");
          
        }
    }
}
