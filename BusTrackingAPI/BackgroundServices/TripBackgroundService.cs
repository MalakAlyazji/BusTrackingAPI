
using BusTrackingBL.Interfaces;

namespace BusTrackingAPI.BackgroundServices
{
    public class TripBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        public TripBackgroundService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }
        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _scopeFactory.CreateScope();

                var tripService = scope.ServiceProvider
                    .GetRequiredService<ITripService>();

                await tripService.AutoDepartTrips();

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
