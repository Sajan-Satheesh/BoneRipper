using System;

public class EventsEnemy
{
    private Action onEnemyDestroyed;

    #region Invoke Events
    public void InvokeOnEnemyDestroyed()
    {
        onEnemyDestroyed?.Invoke();
    }
    #endregion

    #region Subscribe Events
    public void Subscribe_OnEnemyDestroyed(Action subscribeFunc)
    {
        onEnemyDestroyed += subscribeFunc;
    }
    #endregion

    #region UnSubscribe Events
    public void UnSubscribe_OnEnemyDestroyed(Action subscribeFunc)
    {
        onEnemyDestroyed -= subscribeFunc;
    }
    #endregion
}
