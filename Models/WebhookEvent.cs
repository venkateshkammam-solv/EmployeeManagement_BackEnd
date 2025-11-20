using System;
using Newtonsoft.Json;

namespace AzureFunctionPet.Models
{
    public class WebhookEvent
    {
        [JsonProperty("id")]
        public string Id { get; set; } = null!;

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("eventType")]
        public string EventType { get; set; }

        [JsonProperty("payload")]
        public object Payload { get; set; }

        [JsonProperty("receivedAt")]
        public DateTime ReceivedAt { get; set; } = DateTime.UtcNow;
    }
}
