using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Bot.Connector;
using MarvinSWI.Controllers;

namespace MarvinSWI
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        BuildReportController buildController;
        ConnectorClient connector;
        Activity activity;

        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            this.activity = activity;
            connector = new ConnectorClient(new Uri(activity.ServiceUrl));

            buildController = new BuildReportController();
            buildController.RaiseBuildEvent += HandleBuildEvent;
            buildController.StartGettingBuilds();

            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async void HandleBuildEvent(object sender, BuildEventArgs e)
        {
            Activity reply = activity.CreateReply($"BUILD FAILED: {e.Message}.");
            await connector.Conversations.SendToConversationAsync((Activity)reply);
        }

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
