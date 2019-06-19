using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    private GameObject m_lastHitObject;

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
        BaseEventData baseEvent = new BaseEventData(EventSystem.current);
        if (Physics.Raycast(m_hand.transform.position, m_hand.transform.forward, out m_lastHit, m_maxRaycastDistance))
        {
            // if raycast hits UI
            if (m_lastHit.collider.gameObject.layer == LayerMask.NameToLayer("UI"))
            {
                m_lastHitObject = m_lastHit.collider.gameObject;
                // set button colour
                ColorBlock colorBlock = m_lastHitObject.GetComponent<Button>().colors;
                colorBlock.normalColor = Color.red;
                m_lastHitObject.GetComponent<Button>().colors = colorBlock;
                if (OVRInput.Get(OVRInput.Button.PrimaryIndexTrigger))
                {
                    m_lineRenderer.startColor = m_buttonDownStartColor;
                    m_lineRenderer.endColor = m_buttonDownEndColor;
                    // press button
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
            ColorBlock colorBlock = m_lastHitObject.GetComponent<Button>().colors;
            colorBlock.normalColor = Color.white;
            m_lastHitObject.GetComponent<Button>().colors = colorBlock;
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
