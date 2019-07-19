
using System.Collections.Generic;
using DefaultNamespace;
using InterfaceNamespace;
using UnityEngine;

public class Publisher : IPublisher
{

    private readonly IList<ISubscriber> _subscribers;
    public Publisher()
    {
        _subscribers = new List<ISubscriber>();
    }
        
    public void Publish(Object source, CustomEventArgs data)
    {
        foreach (var _subscriber in _subscribers)
        {
            _subscriber.OnEvent(data);
        }
    }

    public void AddSubscriber(ISubscriber subscriber)
    {
        if (_subscribers.Contains(subscriber) == false)
        {
            _subscribers.Add(subscriber);
        }
    }
    public void RemoveSubscriber(ISubscriber subscriber)
    {    
        if (_subscribers.Contains(subscriber))
        {
            _subscribers.Remove(subscriber); 
        }
    }
}
