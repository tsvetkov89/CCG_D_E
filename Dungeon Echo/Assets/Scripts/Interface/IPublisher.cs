using DefaultNamespace;
using UnityEngine;
//using Object = System.Object;

namespace InterfaceNamespace
{
    public interface IPublisher
    {
        void Publish(Object source, CustomEventArgs data);
        void AddSubscriber(ISubscriber subscriber);
        void RemoveSubscriber(ISubscriber subscriber);
    }
}