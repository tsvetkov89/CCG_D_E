using System;
using DefaultNamespace;

namespace InterfaceNamespace
{
    public interface ISubscriber
    {
        void OnEvent(CustomEventArgs  messageData);
    }
}