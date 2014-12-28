using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.Controllers
{
    public class OrganisationController : Controller
    {
        // GET: Organisation
        public ActionResult Index()
        {
            List<Organisation> orgs = OrganisationDA.GetOrganisations();
            return View(orgs);
        }

        // GET: Organisation/Details/5
        public ActionResult Details(int id)
        {
            return View(OrganisationDA.GetOrganisation(id));
        }

        // GET: Organisation/Create
        public ActionResult Create()
        {
            return View(new Organisation());
        }

        // POST: Organisation/Create
        [HttpPost]
        public ActionResult Create(Organisation item)
        {
            try
            {
                // TODO: Add insert logic here

                OrganisationDA.InsertOrganisation(item);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Organisation/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Organisation/Edit/5
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

        // GET: Organisation/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Organisation/Delete/5
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
