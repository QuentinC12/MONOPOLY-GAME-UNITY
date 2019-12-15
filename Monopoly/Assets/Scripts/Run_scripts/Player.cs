using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private static int nbPlayer;
    private int idPlayer; //Id du player
    private int position; //position sur le plateau 0-39
    private int cash; //Cash qu'à le joueur
    private int statut; //Statut comme dit auparavant 4 -> 1er tour de prison ... 0 libre.
    private string username; //Nom du joueur !
    private int nbProperty;
    private bool playing;


    public Player()
    {
        nbPlayer++;
        idPlayer = nbPlayer;
        position = 0;
        cash = 1500;
        statut = 0;
        username = "unknow";
        nbProperty = 0;
        playing = true;
    }
    public Player(string username)
    {
        nbPlayer++;
        idPlayer = nbPlayer;
        position = 0;
        this.cash = 1500;
        this.statut = 0;
        this.username = username;
        nbProperty = 0;
        playing = true;
    }
    public Player(int cash, int statut, string username)
    {
        nbPlayer++;
        idPlayer = nbPlayer;
        position = 0;
        this.cash = cash;
        this.statut = statut;
        this.username = username;
        nbProperty = 0;
        playing = true;
    }


    public string Username { get { return username; } }
    public int Cash { get { return cash; } set { cash = value; } }
    public int Statut { get { return statut; } set { statut = value; } }
    public int Position { get { return position; } set { position = value; } }
    public int NbProperty { get { return nbProperty; } set { nbProperty = value; } }
    public bool Playing { get { return playing; } set { playing = value; } }

    public int Forward(int nb)
    {
        position += nb;
        if (position > 39)
        {
            cash += 200;
            position = position - 40;
            if (position == 0) { cash += 200; }
        }
        if (position == 30)
        {
            position = 10;
            statut = 1;
            nb += 20;
        }
        return nb;
    }

    public int DoubleToPrison()
    {
        int forwardToPrison = 0;

        statut = 1;

        if (position > 10)
        {
            forwardToPrison = 11 + (39 - position);
        }
        else
        {
            forwardToPrison = 10 - position;
        }
        position = 10;
        return forwardToPrison;
    }
}
