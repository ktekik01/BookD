using APIBookD.Models.Entities.Chatting.ChattingDTOs;
using APIBookD.Models.Entities.Chatting;
using Microsoft.AspNetCore.SignalR;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class ChatHub : Hub
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ChatHub(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task SendMessage(Guid senderId, Guid receiverId, string messageContent)
    {
        // Create a MessageDTO to pass to the API endpoint
        var messageDTO = new MessageDTO
        {
            Content = messageContent
        };

        var httpClient = _httpClientFactory.CreateClient();

        // Create the request content
        var requestContent = new
        {
            senderId = senderId,
            receiverId = receiverId,
            messageDTO = messageDTO
        };

        var content = new StringContent(JsonSerializer.Serialize(requestContent), Encoding.UTF8, "application/json");

        // Call the API endpoint to add the message to the database
        var response = await httpClient.PostAsync("https://localhost:7267/api/Chatting/AddMessage", content);

        var responseBody = await response.Content.ReadAsStringAsync();



        if (response.IsSuccessStatusCode)
        {
            var message = JsonSerializer.Deserialize<Message>(responseBody);
            await Clients.User(receiverId.ToString()).SendAsync("ReceiveMessage", senderId.ToString(), message.Content);
        }
        else
        {
            // Handle errors as needed
            var errorMessage = await response.Content.ReadAsStringAsync();
            // Log or handle the error message
            Console.WriteLine($"Error adding message: {errorMessage}");
        }
    }

    // Overload for sending messages from a specific sender
    public async Task SendMessageToUser(string receiverId, string senderId, string message)
    {
        // Logic to handle sending the message
        await Clients.User(receiverId).SendAsync("ReceiveMessage", senderId, message);
    }
}
