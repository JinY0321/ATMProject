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
        Refresh(); // ���� ���� �� UI �ݿ�
    }

    public void Refresh()
    {
        if (GameManager.Instance != null)
        {
            UserData userData = GameManager.Instance.GetUserData();
            userNameText.text = $"{userData.Name}";
            cashText.text = $"{userData.Cash:N0} ��";          // õ ���� �޸�
            amountText.text = $" {userData.Amount:N0} ��";  // õ ���� �޸�
        }
    }
}
