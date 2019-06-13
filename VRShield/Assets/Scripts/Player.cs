using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // time for the parry to remain active
    public float m_parryTime = 0.2f;
    // cooldown between being able to parry
    public float m_parryCooldown = 2f;
    // stores a reference to the shield object
    public GameObject m_physicsShield;
    public GameObject m_fakeShield;
    public AudioClip[] m_audioClips;

    // timer until the parry runs out
    private float m_fParryTimer = 0f;
    // timer until player can parry again
    private float m_fParryCooldownTimer = 0f;
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_audioSource = m_physicsShield.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        // decrement timers
        m_fParryTimer -= Time.deltaTime;
        m_fParryCooldownTimer -= Time.deltaTime;

        // parry
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger)
            && m_fParryCooldownTimer <= 0f)
        {
            m_fParryTimer = m_parryTime;
            m_fParryCooldownTimer = m_parryCooldown;
        }
        if (m_fParryTimer > 0f)
            m_physicsShield.GetComponent<Renderer>().material.color = new Color(1, 0, 0);
        else
            m_physicsShield.GetComponent<Renderer>().material.color = new Color(1, 1, 1);

        m_physicsShield.GetComponent<Rigidbody>().MovePosition(m_fakeShield.transform.position);
        m_physicsShield.GetComponent<Rigidbody>().MoveRotation(m_fakeShield.transform.rotation);
    }

    /// <summary>
    /// Resets the parry cooldown timer to 0
    /// </summary>
    public void ResetParryCooldown() => m_fParryCooldownTimer = 0f;

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

    public void PlayHitSound(float fSwingForce)
    {
        m_audioSource.pitch = Random.Range(0.8f, 1.2f);
        m_audioSource.volume = fSwingForce;
        m_audioSource.PlayOneShot(m_audioClips[Random.Range(0, m_audioClips.Length)]);
    }
}