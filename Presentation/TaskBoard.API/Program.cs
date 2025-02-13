using Microsoft.AspNetCore.Identity;
using TaskBoard.Application;
using TaskBoard.Domain.Entities.Identity;
using TaskBoard.Persistence;
using TaskBoard.Infrastructure;
using TaskBoard.Persistence.Contexts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationServices();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
