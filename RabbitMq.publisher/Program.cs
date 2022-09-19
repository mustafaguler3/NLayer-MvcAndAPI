// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;



var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://oroqgsii:wiFEz46gO6LYiCsRg0pIzBJiQBDUO8ZT@hawk.rmq.cloudamqp.com/oroqgsii");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

channel.ExchangeDeclare("logs-fanout", durable: true,type:ExchangeType.Direct);

Enum.GetNames(typeof(LogNames)).ToList().ForEach(x =>
{
    var queueName = $"direct-queue-{x}";
    channel.QueueDeclare(queueName, true, false, false);

    channel.QueueBind(queueName, "logs-direct", null);
});


var randomQueuName = channel.QueueDeclare().QueueName;

channel.QueueBind(randomQueuName, "logs-fanout", "", null);

channel.BasicQos(0, 1, false);
var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume(randomQueuName, false, consumer);

Console.WriteLine("Logları dinleniyor...");


Enumerable.Range(1, 50).ToList().ForEach(i =>
{
    LogNames log = (LogNames)new Random().Next(1, 4);

    string message = $"Message {i}";

    var messageBody = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("logs-fanout", "", null, messageBody);

    Console.WriteLine("message sent");
});


Console.ReadLine();

public enum LogNames
{
    Critical = 1,
    Error = 2,
    Warning = 3,
    Info = 4,
}