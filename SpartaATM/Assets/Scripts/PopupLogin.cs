using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopupLogin : MonoBehaviour
{
    public GameObject Popuplogin;  // �α��� �˾�
    public GameObject Popupbank;   // ���� �˾� (�α��� ���� �� Ȱ��ȭ)

    public TMP_InputField inputID;        // ID �Է� �ʵ�
    public TMP_InputField inputPassword;  // ��й�ȣ �Է� �ʵ�
    public Button loginButton;            // �α��� ��ư
    public Button signUpButton;           // ȸ������ ��ư
    public TMP_Text errorText;            // ���� �޽��� ��¿�

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
            Debug.Log("�α��� ����");
            Popuplogin.SetActive(false);  // �α��� �˾� �ݱ�
            Popupbank.SetActive(true);    // ���� �˾� Ȱ��ȭ
        }
        else
        {
            Debug.Log("�α��� ���� - �߸��� ID �Ǵ� ��й�ȣ");
            errorText.text = "�߸��� ID �Ǵ� ��й�ȣ�Դϴ�.";
            errorText.gameObject.SetActive(true);
        }
    }

    public void OnSignUp()
    {
        Debug.Log("ȸ������ ��ư Ŭ���� (���� �ʿ�)");
        // ȸ������ ���� �߰� ����
    }
}
