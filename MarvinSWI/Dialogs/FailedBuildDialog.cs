using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MarvinSWI.Controllers;

namespace MarvinSWI.Dialogs
{
    [Serializable]
    public class FailedBuildDialog : IDialog
    {
        IDialogContext _context;
        BuildReportController buildController;

        public async Task StartAsync(IDialogContext context)
        {
            _context = context;
            await _context.PostAsync("Starting FailedBuildDialog");
            //buildController = new BuildReportController();
            //buildController.StartGettingBuilds(HandleBuildEvent);
        }

        private async void HandleBuildEvent(object sender, BuildEventArgs e)
        {
           await _context.PostAsync($"BUILD FAILED: {e.Message}");
        }

        //private async Task PostFailedBuildsAsync()
        //{
        //    context.Wait(PostFailedBuildsAsync);
            
        //    var activity = await result as Activity;

        //    // return our reply to the user
        //    await context.PostAsync($"You sent {activity.Text}. TResponded by RootDialog");

        //    return Task.CompletedTask;
        //}
    }
}