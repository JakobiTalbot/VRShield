using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public GameObject m_radarUI;

    //EnemySpawner m_enemySpawner;

    List<GameObject> m_enemies;
    public List<GameObject> m_enemyUITokens;

    public GameObject m_enemyUITokenPrefab;

    public void AddEnemy(GameObject enemy)
    {
        m_enemies.Add(enemy);

        GameObject temp = Instantiate(m_enemyUITokenPrefab, m_radarUI.transform);
        m_enemyUITokens.Add(temp);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        m_enemies.Remove(enemy);
    }

    private void Update()
    {
        for(int i = 0; i < m_enemyUITokens.Count; i++)
        {
            m_enemyUITokens[i].transform.position = m_enemies[i].transform.position * m_radarUI.transform.lossyScale.x;
        }
    }
}
