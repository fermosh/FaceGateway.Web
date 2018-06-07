using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace FaceGateway.Web.Models
{
    public class ImageRecognitionMessage
    {
        public Guid CamId { get; set; }
        [JsonProperty("in")]
        public string FileName { get; set; }
        [JsonProperty("t")]
        public long TimeStamp { get; set; }
        public IEnumerable<Guid> FaceIds { get; set; }
    }
}