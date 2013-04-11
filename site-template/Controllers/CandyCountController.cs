using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using mywebsite.backend.Service;

namespace mywebsite.Controllers
{
    [Authorize]
    public class CandyCountController : Controller
    {
        private readonly ICandyService _candyService;

        public CandyCountController(ICandyService candyService)
        {
            _candyService = candyService;
        }

        //
        // GET: /CandyCount/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Add()
        {
            return View("Index");
        }

        public ActionResult Stuff()
        {
            return View("Index");
        }
    }
}
