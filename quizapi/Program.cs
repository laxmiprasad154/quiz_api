using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using quizapi.Data_Access_Layer.context;
using quizapi.Data_Access_Layer.Repository.Implementation;
using quizapi.Data_Access_Layer.Repository.Interface;
using quizapi.Infrastructure;
using System.Text;
using Swashbuckle.AspNetCore.Filters;


var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<quizdbcontext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DevConnection"));
});
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddScoped<IQuestionListingRepo, QuestionRepo>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = false,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JWT : Issuer"],
        ValidAudience = builder.Configuration["JWT : Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration
        ["JWT:Key"]))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ParticipantOnly", policy => policy.RequireRole("Participant"));
});



builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();


builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme(\"bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });
    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

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