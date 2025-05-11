using Microsoft.EntityFrameworkCore;
using NguyenVinhSon_2122110315.Model;

namespace NguyenVinhSon_2122110315.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
           : base(options)
        {
        }

        // DbSet các bảng
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ======================= PRODUCT ========================

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Brand)
                .WithMany(b => b.Products)
                .HasForeignKey(p => p.BrandId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.CreatedByUser)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .HasConstraintName("FK_Products_Users_CreatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.UpdatedByUser)
                .WithMany()
                .HasForeignKey(p => p.UpdatedBy)
                .HasConstraintName("FK_Products_Users_UpdatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.DeletedByUser)
                .WithMany()
                .HasForeignKey(p => p.DeletedBy)
                .HasConstraintName("FK_Products_Users_DeletedBy")
                .OnDelete(DeleteBehavior.Restrict);

            // ======================= BRAND ========================

            modelBuilder.Entity<Brand>()
                .HasOne(b => b.CreatedByUser)
                .WithMany()
                .HasForeignKey(b => b.CreatedBy)
                .HasConstraintName("FK_Brands_Users_CreatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Brand>()
                .HasOne(b => b.UpdatedByUser)
                .WithMany()
                .HasForeignKey(b => b.UpdatedBy)
                .HasConstraintName("FK_Brands_Users_UpdatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Brand>()
                .HasOne(b => b.DeletedByUser)
                .WithMany()
                .HasForeignKey(b => b.DeletedBy)
                .HasConstraintName("FK_Brands_Users_DeletedBy")
                .OnDelete(DeleteBehavior.Restrict);

            // ======================= CATEGORY ========================

            modelBuilder.Entity<Category>()
                .HasOne(c => c.CreatedByUser)
                .WithMany()
                .HasForeignKey(c => c.CreatedBy)
                .HasConstraintName("FK_Categories_Users_CreatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.UpdatedByUser)
                .WithMany()
                .HasForeignKey(c => c.UpdatedBy)
                .HasConstraintName("FK_Categories_Users_UpdatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Category>()
                .HasOne(c => c.DeletedByUser)
                .WithMany()
                .HasForeignKey(c => c.DeletedBy)
                .HasConstraintName("FK_Categories_Users_DeletedBy")
                .OnDelete(DeleteBehavior.Restrict);

            // ======================= ORDER ========================

            modelBuilder.Entity<Order>()
                .HasOne(o => o.User)
                .WithMany()
                .HasForeignKey(o => o.UserId)
                .HasConstraintName("FK_Orders_Users_UserId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.CreatedByUser)
                .WithMany()
                .HasForeignKey(o => o.CreatedBy)
                .HasConstraintName("FK_Orders_Users_CreatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.UpdatedByUser)
                .WithMany()
                .HasForeignKey(o => o.UpdatedBy)
                .HasConstraintName("FK_Orders_Users_UpdatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.DeletedByUser)
                .WithMany()
                .HasForeignKey(o => o.DeletedBy)
                .HasConstraintName("FK_Orders_Users_DeletedBy")
                .OnDelete(DeleteBehavior.Restrict);

            // ======================= ORDER DETAIL ========================

            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.Order)
                .WithMany(o => o.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.Product)
                .WithMany()
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.CreatedByUser)
                .WithMany()
                .HasForeignKey(d => d.CreatedBy)
                .HasConstraintName("FK_OrderDetails_Users_CreatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.UpdatedByUser)
                .WithMany()
                .HasForeignKey(d => d.UpdatedBy)
                .HasConstraintName("FK_OrderDetails_Users_UpdatedBy")
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderDetail>()
                .HasOne(d => d.DeletedByUser)
                .WithMany()
                .HasForeignKey(d => d.DeletedBy)
                .HasConstraintName("FK_OrderDetails_Users_DeletedBy")
                .OnDelete(DeleteBehavior.Restrict);

            // ======================= DECIMAL CONFIG ========================

            modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(18, 2);
            modelBuilder.Entity<Product>().Property(p => p.PriceSale).HasPrecision(18, 2);
            modelBuilder.Entity<Order>().Property(o => o.TotalAmount).HasPrecision(18, 2);
            modelBuilder.Entity<OrderDetail>().Property(d => d.Price).HasPrecision(18, 2);
        }
    }
}
