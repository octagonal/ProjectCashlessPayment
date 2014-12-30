using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Models;
using nmct.ba.cashlessproject.web.Models.VM;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            return View(Organisation_RegisterDA.ReadOrganisation_Registers());
        }

        // GET: Organisation_Register/Details/5/1
        public ActionResult Details(int regId, int orgId)
        {
            return View(Organisation_RegisterDA.ReadOrganisation_Register(regId,orgId));
        }

        // GET: Organisation_Register/Create
        public ActionResult Create()
        {
            Organisation_RegisterVM orgreg = new Organisation_RegisterVM();
            //Debug.WriteLine()
            return View(orgreg);
        }

        // POST: Organisation_Register/Create
        [HttpPost]
        public ActionResult Create(Organisation_RegisterVM item)
        {
            try
            {
                // TODO: Add insert logic here
                Organisation_RegisterDA.CreateOrganisation_Register(item.ORInstance);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Organisation_Register/Edit/5
        public ActionResult Edit(int regId, int orgId)
        {
            return View(new Organisation_RegisterVM() { ORInstance = Organisation_RegisterDA.ReadOrganisation_Register(regId, orgId) });
        }

        // POST: Organisation_Register/Edit/5
        [HttpPost]
        public ActionResult Edit(Organisation_RegisterVM item)
        {
            try
            {
                Organisation_RegisterDA.UpdateOrganisation_Register(item.ORInstance, item.NewOrgID, item.NewRegID);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Organisation_Register/Delete/5/4
        [HttpGet]
        public ActionResult Delete(int regId, int orgId)
        {
            return View(Organisation_RegisterDA.ReadOrganisation_Register(regId,orgId));
        }

        // POST: Organisation_Register/Delete/5/4
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteOrgReg(int regId, int orgId)
        {
            try
            {
                Organisation_RegisterDA.DeleteOrganisation_Register(regId, orgId);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
