using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using BotFramework.Helpers;
using BotFramework.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;

namespace BotFramework.Dialogs
{
    [Serializable]
    [LuisModel("14938398-3ef0-4895-8c80-fc8398019ef8", "36a46c414ec047f8a9d5a1df3f2f9437")]
    public class LuisDialog : LuisDialog<object>
    {
        [LuisIntent("None")]
        private async Task None(IDialogContext context, LuisResult result)
        {
            var message = result.Query;

            if (!message.IsCpf())
                await context.PostAsync($"Desculpe, não pude compreender a sua frase: '{result.Query}'");

            await context.PostAsync("Aguarde, estou analisando...");

            var endpoint = "http://creditanalysisapi.azurewebsites.net/api/analysis";

            using (var client = new HttpClient())
            {
                var request = new ApiRequest { Cpf = message };
                var httpContent = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var response = await client.PostAsync(endpoint, httpContent);

                if (!response.IsSuccessStatusCode)
                {
                    await context.PostAsync("Desculpe, não foi possível realizar a operação. Por favor, tente novamente em instantes.");
                    return;
                }

                var isCpfClean = bool.Parse(await response.Content.ReadAsStringAsync());

                if (isCpfClean)
                    await context.PostAsync("Excelente notícia! Este CPF está limpo!");
                else
                    await context.PostAsync("Infelizmente, este CPF está sujo na praça. Regularize o mais rápido possível!");
            }
        }

        [LuisIntent("Greeting")]
        private async Task Greeting(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá! Eu sou o BOT da CreditAnalysis, e posso lhe auxiliar na consulta à sua análise de crédito. " +
                "Para isso, precisaremos de alguns de seus dados, tudo bem?");
        }

        [LuisIntent("Confirmation")]
        private async Task Confirmation(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Ótimo! Vamos prosseguir, então. Por favor, me informe o seu CPF para analisar.");
        }

        [LuisIntent("Denial")]
        private async Task Denial(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Agradecemos a visita! Caso mude de ideia, fique à vontade para retornar!");
        }
    }
}