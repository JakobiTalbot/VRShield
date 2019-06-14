using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject m_blackImage;
    public GameObject[] m_texts;
    public GameObject m_backText;
    public float m_crossFadeTime = 2f;

    private bool m_bCanPressBack = false;

    // Start is called before the first frame update
    void Start()
    {
        m_blackImage.GetComponent<RawImage>().CrossFadeAlpha(0f, 0f, true);
        foreach (GameObject g in m_texts)
            g.GetComponent<Text>().CrossFadeAlpha(0f, 0f, true);
        m_backText.GetComponent<Text>().CrossFadeAlpha(0f, 0f, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.Back) && m_bCanPressBack)
            SceneManager.LoadScene(0);
    }

    public void StartDisplaying()
    {
        m_blackImage.GetComponent<RawImage>().CrossFadeAlpha(1f, m_crossFadeTime, true);
        Invoke("CrossFadeText", m_crossFadeTime);
    }
    
    private void CrossFadeText()
    {
        foreach (GameObject g in m_texts)
            g.GetComponent<Text>().CrossFadeAlpha(1f, m_crossFadeTime, true);
        Invoke("CrossFadeBackText", m_crossFadeTime);
    }

    private void CrossFadeBackText()
    {
        m_backText.GetComponent<Text>().CrossFadeAlpha(1f, m_crossFadeTime, true);
        Invoke("EnableBackButton", m_crossFadeTime);
    }

    private void EnableBackButton() => m_bCanPressBack = true;
}
