using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_speed = 60f;
    public float m_speedIncreaseWhenParried = 60f;
    public float m_timeToDestroyAfterBlocked = 2f;
    public int m_scoreForKillingEnemy = 100;
    public int m_scoreForHittingProjectile = 50;
    public GameObject m_hitParticlePrefab;
    public GameObject m_explodeEnemyParticlePrefab;
    public GameObject m_scoreGainPopupPrefab;

    private GameObject m_projectileOwner;
    private Player m_player;
    private Vector3 m_v3TargetDirection;
    private bool m_bIsFiring = false;
    private bool m_bIsHoming = false;
    private Rigidbody m_rb;
    private ScoreManager m_scoreManager;

    void Awake()
    {
        m_scoreManager = FindObjectOfType<ScoreManager>();
        m_rb = GetComponent<Rigidbody>();
        m_player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        // in case other projectile kills owner
        if (!m_projectileOwner)
            Destroy(gameObject);

        if (m_bIsFiring)
            m_rb.velocity = m_v3TargetDirection * m_speed * Time.deltaTime;
        else if (m_bIsHoming)
            m_rb.velocity += (m_projectileOwner.transform.position - transform.position).normalized * (m_speed / 8f) * Time.deltaTime;
    }

    /// <summary>
    /// Sets the direction and rotation for the projectile to fire at
    /// </summary>
    /// <param name="v3TargetPosition">
    /// The target for the bullet to fire at
    /// </param>
    /// <param name="projectileOwner">
    /// The gameobject that fired the bullet
    /// </param>
    public void Fire(Vector3 v3TargetPosition, GameObject projectileOwner)
    {
        // set direction
        m_v3TargetDirection = (v3TargetPosition - transform.position).normalized;
        // start in front of enemy so it doesn't collide
        if (projectileOwner.CompareTag("Enemy"))
            transform.position += m_v3TargetDirection * projectileOwner.transform.localScale.z;
        // set projectile owner
        m_projectileOwner = projectileOwner;
        // rotate to face target
        transform.LookAt(v3TargetPosition);
        // enable firing
        m_bIsFiring = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ignore collision to other projectiles
        if (collision.gameObject.CompareTag("Projectile"))
        {
            Physics.IgnoreCollision(GetComponent<Collider>(), collision.collider);
            return;
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            CollideEnemy(collision);
            return;
        }
        else if (collision.gameObject.CompareTag("Shield"))
        {
            // play hit sound
            FindObjectOfType<Player>().PlayHitSound(1f);
        }
        if (m_bIsHoming
            || !m_bIsFiring)
            return;

        m_bIsFiring = false;
        // if projectile hits shield
        if (collision.gameObject.CompareTag("Shield"))
        {
            CollideShield(collision);
        }
        else if (collision.gameObject.CompareTag("Player")
                 && m_bIsHoming == false)
        {
            CollidePlayer(collision);
        }
    }

    private void CollideShield(Collision collision)
    {
        Player player = FindObjectOfType<Player>();
        float fForce = (player.m_physicsShield.GetComponent<Rigidbody>().angularVelocity.magnitude);
        // artificial velocity
        m_rb.AddForce(collision.GetContact(0).normal * fForce);
        // add score
        m_scoreManager.AddScore(m_scoreForHittingProjectile + (int)fForce);
        // the lord giveth, but he also taketh away
        Destroy(Instantiate(m_hitParticlePrefab, collision.GetContact(0).point, Quaternion.Euler(Vector3.zero)), 1f);
        // if shield is parrying
        if (m_player.IsParrying())
        {
            // reset parry cooldown
            m_player.ResetParryCooldown();
            m_bIsHoming = true;
            GetComponent<ParticleSystem>().Play();
        }
        else // if shield is hit and not parrying
        {
            m_rb.constraints = RigidbodyConstraints.None;
            Destroy(gameObject, m_timeToDestroyAfterBlocked);
        }
    }

    private void CollideEnemy(Collision collision)
    {
        Destroy(Instantiate(m_explodeEnemyParticlePrefab, collision.transform.position, Quaternion.Euler(Vector3.zero)), 2f);
        GameObject popup = Instantiate(m_scoreGainPopupPrefab, collision.transform.position, Quaternion.Euler(Vector3.zero));
        popup.GetComponent<ScoreGainText>().SetText((m_scoreForKillingEnemy * m_scoreManager.GetMultiplier()).ToString());
        popup.transform.LookAt(m_player.transform);
        // add score
        m_scoreManager.AddScore(m_scoreForKillingEnemy);
        m_scoreManager.IncrementMultiplier();
        // kill/damage enemy
        Destroy(collision.gameObject); // temp
        Destroy(gameObject);
        Destroy(Instantiate(m_explodeEnemyParticlePrefab, collision.transform.position, Quaternion.Euler(Vector3.zero)), 2f);
    }

    private void CollidePlayer(Collision collision)
    {
        m_scoreManager.ResetMultiplier();
        // damage player
        collision.gameObject.GetComponent<Player>().TakeDamage(1);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Radar r = FindObjectOfType<Radar>();
        if (r)
            r.RemoveProjectile(gameObject);
    }
}