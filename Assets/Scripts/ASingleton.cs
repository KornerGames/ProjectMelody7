using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ASingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    #region Singleton Instance

    private static readonly object s_lock = new object();
    private static T s_instance;
    public static T Instance
    {
        get
        {
            lock (s_lock)
            {
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<T>();
                }

                return s_instance;
            }
        }
    }

    #endregion

    protected virtual void Awake()
    {
        InitSingleton();
        DontDestroyOnLoad(gameObject);
    }

    private void InitSingleton()
    {
        if (Instance.GetInstanceID() != GetInstanceID())
        {
            Debug.LogWarning($"Cannot have more than 1 instances. Destroying " +
                $"{gameObject.name}", gameObject);
            Destroy(gameObject);
        }
    }
}