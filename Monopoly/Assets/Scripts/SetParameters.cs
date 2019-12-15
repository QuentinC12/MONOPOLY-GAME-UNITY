using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// This script is the only one used in the main menu to get the main parameters
/// that will be used in the game.
/// Number of players, their usernames and the number of laps that the players are going to do.
/// All the variables are in public and static to be accessible form another scene in Unity.
/// If we don't do that, all the variables created will destroyed when loading the next seen.
/// </summary>
public class SetParameters : MonoBehaviour
{
    protected string player1;
    protected string player2;
    protected string player3;
    protected string player4;

    protected int notNullCounter;
    protected string[] listUsernames;

    protected int lapNumber = 1;

    public GameObject inputField1; //Players usernames
    public GameObject textField1;
    
    public GameObject inputField2; //Laps the players want to do
    public GameObject textField2;


    static SetParameters instance;

    // Start is called before the first frame update
    void Start()
    {
        notNullCounter = 0;

        //We use the singleton pattern to create only one instance of the main menu

        if(instance != null)
        {
            //Someone else is the singleton already
            //So let's jsut destroy ourselevs before we cause trouble.
            Destroy(this.gameObject);
            return;
        }
        // If we get here , then we are "the one and only"
        instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject); //Become "immortal"
    }

    public static SetParameters GetInstance()
    {
        return instance;
    }
    public string Player1 { get; }
    public string Player2 { get; }
    public string Player3 { get; }
    public string Player4 { get; }

    public string[] ListUsernames()
    {
        return listUsernames;
    }
    public int NotNullCounter()
    {
        return notNullCounter;
    }
    public int LapNumber()
    {
        return lapNumber;
    }

    public void StoreUsername()
    {
        bool modif = false;
        string username = inputField1.GetComponent<Text>().text;
        if(player1 == null)
        {
            player1 = username;
            modif = true;
            notNullCounter++;
        }
        else if (player2 == null && username != player1)
        {
            player2 = username;
            modif = true;
            notNullCounter++;
        }
        else if (player3 == null && username != player1 && username != player2)
        {
            player3 = username;
            modif = true;
            notNullCounter++;
        }
        else if (player4 == null && username != player1 && username != player2 && username != player3)
        {
            player4 = username;
            modif = true;
            notNullCounter++;
        }

        if (modif)
        {
            UpdateWelcomePlayers();
        }
    }

    public void UpdateWelcomePlayers()
    {
        string welcomePlayers = " ";


        if (player1 != null)
        {
            welcomePlayers = " Welcome player " + player1 + " \n\n";
            if (player2 != null)
            {
                welcomePlayers += " Welcome player " + player2 + " \n\n";
                if (player3 != null)
                {
                    welcomePlayers += " Welcome player " + player3+ " \n\n";
                    if (player4 != null)
                    {
                        welcomePlayers += " Welcome payer " + player4 + " \n\n";
                    }
                }
                if (lapNumber != 0)
                {
                    welcomePlayers += "\n Number of lap(s): " + Convert.ToString(lapNumber);
                }
            }

        }

        textField1.GetComponent<Text>().text = welcomePlayers;
    }

    public void StoreLapNumber()
    {
        int tmp = Convert.ToInt32(inputField2.GetComponent<Text>().text);
        if(tmp > 0 && player1 != null && player2 != null)
        {
            lapNumber = tmp;
            UpdateWelcomePlayers();
        }
    }

    public void LoadGame()
    {

        if(notNullCounter >1)
        {
            listUsernames = new string[notNullCounter];
            for (int i = 0; i < notNullCounter; i++)
            {
                if (i + 1 == 1)
                {
                    listUsernames[i] = player1;
                }
                else if (i + 1 == 2)
                {
                    listUsernames[i] = player2;
                }
                else if (i + 1 == 3)
                {
                    listUsernames[i] = player3;
                }
                else if (i + 1 == 4)
                {
                    listUsernames[i] = player4;
                }
            }
            SceneManager.LoadScene(1);
        }
        else
        {
            textField2.GetComponent<Text>().text = "Not enough players";
        }
    }

    public void Quit()
    {
        Application.Quit();
    }
}
