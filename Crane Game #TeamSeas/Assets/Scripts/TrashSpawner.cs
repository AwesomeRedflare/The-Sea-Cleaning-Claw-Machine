using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashSpawner : MonoBehaviour
{
    public Transform trashHolder;

    public int trashAmount;
    [HideInInspector]
    public int maxTrash;

    public GameObject[] trash;

    public List<Transform> spawnPoints;

    private void Start()
    {
        maxTrash = spawnPoints.Count;

        for (int i = 0; i < maxTrash && i < trashAmount; i++)
        {
            int n = Random.Range(0, spawnPoints.Count);

            Instantiate(trash[Random.Range(0, trash.Length)], spawnPoints[n].transform.position, Quaternion.Euler(0, 0, Random.Range(0f, 360f)), trashHolder);

            Destroy(spawnPoints[n].gameObject);
            spawnPoints.Remove(spawnPoints[n].transform);
        }
    }
}
