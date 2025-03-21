using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupLogin : MonoBehaviour
{
    public GameObject Popuplogin;  // 로그인 팝업
    public GameObject Popupbank;   // 은행 팝업 (로그인 성공 시 활성화)

    public TMP_InputField inputID;        // ID 입력 필드
    public TMP_InputField inputPassword;  // 비밀번호 입력 필드
    public Button loginButton;            // 로그인 버튼
    public Button signUpButton;           // 회원가입 버튼
    public TMP_Text errorText;            // 오류 메시지 출력용

    private GameManager gameManager;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        loginButton.onClick.AddListener(OnLogin);
        signUpButton.onClick.AddListener(OnSignUp);
    }

    public void OnLogin()
    {
        string enteredID = inputID.text;
        string enteredPassword = inputPassword.text;

        if (enteredID == gameManager.userData.ID && enteredPassword == gameManager.userData.PS)
        {
            Debug.Log("로그인 성공");
            Popuplogin.SetActive(false);  // 로그인 팝업 닫기
            Popupbank.SetActive(true);    // 은행 팝업 활성화
        }
        else
        {
            Debug.Log("로그인 실패 - 잘못된 ID 또는 비밀번호");
            errorText.text = "잘못된 ID 또는 비밀번호입니다.";
            errorText.gameObject.SetActive(true);
        }
    }

    public void OnSignUp()
    {
        Debug.Log("회원가입 버튼 클릭됨 (구현 필요)");
        // 회원가입 로직 추가 가능
    }
}
