using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    public GameObject m_hand;
    public float m_maxRaycastDistance = 20f;
    public Color m_buttonDownStartColor;
    public Color m_buttonDownEndColor;
    public LineRenderer m_lineRenderer;

    private RaycastHit m_lastHit;
    private OVRRaycaster m_ovrRaycaster;
    private Color m_defaultStartColor;
    private Color m_defaultEndColor;

    // Start is called before the first frame update
    void Awake()
    {
        m_ovrRaycaster = GetComponent<OVRRaycaster>();
        m_defaultStartColor = m_lineRenderer.startColor;
        m_defaultEndColor = m_lineRenderer.endColor;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(m_hand.transform.position, m_hand.transform.forward, out m_lastHit, m_maxRaycastDistance))
        {
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                m_lineRenderer.startColor = m_buttonDownStartColor;
                m_lineRenderer.endColor = m_buttonDownEndColor;

                // if raycast hits UI
                if (m_lastHit.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
                {
                    // press button
                    BaseEventData baseEvent = new BaseEventData(EventSystem.current);
                    ExecuteEvents.Execute(m_lastHit.collider.gameObject, baseEvent, ExecuteEvents.submitHandler);
                }
            }
            else
            {
                m_lineRenderer.startColor = m_defaultStartColor;
                m_lineRenderer.endColor = m_defaultEndColor;
            }
            m_lineRenderer.SetPosition(1, m_lastHit.point);
        }
        else
        {
            m_lineRenderer.SetPosition(1, m_hand.transform.position + (m_hand.transform.forward * m_maxRaycastDistance));
            if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
            {
                m_lineRenderer.startColor = m_buttonDownStartColor;
                m_lineRenderer.endColor = m_buttonDownEndColor;
            }
            else
            {
                m_lineRenderer.startColor = m_defaultStartColor;
                m_lineRenderer.endColor = m_defaultEndColor;
            }
        }

        // update line renderer
        m_lineRenderer.SetPosition(0, m_hand.transform.position);
        
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    public void StartButton()
    {
        SceneManager.LoadScene(1);
    }
}
