using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveNameType : MonoBehaviour
{
    private TMP_InputField inputField;//인풋필드 선언
    public string nameText;
    public bool isFemale;
    public GameObject[] deviceType = new GameObject[2];


    [Header("스크린페이드인자값")]
    public string SceneName;
    public UnityEngine.UI.Image img;
    private bool IsDone;

    private Renderer renderer = null;
    public float gradientTime = 2;
    private float currentAlpha;
    private float nowFadeAlpha;


    [Tooltip("FadeIn color.")]
    public Color fadeInColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    [Tooltip("FadeOut color.")]
    public Color fadeOutColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    AsyncOperation async;

    private void Awake()
    {
#if ForAndroid
        deviceType[0].SetActive(true);
        deviceType[1].SetActive(false);
        StartCoroutine(imgFade(1, 0, fadeInColor,false)); 
        return;
#endif
#if ForVR
        deviceType[0].SetActive(false);
        deviceType[1].SetActive(true);
#endif
    }
    private void Start()
    {
        inputField = FindObjectOfType<TMP_InputField>(); //인풋필드 찾기.

        //Use onEndEdit
        inputField.onEndEdit.AddListener(delegate { LockInput(inputField); });

        //Use onSubmit
        inputField.onSubmit.AddListener(delegate { LockInput(inputField); });

        async = SceneManager.LoadSceneAsync(SceneName,LoadSceneMode.Single);//add
        async.allowSceneActivation= false;
    }


    void SetName(string text)
    {
        nameText = text;
    }
    public void IsFemale(bool isTrue)
    {
        isFemale = isTrue;
    }

    void LockInput(TMP_InputField input)
    {
        if (input.text.Length > 0)
        {
            nameText = input.text;
        }
        else if (input.text.Length == 0)
        {
            Debug.Log("이름없슴");
        }
    }
    public void EnterRoom()
    {
        //성별 저장
        if (isFemale)
            PlayerPrefs.SetString("m_chType", "female");
        else
            PlayerPrefs.SetString("m_chType", "male");
        //이름저장
        if (nameText.Length > 0)
            PlayerPrefs.SetString("m_name", inputField.text);
        else
            PlayerPrefs.SetString("m_name", "이름없슴");

        //로드씬
        Debug.Log(PlayerPrefs.GetString("m_chType"));
        Debug.Log(PlayerPrefs.GetString("m_name"));
#if ForAndroid
        Debug.Log("안드로이드용");
        StartCoroutine(imgFade(0, 1, fadeOutColor,true));
        StartCoroutine(loadSc(SceneName));
        return;
#endif
#if ForVR
        Debug.Log("VR용");
        StartCoroutine(ScreenFade(0, 1, fadeOutColor));
        StartCoroutine(loadSc(SceneName));

#endif

    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    PlayerPrefs.DeleteAll();
        //}
        if (inputField.text.Length > 0)
        {
            nameText = inputField.text;
        }
    }

    IEnumerator loadSc(string scName)
    {
        //yield return null;
        //AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scName);
        //asyncOperation.allowSceneActivation = false;
        //while (!asyncOperation.isDone)
        //{
        //    //if (Input.GetKeyDown(KeyCode.L))
        //    if (IsDone)
        //        asyncOperation.allowSceneActivation = true;
        //    yield return null;
        //}

        yield return null;
        while (!async.isDone)
        {
            //if (Input.GetKeyDown(KeyCode.L))
            if (IsDone)
                async.allowSceneActivation = true;
            yield return null;
        }

    }

    ////스크린 페이드 ///
    ///

    public IEnumerator imgFade(float start, float end, Color FadeColor, bool isEndEvent)
    {

        img.enabled = true;
        float nowTime = 0.0f;
        while (nowTime < gradientTime)
        {
            nowTime += Time.deltaTime;
            nowFadeAlpha = Mathf.Lerp(start, end, Mathf.Clamp01(nowTime / gradientTime));
            SetImgAlpha(FadeColor);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        if (isEndEvent) IsDone = true;

    }

    public IEnumerator ScreenFade(float start, float end, Color FadeColor)
    {
        Camera cam = Camera.main;
        renderer = cam.GetComponent<Renderer>();
        renderer.enabled = true;
        float nowTime = 0.0f;
        while (nowTime < gradientTime)
        {
            Debug.Log(nowFadeAlpha);
            nowTime += Time.deltaTime;
            nowFadeAlpha = Mathf.Lerp(start, end, Mathf.Clamp01(nowTime / gradientTime));
            SetAlpha(FadeColor);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        IsDone = true;
    }
    private void SetImgAlpha(Color fadeColor)
    {
        Color color = fadeColor;
        color.a = Mathf.Max(currentAlpha, nowFadeAlpha);
        img.color = color;
    }

    private void SetAlpha(Color fadeColor)
    {
        Color color = fadeColor;
        color.a = Mathf.Max(currentAlpha, nowFadeAlpha);
        renderer.material.color = color;
    }
}
