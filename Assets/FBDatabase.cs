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

    //Firebase Database를 관리하는 클래스
    private FirebaseDatabase database;

    //내 정보
    public UserInfo myInfo;

    private void Start()
    {
        database = FirebaseDatabase.DefaultInstance;
        //내 정보 입력
        myInfo = new UserInfo();
        myInfo.username = "배민재";
        myInfo.age = 20;
        myInfo.height = 180.5f;
        myInfo.favoriteList = new List<string>();
        myInfo.favoriteList.Add("오이무침");
        myInfo.favoriteList.Add("떡볶이");
        myInfo.favoriteList.Add("순대");
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
        //내 정보를 Json 형태로 바꾸자
        string jsonData = JsonUtility.ToJson(myInfo);

        //저장 요청
        var task = database.GetReference(path).SetRawJsonValueAsync(jsonData);

        //통신이 완료되때까지 기다리자
        yield return new WaitUntil(() => task.IsCompleted);

        //예외사힝이 없다면
        //저장성공
        //그렇지 않으면
        //저장실패
        if (task.Exception == null)
        {
            Debug.Log("내 정보 저장 성공");
        }
        else
        {
            Debug.Log("내 정보 저장 실패" + task.Exception.Message);
        }
    }

    public void LoadMyInfo(Action<string> complete)
    {
        StartCoroutine(CoLoadMyInfo(complete));
    }

    private IEnumerator CoLoadMyInfo(Action<string> complete)
    {
        string path = "user_info/ " + FirebaseAuth.DefaultInstance.CurrentUser.UserId;
        //데이터 읽기 요청
        var task = database.GetReference(path).GetValueAsync();

        yield return new WaitUntil(() => task.IsCompleted);

        if (task.Exception == null)
        {
            task.Result.GetRawJsonValue();
            print("데이터 읽기 성공");
            if (complete != null)
            {
                complete(task.Result.GetRawJsonValue());
            }
        }
        else
        {
            print("데이터 읽기 실패" + task.Exception.Message);
        }
    }
}