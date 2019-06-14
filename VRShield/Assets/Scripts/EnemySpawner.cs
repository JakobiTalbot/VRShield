﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Radar))]
public class EnemySpawner : MonoBehaviour
{
    public float m_spawnTimer = 10.0f;       //the time between enemy spawns

    public float m_spawnTimeMultiplier = 0.05f;
    public float m_minimumSpawnTime = 2.0f;

    public Radar m_radar;

    public Vector2 m_spawnMinMaxHeight;     //the min and max height of the spawns
    public float m_spawnRadius;     //the radius at which the enemies spawn
    public GameObject m_enemyPrefab;        //reference to the enemy prefab

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
        Vector2 tempLoc = Random.insideUnitCircle.normalized * m_spawnRadius;       //find the x and z coord for where to put enemy
        Vector3 spawnLoc = new Vector3(tempLoc.x, Random.Range(m_spawnMinMaxHeight.x, m_spawnMinMaxHeight.y), tempLoc.y);        //puts x y z together

        GameObject temp = Instantiate(m_enemyPrefab);       //creates enemy
        temp.transform.position = spawnLoc;     //puts enemy into position

        m_radar.AddEnemy(temp);
    }
}
