using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

var apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY")
            ?? throw new Exception("Set GROQ_API_KEY first (https://console.groq.com/keys)");
 
using var http = new HttpClient
                    {
                        BaseAddress = new Uri("https://api.groq.com/openai/v1/")
                    };

http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

var requestJson = """
{
  "model": "openai/gpt-oss-20b",
  "max_completion_tokens": 1000,
  "messages": [
    {
      "role": "system",
      "content": "You are a senior software engineering assistant."
    },
    {
      "role": "user",
      "content": "Explain the Middleware in .NET Core in simple terms. Write in 3 bullet points"
    }
  ]
}
""";

using var content = new StringContent(requestJson, Encoding.UTF8, "application/json");
var response = await http.PostAsync("chat/completions", content);
response.EnsureSuccessStatusCode();

var maxCompletionTokens = 1000;
using var responseJson = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
var root = responseJson.RootElement;
var choice = root.GetProperty("choices")[0];
var answer = choice.GetProperty("message").GetProperty("content").GetString();
var finishReason = choice.GetProperty("finish_reason").GetString();
var usage = root.GetProperty("usage");
var promptTokens = usage.GetProperty("prompt_tokens").GetInt32();
var completionTokens = usage.GetProperty("completion_tokens").GetInt32();
var remainingTokens = maxCompletionTokens - completionTokens;

Console.WriteLine(answer);
Console.WriteLine($"Prompt tokens: {promptTokens}");
Console.WriteLine($"Completion tokens: {completionTokens}");
Console.WriteLine($"Remaining tokens: {remainingTokens}");
Console.WriteLine($"Finish reason: {finishReason}");

