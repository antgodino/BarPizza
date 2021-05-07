using BarPizza.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BarPizza.Service
{
    public class MyService
    {
        private IMongoCollection<User> _users;
        private IMongoCollection<Products> _products;
        private IMongoCollection<WarehouseOperations> _operations;
        private IMongoCollection<Warehouse> _warehouse;
        private IMongoCollection<WarehouseRequest> _warehousRequest;
        private IMongoCollection<UserType> _usersTypes;
        public MyService(IMongoClient client)
        {
            var db = client.GetDatabase("pizzeria_bar");
            _users = db.GetCollection<User>("user");
            _products = db.GetCollection<Products>("products");
            _warehouse = db.GetCollection<Warehouse>("warehouse");
            _warehousRequest = db.GetCollection<WarehouseRequest>("warehouse_request");
            _operations = db.GetCollection<WarehouseOperations>("warehouse_operation");
            _usersTypes = db.GetCollection<UserType>("user_type");
        }
        public User getUser(String username)
        {
            return _users.Find(u => u.username == username).First();
        }
        public User getUser(String username, String password)
        {
            return _users.Find(u => u.username == username && u.password == password).First();
        }
        public UserType getUserType(Object id) {
            return _usersTypes.Find(ut => ut.id == id).First();
        }
        public Products getProductById(Object id)
        {
            return _products.Find(p => p.id.Equals(id)).First();
        }
        public List<BsonDocument> getAllProducts()
        {
            return _products.Aggregate()
                     .Lookup("warehouse", "warehouse", "_id", "_warehouse")
                     .As<BsonDocument>()
                     .ToList();
        }
        public List<BsonDocument> getMyProduct(Object _warehouseid)
        {
            var filter = Builders<Products>.Filter.Eq(p => p.warehouse, _warehouseid);

            return _products.Aggregate().Match(filter)
                     .Lookup("warehouse", "warehouse", "_id", "_warehouse")
                     .As<BsonDocument>()
                     .ToList();
        }
        public List<WarehouseRequest> getMyRequest(Object _warehouseid)
        {
            return _warehousRequest.Find(r => r.pending && (r.toWarehouse == _warehouseid || r.fromWarehouse == _warehouseid)).ToList();
        }
        public Warehouse getWarehouseByCode(String code)
        {
            return _warehouse.Find(w => w.code == code).First();
        }
        public List<BsonDocument> getMyRequestPending(Object _warehouseid)
        {
            //where
            var filter = Builders<WarehouseRequest>.Filter.Eq(r => r.pending, true);
            filter &= (Builders<WarehouseRequest>.Filter.Eq(r => r.toWarehouse, _warehouseid) |
                Builders<WarehouseRequest>.Filter.Eq(r => r.fromWarehouse, _warehouseid));

            return _warehousRequest.Aggregate().Match(filter)
                .Lookup("warehouse", "to_warehouse", "_id", "_fromwarehouse")
                .Lookup("warehouse", "from_warehouse", "_id", "_towarehouse")
                .Lookup("products", "warehouse_item_id", "_id", "_products")
                .Lookup("warehouse_operation", "warehouse_operation", "_id", "_operation")
                .As<BsonDocument>()
                .ToList();
        }
        public WarehouseRequest getWarehouseRequest(ObjectId _requestid)
        {
            return _warehousRequest.Find(w => w.id.Equals(_requestid)).First();
        }
        public Boolean executeRequest(WarehouseRequest wrequest, string operation, string username)
        {
            try
            {
                //trovo prodotto da manipolare
                Products product = _products.Find(p => p.id == wrequest.warehouseItem).First();

                switch (operation)
                {
                    case "move":
                        _products.UpdateOne(
                            Builders<Products>.Filter.Eq(p => p.id, product.id),//filter
                            Builders<Products>.Update.Set(p => p.warehouse, wrequest.toWarehouse));//update
                        break;
                    case "copy":
                        Products p = product.shallowCopy();
                        p.warehouse = wrequest.toWarehouse;
                        _products.InsertOne(p);//insert copy of product
                        break;
                    default:
                        break;
                }
                //update della rischiesta
                _warehousRequest.UpdateOne(Builders<WarehouseRequest>.Filter.Eq(wr => wr.id, wrequest.id),
                    Builders<WarehouseRequest>.Update
                    .Set(wr => wr.pending, false)
                    .Set(wr => wr.response_date, DateTime.Now)
                    .Set(wr => wr.userResponse, this.getUser(username).id));

                return true;
            }
            catch (Exception){}

            return false;
        }
        public List<WarehouseOperations> getOperations()
        {
            return _operations.Find(_ => true).ToList();
        }
        public List<Warehouse> getWarehousesNot(Products product)
        {
            return _warehouse.Find(w => w.id != product.warehouse).ToList();
        }
        public Boolean insertWarehouseRequest(WarehouseRequest warehouseRequest)
        {
            try
            {
                _warehousRequest.InsertOne(warehouseRequest);
                return true;
            }
            catch (Exception) { }
            return false;
        }
    }
}
