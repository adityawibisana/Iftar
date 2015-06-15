using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Background;

namespace IftarUniversal.Service
{
    public class BackgroundTaskService : IBackgroundTask
    {
        #region Field 
        private static readonly string BACKGROUND_TASK_ENTRY_POINT_NAME = "Tasks.IftarBackgroundTask";
        public static readonly string TimeTriggeredTaskName = "TimeTriggeredTask";
        private PrayTime _prayTime;
        private AppSettingService _appSettingService;


        #endregion

        public BackgroundTaskService(PrayTime prayTime, AppSettingService appSettingService)
        {
            this._prayTime = prayTime;
            this._appSettingService = appSettingService; 
        }

        public void Run(IBackgroundTaskInstance taskInstance)
        { 

        }

        public async void Create()
        {
            foreach (var task in Windows.ApplicationModel.Background.BackgroundTaskRegistration.AllTasks)
            {
                if (task.Value.Name == BACKGROUND_TASK_ENTRY_POINT_NAME)
                { 
                    return;
                }
            }

            if (TaskRequiresBackgroundAccess(BACKGROUND_TASK_ENTRY_POINT_NAME))
            {
                await BackgroundExecutionManager.RequestAccessAsync();
            }

            BackgroundTaskBuilder builder = new BackgroundTaskBuilder();
            builder.Name = BACKGROUND_TASK_ENTRY_POINT_NAME;
            builder.TaskEntryPoint = BACKGROUND_TASK_ENTRY_POINT_NAME;
            builder.SetTrigger(new TimeTrigger(60 * 24, true)); //a task that will run every 24 hour

            BackgroundTaskRegistration taskReg = builder.Register();
            taskReg.Completed += TaskReg_Completed; 
        } 

        private void TaskReg_Completed(BackgroundTaskRegistration sender, BackgroundTaskCompletedEventArgs args)
        { 
            CreateToast(DateTime.Now);
        }

        private void CreateToast(DateTime date)
        { 
            var x = _prayTime.getPrayerTimes(date.Year, date.Month, date.Day,
                _appSettingService.UserLatitude, _appSettingService.UserLongitude,
                TimeZoneInfo.Local.BaseUtcOffset.Hours);

            DateTime date2 = date.AddDays(1);
            var x2 = _prayTime.getPrayerTimes(date2.Year, date2.Month, date2.Day,
                _appSettingService.UserLatitude, _appSettingService.UserLongitude,
                TimeZoneInfo.Local.BaseUtcOffset.Hours);

            DateTime fajrTime = new DateTime(date.Year, date.Month, date.Day, MicroTimeConvert(x[0])[0], MicroTimeConvert(x[0])[1], 0);
            if (fajrTime.Ticks < DateTime.Now.Ticks)
            {
                //tomorrow
                fajrTime = new DateTime(date.Year, date.Month, date.Day, MicroTimeConvert(x2[0])[0], MicroTimeConvert(x2[0])[1], 0);
            }

            DateTime maghribTime = new DateTime(date.Year, date.Month, date.Day, MicroTimeConvert(x[5])[0], MicroTimeConvert(x[5])[1], 0);
            if (maghribTime.Ticks < DateTime.Now.Ticks)
            {
                //tomorrow
                maghribTime = new DateTime(date.Year, date.Month, date.Day, MicroTimeConvert(x2[5])[0], MicroTimeConvert(x2[5])[1], 0);
            }

            ToastNotificationService toast = new ToastNotificationService();
            toast.CreateToast(fajrTime.AddMinutes(-20), "20 minutes before Fajr"); 
            toast.CreateToast(maghribTime, "Iftar Time");
        }

        

        private int[] MicroTimeConvert(String time)
        {
            String[] t = time.Split(':');
            return new int[] { int.Parse(t[0]), int.Parse(t[1]) };
        }

        /// <summary>
        /// Determine if task with given name requires background access.
        /// </summary>
        /// <param name="name">Name of background task to query background access requirement.</param>
        public static bool TaskRequiresBackgroundAccess(String name)
        {
#if WINDOWS_PHONE_APP
            return true;
#else
            if (name == TimeTriggeredTaskName)
            {
                return true;
            }
            else
            {
                return false;
            }
#endif
        }
    }
}
