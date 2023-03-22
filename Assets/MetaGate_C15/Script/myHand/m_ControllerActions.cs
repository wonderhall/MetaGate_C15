using System.Collections;
using System.Collections.Generic;
using Unity.XR.PXR;
using UnityEngine;

public class m_ControllerActions : MonoBehaviour
{
    public GameObject[] cont;
    public GameObject[] handcont;
    PXR_Manager manager;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        var controller = PXR_Input.GetDominantHand();
        //핸드트랙킹 입력 체크 bool true==cont on false == handcont on
        if (PXR_Input.IsControllerConnected(controller)) 
        { foreach (var item in cont) item.SetActive(false); foreach (var item in handcont) item.SetActive(true); }
        else { foreach (var item in cont) item.SetActive(true); foreach (var item in handcont) item.SetActive(false); }



    }
}
