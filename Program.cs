
using OfficeOpenXml;
using SecretSantaAPI.Interface;
using SecretSantaAPI.Services;

namespace SecretSantaAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ExcelPackage.LicenseContext = LicenseContext.Commercial;
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddCors();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddScoped<IEmailSenderService,EmailSenderService>();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            
                app.UseSwagger();
                app.UseSwaggerUI();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
