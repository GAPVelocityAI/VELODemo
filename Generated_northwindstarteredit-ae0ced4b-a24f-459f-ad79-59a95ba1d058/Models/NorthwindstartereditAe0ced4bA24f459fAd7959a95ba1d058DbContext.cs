using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

public class NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext : DbContext
{
    public NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext(DbContextOptions<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> options)
        : base(options)
    {
    }

    public DbSet<Customers> Customers { get; set; }

    public DbSet<Employees> Employees { get; set; }

    public DbSet<NorthwindFeatures> NorthwindFeatures { get; set; }

    public DbSet<OrderDetails> OrderDetails { get; set; }

    public DbSet<OrderStatus> OrderStatus { get; set; }

    public DbSet<Orders> Orders { get; set; }

    public DbSet<Products> Products { get; set; }

    public DbSet<SystemSettings> SystemSettings { get; set; }

    public DbSet<Welcome> Welcome { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employees>()
            .HasIndex(e => new { e.FirstName, e.LastName })
            .IsUnique()
            .HasDatabaseName("idxFNLN");

        modelBuilder.Entity<Employees>()
            .HasIndex(e => new { e.LastName, e.FirstName })
            .IsUnique()
            .HasDatabaseName("idxLNFN");

        modelBuilder.Entity<Employees>()
            .HasIndex(e => e.WindowsUserName)
            .IsUnique()
            .HasDatabaseName("WindowsUserName");

        modelBuilder.Entity<OrderDetails>()
            .HasIndex(od => new { od.OrderID, od.ProductID })
            .IsUnique()
            .HasDatabaseName("UniqueIdx");

        modelBuilder.Entity<OrderStatus>()
            .HasIndex(os => os.SortOrder)
            .IsUnique()
            .HasDatabaseName("SortOrder");

        modelBuilder.Entity<OrderStatus>()
            .HasIndex(os => os.StatusCode)
            .IsUnique()
            .HasDatabaseName("StatusCode");

        modelBuilder.Entity<OrderStatus>()
            .HasIndex(os => os.StatusName)
            .IsUnique()
            .HasDatabaseName("StatusName");

        modelBuilder.Entity<Products>()
            .HasIndex(p => p.ProductCode)
            .IsUnique()
            .HasDatabaseName("ProductCode");

        modelBuilder.Entity<Products>()
            .HasIndex(p => p.ProductName)
            .IsUnique()
            .HasDatabaseName("ProductName");

        modelBuilder.Entity<SystemSettings>()
            .HasIndex(s => s.SettingName)
            .IsUnique()
            .HasDatabaseName("SettingName");

        modelBuilder.Entity<Orders>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Orders>()
            .HasOne(o => o.Employee)
            .WithMany(e => e.Orders)
            .HasForeignKey(o => o.EmployeeID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Orders>()
            .HasOne(o => o.Status)
            .WithMany(s => s.Orders)
            .HasForeignKey(o => o.StatusID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetails>()
            .HasOne(od => od.Order)
            .WithMany(o => o.OrderDetails)
            .HasForeignKey(od => od.OrderID)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderDetails>()
            .HasOne(od => od.Product)
            .WithMany(p => p.OrderDetails)
            .HasForeignKey(od => od.ProductID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
