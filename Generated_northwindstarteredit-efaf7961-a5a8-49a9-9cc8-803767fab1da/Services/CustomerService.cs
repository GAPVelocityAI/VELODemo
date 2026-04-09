using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly ILogger<CustomerService> _logger;

    public CustomerService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        ILogger<CustomerService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryCustomerList. Returns customer fields with computed PrimaryContact,
    /// BusinessAddress, and PlainTextNotes values ordered by CustomerName.
    /// </summary>
    public async Task<List<CustomerListDto>> GetCustomerListAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await db.Customers
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
            _logger.LogError(ex, "Error running qryCustomerList.");
            throw;
        }
    }

    public async Task<List<Customers>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Customers.OrderBy(c => c.CustomerName).ToListAsync();
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
            _logger.LogError(ex, "Error retrieving customer {CustomerId}.", customerId);
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
