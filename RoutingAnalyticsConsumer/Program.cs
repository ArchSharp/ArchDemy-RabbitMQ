using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "myroutingexchange", ExchangeType.Direct);

var queueName = channel.QueueDeclare().QueueName;
channel.QueueBind(queue: queueName, exchange: "myroutingexchange", routingKey: "analyticsonly");
channel.QueueBind(queue: queueName, exchange: "myroutingexchange", routingKey: "both");

var consumer = new EventingBasicConsumer(channel);

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Analytics received new message: {message}");
};

//channel.BasicConsume(queue: "letterbox", autoAck: true, consumer: consumer);
channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer); // competing pattern
Console.WriteLine("Analytics ready to consume message");
Console.ReadKey();