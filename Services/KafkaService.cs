using Confluent.Kafka;
using System;
using System.Text;
using Avro;
using Avro.IO;
using Avro.Specific;

namespace MQToKafkaConsoleApp.Services
{
    public class KafkaService
    {
        private readonly string _bootstrapServers;
        private readonly string _topic;

        public KafkaService(string bootstrapServers, string topic)
        {
            _bootstrapServers = bootstrapServers;
            _topic = topic;
        }

        public void ProduceMessage(string key, byte[] message)
        {
            var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

            using (var producer = new ProducerBuilder<string, byte[]>(config).Build())
            {
                producer.Produce(_topic, new Message<string, byte[]> { Key = key, Value = message });
            }
        }
    }
}
