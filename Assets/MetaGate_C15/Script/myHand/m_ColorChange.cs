using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class m_ColorChange : MonoBehaviour
{
    [SerializeField] Renderer m_renderer;

    public void ChangeToGreen()
    {
        m_renderer.material.color = Color.green;
    }
    public void ChangeToRed()
    {
        m_renderer.material.color = Color.red;
    }

}
