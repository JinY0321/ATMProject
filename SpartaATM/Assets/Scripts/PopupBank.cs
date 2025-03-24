using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{

    private GameManager gameManager;

    // �Ա� ȭ��� ��� ȭ���� ���� ������ ����
    public GameObject depositScreen; //�Ա�ȭ��
    public GameObject withdrawScreen; //���ȭ��
    public GameObject tossScreen;//�۱� ȭ��
    public GameObject defaultBtn; //���� ȭ������(BackBtn)

    public GameObject popupError;      // �ܾ� ���� �˾�
    public TextMeshProUGUI errorText; // �ܾ� ���� �˾� �ؽ�Ʈ

    public TextMeshProUGUI amountText;     // �ܾ� ǥ�� UI
    public TextMeshProUGUI CashText;     // ���� �ܾ� ǥ�� UI

    public TMP_InputField inputAmount; // ���� �Ա��� �ݾ� �ʵ�
    public TMP_InputField outputAmount; // ���� �Ա��� �ݾ� �ʵ�

    public GameObject popupSuccess; //�۱� �Ϸ�ÿ� ����� �˾�
    public TextMeshProUGUI SuccessText;//�۱� �Ϸ�� �ߴ� �ؽ�Ʈ

    public TMP_InputField tossAmountID; //�۱� �� ���� ���̵�
    public TMP_InputField tossAmount; //�۱� �� �ݾ�



    // �Ա� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void ShowDepositScreen()
    {
        // �Ա� ȭ���� Ȱ��ȭ�ϰ� ��� ȭ���� ��Ȱ��ȭ
        depositScreen.SetActive(true);
        withdrawScreen.SetActive(false);
        defaultBtn.SetActive(false);
    }

    // ��� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void ShowWithdrawScreen()
    {
        // ��� ȭ���� Ȱ��ȭ�ϰ� �Ա� ȭ���� ��Ȱ��ȭ
        depositScreen.SetActive(false);
        withdrawScreen.SetActive(true);
        defaultBtn.SetActive(false);
    }

    public void ShowDefaultBtn() //�ڷΰ���
    {
        depositScreen.SetActive(false);
        withdrawScreen.SetActive(false);
        tossScreen.SetActive(false);
        defaultBtn.SetActive(true);
    }

    void Start()
    {
        gameManager = GameManager.Instance;
        RefreshUI();
    }

    // UI ���ΰ�ħ (�ܾ� ������Ʈ)
    void RefreshUI()
    {
        amountText.text = $"{gameManager.userData.Amount:N0} ��";
        CashText.text = $"{gameManager.userData.Cash:N0} ��";
    }

    // ������ �ݾ׸�ŭ �Ա�
    public void Deposit(int amount)
    {
        if (gameManager.userData.Cash >= amount)
        {
            gameManager.userData.Cash -= amount;
            gameManager.userData.Amount += amount;
            gameManager.SaveUserData(); //�Ա� �� ����
            RefreshUI();
            Debug.Log($"{amount:N0}�� �Ա�");
        }
        else
        {
            ShowErrorPopup("�ܾ��� �����մϴ�.");
        }
    }

    // ���� �Է��� �ݾ׸�ŭ �Ա�
    public void DepositFromInput()
    {
        int amount;
        if (int.TryParse(inputAmount.text, out amount) && amount > 0) // ����ڰ� �Է��� ���� �������� Ȯ���� ��, 0���� Ŭ ��쿡�� ����
        {
            Deposit(amount);
            inputAmount.text = ""; // �Է�â �ʱ�ȭ
        }
        else
        {
            Debug.Log("��ȿ���� ����");
            ShowErrorPopup("��ȿ�� �ݾ��� �Է��ϼ���.");
        }
    }

    // ��� ��� (�ܾ��� �����ϸ� ���� ǥ��)
    public void Withdraw(int amount)
    {
        if (gameManager.userData.Amount >= amount)
        {
            gameManager.userData.Amount -= amount;
            gameManager.userData.Cash += amount;
            gameManager.SaveUserData(); //��� �� ����
            RefreshUI();
            Debug.Log($"{amount:N0}�� ���");
        }
        else
        {
            Debug.Log("��ȿ���� ����");
            ShowErrorPopup("�ܾ��� �����մϴ�.");
        }
    }

    public void WithdrawFromOutput()
    {
        int amount;
        if (int.TryParse(outputAmount.text, out amount) && amount > 0) // ����ڰ� �Է��� ���� �������� Ȯ���� ��, 0���� Ŭ ��쿡�� ����
        {
            Withdraw(amount);
            outputAmount.text = ""; // �Է�â �ʱ�ȭ
        }
        else
        {
            Debug.Log("��ȿ���� ����");
            ShowErrorPopup("��ȿ�� �ݾ��� �Է��ϼ���.");
        }
    }

    public void ShowTossScreen()
    {
        // �۱�ȭ�� Ȱ��ȭ
        defaultBtn.SetActive(false);
        tossScreen.SetActive(true);
    }

    // �۱� ��ư Ŭ�� �� ȣ��Ǵ� �Լ�
    public void OnSendMoney()
    {
        string targetID = tossAmountID.text.Trim();  // �۱� ��� ID
        string amountText = tossAmount.text.Trim(); // �۱� �ݾ�

        // �۱� ��� ID �Ǵ� �ݾ��� ��� ������ ���� �޽���
        if (string.IsNullOrEmpty(targetID) || string.IsNullOrEmpty(amountText))
        {
            ShowErrorPopup("�۱� ���� �ݾ��� �Է��ϼ���.");
            return;
        }

        int amount;
        if (!int.TryParse(amountText, out amount) || amount <= 0)
        {
            ShowErrorPopup("��ȿ�� �ݾ��� �Է��ϼ���.");
            return;
        }

        // ������ ����� �ܾ��� ������� Ȯ��
        if (gameManager.userData.Amount < amount)
        {
            ShowErrorPopup("�ܾ��� �����մϴ�.");
            return;
        }

        // �۱� ��� ID�� �ش��ϴ� ���� ������ �ҷ�����
        string targetFilePath = Path.Combine(Application.persistentDataPath, "UserData", $"{targetID}.json");

        if (!File.Exists(targetFilePath))
        {
            ShowErrorPopup("�۱� ����� �������� �ʽ��ϴ�.");
            return;
        }

        // ��� ����� ������ �ε�
        string targetJson = File.ReadAllText(targetFilePath);
        UserData targetUser = JsonUtility.FromJson<UserData>(targetJson);

        // �۱� �� �ݾ� ������Ʈ
        gameManager.userData.Amount -= amount;
        targetUser.Amount += amount;

        // ����� �ݾ��� ���Ͽ� ����
        gameManager.SaveUserData(); // ������ ��� ����
        File.WriteAllText(targetFilePath, JsonUtility.ToJson(targetUser, true)); // �޴� ��� ����

        // UI ���ΰ�ħ
        RefreshUI();

        // ���� �޽��� �� UI ����
        Debug.Log($"�۱� ����! {targetID}���� {amount}���� �۱��߽��ϴ�.");
        ShowSuccessPopup("�۱� ����!");
    }

    // �۱� ���� �˾� ǥ��
    void ShowSuccessPopup(string message)
    {
        popupSuccess.SetActive(true);
        SuccessText.GetComponent<TMP_Text>().text = message;
    }

    // �۱� ���� �˾� ǥ��
    void ShowErrorPopup(string message)
    {
        popupError.SetActive(true);
        errorText.GetComponent<TMP_Text>().text = message;

    }

    // ���� �˾� �ݱ�
    public void CloseErrorPopup()
    {
        popupError.SetActive(false);
    }

    public void CloseSuccessPopup()
    {
        popupSuccess.SetActive(false);
    }

}
