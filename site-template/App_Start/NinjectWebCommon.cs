using FluentValidation;
using mywebsite.backend;
using mywebsite.backend.Service;
using mywebsite.backend.Validation;

[assembly: WebActivator.PreApplicationStartMethod(typeof(mywebsite.App_Start.NinjectWebCommon), "Start")]
[assembly: WebActivator.ApplicationShutdownMethodAttribute(typeof(mywebsite.App_Start.NinjectWebCommon), "Stop")]

namespace mywebsite.App_Start
{
    using System;
using System.Web;
using Microsoft.Web.Infrastructure.DynamicModuleHelper;
using mywebsite.Controllers;
using Ninject;
using Ninject.Activation;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Document;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();
        public static IKernel Kernel
        {
            get { return bootstrapper.Kernel; }
        }

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
            kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();
            RegisterServices(kernel);
            return kernel;
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Load<RavenDbModule>();
            kernel.Load<AuthenticationModule>();
            kernel.Load<ValidationModule>();

            kernel.Bind<ICandyService>().To<CandyService>().InRequestScope();

        }
    

        
    }


}
