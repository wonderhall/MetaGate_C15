using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class AndroidEnterRoom : MonoBehaviour
{
    [Header("스크린페이드인자값")]
    public string SceneName;
    public UnityEngine.UI.Image img;
    private bool IsDone;

    public float gradientTime = 2;
    private float currentAlpha;
    private float nowFadeAlpha;


    [Tooltip("FadeIn color.")]
    public Color fadeInColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);
    [Tooltip("FadeOut color.")]
    public Color fadeOutColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ScreenFade(1, 0,fadeInColor));
    }

    public void EnterRoom()
    {
        StartCoroutine(ScreenFade(0, 1,fadeOutColor));
        StartCoroutine(loadSc(SceneName));
    }
    IEnumerator loadSc(string scName)
    {
        yield return null;
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(scName);
        asyncOperation.allowSceneActivation = false;
        while (!asyncOperation.isDone)
        {
            //if (Input.GetKeyDown(KeyCode.L))
            if (IsDone)
                asyncOperation.allowSceneActivation = true;
            yield return null;
        }

        SceneManager.LoadSceneAsync(scName, LoadSceneMode.Single);
    }



    public IEnumerator ScreenFade(float start, float end,Color FadeColor)
    {

        img.enabled = true;
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

    private void SetAlpha(Color fadeColor)
    {
        Color color = fadeColor;
        color.a = Mathf.Max(currentAlpha, nowFadeAlpha);
        img.color = color;

    }
}
