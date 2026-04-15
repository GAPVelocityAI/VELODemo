using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Data;
using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Components;
using Generated_northwindstarteredit_ae0ced4b_a24f_459f_ad79_59a95ba1d058.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();

// DbContext — uses SQL Server if connection string is configured, otherwise InMemory
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext>(options =>
{
    if (string.IsNullOrEmpty(connectionString))
        options.UseInMemoryDatabase("northwindstarteredit-ae0ced4b-a24f-459f-ad79-59a95ba1d058");
    else
        options.UseSqlServer(connectionString);
});
// DbContextFactory for Blazor Server — services use IDbContextFactory to create
// short-lived contexts per operation, avoiding disposed-context errors in circuits
builder.Services.AddDbContextFactory<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext>(options =>
{
    if (string.IsNullOrEmpty(connectionString))
        options.UseInMemoryDatabase("northwindstarteredit-ae0ced4b-a24f-459f-ad79-59a95ba1d058");
    else
        options.UseSqlServer(connectionString);
}, ServiceLifetime.Scoped);

// Application services
builder.Services.AddScoped<IAuditInterceptor, AuditInterceptor>();
builder.Services.AddScoped<IClsErrorHandlerService, ClsErrorHandlerService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ILookupService, LookupService>();
builder.Services.AddScoped<IModDaoService, ModDaoService>();
builder.Services.AddScoped<IModDebugService, ModDebugService>();
builder.Services.AddScoped<IModFilesService, ModFilesService>();
builder.Services.AddScoped<IModGlobalService, ModGlobalService>();
builder.Services.AddScoped<IModOrdersService, ModOrdersService>();
builder.Services.AddScoped<IModStartupService, ModStartupService>();
builder.Services.AddScoped<IOrderDetailService, OrderDetailService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IOrderStatusService, OrderStatusService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<ISystemSettingsService, SystemSettingsService>();

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
    var db = scope.ServiceProvider.GetRequiredService<NorthwindstartereditAe0ced4bA24f459fAd7959a95ba1d058DbContext>();
    db.Database.EnsureCreated();
}

await DataSeeder.SeedAsync(app);

app.Run();
