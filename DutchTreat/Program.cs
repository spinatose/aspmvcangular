using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
    .AddNewtonsoftJson(cfg => cfg.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
builder.Services.AddTransient<IMailService, NullMailService>();
builder.Services.AddIdentity<StoreUser, IdentityRole>(cfg =>
    { 
        cfg.User.RequireUniqueEmail = true; 
    }).AddEntityFrameworkStores<DutchContext>();
builder.Services.AddDbContext<DutchContext>();
builder.Services.AddTransient<DutchSeeder>();
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IDutchRepository, DutchRepository>();

var app = builder.Build();

// populate database with seed data if not already populated
await RunSeeding(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");
else
    app.UseDeveloperExceptionPage();


// The order here is important. 
//app.UseDefaultFiles();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllerRoute("Default", "/{controller}/{action}/{id?}", new { controller = "App", action = "Index" });

//app.UseAuthorization();

app.MapRazorPages();

app.Run();

static async Task RunSeeding(WebApplication app)
{
    var scopeFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopeFactory?.CreateScope())
    {
        var seeder = scope?.ServiceProvider.GetService<DutchSeeder>() as DutchSeeder;
        await seeder?.SeedAsync();
    }
}