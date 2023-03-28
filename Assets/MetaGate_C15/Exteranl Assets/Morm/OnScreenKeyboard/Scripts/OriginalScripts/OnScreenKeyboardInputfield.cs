using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TMP_InputField))]
public class OnScreenKeyboardInputfield : MonoBehaviour, IPointerDownHandler
{
    public OnScreenKeyboard targetOnScreenKeyboard;
    public int characterLimit = 14;
    //private InputField _inputField;
    private TMP_InputField _inputField;
    public string inputtedString;

    private void Awake()
    {
        //_inputField = GetComponent<InputField>();
        _inputField = GetComponent<TMP_InputField>();

        if (_inputField == null)
            return;

        _inputField.shouldHideMobileInput = true;
    }

    public void SaveInputedString(string _inputStr)
    {
        if (inputtedString.Length < characterLimit)
            inputtedString = _inputStr;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (targetOnScreenKeyboard)
        {
            targetOnScreenKeyboard.gameObject.SetActive(true);
            targetOnScreenKeyboard.ShowKeyboard(_inputField, this);
            _inputField.shouldHideMobileInput = true;//다른 모바일 인풋 하이드
        }
    }
}
