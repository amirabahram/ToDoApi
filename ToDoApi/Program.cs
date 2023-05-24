using Data;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WebFramework;

var builder = WebApplication.CreateBuilder(args);


//For Entity Framework

builder.Services.AddDbContext<ToDoContext>(Options=>
{
    Options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
}
);
//For Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>() 
    .AddEntityFrameworkStores<ToDoContext>()
    .AddDefaultTokenProviders();



DependencyContainers.RegisterServices(builder.Services);//related to IoC



builder.Services.AddControllers();



//Adding Authentication

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
    .AddJwtBearer(option=>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["JWT:Issuer"],
            ValidAudience = builder.Configuration["JWT:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//************************************************************************************
//builder.Services.AddIdentity<IdentityUser, IdentityRole>(Options =>
//{
//    Options.Password.RequiredLength = 2;
//    Options.Password.RequireNonAlphanumeric = false;
//    Options.Password.RequireDigit = false;
//    Options.Password.RequireLowercase = false;
//    Options.Password.RequireUppercase = false;
//    Options.Lockout.MaxFailedAccessAttempts = 5;
//    Options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
//})
//    .AddEntityFrameworkStores<ToDoContext>();
//*************************************************************************************
//builder.Services.ConfigureApplicationCookie(options =>
//{
//    options.LoginPath = "/Account/Login";
//    options.AccessDeniedPath = "/Account/AccessDenied";
//});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
