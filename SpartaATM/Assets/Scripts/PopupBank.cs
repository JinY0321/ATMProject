using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{

    private GameManager gameManager;

    // 입금 화면과 출금 화면을 각각 참조할 변수
    public GameObject depositScreen; //입금화면
    public GameObject withdrawScreen; //출금화면
    public GameObject defaultBtn; //원래 화면으로(BackBtn)

    public GameObject popupError;      // 잔액 부족 팝업
    public TextMeshProUGUI amountText;     // 잔액 표시 UI
    public TextMeshProUGUI CashText;     // 현금 잔액 표시 UI
    public TMP_InputField inputAmount; // 직접 입금할 금액 필드
    public TMP_InputField outputAmount; // 직접 입금할 금액 필드


    // 입금 버튼 클릭 시 호출되는 함수
    public void ShowDepositScreen()
    {
        // 입금 화면을 활성화하고 출금 화면을 비활성화
        depositScreen.SetActive(true);
        withdrawScreen.SetActive(false);
        defaultBtn.SetActive(false);
    }

    // 출금 버튼 클릭 시 호출되는 함수
    public void ShowWithdrawScreen()
    {
        // 출금 화면을 활성화하고 입금 화면을 비활성화
        depositScreen.SetActive(false);
        withdrawScreen.SetActive(true);
        defaultBtn.SetActive(false);
    }

    public void ShowDefaultBtn() //뒤로가기
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

    // UI 새로고침 (잔액 업데이트)
    void RefreshUI()
    {
        amountText.text = $"{gameManager.userData.Amount:N0} 원";
        CashText.text = $"{gameManager.userData.Cash:N0} 원";
    }

    // 지정된 금액만큼 입금
    public void Deposit(int amount)
    {
        if (gameManager.userData.Cash >= amount)
        {
            gameManager.userData.Cash -= amount;
            gameManager.userData.Amount += amount;
            gameManager.SaveUserData(); //입금 후 저장
            RefreshUI();
            Debug.Log($"{amount:N0}원 입금");
        }
        else
        {
            ShowErrorPopup("잔액이 부족합니다.");
        }
    }

    // 직접 입력한 금액만큼 입금
    public void DepositFromInput()
    {
        int amount;
        if (int.TryParse(inputAmount.text, out amount) && amount > 0) // 사용자가 입력한 값이 숫자인지 확인한 후, 0보다 클 경우에만 실행
        {
            Deposit(amount);
            inputAmount.text = ""; // 입력창 초기화
        }
        else
        {
            Debug.Log("유효하지 않음");
            ShowErrorPopup("유효한 금액을 입력하세요.");
        }
    }

    // 출금 기능 (잔액이 부족하면 오류 표시)
    public void Withdraw(int amount)
    {
        if (gameManager.userData.Amount >= amount)
        {
            gameManager.userData.Amount -= amount;
            gameManager.userData.Cash += amount;
            gameManager.SaveUserData(); //출금 후 저장
            RefreshUI();
            Debug.Log($"{amount:N0}원 출금");
        }
        else
        {
            Debug.Log("유효하지 않음");
            ShowErrorPopup("잔액이 부족합니다.");
        }
    }

    public void WithdrawFromOutput()
    {
        int amount;
        if (int.TryParse(outputAmount.text, out amount) && amount > 0) // 사용자가 입력한 값이 숫자인지 확인한 후, 0보다 클 경우에만 실행
        {
            Withdraw(amount);
            outputAmount.text = ""; // 입력창 초기화
        }
        else
        {
            Debug.Log("유효하지 않음");
            ShowErrorPopup("유효한 금액을 입력하세요.");
        }
    }


    // 오류 팝업 표시
    void ShowErrorPopup(string message)
    {
        popupError.SetActive(true);
    }

    // 오류 팝업 닫기
    public void CloseErrorPopup()
    {
        popupError.SetActive(false);
    }
}
