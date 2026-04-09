using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;

public class NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext : DbContext
{
    public NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext(DbContextOptions<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customers> Customers => Set<Customers>();
    public DbSet<Employees> Employees => Set<Employees>();
    public DbSet<NorthwindFeatures> NorthwindFeatures => Set<NorthwindFeatures>();
    public DbSet<OrderDetails> OrderDetails => Set<OrderDetails>();
    public DbSet<OrderStatus> OrderStatus => Set<OrderStatus>();
    public DbSet<Orders> Orders => Set<Orders>();
    public DbSet<Products> Products => Set<Products>();
    public DbSet<SystemSettings> SystemSettings => Set<SystemSettings>();
    public DbSet<Welcome> Welcome => Set<Welcome>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Orders>()
            .HasOne(e => e.Customer)
            .WithMany(e => e.OrderRecords)
            .HasForeignKey(e => e.CustomerID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Orders>()
            .HasOne(e => e.Employee)
            .WithMany(e => e.OrderRecords)
            .HasForeignKey(e => e.EmployeeID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Orders>()
            .HasOne(e => e.OrderStatus)
            .WithMany(e => e.OrderRecords)
            .HasForeignKey(e => e.StatusID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetails>()
            .HasOne(e => e.Order)
            .WithMany(e => e.OrderDetailRecords)
            .HasForeignKey(e => e.OrderID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<OrderDetails>()
            .HasOne(e => e.Product)
            .WithMany(e => e.OrderDetailRecords)
            .HasForeignKey(e => e.ProductID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Employees>()
            .HasIndex(e => new { e.FirstName, e.LastName })
            .IsUnique();

        modelBuilder.Entity<Employees>()
            .HasIndex(e => new { e.LastName, e.FirstName })
            .IsUnique();

        modelBuilder.Entity<Employees>()
            .HasIndex(e => e.WindowsUserName)
            .IsUnique();

        modelBuilder.Entity<OrderDetails>()
            .HasIndex(e => new { e.OrderID, e.ProductID })
            .IsUnique();

        modelBuilder.Entity<OrderStatus>()
            .HasIndex(e => e.SortOrder)
            .IsUnique();

        modelBuilder.Entity<OrderStatus>()
            .HasIndex(e => e.StatusCode)
            .IsUnique();

        modelBuilder.Entity<OrderStatus>()
            .HasIndex(e => e.StatusName)
            .IsUnique();

        modelBuilder.Entity<Products>()
            .HasIndex(e => e.ProductCode)
            .IsUnique();

        modelBuilder.Entity<Products>()
            .HasIndex(e => e.ProductName)
            .IsUnique();

        modelBuilder.Entity<SystemSettings>()
            .HasIndex(e => e.SettingName)
            .IsUnique();
    }
}
