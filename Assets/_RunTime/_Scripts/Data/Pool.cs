using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Assertions;

public interface IPooledObject
{
    public void OnInstantiate();

    public void OnGetFromPool();

    public void OnReturnToPool();

}

[System.Serializable]
public class Pool<T> where T : MonoBehaviour, IPooledObject
{
    [SerializeField] private Transform poolRoot;
    [SerializeField] private T prefab;
    [SerializeField] private int initialPoolSize = 5;

    private List<T> containerPool;

    private bool IsInitialized;

    public void InitializePool()
    {
        IsInitialized = true;
        containerPool = new List<T>(initialPoolSize);
        for(int i = 0; i < initialPoolSize; i++)
        {
            containerPool.Add(InstantiateObject());
        }
    }

    private T InstantiateObject()
    {
        Assert.IsNotNull(prefab, "prefab can not be null");
        var obj = Object.Instantiate(prefab, poolRoot);
        obj.OnInstantiate();
        obj.gameObject.SetActive(false);
        return obj;
    }

    public T GetFromPool(Vector3 position)
    {
        T obj = null;
        Assert.IsTrue(IsInitialized, $"Pool Type {prefab.GetType()} is not Initialized!");
        obj = GetSafePool();
        SetupObject(obj, position);

        return obj;
    }

    private T GetSafePool()
    {
        T obj = null;

        T GetUnsafeFromPool()
        {
            int index = containerPool.Count - 1;
            obj = containerPool[index];
            containerPool.RemoveAt(index);
            return obj;
        }

        obj = containerPool.Count > 0 ? GetUnsafeFromPool() : InstantiateObject();
        obj.OnGetFromPool();

        Assert.IsNotNull(obj, "Object cannot be null in GetSafePool context");

        return obj;
    }

    private void SetupObject(T obj, Vector3 position)
    {
        obj.transform.position = position;
        obj.transform.SetParent(poolRoot);
        obj.gameObject.SetActive(true);

    }

    public void ReturnToPool(T obj)
    {
        Assert.IsNotNull(obj, "return to pool parameter can not be null");
        obj.transform.SetParent(poolRoot);
        obj.OnReturnToPool();
        containerPool.Add(obj);
    }

}
