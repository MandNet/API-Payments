using System;
using System.Threading;
using System.Threading.Tasks;
using API_Payments.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace API_Payments.Services
{
    public class FeeUpdateService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ApiSettings _mySettings;


        public FeeUpdateService(IServiceProvider serviceProvider, ApiSettings mySettings)
        {
            _serviceProvider = serviceProvider;
            _mySettings = mySettings;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateFee, null, TimeSpan.Zero, TimeSpan.FromSeconds(_mySettings.Generator));
            return Task.CompletedTask;
        }

        private void UpdateFee(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var feeService = scope.ServiceProvider.GetRequiredService<FeeService>();
                feeService.Generate().Wait();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
