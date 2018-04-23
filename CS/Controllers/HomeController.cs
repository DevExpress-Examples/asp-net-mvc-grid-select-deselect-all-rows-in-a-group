using System.Web.Mvc;

namespace GridViewGroupSelectionMvc {
    public class HomeController : Controller {
        public ActionResult Index() {
            return View();
        }

        public ActionResult GridViewPartial() {
            return PartialView(NorthwindDataProvider.GetProducts());
        }

        public ActionResult GridViewCustomActionPartial(string parameters) {
            ViewData["parameters"] = parameters;
            return PartialView("GridViewPartial", NorthwindDataProvider.GetProducts());
        }
    }
}