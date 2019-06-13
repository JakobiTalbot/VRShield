using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Console : MonoBehaviour
{
    public float m_lerpSpeed = 0.3f;

    public GameObject m_mainCamera;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        // Lerps the UI
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, m_mainCamera.transform.rotation.eulerAngles.y, 0), m_lerpSpeed);
    }
}