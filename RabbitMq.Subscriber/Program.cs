// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

var factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://oroqgsii:wiFEz46gO6LYiCsRg0pIzBJiQBDUO8ZT@hawk.rmq.cloudamqp.com/oroqgsii");

using var connection = factory.CreateConnection();

var channel = connection.CreateModel();

//channel.QueueDeclare("merhaba-queue", true, false, false);

channel.BasicQos(0,1,false) //kaçtane subscriber var toplam değeri 5 gönderir 6 tane ise 2 ona 2 ona gönderir
var consumer = new EventingBasicConsumer(channel);

channel.BasicConsume("merhaba-queue", false, consumer);

consumer.Received += (object sender, BasicDeliverEventArgs e) =>
{
    var message = Encoding.UTF8.GetString(e.Body.ToArray());

    Console.WriteLine("Gelen mesaj: "+message);

    channel.BasicAck(e.DeliveryTag, false);
}

