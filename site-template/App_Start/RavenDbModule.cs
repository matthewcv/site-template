using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using Ninject.Activation;
using Ninject.Modules;
using Ninject.Web.Common;
using Raven.Client;
using Raven.Client.Document;

namespace mywebsite.App_Start
{
    public class RavenDbModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IDocumentStore>()
                .ToMethod(InitDocStore)
                .InSingletonScope();

            Bind<IDocumentSession>()
                .ToMethod(c => c.Kernel.Get<IDocumentStore>().OpenSession())
                .InRequestScope()
                .OnDeactivation((c, d) =>
                    {
                        if (d.Advanced.HasChanges)
                        {
                            d.SaveChanges();
                        }
                    });
        }


        private IDocumentStore InitDocStore(IContext context)
        {
            DocumentStore ds = new DocumentStore { ConnectionStringName = "mywebsite" };
            ds.Initialize();

            return ds;
        }
    }
}