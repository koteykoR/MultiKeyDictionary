using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    class Name
    {
        public string firstName { get; set; }
        public Name(string v1)
        {
            this.firstName = v1;
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as Name);
        }
        public bool Equals(Name name)
        {
            return this.firstName.Equals(name.firstName);
        }
        public override int GetHashCode()
        {
            int hash = 13*firstName.GetHashCode();
            return hash;
        }
    }
}
