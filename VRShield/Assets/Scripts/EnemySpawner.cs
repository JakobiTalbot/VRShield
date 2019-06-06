using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float m_spawnTimer = 3.0f;
    public float m_spawnRadius;
    public GameObject m_enemyPrefab;

    private float m_timer;

    private void Start()
    {
        m_timer = m_spawnTimer;
    }

    private void Update()
    {
        m_timer -= Time.deltaTime;

        if (m_timer <= 0)
        {
            SpawnEnemy();
            m_timer += m_spawnTimer;
        }
    }

    public void SpawnEnemy()
    {
        Vector3 v3SpawnLocation = Random.insideUnitSphere.normalized * m_spawnRadius;
        v3SpawnLocation.y = Mathf.Abs(v3SpawnLocation.y);
        GameObject temp = Instantiate(m_enemyPrefab);
        temp.transform.position = v3SpawnLocation;
    }
}
