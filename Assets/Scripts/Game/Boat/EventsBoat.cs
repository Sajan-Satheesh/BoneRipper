
using System;
using UnityEngine;

public class EventsBoat
{
    private Action onAreaPassed;
    private Action<Vector3> onBoatInNewLevel;

    #region Invoke Events
    public void InvokeOnAreaPassed()
    {
        onAreaPassed?.Invoke();
    }
    public void InvokeOnBoatInNewLevel(Vector3 boatPos)
    {
        onBoatInNewLevel?.Invoke(boatPos);
    }
    #endregion

    #region Subscribe Events
    public void Subscribe_OnAreaPassed(Action subscribeFunc)
    {
        onAreaPassed += subscribeFunc;
    }
    public void Subscribe_OnBoatInNewLevel(Action<Vector3> subscribeFunc)
    {
        onBoatInNewLevel += subscribeFunc;
    }
    #endregion

    #region UnSubscribe Events
    public void UnSubscribe_OnAreaPassed(Action subscribeFunc)
    {
        onAreaPassed -= subscribeFunc;
    }
    public void UnSubscribe_OnBoatInNewLevel(Action<Vector3> subscribeFunc)
    {
        onBoatInNewLevel -= subscribeFunc;
    }
    #endregion
}
