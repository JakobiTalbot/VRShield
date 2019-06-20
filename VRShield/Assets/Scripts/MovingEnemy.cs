using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemy : MonoBehaviour
{
    public GameObject m_projectilePrefab;
    public float m_movementSlerpSpeed = 1f;

    public float m_firstShootTime = 2.0f;
    public float m_shootTime = 10.0f;
    private float m_timer;
    private Vector3 m_v3DestinationPoint;
    private Vector3 m_v3StartPoint;
    private Vector2 m_v2MoveAngleRange;
    private Vector2 m_v2MoveMinMaxHeight;
    private float m_fMoveRadius;
    private Rigidbody m_rb;
    private float m_fCurrentSlerp = 0f;

    void Start()
    {
        m_timer = m_firstShootTime;
        m_rb = GetComponent<Rigidbody>();
        transform.LookAt(FindObjectOfType<Player>().transform);
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        m_v2MoveAngleRange = enemySpawner.m_spawnAngleRange;
        m_v2MoveMinMaxHeight = enemySpawner.m_spawnMinMaxHeight;
        m_fMoveRadius = enemySpawner.m_spawnRadius;
        GetNewMovementPoint();
    }

    void Update()
    {
        m_timer -= Time.deltaTime;
        if (m_timer <= 0)
        {
            Shoot();
            m_timer += m_shootTime;
        }
        // move
        m_fCurrentSlerp += Time.deltaTime * (m_movementSlerpSpeed / Vector3.Distance(m_v3StartPoint, m_v3DestinationPoint));
        // get new point if at point
        if (m_fCurrentSlerp >= 1f)
        {
            m_rb.MovePosition(m_v3DestinationPoint);
            GetNewMovementPoint();
            m_fCurrentSlerp = 0f;
        }
        transform.position = (Vector3.Slerp(m_v3StartPoint, m_v3DestinationPoint, m_fCurrentSlerp));
        // face player
        transform.LookAt(FindObjectOfType<Player>().transform);
    }

    public void Shoot()
    {
        GameObject projectile = Instantiate(m_projectilePrefab, transform.position, Quaternion.Euler(Vector3.zero));
        projectile.GetComponent<Projectile>().Fire(Camera.main.transform.position, this.gameObject);

        Radar r = FindObjectOfType<Radar>();
        if (r)
            r.AddProjectile(projectile);
    }

    private void GetNewMovementPoint()
    {
        float angle = Random.Range(m_v2MoveAngleRange.x, m_v2MoveAngleRange.y) + GameObject.FindGameObjectWithTag("MainCamera").transform.eulerAngles.y;
        m_v3DestinationPoint.x = m_fMoveRadius * Mathf.Sin(angle * Mathf.Deg2Rad);
        m_v3DestinationPoint.z = m_fMoveRadius * Mathf.Cos(angle * Mathf.Deg2Rad);
        m_v3DestinationPoint.y = Random.Range(m_v2MoveMinMaxHeight.x, m_v2MoveMinMaxHeight.y);
        m_v3StartPoint = transform.position;
    }

    private void OnDestroy()
    {
        Radar r = FindObjectOfType<Radar>();
        if (r)
            r.RemoveEnemy(gameObject);
    }
}