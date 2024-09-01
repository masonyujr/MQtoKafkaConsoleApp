namespace MQToKafkaConsoleApp.Models
{
    public class MessageModel
    {
        public string JMSCorrelationID { get; set; }
        public long JMSTimestamp { get; set; } // Unix timestamp in milliseconds
        public string Payload { get; set; }    // XML payload
    }
}
