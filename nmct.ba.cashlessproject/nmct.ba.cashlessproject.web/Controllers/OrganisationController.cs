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
            return View(OrganisationDA.ReadOrganisations());
        }

        // GET: Organisation/Details/5
        public ActionResult Details(int id)
        {
            return View(OrganisationDA.ReadOrganisation(id));
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
                OrganisationDA.CreateOrganisation(item);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Organisation/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {                
            // TODO: Add update logic here

            return View(OrganisationDA.ReadOrganisation(id));
        }

        // POST: Organisation/Edit/5
        [HttpPost]
        public ActionResult Edit(Organisation item)
        {
            try
            {
                OrganisationDA.UpdateOrganisation(item);

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
            return View(OrganisationDA.ReadOrganisation(id));
        }

        // POST: Organisation/Delete/5
        [HttpPost]
        public ActionResult Delete(Organisation item)
        {
            try
            {
                OrganisationDA.DeleteOrganisation(item.ID);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
