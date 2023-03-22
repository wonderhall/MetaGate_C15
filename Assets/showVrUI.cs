using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
public class showVrUI : MonoBehaviour
{
    public InputActionProperty Bt_ShowVrUI;
    public GameObject ui;
    public Image ChImage;
    public Image emoText;
    public Sprite[] ChImages;
    [Header("�ڵ�����")]
    public Sprite savedSprite;

    private void OnEnable()
    {
        //�����տ� ����� ĳ����Ÿ��
        if (PlayerPrefs.GetString("m_chType") == "male") { ChImage.sprite = ChImages[0]; } //0������1������ 
        else { ChImage.sprite = ChImages[1]; }
        savedSprite = ChImage.sprite;
        Bt_ShowVrUI.action.performed += ShowVrUI;
        emoText.gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        Bt_ShowVrUI.action.performed -= ShowVrUI;

    }

    private bool  EmoUIShow = false;
    private bool NowchangingImage;

    private void ShowVrUI(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed) TriggerPressed();
    }
    void TriggerPressed()
    {
        EmoUIShow = !EmoUIShow;
        ui.SetActive(EmoUIShow);
    }

    public void ChangToEmo(int idx)
    {
        ChImage.sprite = ChImages[idx];
        NowchangingImage = true;
    }
    private void Update()
    {
        
    }
}
