using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    // time for the parry to remain active
    public float m_parryTime = 0.2f;
    // cooldown between being able to parry
    public float m_parryCooldown = 2f;
    // stores a reference to the shield object
    public GameObject m_physicsShield;
    public GameObject m_fakeShield;
    public Text m_livesText;
    public AudioClip[] m_hitProjectileAudioClips;
    public AudioClip m_hurtAudioClip;
    public AudioClip m_deathAudioClip;
    public int m_startingLivesCount = 5;
    public int m_hitsNeededToBeAbleToParryAgain = 3;

    // timer until the parry runs out
    private float m_fParryTimer = 0f;
    private int m_nCurrentLivesCount;
    private AudioSource m_audioSource;
    private int m_nHitsSinceLastParry = 0;

    private void Awake()
    {
        m_audioSource = m_physicsShield.GetComponent<AudioSource>();
        m_nCurrentLivesCount = m_startingLivesCount;
        m_livesText.text = m_nCurrentLivesCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        // decrement timers
        m_fParryTimer -= Time.deltaTime;

        // parry
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)
            && m_nHitsSinceLastParry >= 3)
        {
            m_fParryTimer = m_parryTime;
            m_nHitsSinceLastParry = 0;
        }

        // set bat colour
        if (m_fParryTimer > 0f)
            m_physicsShield.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
        else if (m_nHitsSinceLastParry >= 3)
            m_physicsShield.GetComponent<Renderer>().material.color = new Color(0, 1, 0);
        else
            m_physicsShield.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

        m_physicsShield.GetComponent<Rigidbody>().MovePosition(m_fakeShield.transform.position);
        m_physicsShield.GetComponent<Rigidbody>().MoveRotation(m_fakeShield.transform.rotation);
    }

    /// <summary>
    /// Returns the parry state of the player's shield
    /// </summary>
    /// <returns>
    /// Whether the shield should parry or not
    /// </returns>
    public bool IsParrying()
    {
        return m_fParryTimer > 0f;
    }

    public void PlayHitSound(float fVolume)
    {
        m_audioSource.pitch = Random.Range(0.8f, 1.2f);
        m_audioSource.volume = fVolume;
        m_audioSource.PlayOneShot(m_hitProjectileAudioClips[Random.Range(0, m_hitProjectileAudioClips.Length)]);
    }

    public void IncrementHits() => m_nHitsSinceLastParry++;

    public void TakeDamage(int nDamage)
    {
        m_nCurrentLivesCount -= nDamage;
        m_livesText.text = m_nCurrentLivesCount.ToString();
        // if out of lives
        if (m_nCurrentLivesCount == 0)
        {
            // die
            m_audioSource.PlayOneShot(m_deathAudioClip);
            FindObjectOfType<GameOverScreen>().StartDisplaying();
            FindObjectOfType<Console>().gameObject.SetActive(false);
            FindObjectOfType<EnemySpawner>().enabled = false;
            foreach (GameObject enemy in FindObjectOfType<Radar>().m_enemies)
                Destroy(enemy);
            m_physicsShield.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            // play hurt sound
            m_audioSource.PlayOneShot(m_hurtAudioClip);
        }
    }
}