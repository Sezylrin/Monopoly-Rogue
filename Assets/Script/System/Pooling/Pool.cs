using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPoolable<T> where T : MonoBehaviour, IPoolable<T>
{
    public Pool<T> Pool { get; set; }
    /// <summary>
    /// determines if pooled object is new spawn to run any function needed
    /// to initialise the object. Manually set it to false after you run any custom initialisation function
    /// </summary>
    public bool IsNewSpawn { get; set; }
    public bool IsPooled { get; set; }
    /// <summary>
    /// Implement a function to add itself back into the pool.
    /// Use to replace destroy function in execution
    /// </summary>
    public void PoolSelf();
    


}
/// <summary>
/// Used to be able to make a dictionary of any type of Pool
/// </summary>
public interface IPools
{

}
[System.Serializable]
public class Pool<T> : IPools where T : MonoBehaviour, IPoolable<T>
{
    [SerializeField]
    private Stack<T> pooledObj = new Stack<T>();
    private GameObject objToSpawn;
    private Transform poolParent;
    public Pool(GameObject objToSpawn, Transform parent, string poolName)
    {
        this.objToSpawn = objToSpawn;
        poolParent = new GameObject(typeof(T).ToString() + " Pool").transform;
        if (poolName != null)
        {
            poolParent.name = poolName + " Pool";
        }
        poolParent.transform.SetParent(parent);
    }
    /// <summary>
    /// Returns an instance of the store type in the pool
    /// </summary>
    /// <returns></returns>
    public T GetPooledObj()
    {
        if (pooledObj.Count > 0)
        {
            T ScriptToReturn = pooledObj.Pop();
            ScriptToReturn.gameObject.SetActive(true);
            ScriptToReturn.IsPooled = false;
            return ScriptToReturn;
        }
        else
        {
            T spawned = SpawnGameObject();
            spawned.gameObject.SetActive(true);
            return spawned;
        }
    }

    private T SpawnGameObject()
    {
        GameObject spawnedObj = UnityEngine.Object.Instantiate(objToSpawn, poolParent);
        T script = spawnedObj.GetComponentInChildren<T>();
#if UNITY_EDITOR
        if (script == null)
        {
            Debug.LogError("The given objToSpawn does not contain the required Script to store");
            Debug.Break();
            return null;
        }
#endif
        script.Pool = this;
        script.IsPooled = false;
        script.IsNewSpawn = true;
        return script;
    }
    /// <summary>
    /// Adds object back into the pool
    /// </summary>
    /// <param name="objToPool"></param>
    public void PoolObj(T objToPool)
    {
        if (objToPool.IsPooled)
            return;
        objToPool.IsPooled = true;
        pooledObj.Push(objToPool);
        objToPool.gameObject.SetActive(false);
    }
    /// <summary>
    /// Spawn extra objects into pool instantly. 
    /// </summary>
    /// <param name="amountToLoad"> can not be 0 or negative</param>
    public void LoadExtraObject(int amountToLoad)
    {
#if UNITY_EDITOR
        if(amountToLoad < 1)
        {
            Debug.LogWarning("You attempted to load a negative amount of " + objToSpawn.name);
            Debug.Break();
        }
#endif
        for (int i = 0; i < amountToLoad; i++)
        {
            PoolObj(SpawnGameObject());
        }
    }
    /// <summary>
    /// Remove objects from pool instantly. 
    /// </summary>
    /// <param name="amountToLoad"> 0 or negative removes all objects</param>

    public void DeleteObject(int amountToRemove = 0)
    {
        if( amountToRemove < 1)
        {
            while (pooledObj.Count > 0)
            {
                T objToSpawn = pooledObj.Pop();
                UnityEngine.Object.Destroy(objToSpawn.gameObject);
            }
        }
        else
        {
            for (int i = 0; i < amountToRemove; i++)
            {
                T objToSpawn = pooledObj.Pop();
                UnityEngine.Object.Destroy(objToSpawn);
                if (pooledObj.Count == 0)
                    break;
            }
        }
    }
}
