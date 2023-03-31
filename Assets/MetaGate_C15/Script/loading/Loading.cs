using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public static Loading instance { get; private set; }


    [Header("택스트표시")]
    public TMP_Text flashingText;
    public bool IsLoading;
    private bool runCoroutine;

    [Header("씬변경")]
    public string TargetScName;
    public UnityEngine.UI.Image img;
    private bool IsDone;
    public float gradientTime = 2;
    private float currentAlpha;
    private float nowFadeAlpha;
    public bool IsNextBlackScreen = true;//스크린이 블랙일지 화이트일지

    AsyncOperation async;
    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        flashingText.gameObject.SetActive(false);
    }
    private void OnDestroy()
    {
        instance = null;
    }
    private void Update()
    {
        if (IsLoading && !runCoroutine)
        {
            Debug.Log("loading start");
            flashingText.gameObject.SetActive(true);
            runCoroutine = true;
            StartCoroutine(BlinkText());
        }

        if (!IsLoading && runCoroutine)
        {
            if (runCoroutine)
            {
                Debug.Log("loading end");
                runCoroutine = false;
                StopCoroutine(BlinkText());
                flashingText.gameObject.SetActive(false);
            }
        }
    }


    public IEnumerator BlinkText()
    {
        runCoroutine = true;
        while (runCoroutine)
        {
            flashingText.text = "";
            yield return new WaitForSeconds(.5f);
            flashingText.text = "Loading....";
            yield return new WaitForSeconds(.5f);
        }
    }

    //이하 씬변경관련
    public void changeScene(string ScName)
    {

        img.gameObject.SetActive(true);//택스트 활성화.
        Color cColor;//임시 칼라 생성
        if (IsNextBlackScreen) cColor = Color.black; else cColor = Color.white; //페이드할 스크린이 블랙인지 화이트인지
        StartCoroutine(imgFade(0, 1, cColor, true));
        TargetScName = ScName;
        AsyncLoad();
    }

    void AsyncLoad()//비동기 씬 로드하고 씬 임시 비활성화
    {
        //씬로드
        //async = SceneManager.LoadSceneAsync(TargetScName, LoadSceneMode.Single);
        //async.allowSceneActivation = false;//씬 불어오고 하이드
        //
    }

    public IEnumerator imgFade(float start, float end, Color FadeColor, bool isEndEvent)//페이드 스크린
    {
        float nowTime = 0.0f;
        img.enabled = true;

        if (isEndEvent)IsLoading = false;else IsLoading = true;


        while (nowTime < gradientTime)
        {
            nowTime += Time.deltaTime;
            nowFadeAlpha = Mathf.Lerp(start, end, Mathf.Clamp01(nowTime / gradientTime));

            if (isEndEvent)
            {
                if (nowTime > gradientTime / 2)//페이드 중간부터 로드 택스트 표시
                    IsLoading = true;
            }
            else
            {
                if (nowTime > gradientTime / 2)//페이드 중간부터 로드 택스트 끄기
                    IsLoading = false;
            }


            SetImgAlpha(FadeColor);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
        if (isEndEvent)//씬에서 나가는 페이드일 경우
        {
            StartCoroutine(corLoadScene());
            IsLoading = false;
        }
        else //씬에 들어오는 페이드인일 경우
        {
            img.enabled = false;
            IsLoading = false;
        }
    }
    private void SetImgAlpha(Color fadeColor)//페이드 스크린 적용
    {
        Color color = fadeColor;
        color.a = Mathf.Max(currentAlpha, nowFadeAlpha);
        img.color = color;
    }
    IEnumerator corLoadScene()
    {
        Debug.Log("여기");
        async = SceneManager.LoadSceneAsync(TargetScName, LoadSceneMode.Single);
        async.allowSceneActivation = false;//씬 불어오고 하이드
        yield return null;
        while (!async.isDone)
        {
            if (async.progress >= 0.9f) { Debug.Log("여기"); async.allowSceneActivation = true; break; }
            IsLoading = true;
            yield return null;
        }
        Debug.Log("여기");

    }
}
