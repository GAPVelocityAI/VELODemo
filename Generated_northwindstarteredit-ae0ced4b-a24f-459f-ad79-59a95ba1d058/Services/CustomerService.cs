using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public class CustomerListDto
{
    public int CustomerID { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string State { get; set; } = string.Empty;
    public string Zip { get; set; } = string.Empty;
    public string? BusinessPhone { get; set; }
    public string? PrimaryContactLastName { get; set; }
    public string? PrimaryContactFirstName { get; set; }
    public string? PrimaryContactJobTitle { get; set; }
    public string? PrimaryContactEmailAddress { get; set; }
    public string PrimaryContact { get; set; } = string.Empty;
    public string BusinessAddress { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public string? Website { get; set; }
    public string? AddedBy { get; set; }
    public DateTime? AddedOn { get; set; }
    public string? ModifiedBy { get; set; }
    public DateTime? ModifiedOn { get; set; }
    public string PlainTextNotes { get; set; } = string.Empty;
}

public sealed class CustomerService : ICustomerService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        ILogger<CustomerService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryCustomerList: returns customer list with computed primary contact and business address fields.
    /// </summary>
    public async Task<List<CustomerListDto>> GetCustomerListAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Customers
                .AsNoTracking()
                .OrderBy(c => c.CustomerName)
                .Select(c => new CustomerListDto
                {
                    CustomerID = c.CustomerID,
                    CustomerName = c.CustomerName,
                    Address = c.Address,
                    City = c.City,
                    State = c.State,
                    Zip = c.Zip,
                    BusinessPhone = c.BusinessPhone,
                    PrimaryContactLastName = c.PrimaryContactLastName,
                    PrimaryContactFirstName = c.PrimaryContactFirstName,
                    PrimaryContactJobTitle = c.PrimaryContactJobTitle,
                    PrimaryContactEmailAddress = c.PrimaryContactEmailAddress,
                    PrimaryContact = ((c.PrimaryContactFirstName ?? string.Empty) + " " + (c.PrimaryContactLastName ?? string.Empty)).Trim(),
                    BusinessAddress = c.Address + "  " + c.City + ", " + c.State + "  " + c.Zip,
                    Notes = c.Notes,
                    Website = c.Website,
                    AddedBy = c.AddedBy,
                    AddedOn = c.AddedOn,
                    ModifiedBy = c.ModifiedBy,
                    ModifiedOn = c.ModifiedOn,
                    PlainTextNotes = c.Notes ?? string.Empty,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryCustomerList.");
            throw;
        }
    }

    public async Task<List<Customers>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Customers.AsNoTracking().OrderBy(c => c.CustomerName).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all customers.");
            throw;
        }
    }

    public async Task<Customers?> GetByIdAsync(int customerId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Customers.FirstOrDefaultAsync(c => c.CustomerID == customerId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving customer by id {CustomerId}.", customerId);
            throw;
        }
    }

    public async Task<Customers> CreateAsync(Customers customer)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.Customers.Add(customer);
            await db.SaveChangesAsync();
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating customer.");
            throw;
        }
    }

    public async Task<Customers> UpdateAsync(Customers customer)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.Customers.Update(customer);
            await db.SaveChangesAsync();
            return customer;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating customer {CustomerId}.", customer.CustomerID);
            throw;
        }
    }

    public async Task<int> DeleteAsync(int customerId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Customers.Where(c => c.CustomerID == customerId).ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting customer {CustomerId}.", customerId);
            throw;
        }
    }
}
