using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Repositories;
using metalurgicaMVC.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IEmpleadoRepository, EmpleadoRepository>();
builder.Services.AddScoped<ITrabajoRepository, TrabajoRepository>();
builder.Services.AddScoped<IPagoRepository, PagoRepository>();
builder.Services.AddScoped<IFileService, FileService>();

var configuration = builder.Configuration;

builder.Services.AddDbContext<DBContext>(
    dbContextOptions => dbContextOptions
        .UseMySql(
            configuration["ConnectionStrings:MySql"], 
            ServerVersion.AutoDetect(configuration["ConnectionStrings:MySql"])
        )
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// app.UseCors(x => x
// 	.AllowAnyOrigin()
// 	.AllowAnyMethod()
// 	.AllowAnyHeader());

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
