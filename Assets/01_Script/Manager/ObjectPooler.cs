using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-5)]
public class ObjectPooler : Singleton<ObjectPooler>
{
    [SerializeField] private Dictionary<PoolObjectType, Queue<GameObject>> poolDictionary;
    [SerializeField] private List<Pool> pools;

    protected override void Awake()
    {
        base.Awake();
        poolDictionary = new Dictionary<PoolObjectType, Queue<GameObject>>();

        foreach (Pool pool in pools)
        {
            Queue<GameObject> poolQueue = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject go = Instantiate(pool.prefab);
                if (pool.parent)
                    go.transform.SetParent(pool.parent, pool.worldPositionStays);
                go.SetActive(false);
                poolQueue.Enqueue(go);
            }

            poolDictionary.Add(pool.type, poolQueue);
        }
    }

    public GameObject SpawnFromPool(PoolObjectType type, Vector3 position, Quaternion rotation, bool isActive = true, bool instantEnqueue = false)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            return null;
        }

        if (poolDictionary[type].Count != 0)
        {
            GameObject spawned = poolDictionary[type].Dequeue();

            spawned.transform.SetPositionAndRotation(position, rotation);

            if (isActive)
                spawned.SetActive(true);

            if (instantEnqueue)
                poolDictionary[type].Enqueue(spawned);

            return spawned;
        }
        else
        {
            return null;
        }
    }

    public void PushToQueue(PoolObjectType type, GameObject go, bool clearParent = true, bool isActive = false)
    {
        if (clearParent)
            go.transform.SetParent(null, false);
        go.SetActive(isActive);

        poolDictionary[type].Enqueue(go);
    }
}

[System.Serializable]
public class Pool
{
    public PoolObjectType type;
    public int size;
    public GameObject prefab;
    public Transform parent;
    public bool worldPositionStays;
    public bool shouldExpand;
}
public enum PoolObjectType
{
    Car
}