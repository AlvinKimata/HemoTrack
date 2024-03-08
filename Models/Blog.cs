using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;


namespace HemoTrack.Models
{
    public class Blog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id {get; set;}

        [BsonElement("BlogName")]
        [JsonPropertyName("BlogName")]
        public string BlogName {get; set;} = null!;

        public string Title { get; set; }
        public string Content { get; set; }

        //Time based properties
        public DateTime CreateTime { get; set; }
    }
}