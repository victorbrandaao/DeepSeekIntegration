using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        // URL base da API DeepSeek
        string baseUrl = "https://api.deepseek.com";
        // Endpoint específico para completar chat
        string endpoint = "/chat/completions";
        // Chave da API (substitua pela sua chave de API)
        string apiKey = "sua_chave_de_api_aqui";

        // Corpo da solicitação com parâmetros e mensagens
        var requestBody = new
        {
            model = "deepseek-chat",
            messages = new[]
            {
                new { role = "system", content = "Você é um assistente útil." },
                new { role = "user", content = "Olá! Me ajude com programação." },
                new { role = "user", content = "Como faço para criar uma API em .NET?" },
                new { role = "user", content = "Quais são as melhores práticas para segurança em APIs?" }
            },
            temperature = 0.7,
            max_tokens = 150,
            top_p = 1.0,
            frequency_penalty = 0.0,
            presence_penalty = 0.6
        };

        try
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {apiKey}");

                // Serializa o corpo da solicitação para JSON
                string jsonBody = JsonSerializer.Serialize(requestBody);
                StringContent content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                // Envia a solicitação POST para a API
                HttpResponseMessage response = await client.PostAsync(endpoint, content);

                if (response.IsSuccessStatusCode)
                {
                    // Lê e exibe a resposta da API
                    string result = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Resposta da API:");
                    Console.WriteLine(result);
                }
                else
                {
                    // Exibe o código de erro e detalhes da resposta
                    Console.WriteLine($"Erro: {response.StatusCode}");
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"Detalhes: {errorMessage}");
                }
            }
        }
        catch (Exception ex)
        {
            // Exibe a mensagem de erro em caso de exceção
            Console.WriteLine("Erro ao consumir a API:");
            Console.WriteLine(ex.Message);
        }
    }
}