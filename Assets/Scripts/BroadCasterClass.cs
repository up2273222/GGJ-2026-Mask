using UnityEngine;
using System.Collections.Generic;

public interface IObserver
{
    public void OnNotify(WorldState worldState);
}

public abstract class BroadCasterClass : MonoBehaviour
{
    private List<IObserver> _observers = new List<IObserver>();

    public void AddObserver(IObserver observer)
    {
        _observers.Add(observer);
    }

    public void RemoveObserver(IObserver observer)
    {
        _observers.Remove(observer);
    }

    protected void NotifyObservers(WorldState worldState)
    {
        foreach (IObserver observer in _observers)
        {
            observer.OnNotify(worldState);
        }
    }
}
