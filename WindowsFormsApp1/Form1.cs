using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using RabbitMQ;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        ConnectionMultiplexer redis = null;
        IDatabase database = null;
        IModel channel = null;

        public Form1()
        {
            InitializeComponent();
            //Connect();
            connectionCoelho();
            PesquisaCoelho();
        }

        public void connectionCoelho()
        {
            var factory = new ConnectionFactory() { HostName = "40.122.106.36" };
            var connection = factory.CreateConnection();
            channel = connection.CreateModel();

            channel.QueueDeclare(queue: "perguntas",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
        }

        public void Connect()
        {

            redis = ConnectionMultiplexer.Connect("40.122.106.36");
            database = redis.GetDatabase();
            var sub = redis.GetSubscriber();
            sub.Subscribe(new RedisChannel("perguntas", RedisChannel.PatternMode.Auto), (ch, msg) =>
            {
                Pesquisa(msg);
            });
        }

        public void Pesquisa(string question)
        {
            //string question = "Cotação Dollar";
            //string question = "Capital da Bahia";
            //string question = "2+2";
            var client = new WebSearchClient(new ApiKeyServiceClientCredentials("cdab4adbb42a487a9e3423a4ec716739"));
            string answer = BingSearch.WebResults(client, question);

            database.HashSet(question.Substring(0, 4), "SHAZAN", answer);
        }

        public void PesquisaCoelho()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) => {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var client = new WebSearchClient(new ApiKeyServiceClientCredentials("cdab4adbb42a487a9e3423a4ec716739"));
                string answer = BingSearch.WebResults(client, message);

                
                string xx = "oi " + "SHAZAN" + answer;
                var body1 = Encoding.UTF8.GetBytes(xx);
                channel.BasicPublish(exchange: "",
                routingKey: "perguntas",
                basicProperties: null,
                body: body1);

            };

            channel.BasicConsume(queue: "perguntas", autoAck: true, consumer: consumer);
        }
    }
}
