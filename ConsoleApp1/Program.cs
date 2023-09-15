using System.Text.Json;
using Azure;
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

            var getTravelSuggestionsFuntionDefinition = new FunctionDefinition()
            {
                Name = "get_travel_suggestions",
                Description = "Get the information of a given location",
                Parameters = BinaryData.FromObjectAsJson(
                new
                {
                    Type = "object",
                    Properties = new
                    {
                        Location = new
                        {
                            Type = "string",
                            Description = "The city and state, e.g. San Francisco, CA",
                        },
                        Days = new
                        {
                            Type = "number",
                            Description = "The total of days the user stays around the location"
                        }
                    },
                    Required = new[] { "location", "days" },
                },
                new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase }),
            };

            var chatCompletionsOptions = new ChatCompletionsOptions();
            chatCompletionsOptions.Functions.Add(getTravelSuggestionsFuntionDefinition);

            while (true)
            {
                string? input = GetUserInput();

                chatCompletionsOptions.Messages.Add(new ChatMessage(ChatRole.User, input));

                var response = await client.GetChatCompletionsAsync(
                   "gpt-35-turbo-16k",
                   chatCompletionsOptions);

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
    }
}