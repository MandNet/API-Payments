using System;
using System.Threading;
using System.Threading.Tasks;
using API_Payments.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace API_Payments.Services
{
    public class FeeUpdateService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ApiSettings _mySettings;
        private readonly IFeeInterface _feeService;

        public FeeUpdateService(IServiceProvider serviceProvider,
                                IOptions<ApiSettings> mySettings,
                                IFeeInterface feeService)
        {
            _serviceProvider = serviceProvider;
            _mySettings = mySettings.Value;
            _feeService = feeService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(UpdateFee, null, TimeSpan.Zero, TimeSpan.FromSeconds(_mySettings.Generator));
            return Task.CompletedTask;
        }

        private void UpdateFee(object state)
        {
            _feeService.Generate().Wait();
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