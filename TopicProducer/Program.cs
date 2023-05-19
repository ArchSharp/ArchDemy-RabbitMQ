using RabbitMQ.Client;
using System.Text;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "mytopicexchange", ExchangeType.Topic);

var userPayment = "A european user paid for something";

var encodeMessage = Encoding.UTF8.GetBytes(userPayment);

channel.BasicPublish("mytopicexchange", "user.europe.payments", null, encodeMessage);

Console.WriteLine($"Sending message: {userPayment}");


var orderGoods = "A european user ordered for goods";

var orderedMessage = Encoding.UTF8.GetBytes(orderGoods);

channel.BasicPublish("mytopicexchange", "business.europe.order", null, orderedMessage);

Console.WriteLine($"Sending message: {orderGoods}");
