using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Models;
using nmct.ba.cashlessproject.web.Models.VM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.Controllers
{
	[Authorize]
	public class ErrorlogController : Controller
	{
		// GET: Errorlog
		public ActionResult Index()
		{
			return View(Organisation_RegisterVM.FillVMList());
		}

		// GET: Errorlog/Details/5
		public ActionResult Details(int id)
		{
			return View(ErrorlogDA.ReadErrorlogs(id));
		}

		// GET: Errorlog/Create
		public ActionResult Create()
		{
			return View(new Errorlog());
		}

		// POST: Errorlog/Create
		[HttpPost]
		public ActionResult Create(Errorlog item)
		{
			try
			{
				ErrorlogDA.CreateErrorlog(item);
				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}

		// GET: Errorlog/Edit/5
		[HttpGet]
		public ActionResult Edit(int id)
		{
			return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
		}

		// POST: Errorlog/Edit/5
		[HttpPost]
		public ActionResult Edit(Errorlog item)
		{
			return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
		}

		// GET: Errorlog/Delete/5
		public ActionResult Delete(int id)
		{
			return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
		}

		// POST: Errorlog/Delete/5
		[HttpPost]
		public ActionResult Delete(Errorlog item)
		{
			return new HttpStatusCodeResult(HttpStatusCode.MethodNotAllowed);
		}
	}
}
