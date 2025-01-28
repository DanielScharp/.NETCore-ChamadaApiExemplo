using ChamadaApi.Database.MySql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();
builder.Services.AddHttpClient();

// Add services to the container.
builder.Services.Add(new ServiceDescriptor(typeof(LeiloesRepository), new LeiloesRepository(Environment.GetEnvironmentVariable("Dashboard_ConnMySqlLeilao"))));
builder.Services.Add(new ServiceDescriptor(typeof(UsuarioRepository), new UsuarioRepository(Environment.GetEnvironmentVariable("Dashboard_ConnMySqlUsuarios"))));


var tokenSecret = Environment.GetEnvironmentVariable("Dashboard_TokenSecret");
if(string.IsNullOrEmpty(tokenSecret))
{
    throw new Exception("TOKEN_SECRET não foi configurado no ambiente.");
}

var key = Encoding.ASCII.GetBytes(Environment.GetEnvironmentVariable("Dashboard_TokenSecret"));


builder.Services.AddAuthentication(x => {
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x => {
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key),
        ValidateIssuer = false,
        ValidateAudience = false,

    };
    x.Events = new JwtBearerEvents
    {
        OnMessageReceived = context => {
            var authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();

            if(!string.IsNullOrEmpty(authorizationHeader) && !authorizationHeader.StartsWith("Bearer "))
            {
                // Trata como um token sem prefixo
                context.Token = authorizationHeader;
            }

            return Task.CompletedTask;
        },
        OnChallenge = context => {
            context.Response.OnStarting(async () => {
                await context.Response.WriteAsync("Usuário não autenticado!");
            });

            return Task.CompletedTask;
        },

        OnForbidden = context => {
            context.Response.OnStarting(async () => {
                await context.Response.WriteAsync("Usuário não autorizado!");
            });

            return Task.CompletedTask;
        }
    };


});

builder.Services.AddCors(options => {
    options.AddPolicy(name: "CorsOrigensPermitidas",
        builder => {
            builder.WithOrigins(
                "http://localhost:7259",
                "http://localhost:7158"
                )
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dashboard", Version = "v1" });

    // Adiciona o esquema de autenticação Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Authorization: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
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
builder.Services.AddLogging(logging => {
    logging.AddConsole();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsOrigensPermitidas");

app.UseAuthorization();

app.MapControllers();

app.Run();
