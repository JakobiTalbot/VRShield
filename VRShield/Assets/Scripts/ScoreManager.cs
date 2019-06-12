using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public Text m_scoreNumber;
    public Text m_multiplierNumber;

    private int m_nCurrentScore = 0;
    private int m_nCurrentMultiplier = 1;

    public void AddScore(int nScore)
    {
        m_nCurrentScore += nScore * m_nCurrentMultiplier;
        m_scoreNumber.text = m_nCurrentScore.ToString();
    }

    public void IncrementMultiplier()
    {
        ++m_nCurrentMultiplier;
        m_multiplierNumber.text = m_nCurrentMultiplier.ToString() + 'x';
    }

    public void ResetMultiplier()
    {
        m_nCurrentMultiplier = 1;
        m_multiplierNumber.text = m_nCurrentMultiplier.ToString() + 'x';
    }

    public int GetMultiplier() => m_nCurrentMultiplier;
}