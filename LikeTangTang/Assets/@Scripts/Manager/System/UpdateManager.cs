using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITickable
{
    void Tick(float _deltaTime);
}
public class UpdateManager : MonoBehaviour
{
    private readonly List<ITickable> tickable = new();
    private readonly List<ITickable> toAdd = new();
    private readonly List<ITickable> toRemove = new();

    public void Register(ITickable _tickable)
    {
        if(!tickable.Contains(_tickable)) toAdd.Add(_tickable);
    }

    public void Unregister(ITickable _tickable)
    {
        toRemove.Add(_tickable);
    }

    void Update()
    {
        float deltaTime = Time.deltaTime;

        foreach(var t in toAdd)
            if(!tickable.Contains(t)) tickable.Add(t);
        toAdd.Clear();

        foreach(var t in toRemove)  tickable.Remove(t);
        toRemove.Clear();

        foreach(var tick in tickable) tick.Tick(deltaTime);
    }

    public void Clear()
    {
        tickable.Clear();
        toAdd.Clear();
        toRemove.Clear();
    }
}
