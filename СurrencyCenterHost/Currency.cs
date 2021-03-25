using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace СurrencyCenterHost
{
    public class Currency
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("last")]
        public decimal Last { get; set; }
    }
}
