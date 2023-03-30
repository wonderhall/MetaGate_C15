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
    //ĳ���� ����

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

    [Header("vr��� ����")]//vr��� ����
    public showVrUI _showVrUI;
    public Sprite[] EmoSprites;

    [Header("�ȵ���̵� ����")]//�ȵ���̵� ����
    public GameObject emoUI;
    public Image ChImage;
    public Sprite[] ChImages;
    public GameObject emoTextBox;

    // Start is called before the first frame update
    //�����
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
        //�����տ� ����� ĳ����Ÿ��
        if (PlayerPrefs.GetString("m_chType") == "male") { ChImage.sprite = ChImages[0]; } //0������1������ 
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

        //�ȵ���̵� ������Ʈ
        if (_dvType == deviceType.android && UseNetWork)
        {
            //Debug.Log("and");
            if (IsWalk && UseNetWork)//if (MoveUpdate)���� ����
            {
                NetworkManager.Instance.OnPlayerMove(thePlayer.transform.position.x, thePlayer.transform.position.y, thePlayer.transform.position.z);
            }
            if (IsRotate && UseNetWork)//if (MoveUpdate)���� ����
            {
                NetworkManager.Instance.OnPlayerRotate(thePlayer.transform.eulerAngles.x, thePlayer.transform.eulerAngles.y, thePlayer.transform.eulerAngles.z);
            }
        }//�ȵ���̵� �� �� ������ ���� ������Ʈ ������Ʈ
        //vr������Ʈ
        if (_dvType == deviceType.vr && UseNetWork)
        {
            Debug.Log("vr");
            NetworkManager.Instance.OnPlayerMove(thePlayer.transform.position.x, thePlayer.transform.position.y, thePlayer.transform.position.z);
            NetworkManager.Instance.OnPlayerRotate(thePlayer.transform.eulerAngles.x, thePlayer.transform.eulerAngles.y, thePlayer.transform.eulerAngles.z);

        }//vr �� �� ������ ���� ������Ʈ ������Ʈ
 
        if (emoEvent != 0 && UseNetWork)
        {
            EmoTime += Time.deltaTime;
            Debug.Log("����");
            if (_dvType == deviceType.android)//�ȵ���̵��϶� �̸�Ƽ�� ����
            {
                if (emoEvent > 8)//�̸�Ƽ���� �ؽ�Ʈ���� �̹������� üũ
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

            else//vr�϶� �̸�Ƽ�� ����
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
                    else//�߰��ؽ�Ʈ
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
        Debug.Log("�̸�Ƽ�� " + num + " ���� ����");
#if ForAndroid
        if (num > 8) { emoTextBox.SetActive(true); emoTextBox.transform.GetChild(0).GetComponent<Image>().sprite = EmoSprites[num - 1]; }
        //ChImage.sprite = EmoSprites[num - 1];
#endif
    }//�̸�Ƽ�� �ٲٱ�.������ ��ư���� ����

    public void insertName(string name)
    {
        id = name;
    }//�̸�Ƽ�� �ٲٱ�.������ ��ư���� ����

    private void OnConnectedUser(bool obj)
    {
        Debug.Log("OnConnectedUser----------");
        NetworkManager.Instance.OnJoinReal(thePlayer.transform.position.x, thePlayer.transform.position.y, thePlayer.transform.position.z,
             thePlayer.transform.eulerAngles.x, thePlayer.transform.eulerAngles.y, thePlayer.transform.eulerAngles.z);

    }//��Ʈ��ũ����
    private void OnJoinSuccessUser(bool obj)
    {

        Debug.Log("OnJoinSuccessUser----------");
        MoveUpdate = true;
    }//���� Ȯ��

    private void OnSocket()
    {

        Debug.Log("--------------OnSocket-------");

        NetworkManager.Instance.Socket();

    }//���Ͽ���
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
    }//Ÿ�Լ��� ����ġ��

    public void EndGame()
    {
        Debug.Log("��������");
        Application.Quit();
    }//��������

    //�����//
    #region �����������
    void OnGUI()
    {
        guiStyle.fontSize = 40;
        guiStyle.normal.textColor = Color.white;


        if (isGuiDebug)
        {
            GUI.Label(new Rect(100, 40, 200, 40), "�����ػ� : " + Screen.currentResolution, guiStyle);
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
