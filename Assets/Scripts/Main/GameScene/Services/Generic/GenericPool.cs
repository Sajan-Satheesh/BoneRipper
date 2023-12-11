using System;
using System.Collections.Generic;
using UnityEngine;

abstract public class GenericPool<W,T>
{
    protected W item;

    Stack<W> available = new Stack<W>();
    public List<W> inUse = new List<W>();

    public GenericPool(W item)
    {
        this.item = item;
    }

    abstract protected void instantiationLogic(out W newItem , T spwanRef);
    abstract protected void getLogic(ref W releasedItem , T spwanRef);
    abstract protected void returnLogic(ref W returnedItem);

    public void returnItem(W returnedItem)
    {
        if (inUse.Count == 0) return;

        inUse.Remove(returnedItem);
        returnLogic(ref returnedItem);
        available.Push(returnedItem);
    }


    public W getItem(T spawnRef)
    {
        W topElement;
        if (available.Count == 0)
        {
            instantiationLogic(out topElement, spawnRef);
            inUse.Add(topElement);
        }
        else
        {
            topElement = available.Pop();
            getLogic(ref topElement, spawnRef);
            inUse.Add(topElement);

        }
        return topElement;
    }
}
