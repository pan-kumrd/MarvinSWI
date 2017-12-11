using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Timers;
using Microsoft.Bot.Connector;
using MarvinSWI.Controllers;
using MarvinSWI.Dialogs;
using Microsoft.Bot.Builder.Dialogs;

namespace MarvinSWI
{
    // [BotAuthentication]
    public class MessagesController : ApiController
    {
        BuildReportController buildController;
        ConnectorClient connector;
        Activity activity;
        Timer timer;
        string failedBuildsUri;
        int tickAmount;


        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            if (activity.Type == ActivityTypes.Message)
            {
                /* Creates a dialog stack for the new conversation, adds RootDialog to the stack, and forwards all 
                 *  messages to the dialog stack. */
                await Conversation.SendAsync(activity, () => new RootDialog());
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }


        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        //public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        //{
        //    this.activity = activity;
        //    if (connector == null)
        //    {
        //        connector = new ConnectorClient(new Uri(activity.ServiceUrl));
        //    }

        //    //uri = "http://dev-aus-tc-01.swdev.local/guestAuth/feed.html?buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevTds2Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevTdsV2Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevDbBackport&buildTypeId=web_development_SolarwindsSitecode_DevSitecoreSonerQube&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevCdCiAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevCdDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevCi&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevDeploy&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevSchedulerCi&buildTypeId=webdev_websites_SolarwindsSitecore_Dev_DevSchedulerDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_TdsV1Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_TdsV2Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_V1PublishToCdManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_V2publishToCdManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_CiAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_Cd01deployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_DeployCd02Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cms_CmsCiAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cms_DeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cms_Sc_CiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cms_Sc_CmsDeploymentManual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Dr_CiAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Dr_DeployDr01Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Dr_DeployDr02Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_Scheduler_Ci&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_Scheduler_DeployAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Production_Cd_Scheduler_DrDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaTdsV1Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaTdsV2Manual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaCdCiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_Cd_CdDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaScheduler_QaCdSchedulerCiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_Cd_Sche_DeployAuto&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaCiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_Cms_CmsDeployManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_QaCmsSchedulerCiManual&buildTypeId=webdev_websites_SolarwindsSitecore_Qa_Cms_Scheduler_Deploy&itemsType=builds&buildStatus=failed&userKey=guest";
        //    failedBuildsUri = "C://Temp/builds.xml";
        //    tickAmount = 0;


        //    if (buildController == null)
        //    {
        //        buildController = new BuildReportController();
        //        buildController.RaiseBuildEvent += HandleBuildEvent;
        //    }

        //    if (timer == null)
        //    {
        //        timer = new Timer(20000); // 20 seconds
        //        timer.Elapsed += OnTimerTick;
        //        timer.Start();
        //    }

        //    var response = Request.CreateResponse(HttpStatusCode.OK);
        //    return response;
        //}

        //private void OnTimerTick(object sender, EventArgs e)
        //{
        //    if (tickAmount > 3)
        //    {
        //        failedBuildsUri = "C://Temp/builds2.xml";
        //    }

        //    buildController.GetBuilds(failedBuildsUri);
        //    tickAmount++;
        //}

        //private async void HandleBuildEvent(object sender, BuildEventArgs e)
        //{

        //    Activity reply = activity.CreateReply($"BUILD FAILED: {e.Message}.");
        //    await connector.Conversations.SendToConversationAsync((Activity)reply);

        //}

        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }
            return null;
        }
    }
}
