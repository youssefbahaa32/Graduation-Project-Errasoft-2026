using Microsoft.AspNetCore.Mvc.Rendering;

public interface ILookupService
{
    Task<IEnumerable<SelectListItem>> GetAirportsAsync(
        CancellationToken cancellationToken = default);

    Task<IEnumerable<SelectListItem>> GetAircraftsAsync(
        CancellationToken cancellationToken = default);
}