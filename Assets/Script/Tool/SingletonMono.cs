using System.Collections;
using UnityEngine;

public class SingletonMono<T>: MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = (T)(object)this;
        else
            Destroy(gameObject);
    }

}
