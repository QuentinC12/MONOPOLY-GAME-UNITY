using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Companies : Case
{
    private int[] prix;
    private bool owned;
    private int idOwner;
    public Companies(int id, string stateCase, string name) : base(id, stateCase, name)
    {
        this.prix = new int[] { 150, 4, 10};
        this.owned = false;
        this.idOwner = -1;
    }
    public override int Id { get { return this.id; } }
    public override string StateCase { get { return this.stateCase; } }
    public override string NameCase { get { return this.nameCase; } }
    public int[] Prix { get { return prix; } }
    public bool Owned { get { return owned; } set { owned = value; } }
    public int IDOwner { get { return idOwner; } set { idOwner = value; } }

    public int PayCompanies(Board plateau)
    {
        int nbCompanies = 0;
        int pos = 12;
        

        while (pos < 29)
        {
            Companies station = (Companies)plateau.TheBoard[pos];

            if (station.IDOwner == this.idOwner)
            {
                nbCompanies++;
            }
            pos += 16;
        }

        return prix[nbCompanies];
    }
}
