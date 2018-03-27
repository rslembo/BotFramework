using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;

namespace BotFramework.Dialogs
{
    [Serializable]
    [LuisModel("", "")]
    public class LuisDialog : LuisDialog<object>
    {
        [LuisIntent("None")]
        private async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Desculpe, não pude compreender a sua frase: '{result.Query}'");
        }

        [LuisIntent("Greeting")]
        private async Task Greeting(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá! Eu sou o BOT da AudioStore. Por favor, informe o álbum desejado.");
        }
    }
}