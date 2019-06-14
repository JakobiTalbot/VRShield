using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject m_projectilePrefab;

    public float m_firstShootTime = 2.0f;
    public float m_shootTime = 10.0f;
    private float m_timer;

    void Start()
    {
        m_timer = m_firstShootTime;
    }

    void Update()
    {
        m_timer -= Time.deltaTime;
        if(m_timer <=0)
        {
            Shoot();
            m_timer += m_shootTime;
        }
    }

    public void Shoot()
    {
        GameObject projectile = Instantiate(m_projectilePrefab, transform.position, Quaternion.Euler(Vector3.zero));
        projectile.GetComponent<Projectile>().Fire(Camera.main.transform.position, this.gameObject);

        Radar r = FindObjectOfType<Radar>();
        if (r)
            r.AddProjectile(projectile);
    }
    private void OnDestroy()
    {
        Radar r = FindObjectOfType<Radar>();
        if (r)
            r.RemoveEnemy(gameObject);
    }
}