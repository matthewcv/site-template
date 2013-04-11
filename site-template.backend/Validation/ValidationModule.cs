using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Common;

namespace SiteTemplate.Backend.Validation
{
    public class ValidationModule:matthewcv.common.Validation.ValidationModule
    {
        public override void Load()
        {
            base.Load();
            //make sure base.Load() is called and then register your validators here
        }
    }
}
