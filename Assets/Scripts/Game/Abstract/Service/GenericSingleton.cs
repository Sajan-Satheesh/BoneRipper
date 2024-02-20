
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : GenericSingleton<T>
{
    public static T instance;
    // Start is called before the first frame update
    protected virtual void Awake()
    {
        if (instance != null && instance != (T)this)
        {
            Destroy((T)this);
        }
        else { instance = (T)this; }
    }
}
