using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPitchOnStart : MonoBehaviour
{
    public Vector2 m_minMaxPitch;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<AudioSource>().pitch = Random.Range(m_minMaxPitch.x, m_minMaxPitch.y);
    }
}