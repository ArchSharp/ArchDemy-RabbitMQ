using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "myroutingexchange", ExchangeType.Direct);

var message = "This message needs to be routed";

var encodeMessage = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("myroutingexchange", "both", null, encodeMessage);

Console.WriteLine($"Sending message: {message}");

