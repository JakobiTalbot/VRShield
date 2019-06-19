using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public GameObject m_blackImage;
    public Text m_scoreNum;
    public Text m_highScoreNum;
    public Text[] m_texts;
    public Text m_backText;
    public float m_crossFadeTime = 2f;
    public float m_scoreLerpSpeed = 0.2f;

    private float m_fCurrentScoreValue = 0;
    private bool m_bCanPressBack = false;
    private bool m_bLerpScore = false;


    // Start is called before the first frame update
    void Start()
    {
        m_blackImage.GetComponent<RawImage>().CrossFadeAlpha(0f, 0f, true);
        foreach (Text g in m_texts)
            g.CrossFadeAlpha(0f, 0f, true);
        m_backText.CrossFadeAlpha(0f, 0f, true);
        m_scoreNum.CrossFadeAlpha(0f, 0f, true);
        m_highScoreNum.CrossFadeAlpha(0f, 0f, true);
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bLerpScore)
        {
            int nScore = FindObjectOfType<ScoreManager>().GetScore();
            // lerp score
            m_fCurrentScoreValue = Mathf.Lerp(m_fCurrentScoreValue, nScore, m_scoreLerpSpeed);
            // if reached score
            if (m_fCurrentScoreValue >= nScore - (nScore * 0.01f))
            {
                m_fCurrentScoreValue = nScore;
                // if new high score
                if (nScore > PlayerPrefs.GetInt("highscore"))
                {
                    // set new highscore
                    PlayerPrefs.SetInt("highscore", nScore);
                    m_highScoreNum.text = nScore.ToString() + "!";
                }
                CrossFadeBackText();
                m_bLerpScore = false;
            }
            m_scoreNum.text = ((int)m_fCurrentScoreValue).ToString();
        }
        if (OVRInput.GetDown(OVRInput.Button.Back) && m_bCanPressBack)
            SceneManager.LoadScene(0);
    }

    public void StartDisplaying()
    {
        m_highScoreNum.text = PlayerPrefs.GetInt("highscore").ToString();

        m_blackImage.GetComponent<RawImage>().CrossFadeAlpha(1f, m_crossFadeTime, true);
        Invoke("CrossFadeGameOver", m_crossFadeTime);
    }
    
    private void CrossFadeGameOver()
    {
        m_texts[0].CrossFadeAlpha(1f, m_crossFadeTime, true);
        Invoke("CrossFadeHighScore", m_crossFadeTime);
    }

    private void CrossFadeHighScore()
    {
        // cross fade in highscore texts
        m_texts[2].CrossFadeAlpha(1f, m_crossFadeTime, true);
        m_highScoreNum.CrossFadeAlpha(1f, m_crossFadeTime, true);
        Invoke("CrossFadeScore", m_crossFadeTime);
    }

    private void CrossFadeScore()
    {
        m_texts[1].CrossFadeAlpha(1f, m_crossFadeTime, true);
        m_scoreNum.CrossFadeAlpha(1f, m_crossFadeTime, true);
        m_bLerpScore = true;
    }

    private void CrossFadeBackText()
    {
        m_backText.CrossFadeAlpha(1f, m_crossFadeTime, true);
        Invoke("EnableBackButton", m_crossFadeTime);
    }

    private void EnableBackButton() => m_bCanPressBack = true;
}
