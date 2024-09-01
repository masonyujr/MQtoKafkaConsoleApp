using System;
using Avro.IO;
using Avro.Specific;
using Avro;
using MQToKafkaConsoleApp.Models;
using MQToKafkaConsoleApp.Services;
using Newtonsoft.Json;

namespace MQToKafkaConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var mqService = new MQService("QUEUE_MANAGER", "QUEUE_NAME", "localhost(1414)");
            var kafkaService = new KafkaService("bootstrap.kafka.server:9098", "your-topic");

            var messageModel = mqService.GetMessage();

            if (messageModel != null)
            {
                var schema = Avro.Schema.Parse(System.IO.File.ReadAllText("AvroSchemas/MessageSchema.avsc"));
                var serializer = new SpecificDatumWriter<MessageModel>(schema);

                using (var stream = new System.IO.MemoryStream())
                {
                    var encoder = new BinaryEncoder(stream);
                    serializer.Write(messageModel, encoder);
                    byte[] byteArray = stream.ToArray();

                    kafkaService.ProduceMessage(messageModel.JMSCorrelationID, byteArray);
                }

                Console.WriteLine("Message produced to Kafka");
            }
            else
            {
                Console.WriteLine("No message retrieved from MQ.");
            }
        }
    }
}
