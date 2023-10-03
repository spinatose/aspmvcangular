var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Error");
else
    app.UseDeveloperExceptionPage();


// The order here is important. 
//app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute("Default", "/{controller}/{action}/{id?}", new { controller = "App", action = "Index" });

//app.UseAuthorization();

app.MapRazorPages();

app.Run();

