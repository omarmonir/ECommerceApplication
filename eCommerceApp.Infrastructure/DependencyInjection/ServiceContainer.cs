using eCommerceApp.Domain.Entities;
using eCommerceApp.Domain.Interfaces;
using eCommerceApp.Infrastructure.Data;
using eCommerceApp.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using EntityFramework.Exceptions.SqlServer;
using Microsoft.AspNetCore.Builder;
using eCommerceApp.Infrastructure.Middleware;
using eCommerceApp.Application.Services.Interfaces.Logging;
using eCommerceApp.Infrastructure.Services;
using eCommerceApp.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using eCommerceApp.Domain.Interfaces.Authentication;
using eCommerceApp.Infrastructure.Repositories.Authentication;
using eCommerceApp.Domain.Interfaces.Cart;
using eCommerceApp.Infrastructure.Repositories.Cart;
using eCommerceApp.Application.Services.Interfaces.Cart;
using eCommerceApp.Domain.Interfaces.CategorySpacifics;
using eCommerceApp.Infrastructure.Repositories.CategorySpacifics;

namespace eCommerceApp.Infrastructure.DependencyInjection
{
	public static class ServiceContainer
	{
		public static IServiceCollection AddInfrastructureService(this IServiceCollection services, IConfiguration config)
		{
			string connectionString = "Default";
			services.AddDbContext<AppDbContext>(option =>
			option.UseSqlServer(config.GetConnectionString(connectionString), 
			sqlOptions =>
			{
				sqlOptions.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
				sqlOptions.EnableRetryOnFailure(); //Enable automatic retries for transient failures
			}).UseExceptionProcessor()
			, ServiceLifetime.Scoped);

			services.AddScoped<IGeneric<Product>, GenericRepository<Product>>();
			services.AddScoped<IGeneric<Category>, GenericRepository<Category>>();
			services.AddScoped(typeof(IAppLogger<>), typeof(SerilogLoggerAdapter<>));

			services.AddDefaultIdentity<AppUser>(options =>
			{
				options.SignIn.RequireConfirmedEmail = true;
				options.Tokens.EmailConfirmationTokenProvider = TokenOptions.DefaultProvider;
				options.Password.RequireDigit = true;
				options.Password.RequireNonAlphanumeric = true;
				options.Password.RequireUppercase = true;
				options.Password.RequireLowercase = true;
				options.Password.RequiredLength = 8;
				options.Password.RequiredUniqueChars = 1;



			})
				.AddRoles<IdentityRole>()
				.AddEntityFrameworkStores<AppDbContext>();

			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;


			}).AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
				{
					ValidateAudience = true,
					ValidateIssuer = true,
					ValidateLifetime = true,
					RequireExpirationTime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = config["JWT:Issuer"],
					ValidAudience = config["JWT:Audience"],
					ClockSkew = TimeSpan.Zero,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWT:Key"]!))
				};
			});


			services.AddScoped<IUserMangement, UserManagement>();
			services.AddScoped<ITokenMangement, TokenManagement>();
			services.AddScoped<IRoleMangement, RoleManagement>();
			services.AddScoped<IPaymentMethod, PaymentMethodRepository>();
			services.AddScoped<IPaymentService, StripePaymentService>();
			services.AddScoped<ICategory, CategoryRepositroy>();
			services.AddScoped<ICart, CartRepository>();

			Stripe.StripeConfiguration.ApiKey = config["Stripe:SecretKey"];
			return services;
		}
		public static IApplicationBuilder UseInfrastructureService(this IApplicationBuilder application)
		{
			application.UseMiddleware<ExceptionHandlingMiddleware>();
			return application;
		}

	}
}
