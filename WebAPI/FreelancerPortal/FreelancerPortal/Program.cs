using AutoMapper;
using FreelancerPortal;
using FreelancerPortal.Data;
using FreelancerPortal.Repository;
using FreelancerPortal.Service;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

//builder.Services.AddAutoMapper(typeof(Program));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Freelancer API",
        Version = "V1",
        Description = "An API to perform freelancer CRUD operation"
    });
    var xmlfile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlfile);
    c.IncludeXmlComments(xmlPath);
});


builder.Services.AddDbContext<AppDbContext>(db => db.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));
builder.Services.AddScoped<IUserRepository,UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();


//builder.Services.AddHttpsRedirection(options =>
//{
//    options.HttpsPort = 5001;  // Specify the HTTPS port
//});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAntiforgery(options => options.HeaderName = "X-CSRF-TOKEN");
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("X-CSRF-TOKEN", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Name = "X-CSRF-TOKEN",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Description = "CSRF Token for API requests"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "X-CSRF-TOKEN"
                }
            },
            new string[] { }
        }
    });
});



var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseCors("AllowAll");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Freelancer API V1");
    });
    
    
}



app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
//app.UseMiddleware<ValidateAntiforgeryTokenMiddleware>();
app.Run();
