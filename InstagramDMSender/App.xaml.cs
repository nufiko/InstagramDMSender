using AutoMapper;
using InstagramApiSharp.API.Builder;
using InstagramDMSender.ApiWrapper;
using InstagramDMSender.Infrastructure;
using Ninject;
using System.Windows;

namespace InstagramDMSender
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        IKernel kernel;
        private MapperConfiguration mapperConfiguration;

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            ConfigureMapper();
            ConfigureDi();
            CreateWindow();
        }

        private void ConfigureMapper()
        {
            mapperConfiguration = new MapperConfiguration( cfg =>
                {
                    cfg.AddProfile(new MappingProfiles());
                }
            );
        }

        private void CreateWindow()
        {
            Current.MainWindow = kernel.Get<MainWindow>();
            Current.MainWindow.Show();
        }

        private void ConfigureDi()
        {
            this.kernel = new StandardKernel();

            kernel.Bind<IInstaApiBuilder>().ToMethod(context => InstaApiBuilder.CreateBuilder()).InSingletonScope();
            kernel.Bind<IApiWrapperBuilder>().To<ApiWrapperBuilder>().InSingletonScope();
            kernel.Bind<IMapper>().ToMethod(a => new Mapper(mapperConfiguration)).InSingletonScope();
            
        }
    }
}