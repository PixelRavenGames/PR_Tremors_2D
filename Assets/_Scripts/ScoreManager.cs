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
    public bool GameActive = true;
    
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
        if (!GameActive)
        {
            return;
        }
        
        var multipliers = new float[PlayerManager.instance.humans.Length];
            
        for(var i = 0; i < scores.Length; i++)
        {
            var human = PlayerManager.instance.humans[i];
            if (human.IsAlive())
            {
                multipliers[i] = Mathf.Max(1, human.transform.position.y - groundY);
                scores[i] += Time.deltaTime * multipliers[i];
            }
        }

        var sorted = scores.Select((x, i) => new KeyValuePair<int, float>(i, x))
            .OrderBy(x => x.Value).Reverse();

        scoreOrder = sorted.Select(x => x.Key).ToArray();

        for (var place = 0; place < scoreOrder.Length; place++)
        {
            var score_i = scoreOrder[place];
            if (score_i < scoreIndicators.Length)
            {
                var fontSizes = new float[] {14, 13, 12, 11};
                
                scoreIndicators[score_i].fontSize = place < fontSizes.Length ? fontSizes[place] : fontSizes[fontSizes.Length - 1];
                
                scoreIndicators[score_i].gameObject.transform.SetAsLastSibling();
            }
        }

        for (var i = 0; i < scoreIndicators.Length; i++)
        {
            if (i < scores.Length)
            {
                var multiplier = multipliers[i].ToString("F1");
                var score = scores[i].ToString("F0");
                scoreIndicators[i].text = $"P{i + 1} x{multiplier}\n{score}";
            }
        }
    }

	public float[] GetScores()
	{
		return scores;
	}

}
