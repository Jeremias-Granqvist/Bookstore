using Bookstore.Service;
using Bookstore.Service.Interfaces;
using Bookstore.viewModel;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Net.Http;
using System.Windows;

namespace Bookstore
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Bootstrapper _bootstrapper;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            _bootstrapper = new Bootstrapper();
            _bootstrapper.Run();
        }

    }
    internal class Bootstrapper
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public void Run()
        {
            var services = new ServiceCollection();

            RegisterServices(services);
            RegisterViewModels(services);
            RegisterWindows(services);

            ServiceProvider = services.BuildServiceProvider();

            // Resolve MainWindow via DI, which will inject MainViewModel automatically
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IEventDispatcher, EventDispatcher>();
            services.AddHttpClient();
            //services.AddSingleton(new HttpClient { BaseAddress = new Uri("http://localhost:5252") });

            services.AddTransient<IAuthorService, AuthorService>();
            services.AddTransient<IBookService, BookService>();
            services.AddTransient<IInventoryService, InventoryService>();
            services.AddTransient<IStoreService, StoreService>();

        }

        private void RegisterViewModels(IServiceCollection services)
        {
            services.AddTransient<MainViewModel>();
            services.AddTransient<EditBookDialogViewModel>();
            services.AddTransient<EditAuthorDialogViewModel>();
            services.AddTransient<AddBookDialogViewModel>();
            services.AddTransient<AddAuthorDialogViewModel>();
            services.AddTransient<BarcodeSimulatorViewModel>();
        }

        private void RegisterWindows(IServiceCollection services)
        {
            services.AddTransient<MainWindow>();
        }
    }
}
