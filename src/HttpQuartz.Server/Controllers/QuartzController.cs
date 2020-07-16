using System;
using System.Net;
using System.Threading.Tasks;
using Autowired.Core;
using HttpQuartz.Client.Models;
using HttpQuartz.Server.BgServices.QuartzScheduler;
using HttpQuartz.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Quartz;
using IntervalUnit = Quartz.IntervalUnit;

namespace HttpQuartz.Server.Controllers
{
    /// <summary>
    /// 外部API
    /// </summary>
    [AllowAnonymous]
    public class QuartzController : Controller
    {
        [Autowired] private IScheduler scheduler;

        [Autowired] private ILogger<QuartzController> logger;
        [Autowired] private AppSettings appSettings;

        public QuartzController(AutowiredService autowiredService)
        {
            autowiredService.Autowired(this);
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleJob([FromBody] TriggerModel model)
        {
            try
            {
                //check;
                if (model?.Key == null || model.JobData == null)
                {
                    return Ok("key和jobdata不能为空");
                }

                var key = new TriggerKey(model.Key.Name, model.Key.Group);
                var builder = TriggerBuilder.Create();
                builder.WithIdentity(key);
                if (model.StartTime.HasValue)
                {
                    builder.StartAt(model.StartTime.Value);
                }
                else
                {
                    builder.StartNow();
                }

                builder.EndAt(model.EndTime);
                builder.ForJob(HttpJob.JobKey);
                if (model.Priority.HasValue)
                {
                    builder.WithPriority(model.Priority.Value);
                }

                builder.UsingJobData("method", model.JobData.method);
                builder.UsingJobData("timeout", model.JobData.timeout.ToString());
                builder.UsingJobData("url", model.JobData.url);
                builder.UsingJobData("expRefire", model.JobData.expRefire.ToString());
                builder.UsingJobData("expRemove", model.JobData.expRemove.ToString());

                #region trigger

                if (model.SimpleTrigger != null)
                {
                    builder.WithSimpleSchedule(scheduleBuilder =>
                    {
                        scheduleBuilder
                            .WithRepeatCount(model.SimpleTrigger.RepeatCount)
                            .WithInterval(model.SimpleTrigger.RepeatInterval);
                        switch (model.SimpleTrigger.misfireInstruction)
                        {
                            case -1:
                                scheduleBuilder.WithMisfireHandlingInstructionIgnoreMisfires();
                                break;
                            case 1:
                                scheduleBuilder.WithMisfireHandlingInstructionFireNow();
                                break;
                            case 2:
                                scheduleBuilder.WithMisfireHandlingInstructionNowWithExistingCount();
                                break;
                            case 3:
                                scheduleBuilder.WithMisfireHandlingInstructionNowWithRemainingCount();
                                break;
                            case 4:
                                scheduleBuilder.WithMisfireHandlingInstructionNextWithRemainingCount();
                                break;
                            case 5:
                                scheduleBuilder.WithMisfireHandlingInstructionNextWithExistingCount();
                                break;
                        }
                    });
                }

                if (model.CronTrigger != null)
                {
                    builder.WithCronSchedule(model.CronTrigger.CronExpression, scheduleBuilder =>
                    {
                        switch (model.CronTrigger.MisfireInstruction)
                        {
                            case -1:
                                scheduleBuilder.WithMisfireHandlingInstructionIgnoreMisfires();
                                break;
                            case 1:
                                scheduleBuilder.WithMisfireHandlingInstructionFireAndProceed();
                                break;
                            case 2:
                                scheduleBuilder.WithMisfireHandlingInstructionDoNothing();
                                break;
                        }
                    });
                }

                if (model.CalendarIntervalTrigger != null)
                {
                    builder.WithCalendarIntervalSchedule(scheduleBuilder =>
                    {
                        scheduleBuilder.WithInterval(model.CalendarIntervalTrigger.RepeatInterval,
                            (IntervalUnit) (int) model.CalendarIntervalTrigger.RepeatIntervalUnit);

                        scheduleBuilder.SkipDayIfHourDoesNotExist(model.CalendarIntervalTrigger
                            .SkipDayIfHourDoesNotExist);
                        scheduleBuilder.PreserveHourOfDayAcrossDaylightSavings(model.CalendarIntervalTrigger
                            .PreserveHourOfDayAcrossDaylightSavings);
                        switch (model.CalendarIntervalTrigger.MisfireInstruction)
                        {
                            case -1:
                                scheduleBuilder.WithMisfireHandlingInstructionIgnoreMisfires();
                                break;
                            case 1:
                                scheduleBuilder.WithMisfireHandlingInstructionFireAndProceed();
                                break;
                            case 2:
                                scheduleBuilder.WithMisfireHandlingInstructionDoNothing();
                                break;
                        }
                    });
                }

                if (model.DailyTimeIntervalTrigger != null)
                {
                    builder.WithDailyTimeIntervalSchedule(scheduleBuilder =>
                    {
                        scheduleBuilder.WithInterval(model.DailyTimeIntervalTrigger.RepeatInterval,
                            (IntervalUnit) (int) model.DailyTimeIntervalTrigger.RepeatIntervalUnit);

                        scheduleBuilder.WithRepeatCount(model.DailyTimeIntervalTrigger.RepeatCount);
                        if (model.DailyTimeIntervalTrigger.DaysOfWeek?.Count > 0)
                        {
                            scheduleBuilder.OnDaysOfTheWeek(model.DailyTimeIntervalTrigger.DaysOfWeek);
                        }

                        if (model.DailyTimeIntervalTrigger.StartTimeOfDay != null)
                        {
                            scheduleBuilder.StartingDailyAt(new TimeOfDay(
                                model.DailyTimeIntervalTrigger.StartTimeOfDay.Hour,
                                model.DailyTimeIntervalTrigger.StartTimeOfDay.Minute,
                                model.DailyTimeIntervalTrigger.StartTimeOfDay.Second));
                        }

                        if (model.DailyTimeIntervalTrigger.EndTimeOfDay != null)
                        {
                            scheduleBuilder.EndingDailyAt(new TimeOfDay(
                                model.DailyTimeIntervalTrigger.EndTimeOfDay.Hour,
                                model.DailyTimeIntervalTrigger.EndTimeOfDay.Minute,
                                model.DailyTimeIntervalTrigger.EndTimeOfDay.Second));
                        }

                        switch (model.DailyTimeIntervalTrigger.MisfireInstruction)
                        {
                            case -1:
                                scheduleBuilder.WithMisfireHandlingInstructionIgnoreMisfires();
                                break;
                            case 1:
                                scheduleBuilder.WithMisfireHandlingInstructionFireAndProceed();
                                break;
                            case 2:
                                scheduleBuilder.WithMisfireHandlingInstructionDoNothing();
                                break;
                        }
                    });
                }

                #endregion

                var trigger = builder.Build();


                if (model.Reschedule)
                {
                    var oldTrigger = await scheduler.GetTrigger(key);
                    if (oldTrigger == null)
                    {
                        var result = await scheduler.ScheduleJob(trigger);
                        return Ok("success");
                    }
                    else
                    {
                        var result = await scheduler.RescheduleJob(key, trigger);
                        return Ok(result.HasValue ? "success" : "fail");
                    }
                }
                else
                {
                    var result = await scheduler.ScheduleJob(trigger);
                    return Ok("success");
                }
            }
            catch (Exception e)
            {
                //log?
                await Console.Error.WriteLineAsync(e.StackTrace);
                return Ok(e.Message);
            }
        }

        [HttpPost]
        public async Task<IActionResult> UnscheduleJob([FromBody] TriggerKeyModel model)
        {
            var key = new TriggerKey(model.Name, model.Group);
            await scheduler.UnscheduleJob(key);
            return Ok("success");
        }

        [HttpPost]
        public async Task<IActionResult> PauseTrigger([FromBody] TriggerKeyModel model)
        {
            var key = new TriggerKey(model.Name, model.Group);
            await scheduler.PauseTrigger(key);
            return Ok("success");
        }

        [HttpPost]
        public async Task<IActionResult> ResumeTrigger([FromBody] TriggerKeyModel model)
        {
            var key = new TriggerKey(model.Name, model.Group);
            await scheduler.ResumeTrigger(key);
            return Ok("success");
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var remoteIp = context.HttpContext.Connection.RemoteIpAddress;
            logger.LogDebug(
                "Remote IpAddress: {RemoteIp}", remoteIp);

            var badIp = true;
            foreach (var address in appSettings.SafeClients)
            {
                if (remoteIp.IsIPv4MappedToIPv6)
                {
                    remoteIp = remoteIp.MapToIPv4();
                }

                var testIp = IPAddress.Parse(address);
                if (testIp.Equals(remoteIp))
                {
                    badIp = false;
                    break;
                }
            }

            if (badIp)
            {
                logger.LogWarning(
                    "Forbidden Request from Remote IP address: {RemoteIp}", remoteIp);
                context.Result = new StatusCodeResult(StatusCodes.Status403Forbidden);
                return;
            }

            base.OnActionExecuting(context);
        }
    }
}