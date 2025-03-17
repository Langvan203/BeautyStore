using Microsoft.EntityFrameworkCore;

namespace my_cosmetic_store.Models
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }
        public DbSet<ChildrenCategory> ChildrenCategories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cart_Item> cart_Items { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Shipping> shippings { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Product_Images> Product_Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasMany(p => p.ProductImages).WithOne().HasForeignKey(pi => pi.ProductID);
            modelBuilder.Entity<Product>()
                        .HasOne(p => p.Brand)
                        .WithMany(b => b.Products)
                        .HasForeignKey(p => p.BrandID)
                        .OnDelete(DeleteBehavior.NoAction); // hoặc NoAction

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.NoAction); // hoặc NoAction
            modelBuilder.Entity<Cart>().HasMany(p => p.Cart_Items).WithOne().HasForeignKey(p => p.CartID);
            //modelBuilder.Entity<Brand>().HasMany(p => p.Products).WithOne().HasForeignKey(pi => pi.BrandID).OnDelete(DeleteBehavior.NoAction);
            //modelBuilder.Entity<Category>().HasMany(p => p.Products).WithOne().HasForeignKey(pi => pi.CategoryID).OnDelete(DeleteBehavior.NoAction);
        }

    }
}
