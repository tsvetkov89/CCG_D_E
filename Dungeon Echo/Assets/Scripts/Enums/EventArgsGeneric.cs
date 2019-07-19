using System;
using System.Collections.Generic;

public  sealed class EventArgsGeneric<T> : EventArgs
{
    #region Fields


    #endregion
    #region Properties

    public EventTypeEnum EventType { get; private set;}
    public object SomeObject { get; private set; }

    #endregion
    #region Initialization

    public EventArgsGeneric(EventTypeEnum eventType, object someObject)
    {
        EventType = eventType;
        SomeObject = someObject;
    }

    #endregion
}