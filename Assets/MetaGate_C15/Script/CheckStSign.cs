using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class CheckStSign : MonoBehaviour
{

    //신호받기
    public bool[] Sign = new bool[4];
    public bool Succcess;
    public int num = 0;
    public GameObject StStatue;
    private int sum;

    //페이드인
    [Header("성호 ui")]
    public InputActionProperty Bt_Selector;
    public Image myPanel;
    public float fadeTime = 2f;
    public AnimationCurve fadeInCurve;
    public bool SignUI_Ready;
    public Sprite[] changeUI = new Sprite[5];
    public ParticleSystem DisarpearEff;
    public GameObject colisonBoxs;
    public GameObject StMaria;


    private void OnEnable()
    {
        myPanel.sprite = changeUI[0];
        Bt_Selector.action.performed += ShowSingUI;
        Bt_Selector.action.performed += HideSingUI;
    }
    private void OnDisable()
    {
        Bt_Selector.action.performed -= ShowSingUI;
        Bt_Selector.action.performed -= HideSingUI;

    }
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SignShow());
        //StartCoroutine(ShowStMaria());


        //myPanel.color = new Color(0.0f, 0.0f, 0.0f, Mathf.Lerp(myPanel.color.a, 0.0f, Time.deltaTime * 1.1f));
        //myPanel.color = new Color(myPanel.color.r, myPanel.color.g, myPanel.color.b,0);
    }

    // Update is called once per frame

    float due = 4;
    public float t;
    void Update()
    {
        //if (Succcess)
        //{
        //    StartCoroutine(ShowStMaria());
        //}

        //if (Input.GetKeyDown(KeyCode.Space))
        //    TriggerPressed();

        //if (Input.GetKeyDown(KeyCode.UpArrow))
        //    CheckInSign(0);
        //if (Input.GetKeyDown(KeyCode.DownArrow))
        //    CheckInSign(1);
        //if (Input.GetKeyDown(KeyCode.LeftArrow))
        //    CheckInSign(2);
        //if (Input.GetKeyDown(KeyCode.RightArrow))
        //    CheckInSign(3);
    }

    private void ShowSingUI(InputAction.CallbackContext context)
    {
        if (context.action.phase == InputActionPhase.Performed) TriggerPressed();
        else if (context.action.phase != InputActionPhase.Performed) TriggerReleased();

    }
    private void HideSingUI(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() == 0 && context.action.phase != InputActionPhase.Performed)
        {
            TriggerReleased();
        }


    }

    void TriggerPressed()
    {

        if (SignUI_Ready == false&&Succcess==false)
        {
            Debug.Log("show");
            StartCoroutine(SignShow());
        }
        else
        {
            Debug.Log("hide");
            StartCoroutine(Signhide());
        }


    }
    void TriggerReleased()
    {
        StartCoroutine(Signhide());

    }



    public void CheckInSign(int num)
    {
        if (SignUI_Ready == false || Succcess == true)
        {
            Debug.Log("하지마");
            return;//유아이가 안켜져있으면 그냥 나가기.
        }
        Debug.Log("하지마 다음");
        if (Sign[num] == true) return;//키려는 불이 꺼져있을때만 작동.

        if (sum == num)//총합과 키려는 불이 같아야 순서대로 작동.
        {
            myPanel.sprite = changeUI[num + 1];
            StartCoroutine(timeLimitCheck(num));
            if (sum == 3)
            {
                StartCoroutine(timeLimitSuccessCheck());
                sum = 0;
            }
            else sum += 1;
        }
        else // 순서대로 들어오지 않으면 모두 거짓.
        {
            for (int i = 0; i < Sign.Length; i++)
            {
                Sign[i] = false;
            }
            sum = 0;
            myPanel.sprite = changeUI[0];
            return;
        }




    }

    IEnumerator timeLimitCheck(int num)
    {
        Sign[num] = true;
        t = 0;
        yield return new WaitForSeconds(3);
        Sign[num] = false;
    }
    IEnumerator timeLimitSuccessCheck()
    {
        Succcess = true;
        StartCoroutine(Signhide());
        yield return new WaitForSeconds(5f);
        Succcess = false;
    }



    //사인 이미지 조정과 신호 받기 준비상태 설정
    private IEnumerator SignShow()
    {
        Debug.Log("여기 어떻게 왔나");
        if (SignUI_Ready == true) yield break;
        //유아이,컬리전 체크 박스 온
        myPanel.gameObject.SetActive(true);
        colisonBoxs.SetActive(true);
        SignUI_Ready = true;
        //
        float timeStamp = Time.time;
        while (Time.time < timeStamp + fadeTime)
        {
            float t = (Time.time - timeStamp) / fadeTime;
            t = fadeInCurve.Evaluate(t);
            // 0에서 10까지 에니메이션 커브를 따라 변형
            float value = Mathf.LerpUnclamped(0f, 1, t);
            myPanel.color = new Color(1, 1, 1, value);

            yield return null;

        }

    }



    private IEnumerator Signhide()
    {
        StopCoroutine(SignShow());
        DisarpearEff.gameObject.SetActive(true);
        float timeStamp = Time.time;
        while (Time.time < timeStamp + fadeTime)
        {
            float t = (Time.time - timeStamp) / fadeTime;
            t = fadeInCurve.Evaluate(t);
            // 0에서 10까지 에니메이션 커브를 따라 변형
            float value = Mathf.LerpUnclamped(1, 0, t);
            myPanel.color = new Color(1, 1, 1, value);
            yield return null;
        }
        yield return new WaitForSeconds(fadeTime * 0.5f);
        myPanel.gameObject.SetActive(false);
        colisonBoxs.SetActive(false);
        DisarpearEff.gameObject.SetActive(false);
        SignUI_Ready = false;

    }

    IEnumerator ShowStMaria()
    {
        StMaria.SetActive(true);
        float timeStamp = Time.time;
        Material mat = StMaria.GetComponent<Renderer>().material;
        Color color = mat.color;
        yield return null;
        while (Time.time < timeStamp + fadeTime)
        {
            float t = (Time.time - timeStamp) / fadeTime;
            t = fadeInCurve.Evaluate(t);
            // 0에서 10까지 에니메이션 커브를 따라 변형
            color.a = Mathf.LerpUnclamped(0, 1, t);
            mat.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(HideStMaria());
    }

    IEnumerator HideStMaria()
    {
        Debug.Log("하이드 마리아");
        float timeStamp = Time.time;
        Material mat = StMaria.GetComponent<Renderer>().material;
        Color color = mat.color;
        yield return null;
        while (Time.time < timeStamp + fadeTime)
        {
            float t = (Time.time - timeStamp) / fadeTime;
            t = fadeInCurve.Evaluate(t);
            // 0에서 10까지 에니메이션 커브를 따라 변형
            color.a = Mathf.LerpUnclamped(1, 0, t);
            mat.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        StMaria.SetActive(false);
        Succcess = false;
    }
}
