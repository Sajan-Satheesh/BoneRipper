using System;
using System.Collections.Generic;
using UnityEngine;

public class EventsPlayer
{
    private Action<List<Vector3>> onInitialtingJump;
    private Action onReachingLand;
    private Action onReachingBoat;
    private Action onGameOver;

    #region Invoke Events
    public void InvokeOnReachingLand()
    {
        onReachingLand?.Invoke();
    }

    public void InvokeOnGameOver()
    {
        onGameOver?.Invoke();
    }

    public void InvokeOnReachingBoat()
    {
        onReachingBoat?.Invoke();
    }

    public void InvokeOnInitialtingJump(List<Vector3> pathList)
    {
        onInitialtingJump?.Invoke(pathList);
    } 
    #endregion

    #region Subscribe Events
    public void Subscribe_OnInitialtingJump(Action<List<Vector3>> subscribeFunc)
    {
        onInitialtingJump += subscribeFunc;
    }
    public void Subscribe_OnReachingLand(Action subscribeFunc)
    {
        onReachingLand += subscribeFunc;
    }
    public void Subscribe_OnGameOver(Action subscribeFunc)
    {
        onGameOver += subscribeFunc;
    }
    public void Subscribe_OnReachingBoat(Action subscribeFunc)
    {
        onReachingBoat += subscribeFunc;
    }
    #endregion

    #region UnSubscribe Events
    public void UnSubscribe_OnInitialtingJump(Action<List<Vector3>> subscribeFunc)
    {
        onInitialtingJump -= subscribeFunc;
    }

    public void UnSubscribe_OnReachingLand(Action subscribeFunc)
    {
        onReachingLand -= subscribeFunc;
    }

    public void UnSubscribe_OnGameOver(Action subscribeFunc)
    {
        onGameOver -= subscribeFunc;
    }
    public void UnSubscribe_OnReachingBoat(Action subscribeFunc)
    {
        onReachingBoat -= subscribeFunc;
    }
    #endregion
}
