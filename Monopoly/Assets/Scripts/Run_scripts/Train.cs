using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Train : Case
{
    private int[] prix;
    private bool owned;
    private int idOwner;
    public Train(int id, string stateCase, string name) : base(id, stateCase, name)
    {
        this.prix = new int[] { 200, 25, 50, 100, 200 };
        this.owned = false;
        this.idOwner = -1;
    }
    public override int Id { get { return this.id; } }
    public override string StateCase { get { return this.stateCase; } }
    public override string NameCase { get { return this.nameCase; } }
    public int[] Prix { get { return prix; } }
    public bool Owned { get { return owned; } set { owned = value; } }
    public int IDOwner { get { return idOwner; } set { idOwner = value; } }

    public int PayTrain(Board plateau)
    {
        int nbGares = 0;
        int pos = 5;

        while (pos < 36)
        {
            Train station = (Train)plateau.TheBoard[pos];

            if (station.IDOwner == this.idOwner)
            {
                nbGares++;
            }
            pos += 10;
        }

        return prix[nbGares];
    }
}
