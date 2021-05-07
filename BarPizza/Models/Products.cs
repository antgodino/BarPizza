using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarPizza.Models
{
    public class Products
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public Object id { get; set; }
        [BsonElement("description")]
        public string desription{ set; get; }
        [BsonElement("quantity")]
        public int quantity { set; get; }
        [BsonElement("warehouse")]
        public object warehouse { set; get; }

        public Products shallowCopy() {
            Products p = (Products)this.MemberwiseClone();
            p.id = null;
            return p;
        }
    }
}
