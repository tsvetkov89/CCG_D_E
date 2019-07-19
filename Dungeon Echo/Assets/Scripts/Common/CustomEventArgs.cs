using System;
using EnumNamespace;

/// <summary>
/// Data transfer object
/// </summary>
public sealed class CustomEventArgs
{
    private readonly object _value;
    private readonly GameEventName _message;
    public CustomEventArgs(GameEventName message, Object data)
    {
        _value = data;
        _message = message;
    }

    public CustomEventArgs(GameEventName message)
    {
        _message = message;
    }

    public object Value
    {
        get { return _value; }
    }
    public GameEventName Message
    {
        get { return _message; }
    }
}

