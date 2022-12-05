using EShopping.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//var connectionStringSqlServer = builder.Configuration.GetConnectionString("DefaultConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseSqlServer(connectionStringSqlServer));

//var connectionStringOnLineMysql = builder.Configuration.GetConnectionString("MysqlConnection");
//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//    options.UseMySql(connectionStringOnLineMysql,ServerVersion.AutoDetect(connectionStringOnLineMysql)));


    var connectionStringOfflineMysql = builder.Configuration.GetConnectionString("LocalMysqlConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySql(connectionStringOfflineMysql, ServerVersion.AutoDetect(connectionStringOfflineMysql)));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area=UI}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
