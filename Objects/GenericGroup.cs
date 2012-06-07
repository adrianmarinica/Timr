using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MongoDB.Bson;

namespace Objects
{
    public class GenericGroup
    {
        public List<GenericGroup> Groups { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }

        public GenericGroup(string name)
        {
            Id = Guid.NewGuid().ToString();
            Groups = new List<GenericGroup>();
            Name = name;
        }
    }
}
