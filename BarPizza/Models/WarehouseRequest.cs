using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarPizza.Models
{
    public class WarehouseRequest
    {
        [BsonId(IdGenerator = typeof(ObjectIdGenerator))]
        public object id { set; get; }
        [BsonElement("token")]
        public string token { get; set; }
        [BsonElement("pending")]
        public Boolean pending { get; set; }
        [BsonElement("user_id_request")]
        public object userRequest { get; set; }
        [BsonElement("user_id_response")]
        public object userResponse { get; set; }
        [BsonElement("warehouse_item_id")]
        public object warehouseItem { get; set; }
        [BsonElement("warehouse_operation")]
        public object warehouseOperation { get; set; }
        [BsonElement("to_warehouse")]
        public object toWarehouse { get; set; }
        [BsonElement("from_warehouse")]
        public object fromWarehouse { get; set; }
        [BsonElement("request_date")]
        public DateTime request_date { get; set; }
        [BsonElement("response_date")]
        public DateTime response_date { get; set; }
    }
}
