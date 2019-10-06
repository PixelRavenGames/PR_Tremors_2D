using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngineInternal;

[RequireComponent(typeof(Cutter), typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private Explosion explosionPrefab;
    [Header("Sounds")]
    [SerializeField] private AudioClip[] fallingSounds;
    [SerializeField] private AudioClip[] explosionSounds;
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
    private AudioSource asc;
    private CinemachineCollisionImpulseSource collisionImpulse;
    
    private void Start()
    {
        cutter = GetComponent<Cutter>();
        rb = GetComponent<Rigidbody2D>();
        asc = GetComponent<AudioSource>();
        collisionImpulse = GetComponent<CinemachineCollisionImpulseSource>();
            
        asc.PlayOneShot(fallingSounds[Random.Range(0, fallingSounds.Length)]);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Touchdown();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Touchdown();
    }
    
    private void Touchdown() {
        var hitObjects = Physics2D.OverlapCircleAll(transform.position, maxDamageDistance);
        foreach (var hit in hitObjects)
        {
            var damageables = hit.GetComponents<IDamageable>();
            if (damageables.Length > 0)
            {
                var distance = (transform.position - hit.transform.position).magnitude;
                foreach (var damageable in damageables)
                {
                    damageable.Damage(damageCurve.Evaluate(distance));
                }
            }
            
            var particleSplatter = hit.GetComponent<ParticleSplatter>();
            if (particleSplatter != null)
            {
                print(1f / hitObjects.Length);
                particleSplatter.Splatter(transform.position, 1f / hitObjects.Length);
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
        
        var meteor_asc = explosion.GetComponent<AudioSource>();
        if (meteor_asc)
        {
            meteor_asc.PlayOneShot(explosionSounds[Random.Range(0, explosionSounds.Length)]);
        }
    }

    private void Update()
    {
        rb.MovePosition(rb.position + (meteorVelocity * Time.deltaTime));
        /*var rot = transform.rotation;
        var eulerAngles = rot.eulerAngles;
        eulerAngles.z = Mathf.Rad2Deg * (Mathf.Atan2(meteorVelocity.y, meteorVelocity.x));
        rot.eulerAngles = eulerAngles;
        transform.rotation = rot;*/
        
        var angle = Mathf.Atan2(meteorVelocity.y, meteorVelocity.x) * Mathf.Rad2Deg + 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        var height = transform.position.y;
        collisionImpulse.m_ImpulseDefinition.m_AmplitudeGain = explosionRadiusCurve.Evaluate((height - lowExplosionHeight) / (highExplosionHeight - lowExplosionHeight));
    }
}
