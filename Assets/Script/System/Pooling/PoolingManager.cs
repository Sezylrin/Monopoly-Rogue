using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoBehaviour
{
    // Start is called before the first frame update

    public Dictionary<GameObject, IPools> Pools = new Dictionary<GameObject, IPools>();

    private static PoolingManager instance;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyImmediate(gameObject);
            return;
        }
    }
    void Start()
    {
        enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    /// <summary>
    /// Locates or generates a new pool to storing a specific script on a GameObject
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objToSpawn">GameObject to spawn</param>
    /// <param name="pool">Local instance to store Pool</param>
    /// <param name="poolName">Custom pool name rather then gameobject name</param>
    public static void FindPool<T>(GameObject objToSpawn, out Pool<T> pool,string poolName = null) where T : MonoBehaviour, IPoolable<T>
    {
#if UNITY_EDITOR
        if(instance == null)
        {
            Debug.LogWarning("Scene has no PoolingManager, Pooling is not possible. Please place a PoolingManager script onto a gameobject in scene.");
            Debug.Break();
        }
#endif
            IPools foundPool;
        if (instance.Pools.TryGetValue(objToSpawn,out foundPool) && foundPool is Pool<T>)
        {
            pool = (Pool<T>)foundPool;
        }
        else
        {
            Pool<T> newPool = new Pool<T>(objToSpawn, instance.transform, poolName);
            instance.Pools.Add(objToSpawn, newPool);
            pool = newPool;
        }
    }

}
