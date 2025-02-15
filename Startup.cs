using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using HealthCare.Infrastructure;
using HealthCare.Infrastructure.Shared;
using HealthCare.Domain.Shared;
using HealthCare.Domain.Staffs;
using HealthCare.Infraestructure.Staffs;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using HealthCare.Domain.Users;
using HealthCare.Infrastructure.Users;
using HealthCare.Domain.Shared.Mailing;
using HealthCare.Domain.Patients;
using HealthCare.Infrastructure.Patients;
using HealthCare.Domain.Shared.Authentication;
using HealthCare.Domain.OperationTypes;
using HealthCare.Domain.OperationRequests;
using HealthCare.Infraestructure.OperationRequests;
using HealthCare.Middleware;

namespace HealthCare
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Use SQL Server Database
            services.AddDbContext<HealthCareDbContext>(opt => opt
                .UseSqlServer(Configuration.GetConnectionString("DefaultConnection"))
                .ReplaceService<IValueConverterSelector, StronglyEntityIdValueConverterSelector>());

            services.AddCors(opt =>
            {
                opt.AddPolicy("AllowAngularApp", policy =>
                {
                    policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            ConfigureMyServices(services);

            services.AddControllers().AddNewtonsoftJson();

            string secretKey = Configuration.GetSection("JwtSettings").GetValue<string>("SecretKey");
            byte[] key = Encoding.UTF8.GetBytes(secretKey); // Secret key for token encryption

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = "HealthCare",
                        ValidAudience = "HealthCare"
                    };
                }
            );

            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // app.UseHsts(); // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                // app.UseHttpsRedirection();
            }

            app.UseMiddleware<TimeOutMiddleware>(TimeSpan.FromMinutes(20));

            app.UseRouting();

            app.UseCors("AllowAngularApp");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        public void ConfigureMyServices(IServiceCollection services)
        {
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            services.AddTransient<IStaffRepository, StaffRepository>();
            services.AddTransient<StaffService>();

            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<UserService>();

            services.AddTransient<IPatientRepository, PatientRepository>();
            services.AddTransient<PatientService>();

            services.AddTransient<IOperationTypeRepository, OperationTypeRepository>();
            services.AddTransient<OperationTypeService>();

            services.AddTransient<IOperationRequestRepository, OperationRequestRepository>();
            services.AddTransient<OperationRequestService>();

            services.AddTransient<AuthenticationService>();

            services.AddTransient(provider =>
                new JwtTokenService(Configuration["JwtSettings:SecretKey"])
            );

            services.Configure<SmtpSettings>(Configuration.GetSection("SmtpSettings"));

            services.AddTransient<EmailService>();
        }
    }
}
