using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Radar))]
public class EnemySpawner : MonoBehaviour
{
    public float m_spawnTimer = 10.0f;       //the time between enemy spawns

    public float m_spawnTimeMultiplier = 0.05f;
    public float m_minimumSpawnTime = 2.0f;

    Radar m_radar;

    public Vector2 m_spawnMinMaxHeight;     //the min and max height of the spawns
    public Vector2 m_spawnAngleRange = new Vector2(-100, 100 );

    public float m_spawnRadius;     //the radius at which the enemies spawn

    [Range(0,100)]
    public int m_basicEnemyChance = 50;
    public GameObject m_enemyPrefab;        //reference to the enemy prefab
    public GameObject m_movingEnemyPrefab;

    private float m_setTimer;       //time used to set the timer with multiplier
    private float m_timer;      //actual timer for enemy spawn

    private void Start()
    {
        m_timer = m_spawnTimer;     //set spawn time
        m_setTimer = m_spawnTimer;
        m_radar = GetComponent<Radar>();
    }

    private void Update()
    {
        m_timer -= Time.deltaTime;      //countdown time

        if (m_timer <= 0)       //if time has run out
        {
            SpawnEnemy();       //spawn enemy

            if (m_setTimer > m_minimumSpawnTime)
            {
                m_setTimer -= m_spawnTimer * m_spawnTimeMultiplier; //makes timer slightly shorter
            }
            if (m_setTimer < m_minimumSpawnTime)
            {
                m_setTimer = m_minimumSpawnTime;
            }

            m_timer += m_setTimer;        //reset time
        }
    }

    public void SpawnEnemy()        //spawn the enemy
    {
        float angle = Random.Range(m_spawnAngleRange.x, m_spawnAngleRange.y) + GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles.y;

        Vector3 v = new Vector3();
        v.x = m_spawnRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        v.z = m_spawnRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        v.y = Random.Range(m_spawnMinMaxHeight.x, m_spawnMinMaxHeight.y);

        //Vector2 tempPos = Random.insideUnitCircle.normalized * m_spawnRadius;       //find the x and z coord for where to put enemy
        //Vector3 spawnPos = new Vector3(tempPos.x, Random.Range(m_spawnMinMaxHeight.x, m_spawnMinMaxHeight.y), tempPos.y);        //puts x y z together

        GameObject temp;

        if (Random.Range(0, 100) < m_basicEnemyChance)
        {
            temp = Instantiate(m_enemyPrefab);       //creates enemy
        }
        else
        {
            temp = Instantiate(m_movingEnemyPrefab);       //creates enemy
        }

        m_radar.AddEnemy(temp);
        temp.transform.position = v;     //puts enemy into position
    }
}
