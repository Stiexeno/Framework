using System;

namespace Framework
{
    public class Binding
    {
        public Type ContractType { get;set; }
        public Type[] Interfaces { get;set; }
        public object Instance { get; set; }
    }
}
