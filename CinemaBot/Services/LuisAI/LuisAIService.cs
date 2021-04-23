﻿using Microsoft.Bot.Builder.AI.Luis;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CinemaBot.Services.LuisAI
{
    public class LuisAIService: ILuisAIService
    {
        public LuisRecognizer _luisRecognizer { get; set; }
        public LuisAIService (IConfiguration configuration)
        {
            var luisApplication = new LuisApplication(
                configuration["Luis.AppId"],
                configuration["Luis.Apikey"],
                configuration["Luis.HostName"]
                );

            var recognizerOptions = new LuisRecognizerOptionsV3(luisApplication)
            {
                PredictionOptions = new Microsoft.Bot.Builder.AI.LuisV3.LuisPredictionOptions()
                {
                    IncludeInstanceData = true
                }
            };
            _luisRecognizer = new LuisRecognizer(recognizerOptions);

        }
    }
}
