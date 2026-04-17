using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace SharpBrowser.Controls.BrowserTabStrip.Data
{
    public class BrowserDbContext : DbContext
    {
        public DbSet<ProductLink> ProductLinks { get; set; }
        public DbSet<ProductInfo> Products { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                // DB file next to your EXE: WebView2WindowsFormsBrowser\bin\...\browserdata.db
                var dbPath = Path.Combine(AppContext.BaseDirectory, "browserdata.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // optional: quick indexes + uniqueness
            modelBuilder.Entity<ProductLink>()
                .HasIndex(p => p.Url)
                .IsUnique();

            modelBuilder.Entity<ProductInfo>()
                .HasIndex(p => p.Url);
        }
    }
}
