using HSBM.Common.Enums;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Core;
using PushSharp.Google;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HSBM.Common.Utils
{
    public class PushDeviceNotification
    {
        #region Variables
        string _GoogleApiKey = string.Empty;
        string _AppleCerName = string.Empty;
        string _AppleCerPass = string.Empty;
        bool _IsAppleProduction = true;
        #endregion

        public void InitDevicePushService(long UserTypeId)
        {

            //_IsAppleProduction = Convert.ToBoolean(ConfigurationManager.AppSettings["IsAppleProduction"].ToString());

            if (UserTypeId == Convert.ToInt32(UserTypes.Customer))
            {
                if (ConfigurationManager.AppSettings["CustomerGoogleApiKey"] != null)
                {
                    _GoogleApiKey = ConfigurationManager.AppSettings["CustomerGoogleApiKey"];
                }
            }

            //if (UserTypeId == Convert.ToInt32(UserTypes.Customer))
            //{
            //    if (ConfigurationManager.AppSettings["AppleCustomerCerName"] != null)
            //    {
            //        _AppleCerName = ConfigurationManager.AppSettings["AppleCustomerCerName"];
            //    }
            //    if (ConfigurationManager.AppSettings["AppleCustomerCerPass"] != null)
            //    {
            //        _AppleCerPass = ConfigurationManager.AppSettings["AppleCustomerCerPass"];
            //    }
            //}
        }

        public void SendDeviceNotification(List<DeviceNotificationData> p_DevicesData, string Message, int Type, int ActionType, string Title)
        {
            List<DeviceNotificationData> _AndroidDevices = p_DevicesData.Where(x => x.Provider != null && x.Provider.ToUpper() == "ANDROID" && x.DeviceId != "" && x.GCMRegistrationId != "").ToList();

            //List<DeviceNotificationData> _AppleDevices = p_DevicesData.Where(x => x.Provider != null && x.Provider.ToUpper() == "APPLE" && x.DeviceId != "" && x.GCMRegistrationId != "").ToList();

            if (_AndroidDevices != null && _AndroidDevices.Count > 0)
            {
                SendToAndroid(_AndroidDevices, Message, Type, ActionType, Title);
            }

            //if (_AppleDevices != null && _AppleDevices.Count > 0)
            //{
            //    SendToApple(_AppleDevices, Message, Type, ActionType, Title);
            //}
        }

        private void SendToApple(List<DeviceNotificationData> p_DeviceData, string Message, int Type, int ActionType, string Title)
        {
            var config = new ApnsConfiguration(_IsAppleProduction ? ApnsConfiguration.ApnsServerEnvironment.Production : ApnsConfiguration.ApnsServerEnvironment.Sandbox, System.Web.HttpContext.Current.Server.MapPath(_AppleCerName), _AppleCerPass);

            // Create a new broker
            var broker = new ApnsServiceBroker(config);

            // Wire up events
            broker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    if (ex is ApnsNotificationException)
                    {
                        var apnsEx = ex as ApnsNotificationException;
                    }
                    else if (ex is ApnsConnectionException)
                    {
                    }
                    else
                    {
                        //Console.WriteLine("Notification Failed (Unknown Reason)!");
                    }
                    return true;
                });
            };

            broker.OnNotificationSucceeded += (notification) =>
            {
                //Console.WriteLine("Notification Sent!");
            };

            // Start the broker
            broker.Start();

            // Queue a notification to send

            //p_DeviceData = new List<DeviceNotificationData>();


            var jobject = JObject.FromObject(new
            {
                aps = new
                {
                    alert = Message,
                    badge = 1,
                    sound = "default"
                },
                data = new
                {
                    NotificationType = Type,
                    ActionType = ActionType,
                    Title = Title
                }
            });

            //p_DeviceData.Add(new DeviceNotificationData() { DeviceId = "111", GCMRegistrationId = "F93C2C9044057BDFE2D8BB8FABCD6375FFCD66F146CA8639C9D5610FF008434B" });

            string _JsonResult = "{\"Message\":\"" + Message + "\",\"NotificationType\":" + Type + ",\"ActionType\":" + ActionType + ",\"Title\":\"" + Title + "\"}";

            foreach (DeviceNotificationData item in p_DeviceData)
            {
                if (!string.IsNullOrEmpty(item.GCMRegistrationId) && item.GCMRegistrationId != "1234")
                {
                    broker.QueueNotification(new ApnsNotification
                    {
                        DeviceToken = item.GCMRegistrationId,//"562A17FB6DD9A5993094A6D157DC8E6FCF15E9CAE73FF80FD8D9BD02F1EB6664",
                        //Payload = JObject.Parse("{\"aps\":{\"badge\":7}}")
                        //Payload = JObject.Parse("{'aps':{'alert':'" + Message + "','badge':1,'sound':'sound.caf','content-available':1},'data': " + _JsonResult + "}")
                        Payload = jobject
                    });
                }

            }


            broker.Stop();
        }

        private void SendToAndroid(List<DeviceNotificationData> p_DeviceData, string Message, int Type, int ActionType, string Title)
        {
            // Configuration
            var config = new GcmConfiguration(_GoogleApiKey);

            // Create a new broker
            var gcmBroker = new GcmServiceBroker(config);

            // Wire up events
            gcmBroker.OnNotificationFailed += (notification, aggregateEx) =>
            {
                aggregateEx.Handle(ex =>
                {
                    if (ex is GcmNotificationException)
                    {
                        var notificationException = (GcmNotificationException)ex;
                    }
                    else if (ex is GcmMulticastResultException)
                    {
                        var multicastException = (GcmMulticastResultException)ex;
                    }
                    else if (ex is DeviceSubscriptionExpiredException)
                    {
                        var expiredException = (DeviceSubscriptionExpiredException)ex;
                    }
                    else if (ex is RetryAfterException)
                    {
                        var retryException = (RetryAfterException)ex;
                    }
                    else
                    {
                        //Console.WriteLine("GCM Notification Failed for some unknown reason");
                    }

                    return true;
                });
            };

            gcmBroker.OnNotificationSucceeded += (notification) =>
            {
                Console.WriteLine("GCM Notification Sent!");
            };

            // Start the broker
            gcmBroker.Start();

            //foreach (var regId in MY_REGISTRATION_IDS)
            //{
            // Queue a notification to send

            List<string> _DeviceIds = p_DeviceData.Select(x => x.GCMRegistrationId).ToList();

            string _JsonResult = "{\"Message\":\"" + Message + "\",\"NotificationType\":" + Type + ",\"ActionType\":" + ActionType + ",\"Title\":\"" + Title + "\"}";

            gcmBroker.QueueNotification(new GcmNotification
            {
                //RegistrationIds = new List<string> { "dU_pQ9lubQI:APA91bHfZsqL2a1qAXbKQMH6zlawG6phJg7czdFq7Z45lkI5ElHPFjYciSG4swmQQ2vCTqC9N6iLMHJRiPM_CUzk5LfJmb1UEvRNdSeXfnyh_H-RvAJWIloMCe4O3VomHWUYiDvYSDZR" },
                RegistrationIds = _DeviceIds,
                //Data = JObject.Parse("{ \"somekey\" : \"somevalue\" }")
                Data = JObject.Parse(_JsonResult)
            });
            //}

            gcmBroker.Stop();
        }
    }
}
