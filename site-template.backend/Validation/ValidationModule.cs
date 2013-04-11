using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;
using mywebsite.backend.Entity;

namespace mywebsite.backend.Validation
{
    public class ValidationModule:NinjectModule
    {
        public override void Load()
        {
            
            Bind<IValidatorFactory>().To<ValidatorFactory>().InSingletonScope();
            Bind<IValidator<Profile>>().To<ProfileValidator>().InRequestScope();
            FluentValidation.Mvc.FluentValidationModelValidatorProvider.Configure(p =>
            {
                p.ValidatorFactory = Kernel.Get<IValidatorFactory>();
            });
        }
    }
}
