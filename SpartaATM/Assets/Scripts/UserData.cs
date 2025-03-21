using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class UserData
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public int Cash; //보유 현금
    [SerializeField]
    public int Amount; //계좌 잔액
    [SerializeField]
    public string ID;//아이디
    [SerializeField]
    public string PS;//비밀번호

    public UserData(string id, string ps, string name, int cash, int amount)
    {

        this.ID = id;
        this.PS = ps;
        this.Name = name;
        this.Cash = cash;
        this.Amount = amount;
    }
}
