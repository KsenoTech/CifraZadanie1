(string host, int port) = ReadConnection();

string serverUri = $"ws://{host}:{port}/";
var client = new WebSocketClient.WebSocketClient.Client(serverUri);

await client.Connect();

Console.WriteLine("Enter messages to send to the server. Type 'exit' to quit.");

while (true)
{
    var message = Console.ReadLine();

    if (message.ToLower() == "exit")
        break;
    else
        await client.SendMessageAsync(message);
}


Console.WriteLine("Client stopped.");

static (string host, int port) ReadConnection()
{
    Console.Write("Host: ");
    var host = Console.ReadLine();

    Console.Write("Port: ");

    int port;

    while (!int.TryParse(Console.ReadLine(),out port))
    {
        Console.Write("Please enter correct number: ");
    }

    return (host, port);
}