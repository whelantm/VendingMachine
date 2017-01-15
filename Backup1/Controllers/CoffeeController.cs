using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VendingMachineUI.Controllers
{
	public class CoffeeController : Controller
	{
		[HttpGet]
		public	ActionResult GetCoffee()
		{
			Coffee coffee = new Coffee();
			coffee.Price = 2.25;
			return Json(coffee, JsonRequestBehavior.AllowGet);
		}
	}
}