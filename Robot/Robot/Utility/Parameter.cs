using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Robot.Utility
{
    public class Parameter : IEquatable<Parameter>
    {
        public string Name { get; private set; }

        public string Type { get; private set; }

        
        public Parameter(string name, string type)
        {
            Name = name;
            Type = type;
        }
        
        public bool Equals(Parameter other)
        {
            return Name == other.Name;
        }
    }
}
