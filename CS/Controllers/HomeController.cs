using DevExpress.Web.Mvc;
using Models;
using System.Web.Mvc;

namespace GridUpdateComboMvc
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [ValidateInput(false)]
        public ActionResult GridViewPartial()
        {
            return PartialView("_GridViewPartial", BatchEditRepository.GridData);
        }

        public ActionResult GridViewCustomAction(string parameters)
        {
            ViewData["data"] = parameters;
            return GridViewPartial();
        }

    }
}