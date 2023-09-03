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
        //auth ����� ��ü������ �Ǹ� �����ϴ� Ŭ���� ��Ƴ���.
        auth = FirebaseAuth.DefaultInstance;
        //�α���, �α׾ƿ� ���� üũ
        auth.StateChanged += OnChangeAuthState;
    }

    private void OnChangeAuthState(object sender, EventArgs eventArgs)
    {
        //���࿡ ���������� ������
        if (auth.CurrentUser != null)
        {
            //�α���, ���̵� ���
            print("Email: " + auth.CurrentUser.Email);
            print("UserId: " + auth.CurrentUser.UserId);
            Debug.Log("�α��� ����");

            //�� ���� �ҷ�����
            FBDatabase.instance.LoadMyInfo(OnLoadMyInfo);
        }
        else
        {
            //�α׾ƿ� ����
            Debug.Log("�α׾ƿ� ����");
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
        //ȸ������ �õ�
        var task = auth.CreateUserWithEmailAndPasswordAsync(inputEmail.text, inputPassword.text);

        //����� �Ϸ�ɶ����� ��ٸ���
        yield return new WaitUntil(() => task.IsCompleted);

        //���࿡ ���ܻ����� ������ ȸ������ ����
        //�׷��� ������ ȸ������ ���� �޽����� �����
        if (task.Exception == null)
        {
            Debug.Log("ȸ������ ����");
        }
        else
        {
            Debug.Log("ȸ������ ����" + task.Exception.Message);
        }
    }

    //�α���
    public void OnClickLogIn()
    {
        StartCoroutine(CoLogin());
    }

    private IEnumerator CoLogin()
    {
        //�α��� �õ�
        var task = auth.SignInWithEmailAndPasswordAsync(inputEmail.text, inputPassword.text);

        //����� �Ϸ�ɶ����� ��ٸ���
        yield return new WaitUntil(() => task.IsCompleted);

        //���࿡ ���ܻ����� ������ �α��� ����
        //�׷����ʴٸ� �α��� ���� �޽����� �����
        if (task.Exception == null)
        {
            Debug.Log("�α��� ����");
        }
        else
        {
            Debug.Log("�α��� ����" + task.Exception.Message);
        }
    }

    //�α׾ƿ�
    public void OnClickSignOut()
    {
        auth.SignOut();
    }
}