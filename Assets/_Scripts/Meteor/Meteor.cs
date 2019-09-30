using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

[RequireComponent(typeof(Cutter), typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Explosion explosionPrefab;
    [Header("Explosion Damage")]
    [SerializeField] private AnimationCurve damageCurve;
    [SerializeField] private float maxDamageDistance;
    [Header("Explosion Size")]
    [SerializeField] private AnimationCurve explosionRadiusCurve;
    [SerializeField] private float lowExplosionHeight;
    [SerializeField] private float highExplosionHeight;

    [HideInInspector] public Vector2 meteorVelocity = new Vector2(-1, -3);
    
    private Cutter cutter;
    private Rigidbody2D rb;
    
    private void Start()
    {
        cutter = GetComponent<Cutter>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        var hitObjects = Physics2D.OverlapCircleAll(transform.position, maxDamageDistance);
        foreach (var hit in hitObjects)
        {
            var damageables = hit.GetComponents<IDamageable>();
            if (damageables.Length <= 0) continue;
            
            var distance = (transform.position - hit.transform.position).magnitude;
            foreach (var damageable in damageables)
            {
                damageable.Damage(damageCurve.Evaluate(distance));
            }
        }

        var height = transform.position.y;
        var explosionRadius = explosionRadiusCurve.Evaluate((height - lowExplosionHeight) / (highExplosionHeight - lowExplosionHeight));
        
        cutter.CutRadius = explosionRadius;
        cutter.Cut();
        
        Destroy(gameObject);

        var explosion = Instantiate(explosionPrefab);
        explosion.transform.position = transform.position;
        explosion.Explode(explosionRadius);
    }

    private void Update()
    {
        rb.MovePosition(rb.position + (meteorVelocity * Time.deltaTime));
    }
}
