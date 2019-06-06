using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float m_speed = 10f;

    private GameObject m_projectileOwner;
    private Player m_player;
    private Vector3 m_v3TargetDirection;
    private bool m_bIsFiring = false;

    void Awake()
    {
        m_player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        if (!m_bIsFiring)
            return;

        transform.position += m_v3TargetDirection * m_speed * Time.deltaTime;
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
        // set projectile owner
        m_projectileOwner = projectileOwner;
        // rotate to face target
        transform.LookAt(v3TargetPosition);
        // enable firing
        m_bIsFiring = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == m_projectileOwner)
            return;

        // if projectile hits shield
        if (collision.gameObject.CompareTag("Shield"))
        {
            // if shield is parrying
            if (m_player.IsParrying())
            {
                // reset parry cooldown
                m_player.ResetParryCooldown();
                // return to sender
                Fire(m_projectileOwner.transform.position, Camera.main.gameObject);
            }
            else
            {
                // destroy projectile
                Destroy(gameObject);
            }
        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            // kill/damage enemy
            Destroy(collision.gameObject); // temp
        }
    }
}