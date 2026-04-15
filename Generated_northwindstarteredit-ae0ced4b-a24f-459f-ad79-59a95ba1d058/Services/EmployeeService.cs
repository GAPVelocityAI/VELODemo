using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> _dbFactory;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext> dbFactory,
        ILogger<EmployeeService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryEmployeeDetails: returns all employee columns ordered by FullNameLNFN.
    /// </summary>
    public async Task<List<Employees>> GetEmployeeDetailsAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.AsNoTracking().OrderBy(e => e.FullNameLNFN).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryEmployeeDetails.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryEmployeeList: returns employee list ordered by FullNameFNLN.
    /// </summary>
    public async Task<List<Employees>> GetEmployeeListAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.AsNoTracking().OrderBy(e => e.FullNameFNLN).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryEmployeeList.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryEmployeeLogin: returns employee login-related fields.
    /// </summary>
    public async Task<List<Employees>> GetEmployeeLoginAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryEmployeeLogin.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrycboEmployees: employee lookup ordered by first and last name.
    /// </summary>
    public async Task<List<Employees>> GetEmployeeLookupAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees
                .AsNoTracking()
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qrycboEmployees.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryrptEmployeeEmailList: employee email report.
    /// </summary>
    public async Task<List<Employees>> GetEmployeeEmailListAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.AsNoTracking().OrderBy(e => e.FullNameLNFN).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryrptEmployeeEmailList.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryrptEmployeePhoneList: employee phone report.
    /// </summary>
    public async Task<List<Employees>> GetEmployeePhoneListAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.AsNoTracking().OrderBy(e => e.FullNameLNFN).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error executing qryrptEmployeePhoneList.");
            throw;
        }
    }

    public async Task<List<Employees>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.AsNoTracking().OrderBy(e => e.FullNameLNFN).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving all employees.");
            throw;
        }
    }

    public async Task<Employees?> GetByIdAsync(int employeeId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.FirstOrDefaultAsync(e => e.EmployeeID == employeeId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving employee by id {EmployeeId}.", employeeId);
            throw;
        }
    }

    public async Task<Employees> CreateAsync(Employees employee)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.Employees.Add(employee);
            await db.SaveChangesAsync();
            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employee.");
            throw;
        }
    }

    public async Task<Employees> UpdateAsync(Employees employee)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            db.Employees.Update(employee);
            await db.SaveChangesAsync();
            return employee;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employee {EmployeeId}.", employee.EmployeeID);
            throw;
        }
    }

    public async Task<int> DeleteAsync(int employeeId)
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.Where(e => e.EmployeeID == employeeId).ExecuteDeleteAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employee {EmployeeId}.", employeeId);
            throw;
        }
    }
}
