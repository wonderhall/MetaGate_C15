//using System.Collections;
//using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
//using System;
//using UnityEngine.PlayerLoop;
//using Unity.XR.CoreUtils.GUI;
//using BestHTTP.SecureProtocol.Org.BouncyCastle.Ocsp;

public enum deviceType
{
    /*public string */
    vr, android, ios
}


public class PlayerInfo : MonoBehaviour
{
    //캐릭터 정보

    public string roomName;
    public new string name = "user00";
    public string id = "";
    public string chType;
    public bool IsWalk;
    public int emoEvent;
    public int addEvent;


    private Transform cam;
    public GameObject thePlayer;
    [SerializeField]
    public deviceType _dvType;


    //network
    public GameObject userPrefab;
    public bool UseNetWork;
    [SerializeField]
    private bool MoveUpdate = false;

    [Header("vr모드 전용")]//vr모드 전용
    public showVrUI _showVrUI;
    public Sprite[] EmoSprites;

    [Header("안드로이드 전용")]//안드로이드 전용
    public GameObject emoUI;
    public Image ChImage;
    public Sprite[] ChImages;
    public GameObject emoTextBox;

    // Start is called before the first frame update
    //디버깅
    private GUIStyle guiStyle = new GUIStyle();
    public bool isGuiDebug;
    public Text debugText;
    public bool IsRotate = false;

    private void Awake()
    {
        emoTextBox.SetActive(false);
        if (PlayerPrefs.GetString("m_name") != null)
            name = PlayerPrefs.GetString("m_name");
        if (PlayerPrefs.GetString("m_chType") != null)
            chType = PlayerPrefs.GetString("m_chType");
#if ForAndroid
        _dvType = deviceType.android;
        if (SceneManager.GetActiveScene().name != "Ss_Lobby"&& SceneManager.GetActiveScene().name != "vatican")
        {
            Debug.Log(SceneManager.GetActiveScene().name);
            emoUI.SetActive(false);
        }
        if(GameObject.Find("WarningWall")!=null)
            GameObject.Find("WarningWall").SetActive(false);
        //프리팹에 저장된 캐릭터타입
        if (PlayerPrefs.GetString("m_chType") == "male") { ChImage.sprite = ChImages[0]; } //0은남자1은여자 
        else { ChImage.sprite = ChImages[1]; }
        savedSprite = ChImage.sprite;
#endif
#if ForVR
        _dvType = deviceType.vr;

#endif

    }

    private void OnEnable()
    {

        deviceCheck();
        thePlayer = new GameObject("thePlayer");

        cam = Camera.main.transform;

        if (UseNetWork)
        {
            OnSocket();
            NetworkManager.Instance.OnConnectedUser = OnConnectedUser;
            NetworkManager.Instance.OnJoinSuccessUser = OnJoinSuccessUser;
        }
    }
    void Start()
    {


    }

    // Update is called once per frame

    float deltaTime = 0.0f;
    float EmoTime = 0.0f;
    private Sprite savedSprite;

    void Update()
    {
        if (isGuiDebug) deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        if (UseNetWork)
        {
            float yRotation = cam.eulerAngles.y;
            Quaternion rot = cam.rotation * Quaternion.Euler(0, 1, 0);
            Vector3 newPos = new Vector3(cam.position.x, cam.parent.parent.position.y, cam.position.z);
            thePlayer.transform.position = newPos;
            thePlayer.transform.eulerAngles = new Vector3(0, yRotation, 0);
        }

        //Debug.Log("player:x:" + thePlayer.transform.position.x + "y:" + thePlayer.transform.position.y + "z:" + thePlayer.transform.position.z);

        //안드로이드 업데이트
        if (_dvType == deviceType.android && UseNetWork)
        {
            //Debug.Log("and");
            if (IsWalk && UseNetWork)//if (MoveUpdate)에서 변경
            {
                NetworkManager.Instance.OnPlayerMove(thePlayer.transform.position.x, thePlayer.transform.position.y, thePlayer.transform.position.z);
            }
            if (IsRotate && UseNetWork)//if (MoveUpdate)에서 변경
            {
                NetworkManager.Instance.OnPlayerRotate(thePlayer.transform.eulerAngles.x, thePlayer.transform.eulerAngles.y, thePlayer.transform.eulerAngles.z);
            }
        }//안드로이드 일 때 서버에 무브 로테이트 업데이트
        //vr업데이트
        if (_dvType == deviceType.vr && UseNetWork)
        {
            Debug.Log("vr");
            NetworkManager.Instance.OnPlayerMove(thePlayer.transform.position.x, thePlayer.transform.position.y, thePlayer.transform.position.z);
            NetworkManager.Instance.OnPlayerRotate(thePlayer.transform.eulerAngles.x, thePlayer.transform.eulerAngles.y, thePlayer.transform.eulerAngles.z);

        }//vr 일 때 서버에 무브 로테이트 업데이트
 
        if (emoEvent != 0 && UseNetWork)
        {
            EmoTime += Time.deltaTime;
            Debug.Log("여기");
            if (_dvType == deviceType.android)//안드로이드일때 이모티콘 변경
            {
                if (emoEvent > 8)//이모티콘이 텍스트인지 이미지인지 체크
                {
                    emoTextBox.SetActive(true);
                    emoTextBox.transform.GetChild(0).gameObject.SetActive(true);
                    ChImage.sprite = savedSprite;
                    emoTextBox.transform.GetChild(0).GetComponent<Image>().sprite = EmoSprites[emoEvent - 1];
                    if (EmoTime > 3)
                    {
                        emoEvent = 0;
                        EmoTime = 0;
                        emoTextBox.SetActive(false);
                        emoTextBox.transform.GetChild(0).gameObject.SetActive(false);
                    }
                }
                else
                {
                    ChImage.sprite = EmoSprites[emoEvent - 1];
                    if (EmoTime > 3)
                    {
                        emoEvent = 0;
                        EmoTime = 0;
                        ChImage.sprite = savedSprite;
                    }
                }
            }

            else//vr일때 이모티콘 변경
            {
                    if (emoEvent < 9)
                    {
                        EmoTime += Time.deltaTime;
                        _showVrUI.ChImage.sprite = EmoSprites[emoEvent - 1];
                        if (EmoTime > 3)
                        {
                            emoEvent = 0;
                            EmoTime = 0;
                            _showVrUI.ChImage.sprite = _showVrUI.savedSprite;
                        }
                    }
                    else//추가텍스트
                    {
                        EmoTime += Time.deltaTime;
                        _showVrUI.emoText.gameObject.SetActive(true);
                        _showVrUI.emoText.sprite = EmoSprites[emoEvent - 1];
                        if (EmoTime > 3)
                        {
                            emoEvent = 0;
                            EmoTime = 0;
                            _showVrUI.emoText.gameObject.SetActive(false);
                        }
                    }
            }
            NetworkManager.Instance.OnPlayerEmoevent(emoEvent);
        }

    }

    public void changeEmo(int num)
    {
        emoEvent = num;
        EmoTime = 0;
        Debug.Log("이모티콘 " + num + " 으로 변경");
#if ForAndroid
        if (num > 8) { emoTextBox.SetActive(true); emoTextBox.transform.GetChild(0).GetComponent<Image>().sprite = EmoSprites[num - 1]; }
        //ChImage.sprite = EmoSprites[num - 1];
#endif
    }//이모티콘 바꾸기.에디터 버튼에서 변경

    public void insertName(string name)
    {
        id = name;
    }//이모티콘 바꾸기.에디터 버튼에서 변경

    private void OnConnectedUser(bool obj)
    {
        Debug.Log("OnConnectedUser----------");
        NetworkManager.Instance.OnJoinReal(thePlayer.transform.position.x, thePlayer.transform.position.y, thePlayer.transform.position.z,
             thePlayer.transform.eulerAngles.x, thePlayer.transform.eulerAngles.y, thePlayer.transform.eulerAngles.z);

    }//네트워크연결
    private void OnJoinSuccessUser(bool obj)
    {

        Debug.Log("OnJoinSuccessUser----------");
        MoveUpdate = true;
    }//접속 확인

    private void OnSocket()
    {

        Debug.Log("--------------OnSocket-------");

        NetworkManager.Instance.Socket();

    }//소켓연결
    private void deviceCheck()
    {
        switch (_dvType)
        {
            case deviceType.vr:
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    if (i == 0)
                        transform.GetChild(i).gameObject.SetActive(true);
                    else
                        transform.GetChild(i).gameObject.SetActive(false);
                }
                break;
            case deviceType.android://vr
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    if (i == 1)
                        transform.GetChild(i).gameObject.SetActive(true);
                    else
                        transform.GetChild(i).gameObject.SetActive(false);
                }
                break;
            case deviceType.ios://vr
                for (int i = 0; i < this.transform.childCount; i++)
                {
                    if (i == 2)
                        transform.GetChild(i).gameObject.SetActive(true);
                    else
                        transform.GetChild(i).gameObject.SetActive(false);
                }
                break;
            default:
                break;
        }
    }//타입선택 스위치문

    public void EndGame()
    {
        Debug.Log("게임종료");
        Application.Quit();
    }//게임종료

    //디버깅//
    #region 디버깅유아이
    void OnGUI()
    {
        guiStyle.fontSize = 40;
        guiStyle.normal.textColor = Color.white;


        if (isGuiDebug)
        {
            GUI.Label(new Rect(100, 40, 200, 40), "현재해상도 : " + Screen.currentResolution, guiStyle);
            debugText.text = Screen.currentResolution.ToString();
            debugText.transform.parent.gameObject.SetActive(true);
            int w = Screen.width, h = Screen.height;


            Rect rect = new Rect(0, 0, w, h * 0.02f);
            float msec = deltaTime * 1000.0f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
            GUI.Label(new Rect(100, 80, 200, 40), text, guiStyle);
        }
    }
    #endregion
}
