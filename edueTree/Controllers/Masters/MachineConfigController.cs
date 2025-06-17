using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using edueTree.Models;

namespace edueTree.Controllers.Masters
{
    public class MachineConfigController : BaseController
    {
        #region -------- Dbcontext ----------

        public readonly dbContainer Db = new dbContainer();
      
        #endregion
        
        #region ----------- Index -----------
        public ActionResult Index()
        {
            var firmId = LoginUserFirmId();
            var data = Db.MachineConfigures.Where(a => a.firmId == firmId && a.Isdeleted==false ).ToList();
            return View(data);
        }

        #endregion 

        #region ---------- Details ----------
      
        public ActionResult Details(int id)
        {
            return View();
        }

        #endregion 

        #region ----------- Create ----------
       
        public ActionResult Create()
        {
            return View();
        }
      
        [HttpPost]
        public ActionResult Create(ConfigMachine machineConfigModel1)
        {
            var firmId = LoginUserFirmId();
            try
            {
                if (ModelState.IsValid)
                {
                    var data = Db.MachineConfigures.FirstOrDefault(a => a.SerialKey == machineConfigModel1.SerialKey && a.Isdeleted == false && a.firmId == firmId);
                    if (data != null)
                    {
                        TempData["notice"] = "exist";
                        return RedirectToAction("Create");
                    }
                    var machineConfigure = new MachineConfigure
                    {
                        IPAddress = machineConfigModel1.IPAddress,
                        Port = machineConfigModel1.Port,
                        SerialKey = machineConfigModel1.SerialKey,
                        firmId = firmId,
                        Isdeleted = false
                    };
                    Db.MachineConfigures.Add(machineConfigure);
                    Db.SaveChanges();
                    TempData["notice"] = "success";
                }
                return RedirectToAction("Create");
            }
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("Create");
            }
        }

        #endregion 

        #region ----------- Edit ------------
        
        public ActionResult Edit(int id)
        {
            var machineConfigModel = new ConfigMachine();
            var data = Db.MachineConfigures.FirstOrDefault(a => a.id == id);
            if (data != null)
            {
                machineConfigModel.IPAddress = data.IPAddress;
                machineConfigModel.Port = data.Port;
                machineConfigModel.id = data.id;
                machineConfigModel.SerialKey = data.SerialKey;

            }
            return View(machineConfigModel);
        }
        
        [HttpPost]
        public ActionResult Edit(ConfigMachine machineConfigModel1)
        {
            
            try
            {
                var data = Db.MachineConfigures.FirstOrDefault(a => a.id == machineConfigModel1.id);
                if (data != null)
                {
                    data.IPAddress = machineConfigModel1.IPAddress;
                    data.Port = machineConfigModel1.Port;
                    data.SerialKey = machineConfigModel1.SerialKey;
                    Db.Entry(data).State = EntityState.Modified;
                    Db.SaveChanges();
                    TempData["notice"] = "update";
                }
             
                return RedirectToAction("Create");
            }
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("Create");
            }
        }

        #endregion 

        #region ----------  Delete ----------
        //
        // GET: /MachineConfig/Delete/5
        public ActionResult Delete(int id)
        {
            var machineConfigModel1 = new ConfigMachine { id = id };
            return View(machineConfigModel1);
        }

        //
        // POST: /MachineConfig/Delete/5
        [HttpPost]
        public ActionResult DeleteConfirm(ConfigMachine machineConfigModel1)
        {
            try
            {
                var machineConfigure = Db.MachineConfigures.FirstOrDefault(a => a.id == machineConfigModel1.id);
                if (machineConfigure != null)
                {
                    machineConfigure.Isdeleted = true;
                    Db.Entry(machineConfigure).State = EntityState.Modified;
                    Db.SaveChanges();
                    TempData["notice"] = "delete";
                    return RedirectToAction("Create");
                }

                TempData["notice"] = "error";
                return RedirectToAction("Create");
            }
            catch
            {
                TempData["notice"] = "error";
                return RedirectToAction("Create");
            }
        } 
        #endregion 
    }
}
