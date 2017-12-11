using System;
using System.Linq;
using System.Collections.Generic;
using System.Timers;
using SimpleFeedReader;
using System.Threading.Tasks;

namespace MarvinSWI.Controllers
{ 
        // Define a class to hold custom event info
     class BuildEventArgs : EventArgs
    {
        public BuildEventArgs(string s)
        {
            message = s;
        }
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
    }

    class BuildReportController
    {
        public event EventHandler<BuildEventArgs> RaiseBuildEvent;

        FeedReader reader;
        IEnumerable<FeedItem> oldItems;
        string uri;
        Timer timer;
        int tickAmount;
     
        public BuildReportController()
        {
            timer = new Timer(20000); // 60 s in miliseconds
            timer.Elapsed += OnTimerTick;
            reader = new FeedReader();
            //uri = "http://dev-aus-tc-01.swdev.local/guestAuth/feed.html?buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevTds2Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevTdsV2Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevDbBackport&buildTypeId=web_development_SolarwindsSitecode_DevSitecoreSonerQube&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevCdCiAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevCdDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevCi&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevDeploy&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevSchedulerCi&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevSchedulerDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_TdsV1Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_TdsV2Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_V1PublishToCdManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_V2publishToCdManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_CiAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_Cd01deployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_DeployCd02Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cms_CmsCiAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cms_DeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cms_Sc_CiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cms_Sc_CmsDeploymentManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Dr_CiAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Dr_DeployDr01Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Dr_DeployDr02Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_Scheduler_Ci&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_Scheduler_DeployAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_Scheduler_DrDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaTdsV1Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaTdsV2Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaCdCiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_Cd_CdDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaScheduler_QaCdSchedulerCiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_Cd_Sche_DeployAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaCiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_Cms_CmsDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaCmsSchedulerCiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_Cms_Scheduler_Deploy&itemsType=builds&buildStatus=failed&userKey=guest";
            uri = "C://Temp/builds.xml";
        }

        public void StartGettingBuilds()
        {
            tickAmount = 0;
            timer.Start();
            Console.WriteLine("Timer has started");
        }

        protected async void OnTimerTick(object sender, EventArgs e)
        {
            if (tickAmount > 3)
            {
                uri = "C://Temp/builds2.xml";
            }
            Console.WriteLine("Timer Tick");
            var items = await Task.Run(() => reader.RetrieveFeed(uri));

            if (oldItems != null)
            {
                var newFails = from newItem in items
                               where !(from oldItem in oldItems
                                       select oldItem.Title)
                                       .Contains(newItem.Title)
                               select newItem;

                foreach (var i in newFails)
                {
                    var newBuildEvent = new BuildEventArgs("hue");
                    OnRaiseBuildEvent(newBuildEvent, i.Title);
                }
            }

            oldItems = items;
            tickAmount++;
        }

        protected virtual void OnRaiseBuildEvent(BuildEventArgs e, string buildMsg)
        {
            EventHandler<BuildEventArgs> handler = RaiseBuildEvent;

            if (handler != null)
            {
                e.Message = buildMsg;
                handler(this, e);
            }
        }
    }
}