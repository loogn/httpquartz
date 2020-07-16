﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using HttpQuartz.Client;
using HttpQuartz.Client.Models;

namespace HttpQuartz.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var key = new TriggerKeyModel("test", "test");


            var m = new TriggerModel()
            {
                Key = key,
                StartTime = DateTimeOffset.Now.AddSeconds(30),
                JobData = new JobDataInfo()
                {
                    url = "http://localhost:5000/home/Time",
                },
                SimpleTrigger = new SimpleTriggerInfo()
                {
                    RepeatInterval = TimeSpan.FromSeconds(5),
                },
                CronTrigger = new CronTriggerInfo() { },
                CalendarIntervalTrigger = new CalendarIntervalTriggerInfo() { },
                DailyTimeIntervalTrigger = new DailyTimeIntervalTriggerInfo()
                {
                    DaysOfWeek = new HashSet<DayOfWeek>()
                    {
                        DayOfWeek.Friday
                    },
                    StartTimeOfDay = new TimeOfDayInfo(3, 3, 3),
                    RepeatIntervalUnit = IntervalUnit.Day,
                }
            };
            Console.WriteLine(m.ToJson());

            return;

            var client = new HttpClient {BaseAddress = new Uri("http://localhost:5000")};
            var httpQuartzClient = new HttpQuartzClient(client);


            var result = httpQuartzClient.UnscheduleJob(key).GetAwaiter().GetResult();
            // var result = httpQuartzClient.ScheduleJob(new TriggerModel()
            // {
            //     Key = key,
            //     StartTime = DateTimeOffset.Now.AddSeconds(30),
            //     JobData = new JobDataInfo()
            //     {
            //         url = "http://localhost:5000/home/Time",
            //     },
            //     SimpleTrigger = new SimpleTriggerInfo()
            //     {
            //         RepeatInterval = TimeSpan.FromSeconds(5)
            //     },
            // }).GetAwaiter().GetResult();

            Console.WriteLine(result);
        }
    }
}