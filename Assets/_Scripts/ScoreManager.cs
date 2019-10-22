using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public float groundY;
    private float[] scores;
    private int[] scoreOrder;
    public TextMeshProUGUI[] scoreIndicators;
    void Start()
    {
        scores = new float[PlayerManager.instance.humans.Length];
        scoreOrder = new int[PlayerManager.instance.humans.Length];
        for (var i = 0; i < scoreIndicators.Length; i++)
        {
            if (i < scores.Length)
            {
                scoreIndicators[i].text = $"P{i + 1}: {scores[i]}";
            }
            else
            {
                scoreIndicators[i].gameObject.SetActive(false);
            }
        }
    }

    void Update()
    {
        for(var i = 0; i < scores.Length; i++)
        {
            var human = PlayerManager.instance.humans[i];
            if (human.IsAlive())
            {
                scores[i] += Time.deltaTime * (Mathf.Max(1, human.transform.position.y - groundY));
            }
        }

        var sorted = scores.Select((x, i) => new KeyValuePair<int, float>(i, x))
            .OrderBy(x => x.Value).Reverse();

        scoreOrder = sorted.Select(x => x.Key).ToArray();

        foreach (var i in scoreOrder)
        {
            if (i < scoreIndicators.Length)
            {
                scoreIndicators[i].gameObject.transform.SetAsLastSibling();
            }
        }

        for (var i = 0; i < scoreIndicators.Length; i++)
        {
            if (i < scores.Length)
            {
                scoreIndicators[i].text = $"P{i + 1}: {(int) (scores[i] * 10)}";
            }
        }
    }

	public float[] GetScores()
	{
		return scores;
	}

}
