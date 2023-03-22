using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class m_ActivateTeleportaionRay : MonoBehaviour
{
    public GameObject leftTeleportaion;
    public GameObject RightTemleportaion;

    public InputActionProperty leftActivate;
    public InputActionProperty RightActivate;

    public InputActionProperty leftCancel;
    public InputActionProperty rightCancel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        leftTeleportaion.SetActive(leftCancel.action.ReadValue<float>()==0 && leftActivate.action.ReadValue<float>()>0.1f);   
        RightTemleportaion.SetActive(rightCancel.action.ReadValue<float>() == 0 && RightActivate.action.ReadValue<float>()>0.1f);

        float var = RightActivate.action.ReadValue<float>();
    }
}
