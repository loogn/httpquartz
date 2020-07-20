using System;
using System.Threading.Tasks;
using Autowired.Core;
using CoreHelper;
using HttpQuartz.Server.BgServices.QuartzScheduler;
using HttpQuartz.Server.Services;
using HttpQuartz.Server.Tools;
using Microsoft.AspNetCore.Mvc;
using Quartz;

namespace HttpQuartz.Server.Controllers
{
    public class TriggerController : MvcController
    {
        [Autowired] private QRTZ_TRIGGERSService service;
        [Autowired] private IScheduler scheduler;

        public TriggerController(AutowiredService autowiredService)
        {
            autowiredService.Autowired(this);
        }

        public IActionResult Index(string name, string group, string state,
            string type, int page = 1)
        {
            int pageSize = 10;
            var plist = service.SelectPage(scheduler.SchedulerName, name, group, state, type, page, pageSize);
            ViewBag.plist = plist.ToStaticPagedList();

            return View();
        }


        public IActionResult Edit()
        {
            var doc = System.IO.File.ReadAllText("triggerDoc.json");
            var tpl = System.IO.File.ReadAllText("triggerTpl.json");
            ViewBag.doc = doc;
            ViewBag.tpl = tpl;
            return View();
        }

        public IActionResult Detail(string name, string group, string type)
        {
            if ("CRON".Equals(type, StringComparison.Ordinal))
            {
                ViewBag.cron = service.SingleCronTrigger(scheduler.SchedulerName, name, group);
            }
            else if ("SIMPLE".Equals(type, StringComparison.Ordinal))
            {
                ViewBag.simple = service.SingleSimpleTrigger(scheduler.SchedulerName, name, group);
            }

            return View();
        }

        public IActionResult Log(DateTime? date)
        {
            if (date == null) date = DateTime.Now;
            var file = $"logs/{date.Value:yyyyMMdd}.log";
            var content = string.Empty;
            if (System.IO.File.Exists(file))
            {
                content = System.IO.File.ReadAllText(file);
            }

            ViewBag.content = content;
            return View();
        }

        public async Task<ResultObject> UnscheduleJob(string name, string group)
        {
            var key = new TriggerKey(name, group);
            var flag = await scheduler.UnscheduleJob(key);
            return new ResultObject(flag);
        }

        public async Task<ResultObject> PauseTrigger(string name, string group)
        {
            var key = new TriggerKey(name, group);
            await scheduler.PauseTrigger(key);
            return new ResultObject(true);
        }

        public async Task<ResultObject> ResumeTrigger(string name, string group)
        {
            var key = new TriggerKey(name, group);
            await scheduler.ResumeTrigger(key);
            return new ResultObject(true);
        }
        public async Task<ResultObject> TriggerJob(string name, string group)
        {
            var key = new TriggerKey(name, group);
            var trigger = await scheduler.GetTrigger(key);
            await scheduler.TriggerJob(HttpJob.JobKey, trigger.JobDataMap);
            return new ResultObject(true);
        }
    }
}