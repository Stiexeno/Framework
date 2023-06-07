using System;

namespace Framework
{
    public class Binding
    {
        public Type ContractType { get;set; }
        public Type[] Interfaces { get;set; }
        public object Instance { get; set; }
        public bool Initialized { get; set; }
        public bool DontDestroyOnLoad { get; set; }
    }
}
