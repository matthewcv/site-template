using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Ninject;
using mywebsite.backend;
using mywebsite.backend.Entity;
using mywebsite.backend.Service;

namespace mywebsite.Infrastructure
{

    public abstract class PageBase<T> : System.Web.Mvc.WebViewPage<T>
    {
        [Inject]
        public IAuthenticationService AuthService { get; set; }
        public Profile CurrentProfile 
        { 
            get
            {
                return AuthService.CurrentProfile;   
            }
        }


    }
}