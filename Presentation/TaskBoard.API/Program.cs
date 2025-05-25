using Microsoft.AspNetCore.Identity;
using TaskBoard.Application;
using TaskBoard.Domain.Entities.Identity;
using TaskBoard.Persistence;
using TaskBoard.Infrastructure;
using TaskBoard.Persistence.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

//builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
//    policy.WithOrigins("http://localhost:7168", "https://localhost:7168").AllowAnyHeader().AllowAnyMethod().AllowCredentials()
//));

// CORS ayarlari.. APIyi baska originlerden gelen istekleri kabul edebilir hale getiriyor..
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowTaskBoardPortal", policy =>
    {
        policy.WithOrigins("https://localhost:7286", "http://localhost:5209")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});


// Identity sistem ayarlari
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 8; // Parola en az 8 karakter uzunlugunda olmali
    options.Password.RequireNonAlphanumeric = true; // Parolada en az bir ozel karakter olmali
    options.Password.RequireDigit = true; // Parolada en az bir rakam olmali
    options.Password.RequireLowercase = true; // Parola en az bir kucuk harf icermeli
    options.Password.RequireUppercase = true; // Parola en az bir buyuk harf icermeli
    // Usernamede belirtilen karakterlerin kullanimina izin veriyor.. (Kullanilmazsa usernamede turkce karaktere izin vermez!)
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+çðýöþüÇÐÝÖÞÜ";
}).AddEntityFrameworkStores<TaskBoardDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddControllers();

builder.Services.AddAuthentication()
    // Adminler için JWT dogrulama
    .AddJwtBearer(builder.Configuration["Authentication:Admin:Scheme"], options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["Authentication:Admin:Audience"],
            ValidIssuer = builder.Configuration["Authentication:Admin:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Authentication:Admin:SecurityKey"]
            ))
        };
    })

    // Normal kullanicilar için JWT dogrulama
    .AddJwtBearer(builder.Configuration["Authentication:User:Scheme"], options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = builder.Configuration["Authentication:User:Audience"],
            ValidIssuer = builder.Configuration["Authentication:User:Issuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                builder.Configuration["Authentication:User:SecurityKey"]
            ))
        };
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowTaskBoardPortal");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
