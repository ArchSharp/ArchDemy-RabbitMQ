﻿using RabbitMQ.Client;
using System.Text;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory { HostName = "localhost" };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

var queueName = channel.QueueDeclare().QueueName;

var consumer = new EventingBasicConsumer(channel);

channel.QueueBind(queue: queueName, exchange: "pubsub", routingKey: "");

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Second Consumer has received new message: {message}");
};

channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
Console.WriteLine("2nd Consumer ready to consume message");
Console.ReadKey();