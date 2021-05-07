using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarPizza.Models
{
    public class UserType
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public Object id { get; set; }
        [BsonElement("descrizione")]
        public string descrizione { get; set; }
        [BsonElement("code")]
        public string code { get; set; }
    }
}
