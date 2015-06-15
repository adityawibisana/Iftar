using System;
using System.Collections.Generic;
using System.Text;
using Windows.ApplicationModel.Background;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace IftarUniversal.Service
{
    public class ToastNotificationService 
    {
        public bool CreateToast(DateTime toastTime, String toastMessage)
        {
            if (toastTime.Ticks < DateTime.Now.Ticks)
            {
                return false;
            }

            ToastTemplateType toastTemplate = ToastTemplateType.ToastText01;
           
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

            var elements = toastXml.GetElementsByTagName("text");
            elements[0].AppendChild(toastXml.CreateTextNode(toastMessage));

            
            ScheduledToastNotification scheduledToast = new ScheduledToastNotification(toastXml, toastTime);
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(scheduledToast);
            return true;
        }
    }
}
