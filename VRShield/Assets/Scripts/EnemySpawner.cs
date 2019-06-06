using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float m_spawnTimer = 3.0f;

    public Vector2 m_spawnMinMaxHeight;
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
        Vector2 tempLoc = Random.insideUnitCircle.normalized * m_spawnRadius;
        Vector3 spawnLoc = new Vector3(tempLoc.x, 0, tempLoc.y);
        spawnLoc.y = Random.Range(m_spawnMinMaxHeight.x, m_spawnMinMaxHeight.y);

        //Vector3 v3SpawnLocation = Random.insideUnitSphere.normalized * m_spawnRadius;
        //v3SpawnLocation.y = Mathf.Abs(v3SpawnLocation.y);
        GameObject temp = Instantiate(m_enemyPrefab);
        temp.transform.position = spawnLoc;
    }
}
