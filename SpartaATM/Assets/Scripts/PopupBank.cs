using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{

    private GameManager gameManager;

    // �Ա� ȭ��� ��� ȭ���� ���� ������ ����
    public GameObject depositScreen; //�Ա�ȭ��
    public GameObject withdrawScreen; //���ȭ��
    public GameObject defaultBtn; //���� ȭ������(BackBtn)

    public GameObject popupError;      // �ܾ� ���� �˾�
    public TextMeshProUGUI amountText;     // �ܾ� ǥ�� UI
    public TextMeshProUGUI CashText;     // ���� �ܾ� ǥ�� UI
    public TMP_InputField inputAmount; // ���� �Ա��� �ݾ� �ʵ�
    public TMP_InputField outputAmount; // ���� �Ա��� �ݾ� �ʵ�


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


    // ���� �˾� ǥ��
    void ShowErrorPopup(string message)
    {
        popupError.SetActive(true);
    }

    // ���� �˾� �ݱ�
    public void CloseErrorPopup()
    {
        popupError.SetActive(false);
    }
}
