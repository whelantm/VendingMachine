using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VendingMachineUI.Controllers
{
    public class SugarController : Controller
    {
        [HttpGet]
		public ActionResult GetSugar()
		{
			Sugar sugar = new Sugar();
			sugar.Price = 0.10;
			return Json(sugar, JsonRequestBehavior.AllowGet);
		}
    }
}
