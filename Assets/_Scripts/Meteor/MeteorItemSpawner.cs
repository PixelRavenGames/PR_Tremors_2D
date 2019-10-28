using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorItemSpawner : MonoBehaviour
{
    public GameObject[] possibleItems;
    public float spawnChance;
    private bool willSpawn;
    
    void Start()
    {
        willSpawn = Random.RandomRange(0, 1f) < spawnChance;
    }

    private void OnDestroy()
    {
        if (willSpawn)
        {
            var powerUp = Instantiate(possibleItems[Random.Range(0, possibleItems.Length)]);
            powerUp.transform.position = transform.position;
        }
    }
}
