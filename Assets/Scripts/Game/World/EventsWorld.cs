
using System;
using UnityEngine;

public class EventsWorld 
{
    private Action<Vector3, Vector3> onEntryExitGeneration;
    private Action onExitTrigger;
    private Action onNewLevel;

    #region Invoke Events
    public void InvokeOnExitTrigger()
    {
        onExitTrigger?.Invoke();
    }

    public void InvokeOnNewLevel()
    {
        onNewLevel?.Invoke();
    }

    public void InvokeOnEntryExitGeneration(Vector3 entry, Vector3 exit)
    {
        onEntryExitGeneration?.Invoke(entry,exit);
    }

    #endregion

    #region Subscribe Events
    public void Subscribe_OnExitTrigger(Action subscribeFunc)
    {
        onExitTrigger += subscribeFunc;
    }
    public void Subscribe_OnNewLevel(Action subscribeFunc)
    {
        onNewLevel += subscribeFunc;
    }
    public void Subscribe_OnEntryExitGeneration(Action<Vector3, Vector3> subscribeFunc)
    {
        onEntryExitGeneration += subscribeFunc;
    }
    #endregion

    #region UnSubscribe Events
    public void UnSubscribe_OnExitTrigger(Action subscribeFunc)
    {
        onExitTrigger -= subscribeFunc;
    }

    public void UnSubscribe_OnNewLevel(Action subscribeFunc)
    {
        onNewLevel -= subscribeFunc;
    }
    public void UnSubscribe_OnEntryExitGeneration(Action<Vector3, Vector3> subscribeFunc)
    {
        onEntryExitGeneration -= subscribeFunc;
    }
    #endregion
}
