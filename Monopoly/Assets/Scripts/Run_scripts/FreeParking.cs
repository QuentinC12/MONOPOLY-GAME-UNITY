using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeParking : Case
{
    private int cashAvailable;
    public FreeParking(int id, string stateCase, string name) : base(id, stateCase, name)
    {
        this.cashAvailable = 100;
    }

    public override int Id { get { return this.id; } }
    public override string StateCase { get { return this.stateCase; } }
    public override string NameCase { get { return this.nameCase; } }
    public int CashAvailable { get { return cashAvailable; } set { cashAvailable = value; } }

}
