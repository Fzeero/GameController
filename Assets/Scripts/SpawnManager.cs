using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [Header("Prefab & Pool")]
    public GameObject obstaclePrefab;        
    public int poolSize = 10;

    [Header("Spawn settings")]
    public float spawnInterval = 2f;
    private float timer = 0f;
    public float[] laneY = { 0.5f, -2.5f, -4f }; 
    public float spawnX = 15f;

    private List<GameObject> pool;

    void Awake()
    {
        if (obstaclePrefab == null)
        {
            Debug.LogError("[SpawnManager] obstaclePrefab belum di-assign!");
            enabled = false;
            return;
        }

        // buat pool awal
        pool = new List<GameObject>(poolSize);
        for (int i = 0; i < poolSize; i++)
        {
            GameObject go = Instantiate(obstaclePrefab, transform); 
            go.SetActive(false);
            pool.Add(go);
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            SpawnObstacle();
            timer = 0f;
        }
    }

    private GameObject GetPooledObject()
    {
        // cari objek yang tidak aktif
        for (int i = 0; i < pool.Count; i++)
        {
            if (!pool[i].activeInHierarchy)
                return pool[i];
        }

        // kalau pool penuh, expand pool (opsional)
        GameObject go = Instantiate(obstaclePrefab, transform);
        go.SetActive(false);
        pool.Add(go);
        return go;
    }

    private void SpawnObstacle()
    {
        int laneIndex = Random.Range(0, laneY.Length);
        Vector3 spawnPos = new Vector3(spawnX, laneY[laneIndex], 0f);

        GameObject obj = GetPooledObject();
        obj.transform.position = spawnPos;
        obj.transform.rotation = Quaternion.identity;
        obj.SetActive(true);
    }

    public void CleanPool()
    {
        for (int i = pool.Count - 1; i >= 0; i--)
        {
            if (pool[i] == null) pool.RemoveAt(i);
        }
    }
}
