using System.Collections;
using UnityEngine;

public class SingletonMono<T>: MonoBehaviour where T : MonoBehaviour
{
    public T Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
            Instance = (T)(object)this;
        else
            Destroy(gameObject);
    }

}
