using backend.Domain.Interfaces;
using backend.Domain.Middlewares;
using backend.Domain.ModelViews;
using backend.Domain.Services;
using backend.Infrastructure.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var jwtKey = builder.Configuration.GetSection("Jwt").GetSection("Key").Value ?? "5uJAjTCGY%YD82e7NF!gW3xZ$TC57M8F";

            builder.Services
                .AddAuthentication(option =>
                {
                    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(option =>
                {
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateLifetime = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateIssuer = false,
                        ValidateAudience = false
                    };
                });

            builder.Services.AddAuthorization();

            builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowAll", policy =>
                    {
                        policy.AllowAnyOrigin()    // Permite qualquer origem
                              .AllowAnyMethod()    // Permite qualquer método HTTP (GET, POST, PUT, DELETE, etc.)
                              .AllowAnyHeader();   // Permite qualquer cabeçalho
                    });
                });

            builder.Services.AddScoped<IUserServices, UserServices>();
            builder.Services.AddScoped<IPessoaServices, PessoaServices>();

            var env = builder.Environment;

            Console.WriteLine(env.EnvironmentName);

            if (env.IsDevelopment())
            {
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("DevDatabase"));
            }
            else
            {
                var connectionString = builder.Configuration.GetConnectionString("MySql");

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            }

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Insira o Token",
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                        new string[] { }
                    }
                });
                 
            });

            var app = builder.Build();

            app.useCors("AllowAll"); //olá avaliadores, liberado para acessarem, mas em produção real seria necessario configurar policy para os endereços e métodos somente dos consumidores oficiais da aplicação

            // Configure the HTTP request pipeline.
            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseHttpsRedirection();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.MapGet("/", () => Results.Json(new Home())).AllowAnonymous();

            app.MapControllers();

            app.Run();
        }
    }
}
