using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "letterbox", durable: false, exclusive: false, autoDelete: false, arguments: null);

channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

var consumer = new EventingBasicConsumer(channel);

var randon = new Random();

consumer.Received += (model, ea) =>
{
    var processingTime = randon.Next(1, 6);
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    //    Console.WriteLine($" Message Received {message}");
    Console.WriteLine($"Received {message} will take {processingTime} to process"); // competing pattern
    Task.Delay(TimeSpan.FromSeconds(processingTime)).Wait(); // competing pattern
    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false); // competing pattern
};

//channel.BasicConsume(queue: "letterbox", autoAck: true, consumer: consumer);
channel.BasicConsume(queue: "letterbox", autoAck: false, consumer: consumer); // competing pattern
Console.WriteLine("Ready to consume message");
Console.ReadKey();