public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedAirportsAsync(context);
        await SeedAircraftsAsync(context);
    }

    private static async Task SeedAirportsAsync(ApplicationDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync(@"
        IF NOT EXISTS (SELECT 1 FROM Airports WHERE IATACode = 'CAI')
        BEGIN
            INSERT INTO Airports
            (Name, IATACode, ICAOCode, City, Country, Status, CreatedAt, IsDeleted)
            VALUES
            ('Cairo International Airport', 'CAI', 'HECA', 'Cairo', 'Egypt', 0, GETUTCDATE(), 0);
        END
        ");
    }

    private static async Task SeedAircraftsAsync(ApplicationDbContext context)
    {
        await context.Database.ExecuteSqlRawAsync(@"
        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-GCM')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('737-800', 'Boeing', 'SU-GCM', 189, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-GCN')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('737-800', 'Boeing', 'SU-GCN', 189, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-GCP')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('737-900ER', 'Boeing', 'SU-GCP', 215, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-GDQ')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('777-300ER', 'Boeing', 'SU-GDQ', 396, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-GEA')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('787-9 Dreamliner', 'Boeing', 'SU-GEA', 290, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-GEB')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('787-9 Dreamliner', 'Boeing', 'SU-GEB', 290, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-A320')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('A320-200', 'Airbus', 'SU-A320', 180, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-A321')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('A321neo', 'Airbus', 'SU-A321', 220, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-A319')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('A319-100', 'Airbus', 'SU-A319', 144, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-A330')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('A330-300', 'Airbus', 'SU-A330', 300, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-A350')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('A350-900', 'Airbus', 'SU-A350', 325, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-E170')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('E170', 'Embraer', 'SU-E170', 76, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-E190')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('E190', 'Embraer', 'SU-E190', 100, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-ATR1')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('ATR 72-600', 'ATR', 'SU-ATR1', 72, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-ATR2')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('ATR 72-600', 'ATR', 'SU-ATR2', 72, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-CRJ9')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('CRJ900', 'Bombardier', 'SU-CRJ9', 90, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-B738')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('737-800', 'Boeing', 'SU-B738', 189, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-B739')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('737-900ER', 'Boeing', 'SU-B739', 215, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-A220')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('A220-300', 'Airbus', 'SU-A220', 145, 0);
        END

        IF NOT EXISTS (SELECT 1 FROM Aircrafts WHERE RegistrationNumber = 'SU-B787')
        BEGIN
            INSERT INTO Aircrafts (Model, Manufacturer, RegistrationNumber, Capacity, Status)
            VALUES ('787-8 Dreamliner', 'Boeing', 'SU-B787', 242, 0);
        END
        ");
    }
}