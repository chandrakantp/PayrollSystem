using System.Web.Mvc;

namespace edueTree.Controllers
{
    public class ShiftController : Controller
    {
        //
        // GET: /Shift/
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Shift/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Shift/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Shift/Create
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

        //
        // GET: /Shift/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Shift/Edit/5
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

        //
        // GET: /Shift/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Shift/Delete/5
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
