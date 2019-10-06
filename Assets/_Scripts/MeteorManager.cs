using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorManager : MonoBehaviour
{
    [HideInInspector] public List<GameObject> meteors;

    [Header("Prefabs")]
    [SerializeField] private Meteor meteorPrefab;
    [Header("Settings")]
    [SerializeField] private AnimationCurve meteorRate;
    [SerializeField] private float maxMeteorTime;
    [SerializeField] private BoxCollider2D spawnArea;
    private float timePassed = 0f;
    
    void Update()
    {
        timePassed += Time.deltaTime;
        //print("---");
        //print(meteorRate.Evaluate(timePassed / maxMeteorTime));
        //print(Time.deltaTime);
        //print(Time.deltaTime * meteorRate.Evaluate(timePassed / maxMeteorTime));
        if (Random.Range(0f, 1f) < Time.deltaTime * meteorRate.Evaluate(timePassed / maxMeteorTime))
        {
            var meteor = Instantiate(meteorPrefab);
            var colliderPos = (Vector2) spawnArea.transform.position + spawnArea.offset;
            meteor.transform.position = new Vector3(
                Random.Range(colliderPos.x - spawnArea.size.x / 2, colliderPos.x + spawnArea.size.x / 2),
                Random.Range(colliderPos.y - spawnArea.size.y / 2, colliderPos.y + spawnArea.size.y / 2),
                0
            );
            meteor.meteorVelocity = new Vector2(Random.Range(-1.5f, 1.5f), Random.Range(-5.5f, -6.5f));
        }
    }
}
