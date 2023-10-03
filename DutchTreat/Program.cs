var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (!app.Environment.IsDevelopment())
//{
//    app.UseExceptionHandler("/Error");
//}

// The order here is important. 
app.UseDefaultFiles();
app.UseStaticFiles();

//app.UseRouting();

//app.UseAuthorization();

// app.MapRazorPages();

app.Run();

