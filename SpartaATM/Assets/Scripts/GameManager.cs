using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private string savePath; // JSON ���� ���

    [SerializeField] public UserData userData; // �ν����Ϳ��� Ȯ�� ����

    void Start()
    {
        LoadUserData();

        // �α��� â Ȱ��ȭ
        GameObject popupLogin = GameObject.Find("PopupLogin");
        GameObject popupBank = GameObject.Find("PopupBank");

        if (popupLogin != null && popupBank != null)
        {
            popupLogin.SetActive(true);
            popupBank.SetActive(false);
        }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        savePath = Path.Combine(Application.persistentDataPath, "userData.json");
        LoadUserData();
    }



    //������ ���� (����� �� �ڵ� ����)
    public void SaveUserData()
    {
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("������ ���� �Ϸ�: " + savePath);
    }

    //������ �ε� (���� ���� �� �ڵ� ����)
    public void LoadUserData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            userData = JsonUtility.FromJson<UserData>(json);
            Debug.Log("������ �ε� �Ϸ�");
        }
        else
        {
            // ������ ������ �⺻ �� ���� �� ����
            userData = new UserData("qwer","1234","������", 100000, 50000);
            SaveUserData();
        }
    }

    internal UserData GetUserData()
    {
        return userData;
    }
}
