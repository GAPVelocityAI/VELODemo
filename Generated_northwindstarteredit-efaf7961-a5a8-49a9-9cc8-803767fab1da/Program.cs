using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Models;
using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Components;
using Generated_northwindstarteredit_efaf7961_a5a8_49a9_9cc8_803767fab1da.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// DbContext — uses SQL Server if connection string is configured, otherwise InMemory
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext>(options =>
{
    if (string.IsNullOrEmpty(connectionString))
        options.UseInMemoryDatabase("northwindstarteredit-efaf7961-a5a8-49a9-9cc8-803767fab1da");
    else
        options.UseSqlServer(connectionString);
});
// DbContextFactory for Blazor Server — services use IDbContextFactory to create
// short-lived contexts per operation, avoiding disposed-context errors in circuits
builder.Services.AddDbContextFactory<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext>(options =>
{
    if (string.IsNullOrEmpty(connectionString))
        options.UseInMemoryDatabase("northwindstarteredit-efaf7961-a5a8-49a9-9cc8-803767fab1da");
    else
        options.UseSqlServer(connectionString);
}, ServiceLifetime.Scoped);

builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<IOrderDetailsService, OrderDetailsService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();
builder.Services.AddScoped<IClsErrorHandlerService, ClsErrorHandlerService>();
builder.Services.AddScoped<IModDAOService, ModDAOService>();
builder.Services.AddScoped<IModDebugService, ModDebugService>();
builder.Services.AddScoped<IModFilesService, ModFilesService>();
builder.Services.AddScoped<IModGlobalService, ModGlobalService>();
builder.Services.AddScoped<IModOrdersService, ModOrdersService>();
builder.Services.AddScoped<IModStartupService, ModStartupService>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<SaveChangesInterceptor, AuditInterceptor>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


// Create InMemory database schema from model on startup (only for InMemory provider)
if (string.IsNullOrEmpty(connectionString))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<NorthwindstartereditEfaf7961A5A849A99Cc8803767Fab1DaDbContext>();
    db.Database.EnsureCreated();
}

await DataSeeder.SeedAsync(app);

app.Run();
