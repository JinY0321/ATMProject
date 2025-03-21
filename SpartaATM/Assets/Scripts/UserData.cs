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
    public int Cash; //���� ����
    [SerializeField]
    public int Amount; //���� �ܾ�
    [SerializeField]
    public string ID;//���̵�
    [SerializeField]
    public string PS;//��й�ȣ

    public UserData(string id, string ps, string name, int cash, int amount)
    {

        this.ID = id;
        this.PS = ps;
        this.Name = name;
        this.Cash = cash;
        this.Amount = amount;
    }
}
