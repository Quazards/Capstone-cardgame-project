using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public GameObject objectPrefab;
    public int poolSize;
    private List<GameObject> objectPool;

    private void Start()
    {
        InitializePool();
    }
    private GameObject CreateObjects()
    {
        GameObject spawnedObject = Instantiate(objectPrefab, transform);
        spawnedObject.transform.position = transform.position;
        spawnedObject.SetActive(false);
        objectPool.Add(spawnedObject);
        return spawnedObject;
    }

    private void InitializePool()
    {
        objectPool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            CreateObjects();
        }
    }

    public GameObject GetPooledObject()
    {
        foreach (GameObject obj in objectPool)
        {

            if (!obj.activeInHierarchy)
            {
                return obj;
            }
        }
        return CreateObjects();
    }
}
