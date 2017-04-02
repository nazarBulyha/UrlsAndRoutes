using System.Web.Mvc;

namespace UrlsAndRoutes.Controllers
{
    [RoutePrefix("Users")]
    public class CustomerController : Controller
    {
        public ActionResult List()
        {
            ViewBag.Controller = "Customer";
            ViewBag.Action = "List";
            return View("ActionName");
        }

        [Route("~/Test")]
        public ActionResult Index()
        {
            ViewBag.Controller = "Customer";
            ViewBag.Action = "Index";
            return View("ActionName");
        }

        [Route("Add/{user}/{id:int}", Name = "AddRoute")]
        public string Create(string user, int id)
        {
            return $"Пользователь: {user}, Id: {id}";
        }

        [Route("Add/{user}/{password:alpha:maxlength(6)}")]
        public string ChangePass(string user, string password)
        {
            return $"Метод ChangePass() - Користувач: <em>{user}</em>, Пароль: <em>{password}</em>";
        }
    }
}