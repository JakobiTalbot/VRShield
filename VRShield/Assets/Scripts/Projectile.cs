using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_speed = 60f;
    public float m_speedIncreaseWhenParried = 60f;
    public float m_timeToDestroyAfterBlocked = 2f;
    public GameObject m_hitParticle;

    private GameObject m_projectileOwner;
    private Player m_player;
    private Vector3 m_v3TargetDirection;
    private bool m_bIsFiring = false;
    private bool m_bIsHoming = false;
    private Rigidbody m_rb;

    void Awake()
    {
        m_rb = GetComponent<Rigidbody>();
        m_player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
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
        if (collision.gameObject == m_projectileOwner
            && !m_bIsHoming)
            return;

        m_bIsFiring = false;
        // if projectile hits shield
        if (collision.gameObject.CompareTag("Shield"))
        {
            // artificial velocity
            m_rb.AddForce(collision.GetContact(0).normal * (FindObjectOfType<Player>().m_physicsShield.GetComponent<Rigidbody>().angularVelocity.magnitude));
            // if shield is parrying
            if (m_player.IsParrying())
            {
                // reset parry cooldown
                m_player.ResetParryCooldown();
                m_bIsHoming = true;
            }
            else // if shield is hit and not parrying
            {
                m_rb.constraints = RigidbodyConstraints.None;
                Destroy(gameObject, m_timeToDestroyAfterBlocked);
            }
            // the lord giveth, but he also taketh
            Destroy(Instantiate(m_hitParticle, collision.GetContact(0).point, Quaternion.Euler(Vector3.zero)), 1f);
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // kill/damage enemy
            Destroy(collision.gameObject); // temp
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Player")
                 && m_bIsHoming == false)
        {
            Destroy(gameObject);
        }
    }
}