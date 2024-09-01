using IBM.XMS;
using MQToKafkaConsoleApp.Models;
using System;

namespace MQToKafkaConsoleApp.Services
{
    public class MQService
    {
        private readonly string _queueManager;
        private readonly string _queueName;
        private readonly string _connectionString;

        public MQService(string queueManager, string queueName, string connectionString)
        {
            _queueManager = queueManager;
            _queueName = queueName;
            _connectionString = connectionString;
        }

        public MessageModel GetMessage()
        {
            using (var connectionFactory = new XMSFactoryFactory().CreateConnectionFactory())
            {
                connectionFactory.SetStringProperty(XMSC.WMQ_CONNECTION_NAME_LIST, _connectionString);
                connectionFactory.SetIntProperty(XMSC.WMQ_CONNECTION_MODE, XMSC.WMQ_CM_CLIENT);
                connectionFactory.SetStringProperty(XMSC.WMQ_QUEUE_MANAGER, _queueManager);

                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var session = connection.CreateSession(false, AcknowledgeMode.AutoAcknowledge))
                    {
                        var queue = session.CreateQueue(_queueName);
                        using (var consumer = session.CreateConsumer(queue))
                        {
                            connection.Start();
                            var message = consumer.Receive(10000); // 10 seconds timeout

                            if (message != null)
                            {
                                var jmsMessage = (ITextMessage)message;
                                return new MessageModel
                                {
                                    JMSCorrelationID = jmsMessage.JMSCorrelationID,
                                    JMSTimestamp = jmsMessage.JMSTimestamp,
                                    Payload = jmsMessage.Text
                                };
                            }
                            return null;
                        }
                    }
                }
            }
        }
    }
}
