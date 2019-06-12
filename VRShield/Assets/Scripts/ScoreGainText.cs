using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreGainText : MonoBehaviour
{
    public float m_upSpeed = 1f;
    public float m_displayTime = 1f;

    private float m_fLerp = 1f;
    private Text m_text;

    private void Awake()
    {
        m_text = GetComponentInChildren<Text>();
    }

    void FixedUpdate()
    {
        // move up
        transform.position += Vector3.up * m_upSpeed * Time.fixedDeltaTime;
        // fade out
        m_fLerp -= Time.fixedDeltaTime / m_displayTime;
        Color c = m_text.color;
        c.a = Mathf.Lerp(0f, 1f, m_fLerp);
        m_text.color = c;
        if (m_text.color.a <= 0f)
            Destroy(gameObject);
    }

    public void SetText(string szText)
    {
        m_text.text = szText;
    }
}