using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarPizza.Models
{
    public class Warehouse
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public Object id { get; set; }
        [BsonElement("description")]
        public string description { set; get; }
        [BsonElement("position")]
        public string position { set; get; }
        [BsonElement("code")]
        public string code { set; get; }
    }
}
