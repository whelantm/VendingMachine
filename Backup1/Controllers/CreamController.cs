using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VendingMachineUI.Controllers
{
    public class CreamController : Controller
    {
        [HttpGet]
		public ActionResult GetCream()
		{
			Cream cream = new Cream();
			cream.Price = 0.15;
			return Json(cream, JsonRequestBehavior.AllowGet);
		}
    }
}
