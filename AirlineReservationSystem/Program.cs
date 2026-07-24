using AirlineReservationSystem.Services.BackgroundJobs;
using Stripe;

namespace AirlineReservationSystem
{
    public class Program
    {
        public static async Task Main(string[] args)
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

                    options.SignIn.RequireConfirmedEmail = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                      options.ClientId = builder.Configuration["Authentication:Google:ClientId"];

                    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                });

            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            builder.Services.AddAutoMapper(cfg =>
                {}, typeof(MappingProfile).Assembly); builder.Services.AddScoped<IAirportRepository, AirportRepository>();
            builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();
            builder.Services.AddScoped<ISeatRepository, SeatRepository>();
            builder.Services.AddScoped<IFlightRepository, FlightRepository>();
            builder.Services.AddScoped<IBookingRepository, BookingRepository>();     
            builder.Services.AddScoped<ITicketRepository, TicketRepository>();
            builder.Services.AddScoped<IGenericRepository<ApplicationUserOTP>, GenericRepository<ApplicationUserOTP>>();
            builder.Services.AddScoped<IGenericRepository<Passenger>, GenericRepository<Passenger>>();
            builder.Services.AddScoped<IPassengerService, PassengerService>();
            builder.Services.AddScoped<IAccountService, Services.Implementations.AccountService>();
            builder.Services.AddScoped<IBookingService, BookingService>();
            builder.Services.AddScoped<IPaymentService, PaymentService>();
            builder.Services.AddScoped<ITicketService, TicketService>();
            builder.Services.AddScoped<ICustomerFlightService, CustomerFlightService>();
            builder.Services.AddScoped<IBookingCustomerService, BookingCustomerService>();
            builder.Services.AddTransient<IEmailSender, EmailSender>();
            builder.Services.AddScoped<DbInitializer>();

           

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IAirportService, AirportService>();
            builder.Services.AddScoped<IAircraftService, AircraftService>();
            builder.Services.AddScoped<ISeatService, SeatService>();
            builder.Services.AddScoped<IFlightService, FlightService>();

            builder.Services.AddScoped<ILookupService, LookupService>();
            builder.Services.AddScoped<ITicketCustomerService, TicketCustomerService>();
            builder.Services.AddScoped<IFileService, Services.Implementations.FileService>();
            builder.Services.AddHostedService<ExpiredBookingsCleanupService>();

            builder.Services.AddScoped<IImageRepository, ImageRepository>();
            builder.Services.AddScoped<IAirportImageRepository, AirportImageRepository>();
            builder.Services.AddScoped<IAircraftImageRepository, AircraftImageRepository>();
            builder.Services.AddScoped<IFlightImageRepository, FlightImageRepository>();
            builder.Services.AddScoped<IPassengerImageRepository, PassengerImageRepository>();

            StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

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
;

            app.MapStaticAssets();
            app.MapControllerRoute(
                name: "default",
                pattern: "{area=Identity}/{controller=Account}/{action=Register}/{id?}")
                .WithStaticAssets();




            //DbInitializer
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {

                    var dbInitializer = services.GetRequiredService<DbInitializer>();
                    await dbInitializer.SeedAsync();
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while running the Data Seeding. ");
                }
            }

            app.Run();
        }
    }
}
