using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace todolist.src
{
    class AlarmManager
    {

        public static void addAlarm(DateTime dateTime, string title, string content)
        {
            var xmlString = @"<toast launch='args' scenario='alarm'>
                                    <visual>
                                        <binding template='ToastGeneric'>
                                            <text>"+ title +@"</text>
                                            <text>"+ content +@"</text>
                                        </binding>
                                    </visual>
                                    <actions>
                                        <action arguments='snooze' content='snooze' />
                                        <action arguments='dismiss' content='dismiss' />
                                </actions>
                            </toast>";
            var doc = new XmlDocument();
            doc.LoadXml(xmlString);

            var toast = new ScheduledToastNotification(doc, dateTime);
            ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast);
        }

    }
}
