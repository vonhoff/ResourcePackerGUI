using Microsoft.Extensions.DependencyInjection;
using ResourcePackerGUI.Application;
using ResourcePackerGUI.Infrastructure;
using WinFormsUI.Forms;

namespace WinFormsUI
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.SetHighDpiMode(HighDpiMode.PerMonitorV2);

            var services = new ServiceCollection();
            ConfigureServices(services);

            using var serviceProvider = services.BuildServiceProvider();
            var form1 = serviceProvider.GetRequiredService<MainForm>();
            Application.Run(form1);
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<MainForm>();
            services.AddApplicationServices();
            services.AddInfrastructureServices();
        }
    }
}