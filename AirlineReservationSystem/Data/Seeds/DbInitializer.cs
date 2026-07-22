public class DbInitializer
{
    private readonly ApplicationDbContext _context;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManger;
    private readonly ILogger<DbInitializer> _logger;

    public DbInitializer(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManger,
        ApplicationDbContext context, ILogger<DbInitializer> logger)
    {
        _roleManager = roleManager;
        _userManger = userManger;
        _context = context;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await SeedRolesAsync();
        await SeedAirportsAsync();
    }

    private async Task SeedRolesAsync()
    {
        try
        {
            if (!_roleManager.Roles.Any())
            {
                await _roleManager.CreateAsync(new IdentityRole(SD.SUPER_ADMIN_ROLE));
                await _roleManager.CreateAsync(new IdentityRole(SD.ADMIN_ROLE));
                await _roleManager.CreateAsync(new IdentityRole(SD.CUSTOMER_ROLE));
                await _roleManager.CreateAsync(new IdentityRole(SD.EMPLOYEE_ROLE));

                var superAdmin = new ApplicationUser()
                {
                    Email = "SuperAdmin@AirLine.com",
                    EmailConfirmed = true,
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "SuperAdmin"
                };

                var result = await _userManger.CreateAsync(superAdmin, password: "Admin123$");

                if (result.Succeeded)
                {
                    await _userManger.AddToRoleAsync(superAdmin, SD.SUPER_ADMIN_ROLE);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"{ex.Message}");
        }
    }

    private async Task SeedAirportsAsync()
    {
        await _context.Database.ExecuteSqlRawAsync(@"
                IF NOT EXISTS (SELECT 1 FROM Airports WHERE IATACode = 'CAI' OR ICAOCode = 'HECA')
                BEGIN
                    INSERT INTO Airports (Name, IATACode, ICAOCode, City, Country, Status, CreatedAt, IsDeleted)
                    VALUES ('Cairo International Airport', 'CAI', 'HECA', 'Cairo', 'Egypt', 0, GETUTCDATE(), 0);
                END
                ");

        await _context.Database.ExecuteSqlRawAsync(@"
                IF NOT EXISTS (SELECT 1 FROM Airports WHERE IATACode = 'DXB' OR ICAOCode = 'OMDB')
                BEGIN
                    INSERT INTO Airports (Name, IATACode, ICAOCode, City, Country, Status, CreatedAt, IsDeleted)
                    VALUES ('Dubai International Airport', 'DXB', 'OMDB', 'Dubai', 'United Arab Emirates', 0, GETUTCDATE(), 0);
                END
                ");
    }
}
