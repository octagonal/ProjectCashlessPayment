using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.Controllers
{
    public class Organisation_RegisterController : Controller
    {
        // GET: Organisation_Register
        public ActionResult Index()
        {
            return View();
        }

        // GET: Organisation_Register/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Organisation_Register/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Organisation_Register/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Organisation_Register/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Organisation_Register/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Organisation_Register/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Organisation_Register/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
