using AirlineReservationSystem.Data;

namespace AirlineReservationSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            
            builder.Services.AddControllersWithViews();

            // DbContext
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Identity
            builder.Services
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = true;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 8;

                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddScoped<IAirportRepository, AirportRepository>();
            builder.Services.AddScoped<IFlightRepository, FlightRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();
            
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}")
                .WithStaticAssets();

            app.Run();
        }
    }
}
