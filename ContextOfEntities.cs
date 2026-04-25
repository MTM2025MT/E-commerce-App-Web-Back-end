using E_commerce_App_Web.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_App_Web
{
    public class ContextOfEntities : DbContext
    {
        public ContextOfEntities(DbContextOptions<ContextOfEntities> options)
            : base(options)
        {
        }

        public DbSet<ApplicationUser> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<favoriteProductForCustomer> FavoriteProducts { get; set; }
        public DbSet<ProductOfOrder> ProductOfOrders { get; set; }
        public DbSet<SubOrder> SubOrders { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<Vendor> Vendors { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<CartItem> shoping_card { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<favoriteProductForCustomer>()
               .HasKey(fp => new { fp.CustomerId, fp.ProductId });
            modelBuilder.Entity<favoriteProductForCustomer>()
                .HasOne(fp => fp.Customer)
                .WithMany(c => c.Favorites)
                .HasForeignKey(fp => fp.CustomerId);
            modelBuilder.Entity<favoriteProductForCustomer>()
                .HasOne(fp => fp.Product)
                .WithMany(p => p.FavoritedBy)
                .HasForeignKey(fp => fp.ProductId);

            modelBuilder.Entity<CartItem>().HasKey(ci => new { ci.CustomerId, ci.ProductId });
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Customer)
                .WithMany(c => c.Shoping_Card)
                .HasForeignKey(ci => ci.CustomerId);
            modelBuilder.Entity<CartItem>()
                .HasOne(ci => ci.Product)
                .WithMany(p => p.product_cartItem)
                .HasForeignKey(ci => ci.ProductId);

            modelBuilder.Entity<ApplicationUser>().UseTpcMappingStrategy();
            modelBuilder.Entity<ApplicationUser>()
                .Property(u => u.Id)
                .UseHiLo("UserSequence");

            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Vendor>().ToTable("Vendors");

            modelBuilder.Entity<ProductOfOrder>()
                .HasOne(po => po.Product)
                .WithMany()
                .HasForeignKey(po => po.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ProductOfOrder>()
                .HasOne(po => po.SubOrder)
                .WithMany(so => so.Products)
                .HasForeignKey(po => po.SubOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SubOrder>()
                .HasOne(so => so.Shipment)
                .WithOne(s => s.SubOrder)
                .HasForeignKey<Shipment>(s => s.SubOrderId);

            base.OnModelCreating(modelBuilder);
        }
    }
}