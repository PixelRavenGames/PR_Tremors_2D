using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private float[] scores;
    private int[] scoreOrder;
    void Start()
    {
        scores = new float[PlayerManager.instance.humans.Length];
        scoreOrder = new int[PlayerManager.instance.humans.Length];
    }

    void Update()
    {
        for(var i = 0; i < scores.Length; i++)
        {
            var human = PlayerManager.instance.humans[i];
            if (human.IsAlive())
            {
                scores[i] += Time.deltaTime * (Mathf.Max(1, human.transform.position.y));
            }
        }

        var sorted = scores.Select((x, i) => new KeyValuePair<int, float>())
            .OrderBy(x => x.Key);

        scoreOrder = sorted.Select(x => x.Key).ToArray();
        
    }
}
