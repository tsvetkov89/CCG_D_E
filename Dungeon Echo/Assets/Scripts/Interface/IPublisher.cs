using UnityEngine;

namespace InterfaceNamespace
{
    public interface IPublisher
    {
        void Publish(Object source, CustomEventArgs data);
        void AddSubscriber(ISubscriber subscriber);
        void RemoveSubscriber(ISubscriber subscriber);
    }
}