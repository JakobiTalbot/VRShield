using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BaseEnemy
{
    override protected void Update()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            Shoot();
            m_timer += m_shootTime;
        }
    }

    private void OnDestroy()
    {
        Radar r = FindObjectOfType<Radar>();
        if (r)
            r.RemoveEnemy(gameObject);
    }
}