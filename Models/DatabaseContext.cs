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
        public DbSet<ProductVariant> Product_Variants { get; set; }
        public DbSet<HistoryOder> HistoryOder { get; set; }
        public DbSet<PaymentMethod> PaymentMethods { get; set; }
        public DbSet<Variant> Variants { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Color> Colors { get; set; }
        public DbSet<ProductColor> Product_Colors { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasMany(p => p.ProductImages).WithOne(pi => pi.Product).HasForeignKey(pi => pi.ProductID);
            modelBuilder.Entity<Product>().HasMany(p => p.ProductVariants).WithOne(pi => pi.Product).HasForeignKey(pi => pi.ProductID);
            modelBuilder.Entity<Product>().HasMany(p => p.ProductColors).WithOne(pi => pi.Product).HasForeignKey(pi => pi.ProductID);

            modelBuilder.Entity<Product>()
                        .HasOne(p => p.Brand)
                        .WithMany(b => b.Products)
                        .HasForeignKey(p => p.BrandID)
                        .OnDelete(DeleteBehavior.NoAction); 

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .HasForeignKey(p => p.CategoryID)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<User>().HasMany(p => p.PaymentMethods).WithOne(p => p.User).HasForeignKey(p => p.UserID);

            modelBuilder.Entity<Cart>().HasMany(p => p.Cart_Items).WithOne(c => c.Cart).HasForeignKey(p => p.CartID);

            modelBuilder.Entity<Order>().HasMany(p => p.Order_Items).WithOne(c => c.Order).HasForeignKey(p => p.OrderID);
            modelBuilder.Entity<Order>().HasMany(p => p.HistoryOder).WithOne(c => c.Order).HasForeignKey(o => o.OrderID);

            modelBuilder.Entity<Cart_Item>()
                        .HasOne(p => p.ProductVariant)
                        .WithMany()
                        .HasForeignKey(p => new { p.ProductID, p.ProductVariantId })
                        .HasPrincipalKey(pv => new { pv.ProductID, pv.VariantId });

            modelBuilder.Entity<Cart_Item>()
                        .HasOne(p => p.ProductColor)
                        .WithMany()
                        .HasForeignKey(p => new { p.ProductID, p.ProductColorId })
                        .HasPrincipalKey(pv => new { pv.ProductID, pv.ColorID });

            modelBuilder.Entity<Order_Item>()
                        .HasOne(p => p.ProductVariant)
                        .WithMany().HasForeignKey(c => new {c.ProductID, c.ProductVariantId})
                        .HasPrincipalKey(pv => new {pv.ProductID, pv.VariantId});

            modelBuilder.Entity<Order_Item>()
            .HasOne(p => p.ProductColor)
            .WithMany().HasForeignKey(c => new { c.ProductID, c.ProductColorId })
            .HasPrincipalKey(pv => new { pv.ProductID, pv.ColorID });

            modelBuilder.Entity<ProductVariant>().HasOne(p => p.Variant).WithMany().HasForeignKey(p => p.VariantId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<ProductColor>().HasOne(p => p.Color).WithMany().HasForeignKey(p => p.ColorID).OnDelete(DeleteBehavior.NoAction);

        }

    }
}
