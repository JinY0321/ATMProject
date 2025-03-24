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
            ShowError("��� �ʵ带 �Է��ϼ���.");
            return;
        }

        if (password != passwordConfirm)
        {
            ShowError("��й�ȣ�� ��ġ���� �ʽ��ϴ�.");
            return;
        }

        // ID���� ���� ���� ����
        string userFolder = Path.Combine(Application.persistentDataPath, "UserData");
        if (!Directory.Exists(userFolder))
        {
            Directory.CreateDirectory(userFolder);
        }

        string filePath = Path.Combine(userFolder, $"{id}.json");

        if (File.Exists(filePath))
        {
            ShowError("�̹� �����ϴ� ID�Դϴ�.");
            return;
        }

        // �� UserData ��ü ����
        UserData newUser = new UserData(id, password, name, 100000, 50000);

        // Json ����ȭ �� ����
        string json = JsonUtility.ToJson(newUser, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"{name} ���� ���� �Ϸ�! ���� ��ġ: {filePath}");
        gameObject.SetActive(false);  // ȸ������ â �ݱ�
    }

    void ShowError(string message)
    {
        errorText.text = message;
        errorPopup.gameObject.SetActive(true);
    }

    public void OnCancel()
    {
        SignupPopup.SetActive(false);  // ȸ������ â �ݱ�
    }

    public void CloseErrorPopup()
    {
        errorPopup.SetActive(false);
    }
}
