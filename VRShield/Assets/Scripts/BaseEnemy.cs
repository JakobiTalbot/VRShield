using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseEnemy : MonoBehaviour
{
    public GameObject m_projectilePrefab;

    public float m_firstShootTime = 2.0f;
    public float m_shootTime = 10.0f;
    protected float m_timer;

    void Start()
    {
        m_timer = m_firstShootTime;
        transform.LookAt(FindObjectOfType<Player>().transform);
    }

    protected abstract void Update();


    public void Shoot()
    {
        GameObject projectile = Instantiate(m_projectilePrefab, transform.position, Quaternion.Euler(Vector3.zero));
        projectile.GetComponent<Projectile>().Fire(Camera.main.transform.position, this.gameObject);

        Radar r = FindObjectOfType<Radar>();
        if (r)
            r.AddProjectile(projectile);
    }

}
