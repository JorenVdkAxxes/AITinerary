﻿using Azure;
using Azure.AI.OpenAI;

namespace ConsoleApp1
{
    internal class Program
    {
        const string ENDPOINT = "https://ca-openai-workshop-sweden.openai.azure.com/";
        const string API_KEY = "802cfb82a80e4528a09b1907abf8d2ef";

        static async Task Main(string[] args)
        {
            var client = new OpenAIClient(
                new Uri(ENDPOINT),
                new Azure.AzureKeyCredential(API_KEY));

            while (true)
            {
                string? input = GetUserInput();

                var response = await client.GetChatCompletionsAsync(
                   "gpt-35-turbo-16k",
                   new ChatCompletionsOptions(new[]
                   {
                       new ChatMessage(ChatRole.System, GetSystemDefinition()),
                       new ChatMessage(ChatRole.User, input)
                   }));

                PrintResponse(response);
            }
        }

        private static void PrintResponse(Response<ChatCompletions> response)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Response: ");
            foreach (var choice in response.Value.Choices)
            {
                Console.WriteLine(choice.Message.Content);
            }
            Console.WriteLine();
        }

        private static string? GetUserInput()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Input: ");
            var input = Console.ReadLine();
            return input;
        }

        private static string GetSystemDefinition()
        {
            return "Assistant is a large language model designed to help users make their travel itinerary based " +
                "On a location and a given duration of the holidays." +
                "You only make suggestions when the user has given a duration." +
                "You do not answer any other questions. " +
                "You do not invent places or activities." +
                "Your name is AITinerary";
        }
    }
}