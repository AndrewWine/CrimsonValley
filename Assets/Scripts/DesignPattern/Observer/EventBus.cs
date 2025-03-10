using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventBus
{
    private static readonly Dictionary<Type, Delegate> s_event = new Dictionary<Type, Delegate>();
    private static readonly object s_lock = new object();

    public static void Subscribe<T>(Action<T> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        lock (s_lock)
        {
            var eventType = typeof(T);
            if (s_event.TryGetValue(eventType, out Delegate existingHandlers))
            {
                s_event[eventType] = Delegate.Combine(existingHandlers, handler);
            }
            else
            {
                s_event[eventType] = handler;
            }
        }
    }

    public static void Unsubscribe<T>(Action<T> handler)
    {
        if (handler == null) throw new ArgumentNullException(nameof(handler));

        lock (s_lock)
        {
            var eventType = typeof(T);
            if (s_event.TryGetValue(eventType, out Delegate existingHandlers))
            {
                var newHandlers = Delegate.Remove(existingHandlers, handler);
                if (newHandlers != null)
                {
                    s_event[eventType] = newHandlers;
                }
                else
                {
                    s_event.Remove(eventType);
                }
            }
        }
    }

    public static void Publish<T>(T eventData)
    {
        Delegate handlers;
        lock (s_lock)
        {
            var eventType = typeof(T);
            if (!s_event.TryGetValue(eventType, out handlers)) return;
        }

        if (handlers is Action<T> action)
        {
            foreach (Action<T> singleHandler in action.GetInvocationList())
            {
                try
                {
                    singleHandler.Invoke(eventData);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error in event handler: {ex.Message}");
                }
            }
        }
    }
}
