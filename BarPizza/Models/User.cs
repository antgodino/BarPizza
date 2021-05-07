using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarPizza.Models
{
    public class User
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public Object id { get; set; }
        [BsonElement("name")]
        public string name { get; set; }
        [BsonElement("surname")]
        public string surname { get; set; }
        [BsonElement("email")]
        public string email { get; set; }
        [BsonElement("pwd")]
        public string password { get; set; }
        [BsonElement("username")]
        public string username { get; set; }
        [BsonElement("user_type")]
        public Object user_type { get; set; }
    }
}
