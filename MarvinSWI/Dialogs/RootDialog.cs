using System;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using MarvinSWI.Dialogs;

namespace MarvinSWI.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;
            if ((message.Text != null) && (message.Text.ToLower() == "marvin start"))
            {
                context.Call(new FailedBuildDialog(), this.FailedBuildDialogResumeAfter);
            }

            context.Done(message.Text);
        }

        private async Task FailedBuildDialogResumeAfter(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync("Notifications started");
        }
    }
}