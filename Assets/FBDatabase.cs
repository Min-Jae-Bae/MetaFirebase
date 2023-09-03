using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UserInfo
{
    public string username;
    public int age;
    public float height;
    public List<string> favoriteList;
}

public class FBDatabase : MonoBehaviour
{
    public static FBDatabase instance;

    //Firebase Database�� �����ϴ� Ŭ����
    private FirebaseDatabase database;

    //�� ����
    public UserInfo myInfo;

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance;
        //�� ���� �Է�
        myInfo = new UserInfo();
        myInfo.username = "�����";
        myInfo.age = 20;
        myInfo.height = 180.5f;
        myInfo.favoriteList = new List<string>();
        myInfo.favoriteList.Add("���̹�ħ");
        myInfo.favoriteList.Add("������");
        myInfo.favoriteList.Add("����");
    }

    private void Awake()
    {
        instance = this;
    }

    public void SaveMyInfo()
    {
        StartCoroutine(CoSaveMyInfo());
    }

    private IEnumerator CoSaveMyInfo()
    {
        string path = "user_info/ " + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        //�� ������ Json ���·� �ٲ���
        string jsonData = JsonUtility.ToJson(myInfo);

        //���� ��û
        var task = database.GetReference(path).SetRawJsonValueAsync(jsonData);

        //����� �Ϸ�Ƕ����� ��ٸ���
        yield return new WaitUntil(() => task.IsCompleted);

        //���ܻ����� ���ٸ�
        //���强��
        //�׷��� ������
        //�������
        if (task.Exception == null)
        {
            Debug.Log("�� ���� ���� ����");
        }
        else
        {
            Debug.Log("�� ���� ���� ����" + task.Exception.Message);
        }
    }

    public void LoadMyInfo(Action<string> complete)
    {
        StartCoroutine(CoLoadMyInfo(complete));
    }

    private IEnumerator CoLoadMyInfo(Action<string> complete)
    {
        string path = "user_info/ " + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        //������ �б� ��û
        var task = database.GetReference(path).GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            task.Result.GetRawJsonValue();
            print("������ �б� ����");
            if (complete != null)
            {
                complete(task.Result.GetRawJsonValue());
            }
        }
        else
        {
            print("������ �б� ����" + task.Exception.Message);
        }
    }
}