using System;
using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;
using Autowired.Core;
using CoreHelper;
using HttpQuartz.Client.Models;
using HttpQuartz.Server.BgServices.QuartzScheduler;
using HttpQuartz.Server.Services;
using HttpQuartz.Server.Tools;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using Quartz.Impl.Triggers;
using Quartz.Util;
using IntervalUnit = HttpQuartz.Client.Models.IntervalUnit;

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

        public async Task<IActionResult> Index(string name, string group, string state,
            string type, int page = 1)
        {
            int pageSize = 10;
            var plist = service.SelectPage(scheduler.SchedulerName, name, group, state, type, page, pageSize);
            ViewBag.plist = plist.ToStaticPagedList();
            ViewBag.groups = await scheduler.GetTriggerGroupNames();
            return View();
        }


        public async Task<IActionResult> Edit(string name, string group, string type)
        {
            var doc = System.IO.File.ReadAllText("triggerDoc.json");
            ViewBag.doc = doc;

            if (string.IsNullOrEmpty(name))
            {
                var tpl = System.IO.File.ReadAllText("triggerTpl.json");
                ViewBag.tpl = tpl;
            }
            else
            {
                var key = new TriggerKey(name, group);
                var trigger = await scheduler.GetTrigger(key);

                TriggerModel model = new TriggerModel();
                model.Key = new TriggerKeyModel(name, group);
                model.Description = trigger.Description;
                model.Reschedule = true;
                model.Priority = trigger.Priority;
                model.StartTime = trigger.StartTimeUtc.ToLocalTime();
                model.EndTime = trigger.EndTimeUtc?.ToLocalTime();
                model.JobData = new JobDataInfo()
                {
                    body = trigger.JobDataMap.TryGetAndReturn("body")?.ToString(),
                    url = trigger.JobDataMap.GetString("url"),
                    method = trigger.JobDataMap.GetString("method"),
                    timeout = trigger.JobDataMap.GetIntValueFromString("timeout"),
                    expRefire = trigger.JobDataMap.GetBooleanValueFromString("expRefire"),
                    expRemove = trigger.JobDataMap.GetBooleanValueFromString("expRemove"),
                };

                if ("CRON".Equals(type, StringComparison.Ordinal))
                {
                    if (trigger is CronTriggerImpl t)
                    {
                        model.CronTrigger = new CronTriggerInfo()
                        {
                            TimeZoneId = t.TimeZone.Id,
                            CronExpression = t.CronExpressionString,
                            MisfireInstruction = t.MisfireInstruction
                        };
                    }
                }
                else if ("SIMPLE".Equals(type, StringComparison.Ordinal))
                {
                    if (trigger is SimpleTriggerImpl t)
                    {
                        model.SimpleTrigger = new SimpleTriggerInfo()
                        {
                            RepeatCount = t.RepeatCount,
                            RepeatInterval = t.RepeatInterval,
                            misfireInstruction = t.MisfireInstruction
                        };
                    }
                }
                else if ("DAILY_I".Equals(type, StringComparison.Ordinal))
                {
                    if (trigger is DailyTimeIntervalTriggerImpl t)
                    {
                        model.DailyTimeIntervalTrigger = new DailyTimeIntervalTriggerInfo()
                        {
                            RepeatCount = t.RepeatCount,
                            RepeatInterval = t.RepeatInterval,
                            RepeatIntervalUnit = (IntervalUnit) (int) t.RepeatIntervalUnit,
                            DaysOfWeek = new HashSet<DayOfWeek>(t.DaysOfWeek),
                            MisfireInstruction = t.MisfireInstruction,
                            TimeZoneId = t.TimeZone.Id
                        };
                        if (t.StartTimeOfDay != null)
                        {
                            model.DailyTimeIntervalTrigger.StartTimeOfDay = new TimeOfDayInfo(t.StartTimeOfDay.Hour,
                                t.StartTimeOfDay.Minute, t.StartTimeOfDay.Second);
                        }

                        if (t.EndTimeOfDay != null)
                        {
                            model.DailyTimeIntervalTrigger.EndTimeOfDay = new TimeOfDayInfo(t.EndTimeOfDay.Hour,
                                t.EndTimeOfDay.Minute, t.EndTimeOfDay.Second);
                        }
                    }
                }
                else if ("CAL_INT".Equals(type, StringComparison.Ordinal))
                {
                    if (trigger is CalendarIntervalTriggerImpl t)
                    {
                        model.CalendarIntervalTrigger = new CalendarIntervalTriggerInfo()
                        {
                            RepeatInterval = t.RepeatInterval,
                            RepeatIntervalUnit = (IntervalUnit) (int) t.RepeatIntervalUnit,
                            MisfireInstruction = t.MisfireInstruction,
                            TimeZoneId = t.TimeZone.Id,
                            PreserveHourOfDayAcrossDaylightSavings = t.PreserveHourOfDayAcrossDaylightSavings,
                            SkipDayIfHourDoesNotExist = t.SkipDayIfHourDoesNotExist
                        };
                    }
                }

                var tpl = JsonSerializer.Serialize(model, new JsonSerializerOptions()
                {
                    IgnoreNullValues = true,
                    WriteIndented = true,
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All)
                });

                ViewBag.tpl = tpl;
            }

            return View();
        }

        public IActionResult Detail(string name, string group, string type)
        {
            ViewBag.type = type;
            if ("CRON".Equals(type, StringComparison.Ordinal))
            {
                ViewBag.cron = service.SingleCronTrigger(scheduler.SchedulerName, name, group);
            }
            else if ("SIMPLE".Equals(type, StringComparison.Ordinal))
            {
                ViewBag.simple = service.SingleSimpleTrigger(scheduler.SchedulerName, name, group);
            }
            else
            {
                ViewBag.prop = service.SinglePropTrigger(scheduler.SchedulerName, name, group);
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

        [HttpPost]
        public async Task<IActionResult> ScheduleJob([FromBody] TriggerModel model)
        {
            return await QuartzController.ScheduleJob(this, scheduler, service, model);
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