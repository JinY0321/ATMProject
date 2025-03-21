using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TMP_Text userNameText;
    public TMP_Text cashText;
    public TMP_Text amountText;

    private void Start()
    {
        Refresh(); // 게임 시작 시 UI 반영
    }

    public void Refresh()
    {
        if (GameManager.Instance != null)
        {
            UserData userData = GameManager.Instance.GetUserData();
            userNameText.text = $"{userData.Name}";
            cashText.text = $"{userData.Cash:N0} 원";          // 천 단위 콤마
            amountText.text = $" {userData.Amount:N0} 원";  // 천 단위 콤마
        }
    }
}
