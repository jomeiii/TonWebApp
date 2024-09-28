using Dynamic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    protected static T instance;
    private static readonly object _lock = new();

    public static T Instance
    {
        get
        {
            if (instance == null)
                lock (_lock)
                {
                    instance = FindObjectOfType<T>();

                    if (instance == null)
                    {
                        var singletonObject = new GameObject();
                        instance = singletonObject.AddComponent<T>();
                        singletonObject.name = typeof(T) + " (Singleton)";
                        DontDestroyOnLoad(singletonObject);
                    }
                }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            DontDestroyOnLoad(gameObject);

            DynamicDebug.Debug(nameof(Singleton<T>), nameof(Awake),
                $"{gameObject.name} был добавлен в DontDestroyOnLoad");
        }
        else if (instance != this) 
            Destroy(gameObject);
    }
}