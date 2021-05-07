using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarPizza.Models
{
    public class WarehouseOperations
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public object id;
        [BsonElement("operation")]
        public string operation { get; set; }
        [BsonElement("description")]
        public string description { get; set; }
    }
}
