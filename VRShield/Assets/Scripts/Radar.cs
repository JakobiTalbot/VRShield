using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    public GameObject m_radarUI;

    public List<GameObject> m_enemies;
    public List<GameObject> m_enemyUITokens;

    public GameObject m_enemyUITokenPrefab;

    public GameObject m_pivotPoint;

    public void AddEnemy(GameObject enemy)
    {
        m_enemies.Add(enemy);

        GameObject temp = Instantiate(m_enemyUITokenPrefab, m_radarUI.transform);
        m_enemyUITokens.Add(temp);
    }

    public void RemoveEnemy(GameObject enemy)
    {
        m_enemies.Remove(enemy);
        Destroy(m_enemyUITokens[m_enemyUITokens.Count - 1]);
        m_enemyUITokens.RemoveAt(m_enemyUITokens.Count - 1);
    }

    private void Update()
    {
        m_radarUI.transform.rotation = Quaternion.Euler(m_radarUI.transform.rotation.eulerAngles.x, m_radarUI.transform.rotation.eulerAngles.y, m_pivotPoint.transform.rotation.eulerAngles.y);

        for(int i = 0; i < m_enemyUITokens.Count; i++)
        {
            //m_enemyUITokens[i].transform.position = new Vector3(m_enemies[i].transform.position.x * (m_radarUI.transform.lossyScale.x * 0.5f), m_enemies[i].transform.position.z * (m_radarUI.transform.lossyScale.z * 0.5f), 0).normalized;
            m_enemyUITokens[i].transform.localPosition = new Vector3(m_enemies[i].transform.position.normalized.x * m_radarUI.GetComponent<RectTransform>().rect.width * 0.5f, m_enemies[i].transform.position.normalized.z * m_radarUI.GetComponent<RectTransform>().rect.height * 0.5f , 0.0f);  //m_enemies[i].transform.position.normalized * m_radarUI.transform.lossyScale.x;
            
        }
    }
}
