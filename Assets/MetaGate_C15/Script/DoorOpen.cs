using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorOpen : MonoBehaviour
{
    public string SceneName;
    public bool IsDone;
    public bool IsBreakNet;



    private Renderer renderer = null;
    public float gradientTime = 2;
    [Header("temp")]
    private float currentAlpha;
    private float nowFadeAlpha;
    [Tooltip("Basic color.")]
    public Color fadeColor = new Color(0.0f, 0.0f, 0.0f, 1.0f);


    //private Animator _animator;

    // Use this for initialization
    //void Start()
    //{
    //    _animator = GetComponent<Animator>();
    //}

    #region 예전코드
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        _animator.SetBool("Open", true);
    //        Debug.Log("Open");
    //    }


    //}
    //void OnTriggerExit(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        _animator.SetBool("Open", false);
    //    }

    //} 
    #endregion
    private PlayerInfo cInfo;
    private void Start()
    {
        //cInfo = GameObject.FindObjectOfType<PlayerInfo>();

       // Debug.Log("Start SID: " + NetworkManager.cInfo.id + "---------------------------");

    }
    void OnTriggerEnter(Collider other)
    {

        //Debug.Log("OnTriggerEnter : GetComponent<getUserInfo>().ID : " + other.GetComponent<getUserInfo>().ID);
        //Debug.Log("OnTriggerEnter : NetworkManager.cInfo.id : " + NetworkManager.cInfo.id);


        //if (other.GetComponent<getUserInfo>().ID == NetworkManager.cInfo.id)
        //{
        //    Debug.Log("OnTriggerEnter : Playertag : " + other.tag);
        //    Debug.Log("OnTriggerEnter : PlayerIsBreakNet : " + IsBreakNet);
        //    StartCoroutine(loadSc(SceneName));
        //    StartCoroutine(ScreenFade(0, 1));
        //    if (IsBreakNet)
        //    {
        //        //여기다 디스커넥트
        //        Debug.Log("OnTriggerEnter. my SID == : " + NetworkManager.cInfo.id + "---------------------------");
        //        NetworkManager.Instance.OnProDisconnected(NetworkManager.cInfo.id);
        //        NetworkManager.Instance.OnDisconnected();
        //    }
        //} else
        //{
        //    string newID = other.GetComponent<getUserInfo>().ID;
        //    Debug.Log("new id is = " + newID);
        //    Debug.Log("OnTriggerEnter : Usertag : " + other.tag);
        //    Debug.Log("OnTriggerEnter : UserIsBreakNet : " + IsBreakNet);

        //    if (IsBreakNet)
        //    {
        //        //여기다 디스커넥트
        //        Debug.Log("OnTriggerEnter. my SID  22: " + NetworkManager.cInfo.id + "---------------------------");
        //        NetworkManager.Instance.OnProDisconnected(newID);
        //    }
        //}



        if (other.tag == "Player")
        {
            Debug.Log("OnTriggerEnter : Playertag : " + other.tag);
            Debug.Log("OnTriggerEnter : PlayerIsBreakNet : " + IsBreakNet);
            StartCoroutine(loadSc(SceneName));
            StartCoroutine(ScreenFade(0, 1));
            if (IsBreakNet)
            {
                //여기다 디스커넥트
                Debug.Log("OnTriggerEnter. my SID Player: " + NetworkManager.cInfo.id + "---------------------------");
                NetworkManager.Instance.OnProDisconnected(NetworkManager.cInfo.id);
                NetworkManager.Instance.OnDisconnected();
            }

        }
        if (other.tag == "User")
        {
            string newID = other.GetComponent<getUserInfo>().ID;
            Debug.Log("new id is = " + newID);
            Debug.Log("OnTriggerEnter : Usertag : " + other.tag);
            Debug.Log("OnTriggerEnter : UserIsBreakNet : " + IsBreakNet);

            if (IsBreakNet)
            {
                //여기다 디스커넥트
                Debug.Log("OnTriggerEnter. my SID User: " + NetworkManager.cInfo.id + "---------------------------");
                NetworkManager.Instance.OnProDisconnected(newID);
            }
        }

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

        //SceneManager.LoadSceneAsync(scName, LoadSceneMode.Single);
    }

    public IEnumerator ScreenFade(float start, float end)
    {
        Camera cam = Camera.main;
        renderer = cam.GetComponent<Renderer>();
        renderer.enabled = true;
        float nowTime = 0.0f;
        while (nowTime < gradientTime)
        {
            //Debug.Log(nowFadeAlpha);
            nowTime += Time.deltaTime;
            nowFadeAlpha = Mathf.Lerp(start, end, Mathf.Clamp01(nowTime / gradientTime));
            SetAlpha();
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(1);
        IsDone = true;
    }

    private void SetAlpha()
    {
        Color color = fadeColor;
        color.a = Mathf.Max(currentAlpha, nowFadeAlpha);
        renderer.material.color = color;
    }
}