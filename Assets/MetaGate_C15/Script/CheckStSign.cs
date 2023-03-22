using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class CheckStSign : MonoBehaviour
{

    //��ȣ�ޱ�
    public bool[] Sign = new bool[4];
    public bool Succcess;
    public int num = 0;
    public GameObject StStatue;
    private int sum;

    //���̵���
    [Header("��ȣ ui")]
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
            Debug.Log("������");
            return;//�����̰� ������������ �׳� ������.
        }
        Debug.Log("������ ����");
        if (Sign[num] == true) return;//Ű���� ���� ������������ �۵�.

        if (sum == num)//���հ� Ű���� ���� ���ƾ� ������� �۵�.
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
        else // ������� ������ ������ ��� ����.
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



    //���� �̹��� ������ ��ȣ �ޱ� �غ���� ����
    private IEnumerator SignShow()
    {
        Debug.Log("���� ��� �Գ�");
        if (SignUI_Ready == true) yield break;
        //������,�ø��� üũ �ڽ� ��
        myPanel.gameObject.SetActive(true);
        colisonBoxs.SetActive(true);
        SignUI_Ready = true;
        //
        float timeStamp = Time.time;
        while (Time.time < timeStamp + fadeTime)
        {
            float t = (Time.time - timeStamp) / fadeTime;
            t = fadeInCurve.Evaluate(t);
            // 0���� 10���� ���ϸ��̼� Ŀ�긦 ���� ����
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
            // 0���� 10���� ���ϸ��̼� Ŀ�긦 ���� ����
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
            // 0���� 10���� ���ϸ��̼� Ŀ�긦 ���� ����
            color.a = Mathf.LerpUnclamped(0, 1, t);
            mat.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(3f);
        StartCoroutine(HideStMaria());
    }

    IEnumerator HideStMaria()
    {
        Debug.Log("���̵� ������");
        float timeStamp = Time.time;
        Material mat = StMaria.GetComponent<Renderer>().material;
        Color color = mat.color;
        yield return null;
        while (Time.time < timeStamp + fadeTime)
        {
            float t = (Time.time - timeStamp) / fadeTime;
            t = fadeInCurve.Evaluate(t);
            // 0���� 10���� ���ϸ��̼� Ŀ�긦 ���� ����
            color.a = Mathf.LerpUnclamped(1, 0, t);
            mat.color = color;
            yield return null;
        }
        yield return new WaitForSeconds(2f);
        StMaria.SetActive(false);
        Succcess = false;
    }
}
