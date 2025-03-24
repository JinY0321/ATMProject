using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class PopupSignUp : MonoBehaviour
{
    public TMP_InputField inputID;
    public TMP_InputField inputName;
    public TMP_InputField inputPassword;
    public TMP_InputField inputPasswordConfirm;

    public TextMeshProUGUI errorText;

    public Button cancelButton;
    public Button signUpButton;
    public GameObject errorPopup;

    public GameObject SignupPopup;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.Instance;
        signUpButton.onClick.AddListener(SignUp);
        cancelButton.onClick.AddListener(OnCancel);
    }

    public void SignUp()
    {
        string id = inputID.text.Trim();
        string name = inputName.text.Trim();
        string password = inputPassword.text;
        string passwordConfirm = inputPasswordConfirm.text;

        if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name) ||
            string.IsNullOrEmpty(password) || string.IsNullOrEmpty(passwordConfirm))
        {
            ShowError("모든 필드를 입력하세요.");
            return;
        }

        if (password != passwordConfirm)
        {
            ShowError("비밀번호가 일치하지 않습니다.");
            return;
        }

        // ID별로 개별 파일 저장
        string userFolder = Path.Combine(Application.persistentDataPath, "UserData");
        if (!Directory.Exists(userFolder))
        {
            Directory.CreateDirectory(userFolder);
        }

        string filePath = Path.Combine(userFolder, $"{id}.json");

        if (File.Exists(filePath))
        {
            ShowError("이미 존재하는 ID입니다.");
            return;
        }

        // 새 UserData 객체 생성
        UserData newUser = new UserData(id, password, name, 100000, 50000);

        // Json 직렬화 후 저장
        string json = JsonUtility.ToJson(newUser, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"{name} 계정 생성 완료! 파일 위치: {filePath}");
        gameObject.SetActive(false);  // 회원가입 창 닫기
    }

    void ShowError(string message)
    {
        errorText.text = message;
        errorPopup.gameObject.SetActive(true);
    }

    public void OnCancel()
    {
        SignupPopup.SetActive(false);  // 회원가입 창 닫기
    }

    public void CloseErrorPopup()
    {
        errorPopup.SetActive(false);
    }
}
