using Huxley.ldbServiceReference;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(Huxley.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(Huxley.App_Start.NinjectWebCommon), "Stop")]

namespace Huxley.App_Start {
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Ninject;
    using Ninject.Web.Common;
    using ldbStaffServiceReference;

    public static class NinjectWebCommon {
        private static readonly Bootstrapper Bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            Bootstrapper.Initialize(CreateKernel);
        }

        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop() {
            Bootstrapper.ShutDown();
        }

        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel() {
            var kernel = new StandardKernel();
            try {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
                kernel.Bind<ILdbClient>().To<LdbClient>().InRequestScope();
                kernel.Bind<LDBServiceSoapClient>().To<LDBServiceSoapClient>().InRequestScope();
                kernel.Bind<LDBSVServiceSoapClient>().To<LDBSVServiceSoapClient>().InRequestScope();
                return kernel;
            } catch {
                kernel.Dispose();
                throw;
            }
        }
    }
}
