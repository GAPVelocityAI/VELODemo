using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using Microsoft.EntityFrameworkCore;

namespace Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

public sealed class EmployeeService : IEmployeeService
{
    private readonly IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> _dbFactory;
    private readonly ILogger<EmployeeService> _logger;

    public EmployeeService(
        IDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext> dbFactory,
        ILogger<EmployeeService> logger)
    {
        _dbFactory = dbFactory;
        _logger = logger;
    }

    /// <summary>
    /// Translates Access query qryEmployeeDetails (Employees.* ordered by FullNameLNFN).
    /// </summary>
    public async Task<List<Employees>> GetEmployeeDetailsAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.OrderBy(e => e.FullNameLNFN).ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryEmployeeDetails.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryEmployeeList.
    /// </summary>
    public async Task<List<EmployeeListDto>> GetEmployeeListAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await db.Employees
                .OrderBy(e => e.FullNameFNLN)
                .Select(e => new EmployeeListDto
                {
                    EmployeeID = e.EmployeeID,
                    LastName = e.LastName,
                    FirstName = e.FirstName,
                    FullNameFNLN = e.FullNameFNLN,
                    EmailAddress = e.EmailAddress,
                    JobTitle = e.JobTitle,
                    PrimaryPhone = e.PrimaryPhone,
                    Title = e.Title,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryEmployeeList.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qryEmployeeLogin.
    /// </summary>
    public async Task<List<EmployeeLoginDto>> GetEmployeeLoginAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await db.Employees
                .Select(e => new EmployeeLoginDto
                {
                    EmployeeID = e.EmployeeID,
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    EmailAddress = e.EmailAddress,
                    JobTitle = e.JobTitle,
                    WindowsUserName = e.WindowsUserName,
                    FullNameLNFN = e.FullNameLNFN,
                    FullNameFNLN = e.FullNameFNLN,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qryEmployeeLogin.");
            throw;
        }
    }

    /// <summary>
    /// Translates Access query qrycboEmployees.
    /// </summary>
    public async Task<List<CboEmployeeDto>> GetCboEmployeesAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();

            return await db.Employees
                .OrderBy(e => e.FirstName)
                .ThenBy(e => e.LastName)
                .Select(e => new CboEmployeeDto
                {
                    EmployeeID = e.EmployeeID,
                    FullNameFNLN = e.FullNameFNLN,
                    WindowsUserName = e.WindowsUserName,
                })
                .ToListAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error running qrycboEmployees.");
            throw;
        }
    }

    public async Task<List<Employees>> GetAllAsync()
    {
        try
        {
            using var db = _dbFactory.CreateDbContext();
            return await db.Employees.OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToListAsync();
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
            _logger.LogError(ex, "Error retrieving employee {EmployeeId}.", employeeId);
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
