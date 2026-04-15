using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using CsvHelper.Configuration;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Data;

/// <summary>
/// Seeds the database with original Access data from CSV files on first startup.
/// Idempotent: skips if data is already present.
/// Supports both InMemory (dev) and SQL Server (production).
/// </summary>
public static class DataSeeder
{
    private const int BulkCopyThreshold = 10_000;
    private static readonly ILogger _logger =
        LoggerFactory.Create(b => b.AddConsole()).CreateLogger("DataSeeder");

    public static async Task SeedAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext>();

        // Idempotency: if the first seeded table already has rows, skip entirely
        if (await db.Welcome.AnyAsync())
        {
            _logger.LogInformation("DataSeeder: Data already present — skipping seed.");
            return;
        }

        var isSqlServer = db.Database.IsSqlServer();
        var dataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");

        _logger.LogInformation("DataSeeder: Starting seed (SQL Server: {IsSql}).", isSqlServer);

        // Insert in FK dependency order (parents first)
        await SeedWelcomeAsync(db, dataDir, isSqlServer);
        await SeedSystemSettingsAsync(db, dataDir, isSqlServer);
        await SeedNorthwindFeaturesAsync(db, dataDir, isSqlServer);
        await SeedCustomersAsync(db, dataDir, isSqlServer);
        await SeedEmployeesAsync(db, dataDir, isSqlServer);
        await SeedOrderStatusAsync(db, dataDir, isSqlServer);
        await SeedProductsAsync(db, dataDir, isSqlServer);
        await SeedOrdersAsync(db, dataDir, isSqlServer);
        await SeedOrderDetailsAsync(db, dataDir, isSqlServer);

        _logger.LogInformation("DataSeeder: Seed completed — {Total} rows inserted.", 311);
    }

    private static async Task SeedWelcomeAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db, string dataDir, bool isSqlServer)
    {
        var csvPath = Path.Combine(dataDir, "Welcome.csv");
        var rows = ReadCsvRows(csvPath);
        if (rows.Count == 0) return;

        var records = rows.Select(row => new Welcome
        {
            ID = int.Parse(row["ID"], CultureInfo.InvariantCulture),
            WelcomeText = N(row["Welcome"]),
            About = N(row["About"]),
            Learn = N(row["Learn"]),
            DataMacro = N(row["DataMacro"]),
        }).ToList();

        if (!isSqlServer || rows.Count <= BulkCopyThreshold)
        {
            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Welcome] ON");

            const int batchSize = 500;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                await db.Welcome.AddRangeAsync(records.Skip(i).Take(batchSize));
                await db.SaveChangesAsync();
            }

            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Welcome] OFF");
        }

        _logger.LogInformation("DataSeeder: Seeded Welcome ({Count} rows).", records.Count);
    }

    private static async Task SeedSystemSettingsAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db, string dataDir, bool isSqlServer)
    {
        var csvPath = Path.Combine(dataDir, "SystemSettings.csv");
        var rows = ReadCsvRows(csvPath);
        if (rows.Count == 0) return;

        var records = rows.Select(row => new SystemSettings
        {
            SettingID = int.Parse(row["SettingID"], CultureInfo.InvariantCulture),
            SettingName = row["SettingName"],
            SettingValue = N(row["SettingValue"]),
            Notes = N(row["Notes"]),
        }).ToList();

        if (!isSqlServer || rows.Count <= BulkCopyThreshold)
        {
            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[SystemSettings] ON");

            const int batchSize = 500;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                await db.SystemSettings.AddRangeAsync(records.Skip(i).Take(batchSize));
                await db.SaveChangesAsync();
            }

            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[SystemSettings] OFF");
        }

        _logger.LogInformation("DataSeeder: Seeded SystemSettings ({Count} rows).", records.Count);
    }

    private static async Task SeedNorthwindFeaturesAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db, string dataDir, bool isSqlServer)
    {
        var csvPath = Path.Combine(dataDir, "NorthwindFeatures.csv");
        var rows = ReadCsvRows(csvPath);
        if (rows.Count == 0) return;

        var records = rows.Select(row => new NorthwindFeatures
        {
            NorthwindFeaturesID = int.Parse(row["NorthwindFeaturesID"], CultureInfo.InvariantCulture),
            ItemName = row["ItemName"],
            Description = N(row["Description"]),
            Navigation = row["Navigation"],
            LearnMore = N(row["LearnMore"]),
            HelpKeywords = N(row["HelpKeywords"]),
        }).ToList();

        if (!isSqlServer || rows.Count <= BulkCopyThreshold)
        {
            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[NorthwindFeatures] ON");

            const int batchSize = 500;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                await db.NorthwindFeatures.AddRangeAsync(records.Skip(i).Take(batchSize));
                await db.SaveChangesAsync();
            }

            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[NorthwindFeatures] OFF");
        }

        _logger.LogInformation("DataSeeder: Seeded NorthwindFeatures ({Count} rows).", records.Count);
    }

    private static async Task SeedCustomersAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db, string dataDir, bool isSqlServer)
    {
        var csvPath = Path.Combine(dataDir, "Customers.csv");
        var rows = ReadCsvRows(csvPath);
        if (rows.Count == 0) return;

        var records = rows.Select(row => new Customers
        {
            CustomerID = int.Parse(row["CustomerID"], CultureInfo.InvariantCulture),
            CustomerName = row["CustomerName"],
            PrimaryContactLastName = N(row["PrimaryContactLastName"]),
            PrimaryContactFirstName = N(row["PrimaryContactFirstName"]),
            PrimaryContactJobTitle = N(row["PrimaryContactJobTitle"]),
            PrimaryContactEmailAddress = N(row["PrimaryContactEmailAddress"]),
            BusinessPhone = N(row["BusinessPhone"]),
            Address = row["Address"],
            City = row["City"],
            State = row["State"],
            Zip = row["Zip"],
            Website = N(row["Website"]),
            Notes = N(row["Notes"]),
            AddedBy = N(row["AddedBy"]),
            AddedOn = NDate(row["AddedOn"]),
            ModifiedBy = N(row["ModifiedBy"]),
            ModifiedOn = NDate(row["ModifiedOn"]),
        }).ToList();

        if (!isSqlServer || rows.Count <= BulkCopyThreshold)
        {
            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Customers] ON");

            const int batchSize = 500;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                await db.Customers.AddRangeAsync(records.Skip(i).Take(batchSize));
                await db.SaveChangesAsync();
            }

            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Customers] OFF");
        }

        _logger.LogInformation("DataSeeder: Seeded Customers ({Count} rows).", records.Count);
    }

    private static async Task SeedEmployeesAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db, string dataDir, bool isSqlServer)
    {
        var csvPath = Path.Combine(dataDir, "Employees.csv");
        var rows = ReadCsvRows(csvPath);
        if (rows.Count == 0) return;

        var records = rows.Select(row => new Employees
        {
            EmployeeID = int.Parse(row["EmployeeID"], CultureInfo.InvariantCulture),
            FirstName = row["FirstName"],
            LastName = row["LastName"],
            FullNameFNLN = N(row["FullNameFNLN"]),
            FullNameLNFN = N(row["FullNameLNFN"]),
            EmailAddress = N(row["EmailAddress"]),
            JobTitle = row["JobTitle"],
            PrimaryPhone = N(row["PrimaryPhone"]),
            SecondaryPhone = N(row["SecondaryPhone"]),
            Title = N(row["Title"]),
            Notes = N(row["Notes"]),
            Attachments = NBytes(row["Attachments"]),
            WindowsUserName = N(row["WindowsUserName"]),
            AddedBy = N(row["AddedBy"]),
            AddedOn = NDate(row["AddedOn"]),
            ModifiedBy = N(row["ModifiedBy"]),
            ModifiedOn = NDate(row["ModifiedOn"]),
        }).ToList();

        if (!isSqlServer || rows.Count <= BulkCopyThreshold)
        {
            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] ON");

            const int batchSize = 500;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                await db.Employees.AddRangeAsync(records.Skip(i).Take(batchSize));
                await db.SaveChangesAsync();
            }

            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Employees] OFF");
        }

        _logger.LogInformation("DataSeeder: Seeded Employees ({Count} rows).", records.Count);
    }

    private static async Task SeedOrderStatusAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db, string dataDir, bool isSqlServer)
    {
        var csvPath = Path.Combine(dataDir, "OrderStatus.csv");
        var rows = ReadCsvRows(csvPath);
        if (rows.Count == 0) return;

        var records = rows.Select(row => new OrderStatus
        {
            StatusID = int.Parse(row["StatusID"], CultureInfo.InvariantCulture),
            StatusCode = row["StatusCode"],
            StatusName = row["StatusName"],
            SortOrder = byte.Parse(row["SortOrder"], CultureInfo.InvariantCulture),
            AddedBy = N(row["AddedBy"]),
            AddedOn = NDate(row["AddedOn"]),
            ModifiedBy = N(row["ModifiedBy"]),
            ModifiedOn = NDate(row["ModifiedOn"]),
        }).ToList();

        if (!isSqlServer || rows.Count <= BulkCopyThreshold)
        {
            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[OrderStatus] ON");

            const int batchSize = 500;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                await db.OrderStatus.AddRangeAsync(records.Skip(i).Take(batchSize));
                await db.SaveChangesAsync();
            }

            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[OrderStatus] OFF");
        }

        _logger.LogInformation("DataSeeder: Seeded OrderStatus ({Count} rows).", records.Count);
    }

    private static async Task SeedProductsAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db, string dataDir, bool isSqlServer)
    {
        var csvPath = Path.Combine(dataDir, "Products.csv");
        var rows = ReadCsvRows(csvPath);
        if (rows.Count == 0) return;

        var records = rows.Select(row => new Products
        {
            ProductID = int.Parse(row["ProductID"], CultureInfo.InvariantCulture),
            ProductCode = row["ProductCode"],
            ProductName = row["ProductName"],
            ProductDescription = N(row["ProductDescription"]),
            UnitPrice = decimal.Parse(row["UnitPrice"], CultureInfo.InvariantCulture),
            AddedBy = N(row["AddedBy"]),
            AddedOn = NDate(row["AddedOn"]),
            ModifiedBy = N(row["ModifiedBy"]),
            ModifiedOn = NDate(row["ModifiedOn"]),
        }).ToList();

        if (!isSqlServer || rows.Count <= BulkCopyThreshold)
        {
            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON");

            const int batchSize = 500;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                await db.Products.AddRangeAsync(records.Skip(i).Take(batchSize));
                await db.SaveChangesAsync();
            }

            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF");
        }

        _logger.LogInformation("DataSeeder: Seeded Products ({Count} rows).", records.Count);
    }

    private static async Task SeedOrdersAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db, string dataDir, bool isSqlServer)
    {
        var csvPath = Path.Combine(dataDir, "Orders.csv");
        var rows = ReadCsvRows(csvPath);
        if (rows.Count == 0) return;

        var records = rows.Select(row => new Orders
        {
            OrderID = int.Parse(row["OrderID"], CultureInfo.InvariantCulture),
            EmployeeID = int.Parse(row["EmployeeID"], CultureInfo.InvariantCulture),
            CustomerID = int.Parse(row["CustomerID"], CultureInfo.InvariantCulture),
            OrderDate = DateTime.Parse(row["OrderDate"], CultureInfo.InvariantCulture),
            ShippedDate = NDate(row["ShippedDate"]),
            PaidDate = NDate(row["PaidDate"]),
            Notes = N(row["Notes"]),
            StatusID = int.Parse(row["StatusID"], CultureInfo.InvariantCulture),
            AddedBy = N(row["AddedBy"]),
            AddedOn = NDate(row["AddedOn"]),
            ModifiedBy = N(row["ModifiedBy"]),
            ModifiedOn = NDate(row["ModifiedOn"]),
        }).ToList();

        if (!isSqlServer || rows.Count <= BulkCopyThreshold)
        {
            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Orders] ON");

            const int batchSize = 500;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                await db.Orders.AddRangeAsync(records.Skip(i).Take(batchSize));
                await db.SaveChangesAsync();
            }

            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Orders] OFF");
        }

        _logger.LogInformation("DataSeeder: Seeded Orders ({Count} rows).", records.Count);
    }

    private static async Task SeedOrderDetailsAsync(
        NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext db, string dataDir, bool isSqlServer)
    {
        var csvPath = Path.Combine(dataDir, "OrderDetails.csv");
        var rows = ReadCsvRows(csvPath);
        if (rows.Count == 0) return;

        var records = rows.Select(row => new OrderDetails
        {
            OrderDetailID = int.Parse(row["OrderDetailID"], CultureInfo.InvariantCulture),
            OrderID = int.Parse(row["OrderID"], CultureInfo.InvariantCulture),
            ProductID = int.Parse(row["ProductID"], CultureInfo.InvariantCulture),
            Quantity = short.Parse(row["Quantity"], CultureInfo.InvariantCulture),
            UnitPrice = decimal.Parse(row["UnitPrice"], CultureInfo.InvariantCulture),
            AddedBy = N(row["AddedBy"]),
            AddedOn = NDate(row["AddedOn"]),
            ModifiedBy = N(row["ModifiedBy"]),
            ModifiedOn = NDate(row["ModifiedOn"]),
        }).ToList();

        if (!isSqlServer || rows.Count <= BulkCopyThreshold)
        {
            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[OrderDetails] ON");

            const int batchSize = 500;
            for (int i = 0; i < records.Count; i += batchSize)
            {
                await db.OrderDetails.AddRangeAsync(records.Skip(i).Take(batchSize));
                await db.SaveChangesAsync();
            }

            if (isSqlServer)
                await db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[OrderDetails] OFF");
        }

        _logger.LogInformation("DataSeeder: Seeded OrderDetails ({Count} rows).", records.Count);
    }

    // ── Shared CSV parsing helper ────────────────────────────────────────────

    private static List<Dictionary<string, string>> ReadCsvRows(string csvPath)
    {
        var rows = new List<Dictionary<string, string>>();
        if (!File.Exists(csvPath))
        {
            _logger.LogWarning("DataSeeder: CSV not found at {Path}", csvPath);
            return rows;
        }

        var config = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            HasHeaderRecord = true,
            MissingFieldFound = null,
            BadDataFound = null,
        };
        using var reader = new StreamReader(csvPath, System.Text.Encoding.UTF8);
        using var csv = new CsvReader(reader, config);

        csv.Read();
        csv.ReadHeader();
        var headers = csv.HeaderRecord!;

        while (csv.Read())
        {
            var row = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var h in headers)
                row[h] = csv.GetField(h) ?? string.Empty;
            rows.Add(row);
        }

        return rows;
    }

    private static string? N(string val) =>
        string.IsNullOrWhiteSpace(val) ? null : val;

    private static int? NInt(string val) =>
        string.IsNullOrWhiteSpace(val) ? null : int.Parse(val, CultureInfo.InvariantCulture);

    private static double? NDbl(string val) =>
        string.IsNullOrWhiteSpace(val) ? null : double.Parse(val, CultureInfo.InvariantCulture);

    private static decimal? NDec(string val) =>
        string.IsNullOrWhiteSpace(val) ? null : decimal.Parse(val, CultureInfo.InvariantCulture);

    private static DateTime? NDate(string val) =>
        string.IsNullOrWhiteSpace(val) ? null : DateTime.Parse(val, CultureInfo.InvariantCulture);

    private static bool? NBool(string val) =>
        string.IsNullOrWhiteSpace(val) ? null
        : (val == "-1" || val.Equals("true", StringComparison.OrdinalIgnoreCase) ? true : false);

    private static byte[]? NBytes(string val)
    {
        if (string.IsNullOrWhiteSpace(val) || val == "<error reading value>")
            return null;

        try
        {
            return Convert.FromBase64String(val);
        }
        catch
        {
            return null;
        }
    }
}
