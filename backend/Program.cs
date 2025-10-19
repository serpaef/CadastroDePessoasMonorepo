using backend.Domain.Interfaces;
using backend.Domain.Services;
using backend.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace backend
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<IUserServices, UserServices>();

            var env = builder.Environment;

            Console.WriteLine(env.EnvironmentName);

            //if (env.IsDevelopment())
            //{
            //    builder.Services.AddDbContext<ApplicationDbContext>(options =>
            //        options.UseInMemoryDatabase("DevDatabase"));
            //}
            //else
            //{
                var connectionString = builder.Configuration.GetConnectionString("MySql");

                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            //}

            // Add services to the container.

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
        }
    }
}
