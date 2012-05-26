using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;
using MongoDB.Driver;
using DataAccessLayer.Collections;
using Objects;

namespace DataAccessLayer
{
    public class Student
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public UserTypes Type { get; set; }
        public BsonInt32 Id { get; set; }

        public override string ToString()
        {
            return String.Format("{0} {1}", Name, Password);
        }
    }
    public class TestDAL
    {
        public void a()
        {

            // Users.Collection;
            // database.DropCollection("students");
            // 
            // if (!database.CollectionExists("students"))
            //     database.CreateCollection("students");
            // 
            // MongoCollection collection = database.GetCollection("students");
            // Users.GetCollection();
            // collection.Insert(new Student { Name = "adrian", Password = "adrian"});
            // collection.Insert(new Student { Name = "adrian1", Password = "adrian1"});
            // collection.Insert(new Student { Name = "adrian3", Password = "adrian3"});
            // collection.Insert(new Student { Name = "adrian2", Password = "adrian2"});
            // //collection.Insert(new BsonDocument("student", "adrian"));
            // var stud = collection.FindAllAs(typeof(Student));
            // string total = "";
            // foreach (var item in stud)
            // {
            //     total += item.ToString();
            // }
            // return total;
            
        }
    }
}
