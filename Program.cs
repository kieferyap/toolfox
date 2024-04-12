using RestApis.Data;

var builder = WebApplication.CreateBuilder(args);

// Register the DB Context so that it is recognized
builder.Services.AddDbContext<AppDbContext>();

// Makes it easy to inject instances of IUnitOfwork into various parts of the application
builder.Services.AddScoped<IUnitOfwork, UnitOfwork>(); 

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"); // Default controller is HOME, default action is INDEX.

app.Run();
