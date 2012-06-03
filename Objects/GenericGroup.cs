using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Objects
{
    public class GenericGroup
    {
        public Faculty Faculty { get; set; }
        public List<GenericGroup> Groups { get; set; }
    }
}
