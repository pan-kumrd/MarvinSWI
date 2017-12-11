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
        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Ok, starting notifications");
        }

    }
}