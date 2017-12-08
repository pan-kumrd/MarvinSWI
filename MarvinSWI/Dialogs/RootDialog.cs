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
        public virtual Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            if (activity.Text == "Gimme failed builds.")
            {
                await context.PostAsync("Getting you failed builds.");
                context.Call(new FailedBuildDialog(), ResumeAfterNewOrderDialog);
            }
            else
            {
                // return our reply to the user
                await context.PostAsync($"You sent {activity.Text}. TResponded by RootDialog");

                context.Wait(MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterNewOrderDialog(IDialogContext context, IAwaitable<object> result)
        {
            // Store the value that NewOrderDialog returned. 
            // (At this point, new order dialog has finished and returned some value to use within the root dialog.)
            var resultFromNewOrder = await result;

            await context.PostAsync($"New order dialog just told me this: {resultFromNewOrder}");

            // Again, wait for the next message from the user.
            context.Wait(this.MessageReceivedAsync);
        }
    }
}