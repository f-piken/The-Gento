using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : Singleton<ObjectPool>
{
    [SerializeField]
    private List<GameObject> pooledObjects = new List<GameObject>();

    [SerializeField]
    private List<PoolData> pool = new List<PoolData>();

    [SerializeField]
    private int amount = 10;

    [SerializeField]
    private Transform spawnPoint;

    public override void Initialization()
    {
    }

    public IEnumerator GeneratingPool()
    {
        for (int j = 0; j < pooledObjects.Count; j++)
        {
            List<GameObject> obj = new List<GameObject>();

            for (int i = 0; i < amount; i++)
            {
                GameObject x = Instantiate(pooledObjects[j], spawnPoint);

                x.SetActive(false);
                x.gameObject.name = pooledObjects[j].name;

                obj.Add(x);
            }

            pool.Add(new PoolData($"{pooledObjects[j].name}", obj));
        }

        return null;
    }

    #region SPAWNING

    private PoolData findPoolData(string _name)
    {
        if (string.IsNullOrEmpty(name))
        {
            Debug.LogError("OBJECT POOLING: Cant find with empty string");
            return null;
        }

        PoolData pd = pool.Find((a) => a.name == _name);

        if (pd == null)
        {
            Debug.LogError($"OBJECT POOLING: Cant find {_name}");

            return null;
        }

        return pd;
    }

    public GameObject Spawn(string name, Transform parent)
    {
        PoolData currentPool = findPoolData(name);

        if (currentPool.objects.Count <= 0)
        {
            Debug.LogError($"OBJECT POOLING: Pool {name} is empty");
            return null;
        }

        GameObject x = currentPool.objects[0].gameObject;

        x.transform.position = parent.position;
        x.transform.rotation = parent.rotation;
        x.transform.SetParent(parent);
        x.SetActive(true);

        currentPool.objects.Remove(x);

        return x;
    }

    #endregion SPAWNING

    public void BackToPool(GameObject back)
    {
        back.SetActive(false);
        back.transform.SetParent(spawnPoint);
        findPoolData(back.name).objects.Add(back);
    }
}

[System.Serializable]
public class PoolData
{
    public string name;

    public List<GameObject> objects = new List<GameObject>();

    public PoolData(string _name, List<GameObject> _objects)
    {
        name = _name;
        objects = _objects;
    }
}