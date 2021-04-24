// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using CinemaBot.Services.LuisAI;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace CinemaBot
{
    public class CinemaBot : ActivityHandler
    {
        private readonly ILuisAIService _luisAIService;

        public CinemaBot(ILuisAIService luisAIService)
        {
            _luisAIService = luisAIService;
                
        }
        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                   // await turnContext.SendActivityAsync(MessageFactory.Text($"Hello world!"), cancellationToken);
                }
            }
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            var luisResul = await _luisAIService._luisRecognizer.RecognizeAsync(turnContext, cancellationToken);
            await Intention(turnContext, luisResul, cancellationToken);
        }

        private async Task Intention(ITurnContext<IMessageActivity> turnContext, RecognizerResult luisResul, CancellationToken cancellationToken)
        {
            var topIntent = luisResul.GetTopScoringIntent();

            switch(topIntent.intent)
            {
                case "Greet":
                    await IntenGreet(turnContext, luisResul, cancellationToken);
                    break;
                case "Dismiss":
                    await IntenDismiss(turnContext, luisResul, cancellationToken);
                    break;
                case "Thank":
                    await IntenThank(turnContext, luisResul, cancellationToken);
                    break;
                case "BuyMovie":
                    await IntenBuyMovie(turnContext, luisResul, cancellationToken);
                    break;
                case "None":
                    await IntenNone(turnContext, luisResul, cancellationToken);
                    break;
                default:
                    break;

            }
        }

        private async Task IntenGreet(ITurnContext<IMessageActivity> turnContext, RecognizerResult luisResul, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("Hola, que gusto hablar contigo.", cancellationToken:cancellationToken);
        }

        private async Task IntenDismiss(ITurnContext<IMessageActivity> turnContext, RecognizerResult luisResul, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("Espero verte pronto.", cancellationToken: cancellationToken);
        }

        private async Task IntenThank(ITurnContext<IMessageActivity> turnContext, RecognizerResult luisResul, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("Gracias a ti por escribirme.", cancellationToken: cancellationToken);
        }

        private Task IntenBuyMovie(ITurnContext<IMessageActivity> turnContext, RecognizerResult luisResul, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task IntenNone(ITurnContext<IMessageActivity> turnContext, RecognizerResult luisResul, CancellationToken cancellationToken)
        {
            await turnContext.SendActivityAsync("Lo siento, no entiendo lo que dices.", cancellationToken: cancellationToken);
        }
    }
}
