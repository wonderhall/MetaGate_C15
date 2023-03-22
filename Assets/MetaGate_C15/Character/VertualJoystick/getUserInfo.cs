using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class getUserInfo : MonoBehaviour
{
    //받아오는 정보
    public string userName = "이름없슴";
    public string userChType = "chtype";
    public string ID;
    //받아오는 상태정보
    public string IsWork = "false";
    public string emoIdx = "0";

    //정보 활용
    public GameObject[] chArray;
    public GameObject[] emoArray;
    public TMP_Text text;
    private Animator animator;
    private int? temp = null;

    public static Vector3 targetPos;


    private Vector3 lastPos;
    // Start is called before the first frame update

    private void OnEnable()
    {
        text.text = userName; //유저네임 표시
        foreach (var ch in chArray) ch.SetActive(false); //캐릭터 타입
        foreach (var ch in emoArray) ch.SetActive(false); //캐릭터 타입
        if (userChType == "male")
            chArray[0].SetActive(true);
        else
            chArray[1].SetActive(true);
        animator = this.transform.GetComponentInChildren<Animator>();
    }

    public bool isWalking;
    public bool isStoping = true;

    public float transOffset = 0.5f;//위치 변화 허용값.

    private void Update()
    {
        if (isWalking && WalkDuring > 0) WalkDuring -= 0.0025f; //워킹상태체크. 빼기로 0이 안되서 비슷한값
        if (!ComparePos() && !isWalking && isStoping)//위치가 변하였을경우 워킹상태가 아니고 멈춤상태일 경우
        {
            Debug.Log("walk");
            isWalking = true; isStoping = false;
            WalkDuring = checkUpdateTime;
            animator.SetBool("IsMove", true);

        }
        else if (ComparePos() && !isStoping && isWalking && WalkDuring <= 0.0025)//위치가 변하지 않을경우.조금움직였을경우
        {
            Debug.Log("stop"); isWalking = false; isStoping = true;
            animator.SetBool("IsMove", false);
        }



        if (int.Parse(emoIdx) != temp) //이모티콘 표시
        {
            int eventID = int.Parse(emoIdx);
            temp = eventID;//바뀌었는지 확인위해 템프에 넣어서 비교

            foreach (var item in emoArray) item.SetActive(false); //일단 모두 하이드
            for (int i = 0; i < emoArray.Length; i++)
            {

                //if (i == eventID) { emoArray[i].SetActive(true); }
                if (i == eventID) StartCoroutine(showEmo(i));
            }
        }
        else if (temp != null && int.Parse(emoIdx) == 0)
        {
            foreach (var item in emoArray) item.SetActive(false); //이벤트가0이면 모두 하이드
            temp = null;
        }
    }

    public void UpdateName(string name)
    {
        userName = name;
        text.text = name;
    }//이름업데이트
    public void UpdateType(string type)
    {
        userChType = type;
        foreach (var ch in chArray) ch.SetActive(false); //캐릭터 타입
        if (userChType == "male")
        {
            chArray[0].SetActive(true);
            animator = chArray[0].GetComponent<Animator>();
        }
        else
        {
            chArray[1].SetActive(true);
            animator = chArray[1].GetComponent<Animator>();
        }
    }//캐릭터 타입업데이트
    IEnumerator showEmo(int idx)
    {
        if (idx > 0)
        {
            Debug.Log("이모티콘" + idx + " 초간 변경");
            emoArray[idx].SetActive(true);
            yield return new WaitForSeconds(3);
            emoArray[idx].SetActive(false);
        }

    }

    public float checkUpdateTime = 0.25f;//무브를 멈출때까지 대기 시간.
    private float WalkDuring;
    bool ComparePos()
    {
        bool isEqual = true;
        if (lastPos != transform.position)
        {
            Vector3 tempPo = lastPos - transform.position;
            float tempComp = Mathf.Abs(tempPo.x) + Mathf.Abs(tempPo.y) + Mathf.Abs(tempPo.z);
            if (tempComp < transOffset)
            {
                return isEqual;//값이 작으면 똑같은것으로 취급하여 돌려준다.
            }
            lastPos = transform.position; //값이 달라졌으니 lasPos에 달라진 값을 넣어준다
            isEqual = !isEqual;
        }

        return isEqual;
    }//위치가 변했는지 체크


}
