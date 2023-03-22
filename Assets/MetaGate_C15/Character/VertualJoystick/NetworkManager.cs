using BestHTTP;
using BestHTTP.JSON;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Events;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
//using static UnityEditor.PlayerSettings;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]
    private string url = "http://121.66.138.211:8889/";
    // 나의 현재 정보 참조
    public static PlayerInfo cInfo;
    //임시
    public GameObject temp;
    //
    private SocketManager Manager;
    private string device = "Mobile";

    //[HideInInspector]
    //public string UserID;

    [HideInInspector]
    public GameObject Player;

    public List<GameObject> players = new List<GameObject>();

    public Action<bool> OnConnectedUser;
    public Action<string, string> OnChatUser;
    public Action<int> OnSetAR;
    public Action<bool> OnJoinSuccessUser;

    private static NetworkManager _Instance;
    public static NetworkManager Instance
    {
        get
        {
            if (!_Instance)
            {
                _Instance = new GameObject().AddComponent<NetworkManager>();
                // name it for easy recognition
                _Instance.name = _Instance.GetType().ToString();
                // mark root as DontDestroyOnLoad();
                //DontDestroyOnLoad(_Instance.gameObject); //2.28
            }
            return _Instance;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
            cInfo = GameObject.FindObjectOfType<PlayerInfo>();

    }

    // Update is called once per frame
    void Update()
    {
    }
    public void Socket()
    {
        players = new List<GameObject>();
        Debug.Log(url);
        Manager = new SocketManager(new Uri(url));
        Debug.Log("Socket : " + Manager.State);
        Manager.Socket.On<ConnectResponse>(SocketIOEventTypes.Connect, OnConnected);
        Manager.Socket.On(SocketIOEventTypes.Disconnect, OnDisconnected);  // 접속끊기

        Manager.Socket.On<System.Object>("LOGIN_SUCCESS", OnJoinSuccess);
        Manager.Socket.On<System.Object>("SPAWN_PLAYER", OnSpawnPlayer);
        Manager.Socket.On<System.Object>("CPLAYERMOVE", OnCPlayerMove);
        Manager.Socket.On<System.Object>("CPLAYERROTATE", OnCPlayerRotate);
        Manager.Socket.On<System.Object>("CPLAYEREMO", OnCPlayerEmoevent);
        //Manager.Socket.On<System.Object>("UPDATE_MOVE_AND_ROTATE", OnUpdateMoveAndRotate);
        Manager.Socket.On<System.Object>("CUSER_DISCONNECTED", OnCDisconnected);
        //Manager.Socket.On<System.Object>("CHATON", OnCHAT);

        

        
    }

    // 연결하자마자 바로 실행
    private void OnConnected(ConnectResponse resp)
    {
        
        Debug.Log("Connected! Socket.IO SID: " + resp.sid + "===========================");
        cInfo.id = resp.sid;

        OnConnectedUser(true);
    }

    public void OnJoinReal(float x, float y, float z, float rotX, float rotY, float rotZ) //보내기
    {
        Dictionary<string, string> join = new Dictionary<string, string>();
        join["name"] = cInfo.name;
        join["chtype"] = cInfo.chType.ToString();
        join["IsWalk"] = cInfo.IsWalk.ToString();
        join["emoEvent"] = cInfo.emoEvent.ToString();
        join["ev1"] = cInfo.addEvent.ToString();
        join["device"] = device;
        join["room"] = cInfo.roomName;
        //포지션
        join["x"] = x.ToString();
        join["y"] = y.ToString();
        join["z"] = z.ToString();
        //회전
        join["rotX"] = rotX.ToString();
        join["rotY"] = rotY.ToString();
        join["rotZ"] = rotZ.ToString();
        string sendData = JsonConvert.SerializeObject(join);
        Manager.Socket.Emit("JOIN", sendData);
        //Debug.Log(sendData + "----OnJoinReal-----");
    }

    private void OnJoinSuccess(System.Object obj) // 받기
    {

        var json = JsonConvert.SerializeObject(obj);
        Debug.Log(json + "----OnJoinSuccess-----");

        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


        Debug.Log(json + "----OnJoinSuccess-----" + data["id"] + "=========================");
        Debug.Log(json + "----OnJoinSuccess---cInfo--" + cInfo.id + "=========================");

        Debug.Log(json + "----OnJoinSuccess-----" + data["room"] + "=========================");

        OnJoinSuccessUser(true);




    }

    // 각 유저를 표시한다.
    private void OnSpawnPlayer(System.Object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);


        Debug.Log("OnSpawnPlayer : " + json + "-----------");
        if (data["device"] == "Mobile" && data["room"]==cInfo.roomName)//추가
        {
            string device = data["device"];
            string id = data["id"];
            string name = data["name"];
            string isWork = data["IsWalk"];
            string chtype = data["chtype"];
            string emoEvent = data["emoEvent"];
            string ev1 = data["ev1"];
            float x = float.Parse(data["x"]);
            float y = float.Parse(data["y"]);
            float z = float.Parse(data["z"]);

            float rotX = float.Parse(data["rotX"]);
            float rotY = float.Parse(data["rotY"]);
            float rotZ = float.Parse(data["rotZ"]);

            Debug.Log(" =======OnSpawnPlayer======id=====" + id);
            Debug.Log("playter.Count : " + players.Count);
            Debug.Log("cInfo.id : " + cInfo.id);
            Debug.Log("id.id : " + id);
            if ((id.Length > 0) && (cInfo.id != id))
            {

                if (players.Find(n => n.name == id) != null)
                {
                    Destroy(players.Find(n => n.name == id));//두번 생성 막기
                }
                var newPlayer = Instantiate(cInfo.userPrefab, new Vector3(x, y, z), Quaternion.identity);
                newPlayer.GetComponent<getUserInfo>().UpdateName(name);
                newPlayer.GetComponent<getUserInfo>().UpdateType(chtype);
                newPlayer.name = id;
                newPlayer.GetComponent<getUserInfo>().ID = id;

                players.Add(newPlayer);
                Debug.Log(" =======OnSpawnPlayer=====players=Add=====" + id);
            }

        }
    }
    // 서버로 현재 오브젝트위치를 보낸다.
    public void OnPlayerMove(float x, float y, float z/*,float rotX, float rotY, float rotZ*/)
    {
        Dictionary<string, string> pindata = new Dictionary<string, string>();
        pindata["room"] = cInfo.roomName;
        pindata["name"] = cInfo.name;
        pindata["device"] = device;
        pindata["x"] = x.ToString();
        pindata["y"] = y.ToString();
        pindata["z"] = z.ToString();
        //pindata["rotX"] = x.ToString();
        //pindata["rotY"] = y.ToString();
        //pindata["rotZ"] = z.ToString();

        Debug.Log(pindata["name"] + "이움직였다");

        string sendData = JsonConvert.SerializeObject(pindata);
        Manager.Socket.Emit("PLAYERMOVE", sendData);
    }
    // 서버로부터 오브젝트위치를 받는다.
    public void OnCPlayerMove(System.Object obj)
    {


        if (players.Count > 0) //이동시킬 유저가 있을때 실행
        {
            var json = JsonConvert.SerializeObject(obj);
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            String id = data["id"];
            float x = float.Parse(data["x"]);
            float y = float.Parse(data["y"]);
            float z = float.Parse(data["z"]);

            Debug.Log("OnCPlayerMove : " + id.Length);
            if (id.Length > 0)
            {
                if (players.Find(n => n.name == id) != null)
                {
                    Vector3 targetPos = new Vector3(x, y, z);
                    GameObject player = players.Find(n => n.name == id);

                    //player.GetComponent<getUserInfo>().IsWork = "true";
                    //player.GetComponent<getUserInfo>().WorkDuring = 0;
                    Vector3 dir = (targetPos - player.transform.position).normalized;//타겟포지션-현재 포지션하여 구한 거리를 노말리이즈해서 방향얻음.
                    if (player.transform.position != targetPos) player.transform.forward = dir;//걷는 방향 보기

                    player.transform.position = targetPos; //이동


                    //Debug.Log("[" + data["id"] + "]" + targetPos + " ");

                }


            }




        }


    }

    public void OnPlayerRotate(float rotX, float rotY, float rotZ)//회전값 보냄
    {
        Dictionary<string, string> pindata = new Dictionary<string, string>();
        pindata["name"] = PlayerPrefs.GetString("m_name");
        pindata["device"] = device;
        pindata["rotX"] = rotX.ToString();
        pindata["rotY"] = rotY.ToString();
        pindata["rotZ"] = rotZ.ToString();
        //pindata["rotX"] = x.ToString();
        //pindata["rotY"] = y.ToString();
        //pindata["rotZ"] = z.ToString();

        Debug.Log(pindata["name"] + "이회전했다");

        string sendData = JsonConvert.SerializeObject(pindata);
        Manager.Socket.Emit("PLAYERROTATE", sendData);
    }
    // 
    public void OnCPlayerRotate(System.Object obj)//회전값 받은
    {

        Debug.Log("[ROTATE-players.Count]"+ players.Count + " ");
        if (players.Count > 0) //
        {
            var PreTime = Time.time;
            var json = JsonConvert.SerializeObject(obj);
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            String id = data["id"];
            float rotX = float.Parse(data["rotX"]);
            float rotY = float.Parse(data["rotY"]);
            float rotZ = float.Parse(data["rotZ"]);
            //Debug.Log("[ROTATE]" + json + " ");
            if (id != null && (players.Find(n => n.name == id) != null))
            {
                GameObject player = players.Find(n => n.name == id);
                Vector3 targetRot = new Vector3(rotX, rotY, rotZ);
                player.transform.rotation = Quaternion.Euler(targetRot);

            }

        }


    }



    public void OnPlayerEmoevent(int emoEvent)//Status
    {
        Dictionary<string, string> pindata = new Dictionary<string, string>();
        pindata["emoEvent"] = emoEvent.ToString();

        Debug.Log(pindata["emoEvent"] + " 표정관리");

        string sendData = JsonConvert.SerializeObject(pindata);
        Manager.Socket.Emit("PLAYEREMO", sendData);
    }
    // 
    public void OnCPlayerEmoevent(System.Object obj)//Status 받은
    {

        
        if (players.Count > 0) //
        {

            var json = JsonConvert.SerializeObject(obj);
            var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            String emoEvent = data["emoEvent"];
            //Debug.Log("[EMOEVENT]" + json + " ");

            GameObject player = players.Find(n => n.name == data["id"]);
            //Vector3 targetRot = new Vector3(rotX, rotY, rotZ);
            player.GetComponent<getUserInfo>().emoIdx = emoEvent;

        }


    }




    public void OnProDisconnected(String id)
    {
        Dictionary<string, string> pindata = new Dictionary<string, string>();
        pindata["id"] = id;
        Debug.Log("OnProDisconnected 접속끊어진다" + id);

        string sendData = JsonConvert.SerializeObject(pindata);
        Manager.Socket.Emit("USER_DISCONNECTED", sendData);
        Debug.Log("OnProDisconnected 접속끊어진다 END " + id);

    }


    private void OnCDisconnected(System.Object obj) // 서버에서 받는다.
    {
        Debug.Log("OnCDisconnected서버에서 받기 접속끊어진다");
        var json = JsonConvert.SerializeObject(obj);
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        string id = data["id"];
        Debug.Log("=OnCDisconnected=============id : " + id);


        int index = players.FindIndex(n => n.name == id);//리스트 비우기 위해 인덱스 찾기
        Debug.Log("삭제할 인덱스는 " + id);
        Destroy(players.Find(n => n.name == id));
        players.Remove(players[index]);//인덱스 삭제
        //StartCoroutine(deleteDelay(id));
        Debug.Log("=OnCDisconnected=============id : " + id);

    }

    IEnumerator deleteDelay(string name)
    {
        Debug.Log("StartCoroutine 여기들어롬");
        yield return new WaitForSeconds(1);
        Debug.Log("StartCoroutine여기들어롬" + name);
        int index = players.FindIndex(n => n.name == name);//리스트 비우기 위해 인덱스 찾기
        Debug.Log("유저 인덱스는 "+ index);
        if (index > 0 )
        {
            Destroy(players.Find(n => n.name == name));//오브젝트 삭제
            players.Remove(players[index]);//인덱스 삭제
        }
        Debug.Log("여기들어롬=================");

    }
    private void OnUpdateMoveAndRotate(System.Object obj)
    {
        var json = JsonConvert.SerializeObject(obj);
        var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
        string name = data["id"];
        float x = float.Parse(data["x"]);
        float y = float.Parse(data["y"]);
        float z = float.Parse(data["z"]);
        //Vector3 pos = GPSUser.GPSToUCS(new Vector2(x, y));

        if (name != null )
        {
            Vector3 cPos = new Vector3(x, 2, z);
            var player = players.Find(n => n.name == name);
            player.transform.position = cPos;

        }
    }

    public void OnDisconnected()
    {

        Dictionary<string, string> pindata = new Dictionary<string, string>();
        pindata["id"] = cInfo.id;
        Debug.Log("접속끊어진다" + cInfo.id);

        string sendData = JsonConvert.SerializeObject(pindata);
        Manager.Socket.Emit("USER_DISCONNECTED", sendData);


        Manager.Socket.Disconnect();
        players.Clear();
        Debug.Log("Socket : Disconnected--------");
    }

    private void KillPlayer(System.Object obj)
    {

        //Destroy(GameObject.Find(data["name"]));

    }

    /// <summary>
    /// //이하 사용치 않음
    /// </summary>

    #region unUsed
    //private void OnCHAT(object obj)
    //{
    //    var json = JsonConvert.SerializeObject(obj);
    //    // Debug.LogError("ONCHAT : " + json);
    //    var data = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
    //    OnChatUser(data["id"], data["chat"]);
    //}

    //public void OnMOVEANDROTATE(float latitude, float longitude, float altitude)
    //{

    //    Dictionary<string, string> pindata = new Dictionary<string, string>();
    //    pindata["x"] = latitude.ToString();
    //    pindata["y"] = longitude.ToString();
    //    pindata["z"] = altitude.ToString();
    //    string sendData = JsonConvert.SerializeObject(pindata);
    //    Manager.Socket.Emit("PINSPAWN", sendData);
    //}

    //public void OnChat(int idx)
    //{
    //    Dictionary<string, string> join = new Dictionary<string, string>();
    //    join["id"] = cInfo.name;
    //    if (idx == 0)
    //    {
    //        join["chat"] = "안녕하세요";
    //    }
    //    else if (idx == 1)
    //    {
    //        join["chat"] = "하하하하";
    //    }
    //    else if (idx == 2)
    //    {
    //        join["chat"] = "아니에요!";
    //    }
    //    else if (idx == 3)
    //    {
    //        join["chat"] = "흠!";
    //    }
    //    string sendData = JsonConvert.SerializeObject(join);

    //    Manager.Socket.Emit("CHAT", sendData);
    //    OnChatUser(join["id"], join["chat"]);
    //} 
    #endregion


}
