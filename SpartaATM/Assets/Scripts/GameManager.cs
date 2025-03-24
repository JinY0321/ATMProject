using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private string savePath; // JSON 파일 경로

    [SerializeField] public UserData userData; // 인스펙터에서 확인 가능

    void Start()
    {
        LoadUserData();

        // 로그인 창 활성화
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



    //데이터 저장 (입출금 후 자동 실행)
    public void SaveUserData()
    {
        string json = JsonUtility.ToJson(userData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("데이터 저장 완료: " + savePath);
    }

    //데이터 로드 (게임 시작 시 자동 실행)
    public void LoadUserData()
    {
        if (File.Exists(savePath))
        {
            string json = File.ReadAllText(savePath);
            userData = JsonUtility.FromJson<UserData>(json);
            Debug.Log("데이터 로드 완료");
        }
        else
        {
            // 파일이 없으면 기본 값 설정 후 저장
            userData = new UserData("qwer","1234","나진영", 100000, 50000);
            SaveUserData();
        }
    }

    internal UserData GetUserData()
    {
        return userData;
    }
}
