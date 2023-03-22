using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class getUserInfo : MonoBehaviour
{
    //�޾ƿ��� ����
    public string userName = "�̸�����";
    public string userChType = "chtype";
    public string ID;
    //�޾ƿ��� ��������
    public string IsWork = "false";
    public string emoIdx = "0";

    //���� Ȱ��
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
        text.text = userName; //�������� ǥ��
        foreach (var ch in chArray) ch.SetActive(false); //ĳ���� Ÿ��
        foreach (var ch in emoArray) ch.SetActive(false); //ĳ���� Ÿ��
        if (userChType == "male")
            chArray[0].SetActive(true);
        else
            chArray[1].SetActive(true);
        animator = this.transform.GetComponentInChildren<Animator>();
    }

    public bool isWalking;
    public bool isStoping = true;

    public float transOffset = 0.5f;//��ġ ��ȭ ��밪.

    private void Update()
    {
        if (isWalking && WalkDuring > 0) WalkDuring -= 0.0025f; //��ŷ����üũ. ����� 0�� �ȵǼ� ����Ѱ�
        if (!ComparePos() && !isWalking && isStoping)//��ġ�� ���Ͽ������ ��ŷ���°� �ƴϰ� ��������� ���
        {
            Debug.Log("walk");
            isWalking = true; isStoping = false;
            WalkDuring = checkUpdateTime;
            animator.SetBool("IsMove", true);

        }
        else if (ComparePos() && !isStoping && isWalking && WalkDuring <= 0.0025)//��ġ�� ������ �������.���ݿ����������
        {
            Debug.Log("stop"); isWalking = false; isStoping = true;
            animator.SetBool("IsMove", false);
        }



        if (int.Parse(emoIdx) != temp) //�̸�Ƽ�� ǥ��
        {
            int eventID = int.Parse(emoIdx);
            temp = eventID;//�ٲ������ Ȯ������ ������ �־ ��

            foreach (var item in emoArray) item.SetActive(false); //�ϴ� ��� ���̵�
            for (int i = 0; i < emoArray.Length; i++)
            {

                //if (i == eventID) { emoArray[i].SetActive(true); }
                if (i == eventID) StartCoroutine(showEmo(i));
            }
        }
        else if (temp != null && int.Parse(emoIdx) == 0)
        {
            foreach (var item in emoArray) item.SetActive(false); //�̺�Ʈ��0�̸� ��� ���̵�
            temp = null;
        }
    }

    public void UpdateName(string name)
    {
        userName = name;
        text.text = name;
    }//�̸�������Ʈ
    public void UpdateType(string type)
    {
        userChType = type;
        foreach (var ch in chArray) ch.SetActive(false); //ĳ���� Ÿ��
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
    }//ĳ���� Ÿ�Ծ�����Ʈ
    IEnumerator showEmo(int idx)
    {
        if (idx > 0)
        {
            Debug.Log("�̸�Ƽ��" + idx + " �ʰ� ����");
            emoArray[idx].SetActive(true);
            yield return new WaitForSeconds(3);
            emoArray[idx].SetActive(false);
        }

    }

    public float checkUpdateTime = 0.25f;//���긦 ���⶧���� ��� �ð�.
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
                return isEqual;//���� ������ �Ȱ��������� ����Ͽ� �����ش�.
            }
            lastPos = transform.position; //���� �޶������� lasPos�� �޶��� ���� �־��ش�
            isEqual = !isEqual;
        }

        return isEqual;
    }//��ġ�� ���ߴ��� üũ


}
