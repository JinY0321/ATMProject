using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class PopupLogin : MonoBehaviour
{
    public GameObject Popuplogin;  // 로그인 팝업
    public GameObject Popupbank;   // 은행 팝업 (로그인 성공 시 활성화)
    public GameObject PopupSignup; // 회원가입 팝업

    public TMP_InputField inputID;        // ID 입력 필드
    public TMP_InputField inputPassword;  // 비밀번호 입력 필드
    public Button loginButton;            // 로그인 버튼
    public Button signUpButton;           // 회원가입 버튼
    public GameObject loginTextError;    // 오류 메시지 출력용

    private GameManager gameManager;
    private UIManager uiManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        loginButton.onClick.AddListener(OnLogin);
        signUpButton.onClick.AddListener(OnSignUp);
    }

    public void OnLogin()
    {
        string enteredID = inputID.text.Trim();
        string enteredPassword = inputPassword.text;

        if (string.IsNullOrEmpty(enteredID) || string.IsNullOrEmpty(enteredPassword))
        {
            ShowError("ID와 비밀번호를 입력하세요.");
            return;
        }

        // ID별로 저장된 JSON 파일 로드
        string filePath = Path.Combine(Application.persistentDataPath, "UserData", $"{enteredID}.json");

        if (!File.Exists(filePath))
        {
            ShowError("존재하지 않는 ID입니다.");
            return;
        }

        // 파일에서 데이터 읽어오기
        string json = File.ReadAllText(filePath);
        UserData loadedUser = JsonUtility.FromJson<UserData>(json);

        // 비밀번호 확인
        if (loadedUser.PS != enteredPassword)
        {
            ShowError("잘못된 비밀번호입니다.");
            return;
        }

        // 로그인 성공 → 게임 매니저에 유저 데이터 로드
        gameManager.userData = loadedUser;
        gameManager.SaveUserData();  // 최신 데이터를 다시 저장
        Debug.Log($"로그인 성공! {loadedUser.Name}님 환영합니다.");

        // 팝업 전환
        Popuplogin.SetActive(false);
        Popupbank.SetActive(true);

        // UI 반영
        FindObjectOfType<UIManager>().Refresh();
    }

    // 오류 메시지 출력 함수
    void ShowError(string message)
    {
        loginTextError.SetActive(true);
        loginTextError.GetComponent<TMP_Text>().text = message;
        Debug.Log(message);
    }

    public void OnSignUp()
    {
        Debug.Log("회원가입 버튼 클릭됨 (구현 필요)");
        // 회원가입 로직 추가 가능
        PopupSignup.SetActive(true);
    }

    public void CloseErrorPopup()
    {
        loginTextError.SetActive(false);
    }
}
