using System;
using System.Collections.Generic;
using UniRx;

public interface IMessage
{

}

public interface IMessenger
{
    IDisposable Subscribe<T>(Action<T> handler) where T : struct, IMessage;
    void SendMessage<T>(T message) where T : struct, IMessage;
}

public class Messenger : IMessenger
{
    private readonly Dictionary<Type, object> _subscribers = new Dictionary<Type, object>();

    public IDisposable Subscribe<T>(Action<T> handler) where T : struct, IMessage
    {
        var type = typeof(T);
        ReactiveProperty<T> subscribers;
        if (_subscribers.TryGetValue(type, out var value))
        {
            subscribers = value as ReactiveProperty<T>;
        }
        else
        {
            subscribers = new ReactiveProperty<T>();
            _subscribers.Add(type, subscribers);
        }

        return subscribers.Subscribe(handler);
    }

    public void SendMessage<T>(T message) where T : struct, IMessage
    {
        var type = typeof(T);
        if (_subscribers.TryGetValue(type, out var value))
        {
            var subscribers = value as ReactiveProperty<T>;
            subscribers?.SetValueAndForceNotify(message);
        }
    }
}
