using System;
using System.Web.Mvc;

using Nest.Issue55.Web.Models;
using Nest.Issue55.Web.Services;

namespace Nest.Issue55.Web.Controllers
{
    public class HomeController : Controller
    {
        protected readonly ElasticSearchService Search;

        public HomeController()
        {
            this.Search = new ElasticSearchService();
        }

        [HttpGet]
        public ActionResult Index()
        {
            var result = this.Search.PerformSearch();
            return View( result );
        }

        [HttpPost]
        public ActionResult AddProduct()
        {
            var p = new ElasticSearchProduct
            {
                Id = Guid.NewGuid(),
                Format = "E-book"
            };
            this.Search.IndexProduct( p );

            this.TempData.Add( "Post", "Product added." );
            return Redirect( "index" );
        }
    }
}