using metalurgicaMVC.Interfaces;
using metalurgicaMVC.Repositories;
using metalurgicaMVC.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
	.AddCookie(options =>
	{
		options.LoginPath = "/Usuario/IniciarSesion";
		options.LogoutPath = "/Usuario/Logout";
		options.AccessDeniedPath = "/Usuario/Restringido";
		options.ExpireTimeSpan = TimeSpan.FromMinutes(60 * 10);
	})
	.AddJwtBearer(options =>
        {
            var secreto = configuration["TokenAuthentication:SecretKey"];
            if (string.IsNullOrEmpty(secreto))
                throw new Exception("Falta configurar TokenAuthentication:Secret");
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = configuration["TokenAuthentication:Issuer"],
                ValidAudience = configuration["TokenAuthentication:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(secreto)),
                NameClaimType = System.Security.Claims.ClaimTypes.NameIdentifier,
                RoleClaimType = System.Security.Claims.ClaimTypes.Role,
            };
        }
    )
;
builder.Services.AddAuthorizationBuilder()
    .AddPolicy("ADMIN", policy =>
        policy.RequireRole("ADMIN"));


builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<ITrabajoRepository, TrabajoRepository>();
builder.Services.AddScoped<IFotoRepository, FotoRepository>();
builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<ITipoOperacionRepository, TipoOperacionRepository>();
builder.Services.AddScoped<IDetalleRepository, DetalleRepository>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserService, UserService>();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Trabajo}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
