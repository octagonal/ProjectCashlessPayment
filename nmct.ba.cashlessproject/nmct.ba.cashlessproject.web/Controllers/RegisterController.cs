using nmct.ba.cashlessproject.model.it;
using nmct.ba.cashlessproject.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace nmct.ba.cashlessproject.web.Controllers
{
	[Authorize]
    public class RegisterController : Controller
    {
        // GET: Register
        public ActionResult Index()
        {
            return View(RegisterDA.ReadRegisters());
        }

        // GET: Register/Details/5
        public ActionResult Details(int id)
        {
            return View(RegisterDA.ReadRegister(id));
        }

        // GET: Register/Create
        public ActionResult Create()
        {
            return View(new Register());
        }

        // POST: Register/Create
        [HttpPost]
        public ActionResult Create(Register item)
        {
            try
            {
                RegisterDA.CreateRegister(item);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Register/Edit/5
        [HttpGet]
        public ActionResult Edit(int id)
        {                
            return View(RegisterDA.ReadRegister(id));
        }

        // POST: Register/Edit/5
        [HttpPost]
        public ActionResult Edit(Register item)
        {
            try
            {
                RegisterDA.UpdateRegister(item);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Register/Delete/5
        public ActionResult Delete(int id)
        {
            return View(RegisterDA.ReadRegister(id));
        }

        // POST: Register/Delete/5
        [HttpPost]
        public ActionResult Delete(Register item)
        {
            try
            {
                RegisterDA.DeleteRegister(item.ID);
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
