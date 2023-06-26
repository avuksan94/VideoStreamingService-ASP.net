using DAL.Models;
using BLL;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using BLL.Services;
using DAL.Repo;
using System.Threading.Tasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

//DI za mappiranje
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddAutoMapper(typeof(BLL.Mapping.AutoMapperProfile));

builder.Services.AddScoped<UnitOfWork>();
builder.Services.AddScoped<VideoService>();
builder.Services.AddScoped<GenreService>();
builder.Services.AddScoped<TagService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ImageService>();
builder.Services.AddScoped<CountryService>();
builder.Services.AddScoped<VideoTagService>();

builder.Services.AddDbContext<RwaMoviesContext>(options =>
{
    options.UseSqlServer("name=ConnectionStrings:RWAConnStr");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Video}/{action=Index}/{id?}");

app.Run();
