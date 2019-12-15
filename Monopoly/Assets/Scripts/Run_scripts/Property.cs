using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Property : Case
{

    private int etatCase; //0, 1, 2, 3, 4, 5 houses knowing that 5 houses equals to an hotel
    private int[] prix; //0, 1, 2, 3, 4, 5 the price to pay to the owner corresponding to the number of houses 6 buying price of the property 7 price to buy a house
    private bool owned;
    private int idOwner;
    private int nbHouse;

    public Property(int id, string stateCase, string name, int[] prix) : base(id, stateCase, name)
    {
        this.etatCase = 0;
        this.prix = prix;
        this.owned = false;
        this.idOwner = -1;
        this.nbHouse = 0;
    }
    public override int Id { get { return this.id; } }
    public override string StateCase { get { return this.stateCase; } }
    public override string NameCase { get { return this.nameCase; } }
    public int EtatCase { get { return this.etatCase; } set { this.etatCase = value; } }
    public int[] Prix { get { return this.prix; } }
    public bool Owned { get { return owned; } set { owned = value; } }
    public int IDOwner { get { return idOwner; } set { idOwner = value; } }

}