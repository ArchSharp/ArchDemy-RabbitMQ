using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "letterbox", durable: false, exclusive: false, autoDelete: false, arguments: null);

var random = new Random();
var messageId = 1;
while (true)
{
    var publishTime = random.Next(1, 4);
    var message = $"Sending MessageId: {messageId}";

    var encodeMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("", "letterbox", null, encodeMessage);

    //Console.WriteLine($"Published message: {message}");
    Console.WriteLine($"Sending message: {message}");

    Task.Delay(TimeSpan.FromSeconds(publishTime)).Wait();

    messageId++;
}
/*
var message = "This is my message";

var encodeMessage = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("", "letterbox", null, encodeMessage);

Console.WriteLine($"Published message: { message}");*/