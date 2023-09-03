using Firebase.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FBAuth : MonoBehaviour
{
    private FirebaseAuth auth;
    public InputField inputEmail;
    public InputField inputPassword;

    private void Start()
    {
        //auth 기능을 전체적으로 권리 실행하는 클래스 담아놓자.
        auth = FirebaseAuth.DefaultInstance;
        //로그인, 로그아웃 상태 체크
        auth.StateChanged += OnChangeAuthState;
    }

    private void OnChangeAuthState(object sender, EventArgs eventArgs)
    {
        //만약에 유저정보가 있으면
        if (auth.CurrentUser != null)
        {
            //로그인, 아이디 출력
            print("Email: " + auth.CurrentUser.Email);
            print("UserId: " + auth.CurrentUser.UserId);
            Debug.Log("로그인 상태");

            //내 정보 불러오기
            FBDatabase.instance.LoadMyInfo(OnLoadMyInfo);
        }
        else
        {
            //로그아웃 상태
            Debug.Log("로그아웃 상태");
        }
    }

    public UserInfo myinfo;

    private void OnLoadMyInfo(string strInfo)
    {
        myinfo = JsonUtility.FromJson<UserInfo>(strInfo);
    }

    public void OnClickSignIn()
    {
        StartCoroutine(CoSignIn());
    }

    private IEnumerator CoSignIn()
    {
        //회원가입 시도
        var task = auth.CreateUserWithEmailAndPasswordAsync(inputEmail.text, inputPassword.text);

        //통신이 완료될때까지 기다리자
        yield return new WaitUntil(() => task.IsCompleted);

        //만약에 예외사항이 없으면 회원가입 성공
        //그렇지 않으면 회원가입 실패 메시지를 띄우자
        if (task.Exception == null)
        {
            Debug.Log("회원가입 성공");
        }
        else
        {
            Debug.Log("회원가입 실패" + task.Exception.Message);
        }
    }

    //로그인
    public void OnClickLogIn()
    {
        StartCoroutine(CoLogin());
    }

    private IEnumerator CoLogin()
    {
        //로그인 시도
        var task = auth.SignInWithEmailAndPasswordAsync(inputEmail.text, inputPassword.text);

        //통신이 완료될때까지 기다리자
        yield return new WaitUntil(() => task.IsCompleted);

        //만약에 예외사항이 없으면 로그인 성공
        //그렇지않다면 로그인 실패 메시지를 띄우자
        if (task.Exception == null)
        {
            Debug.Log("로그인 성공");
        }
        else
        {
            Debug.Log("로그인 실패" + task.Exception.Message);
        }
    }

    //로그아웃
    public void OnClickSignOut()
    {
        auth.SignOut();
    }
}