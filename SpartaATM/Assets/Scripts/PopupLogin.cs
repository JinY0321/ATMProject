using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class PopupLogin : MonoBehaviour
{
    public GameObject Popuplogin;  // �α��� �˾�
    public GameObject Popupbank;   // ���� �˾� (�α��� ���� �� Ȱ��ȭ)
    public GameObject PopupSignup; // ȸ������ �˾�

    public TMP_InputField inputID;        // ID �Է� �ʵ�
    public TMP_InputField inputPassword;  // ��й�ȣ �Է� �ʵ�
    public Button loginButton;            // �α��� ��ư
    public Button signUpButton;           // ȸ������ ��ư
    public GameObject loginTextError;    // ���� �޽��� ��¿�

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
            ShowError("ID�� ��й�ȣ�� �Է��ϼ���.");
            return;
        }

        // ID���� ����� JSON ���� �ε�
        string filePath = Path.Combine(Application.persistentDataPath, "UserData", $"{enteredID}.json");

        if (!File.Exists(filePath))
        {
            ShowError("�������� �ʴ� ID�Դϴ�.");
            return;
        }

        // ���Ͽ��� ������ �о����
        string json = File.ReadAllText(filePath);
        UserData loadedUser = JsonUtility.FromJson<UserData>(json);

        // ��й�ȣ Ȯ��
        if (loadedUser.PS != enteredPassword)
        {
            ShowError("�߸��� ��й�ȣ�Դϴ�.");
            return;
        }

        // �α��� ���� �� ���� �Ŵ����� ���� ������ �ε�
        gameManager.userData = loadedUser;
        gameManager.SaveUserData();  // �ֽ� �����͸� �ٽ� ����
        Debug.Log($"�α��� ����! {loadedUser.Name}�� ȯ���մϴ�.");

        // �˾� ��ȯ
        Popuplogin.SetActive(false);
        Popupbank.SetActive(true);

        // UI �ݿ�
        FindObjectOfType<UIManager>().Refresh();
    }

    // ���� �޽��� ��� �Լ�
    void ShowError(string message)
    {
        loginTextError.SetActive(true);
        loginTextError.GetComponent<TMP_Text>().text = message;
        Debug.Log(message);
    }

    public void OnSignUp()
    {
        Debug.Log("ȸ������ ��ư Ŭ���� (���� �ʿ�)");
        // ȸ������ ���� �߰� ����
        PopupSignup.SetActive(true);
    }

    public void CloseErrorPopup()
    {
        loginTextError.SetActive(false);
    }
}
