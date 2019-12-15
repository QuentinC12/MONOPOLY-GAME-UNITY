using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Case
{
    protected int id;
    protected string stateCase; //Chance, communauté, prison, (achetable ou déjà acheté ça non ça sera ailleurs) 
    protected string nameCase; // Nom exemple rue de la paix

    public Case(int id, string stateCase, string name)
    {
        this.id = id;
        this.stateCase = stateCase;
        this.nameCase = name;
    }


    public abstract int Id { get; }
    public abstract string StateCase { get; }
    public abstract string NameCase { get; }
}
