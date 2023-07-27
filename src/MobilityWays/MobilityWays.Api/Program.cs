using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MobilityWays.Api;
using MobilityWays.Application.Commands;
using MobilityWays.Application.Persistence;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

//Get JwtSecret from User Secrets (Needs to be a 128 bit key)
string jwtSecretKey = builder.Configuration["JwtSecretKey"] ?? string.Empty;

//Configure Swagger for API Exploration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(_ =>
{
    _.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."

    });
    _.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

//Add Auth, using own jwt bearer process
builder.Services.AddAuthorization();
builder.Services.AddHttpContextAccessor();
builder.Services.AddAuthentication().AddJwtBearer(_ =>
{
    _.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey))
    };
});


//Register singleton for in memory user store
builder.Services.AddSingleton<IUserStore, InMemoryUserStore>();


//Register MediatR
builder.Services.AddMediatR(_ =>
{
    _.RegisterServicesFromAssemblyContaining<CreateUserCommand>();
});


var app = builder.Build();

//Map the User Endpoints
app.MapUserEndpoints(jwtSecretKey);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


//Use Auth
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.Run();