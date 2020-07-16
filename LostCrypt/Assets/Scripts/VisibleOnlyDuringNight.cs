using UnityEngine;

public class VisibleOnlyDuringNight : MonoBehaviour
{
    LightColorController m_LightController;
    Renderer m_Renderer;

    void Awake()
    {
		m_LightController = FindObjectOfType<LightColorController>();
		m_Renderer = GetComponent<Renderer>();
    }

    void Update()
    {
        if (m_Renderer != null && m_LightController != null)
            m_Renderer.enabled = m_LightController.timeValue > 0.0f;
    }
}
