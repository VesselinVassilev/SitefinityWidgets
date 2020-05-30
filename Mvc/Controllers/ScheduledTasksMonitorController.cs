using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Telerik.Sitefinity.Mvc;
using Telerik.Sitefinity.Mvc.ActionFilters;
using Telerik.Sitefinity.Scheduling;
using Telerik.Sitefinity.Scheduling.Model;
using Telerik.Sitefinity.Data;

namespace SitefinityWebApp.Mvc.Controllers
{
    [ControllerToolboxItem(Name = "ScheduledTasksMonitor", Title = "Tasks Monitor", SectionName = "Admin Widgets", CssClass = "sfMvcIcn")]
    public class ScheduledTasksMonitorController : Controller
    {
        public string FilterExpression { get; set; }

        public string OrderExpression { get; set; } = "ExecuteTime ASC";

        public int RefreshIntervalMs { get; set; } = 5000;

        [HttpGet]
        public ActionResult Index()
        {            
            ViewBag.RefreshIntervalMs = RefreshIntervalMs;

            return View("TaskMonitor");
        }

        [HttpGet]
        [StandaloneResponseFilter]
        public ActionResult Progress()
        {
            List<ScheduledTaskData> tasks = GetScheduledTasks();

            return Json(tasks, JsonRequestBehavior.AllowGet);
        }

        private List<ScheduledTaskData> GetScheduledTasks()
        {
            var manager = SchedulingManager.GetManager();

            // we don't need total count
            int? count = null;

            var tasks = DataProviderBase.SetExpressions(manager.GetTaskData(),
                                                        FilterExpression,
                                                        OrderExpression,
                                                        null,
                                                        null,
                                                        ref count);
                               
            return tasks.ToList();
        }
    }
}