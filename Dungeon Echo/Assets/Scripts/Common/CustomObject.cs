using UnityEngine;
using UnityEngine.Networking.NetworkSystem;
using Object = System.Object;

namespace DefaultNamespace
{
    /// <summary>
    /// Data transfer object
    /// </summary>
    public sealed class CustomObject
    {
        private readonly object _value;
        public static readonly CustomObject Empty = new CustomObject();
        public CustomObject(Object value)
        {
            _value = value;
        }
        public object Value
        {
            get { return _value; }
        }

        private CustomObject()
        {
            _value = null;
        }
    }
    
}