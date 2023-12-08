using System;
using System.Collections.Generic;
using UnityEngine;

abstract public class GenericPool<T>
{
    protected T item;
    protected Vector3 spwanPosition;

    Stack<T> bulletsAvailable = new Stack<T>();
    public List<T> bulletsInUse = new List<T>();

    public GenericPool(T item)
    {
        this.item = item;
    }

    abstract protected void instantiationLogic(out T item);
    abstract protected void getLogic(ref T item);
    abstract protected void returnLogic();

    public void returnShootable(T shootable)
    {
        if (bulletsInUse.Count == 0) return;

        bulletsInUse.Remove(shootable);
        returnLogic();
        bulletsAvailable.Push(shootable);
    }


    public T getShootable(Vector3 position)
    {
        this.spwanPosition = position;
        T topElement;
        if (bulletsAvailable.Count == 0)
        {
            instantiationLogic(out topElement);
            bulletsInUse.Add(topElement);
        }
        else
        {
            topElement = bulletsAvailable.Pop();
            getLogic(ref topElement);
            bulletsInUse.Add(topElement);

        }
        return topElement;
    }
}
