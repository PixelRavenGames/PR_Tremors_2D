using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.XR.WSA;

public class ParticleSplatter : MonoBehaviour
{
    [SerializeField] private ParticleSystem particles;

    public void Splatter(Vector3 position, float weight)
    {
        var splatter = Instantiate(particles);
        splatter.transform.position = position;

        
        splatter.emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0.0f, (short) (30 * weight), (short) (40 * weight))
        });
        
        /*var emission = splatter.emission;
        
        emission.rateOverTimeMultiplier *= weight;
        
        var bursts = new ParticleSystem.Burst[emission.burstCount];
        emission.GetBursts(bursts);

        foreach (var burst in bursts)
        {
            var curve = burst.count;
            curve.curveMultiplier *= weight;
        }
        
        splatter.emission.SetBursts(bursts);*/
    }
}
