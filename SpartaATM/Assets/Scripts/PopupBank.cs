using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupBank : MonoBehaviour
{

    private GameManager gameManager;

    // 입금 화면과 출금 화면을 각각 참조할 변수
    public GameObject depositScreen; //입금화면
    public GameObject withdrawScreen; //출금화면
    public GameObject tossScreen;//송금 화면
    public GameObject defaultBtn; //원래 화면으로(BackBtn)

    public GameObject popupError;      // 잔액 부족 팝업
    public TextMeshProUGUI errorText; // 잔액 부족 팝업 텍스트

    public TextMeshProUGUI amountText;     // 잔액 표시 UI
    public TextMeshProUGUI CashText;     // 현금 잔액 표시 UI

    public TMP_InputField inputAmount; // 직접 입금할 금액 필드
    public TMP_InputField outputAmount; // 직접 입금할 금액 필드

    public GameObject popupSuccess; //송금 완료시에 사용할 팝업
    public TextMeshProUGUI SuccessText;//송금 완료시 뜨는 텍스트

    public TMP_InputField tossAmountID; //송금 할 계좌 아이디
    public TMP_InputField tossAmount; //송금 할 금액



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
        tossScreen.SetActive(false);
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

    public void ShowTossScreen()
    {
        // 송금화면 활성화
        defaultBtn.SetActive(false);
        tossScreen.SetActive(true);
    }

    // 송금 버튼 클릭 시 호출되는 함수
    public void OnSendMoney()
    {
        string targetID = tossAmountID.text.Trim();  // 송금 대상 ID
        string amountText = tossAmount.text.Trim(); // 송금 금액

        // 송금 대상 ID 또는 금액이 비어 있으면 오류 메시지
        if (string.IsNullOrEmpty(targetID) || string.IsNullOrEmpty(amountText))
        {
            ShowErrorPopup("송금 대상과 금액을 입력하세요.");
            return;
        }

        int amount;
        if (!int.TryParse(amountText, out amount) || amount <= 0)
        {
            ShowErrorPopup("유효한 금액을 입력하세요.");
            return;
        }

        // 보내는 사람의 잔액이 충분한지 확인
        if (gameManager.userData.Amount < amount)
        {
            ShowErrorPopup("잔액이 부족합니다.");
            return;
        }

        // 송금 대상 ID에 해당하는 유저 데이터 불러오기
        string targetFilePath = Path.Combine(Application.persistentDataPath, "UserData", $"{targetID}.json");

        if (!File.Exists(targetFilePath))
        {
            ShowErrorPopup("송금 대상이 존재하지 않습니다.");
            return;
        }

        // 대상 사용자 데이터 로드
        string targetJson = File.ReadAllText(targetFilePath);
        UserData targetUser = JsonUtility.FromJson<UserData>(targetJson);

        // 송금 후 금액 업데이트
        gameManager.userData.Amount -= amount;
        targetUser.Amount += amount;

        // 변경된 금액을 파일에 저장
        gameManager.SaveUserData(); // 보내는 사람 저장
        File.WriteAllText(targetFilePath, JsonUtility.ToJson(targetUser, true)); // 받는 사람 저장

        // UI 새로고침
        RefreshUI();

        // 성공 메시지 및 UI 갱신
        Debug.Log($"송금 성공! {targetID}에게 {amount}원을 송금했습니다.");
        ShowSuccessPopup("송금 성공!");
    }

    // 송금 성공 팝업 표시
    void ShowSuccessPopup(string message)
    {
        popupSuccess.SetActive(true);
        SuccessText.GetComponent<TMP_Text>().text = message;
    }

    // 송금 실패 팝업 표시
    void ShowErrorPopup(string message)
    {
        popupError.SetActive(true);
        errorText.GetComponent<TMP_Text>().text = message;

    }

    // 오류 팝업 닫기
    public void CloseErrorPopup()
    {
        popupError.SetActive(false);
    }

    public void CloseSuccessPopup()
    {
        popupSuccess.SetActive(false);
    }

}
