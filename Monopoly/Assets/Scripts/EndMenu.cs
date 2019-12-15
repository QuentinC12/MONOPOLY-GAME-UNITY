using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndMenu : MonoBehaviour
{
    private Controller theController;

    public GameObject textFieldWinner;
    public GameObject textFieldPlayers;

    // Start is called before the first frame update
    void Start()
    {
        theController = GameObject.FindObjectOfType<Controller>();
        Dislay();
    }

    void Update()
    {
        
    }

    private void Dislay()
    {
        Player[] players = theController.listPlayer;
        Board plateau = theController.Plateau;

        int[] playersMoney = new int[players.Length];

        string displayPlayersMoney = "";

        for (int i = 0; i < players.Length; i++)
        {
            playersMoney[i] = players[i].Cash;
            for (int j = 0; j < plateau.TheBoard.Length; j++)
            {
                string state = plateau.TheBoard[j].StateCase;
                if (state == "P")
                {
                    Property P = (Property)plateau.TheBoard[j];
                    if (P.IDOwner == i)
                    {
                        playersMoney[i] += (P.Prix[6] + P.EtatCase * P.Prix[P.EtatCase]);
                    }
                }
                else if (state == "G")
                {
                    Train T = (Train)plateau.TheBoard[j];
                    if (T.IDOwner == i)
                    {
                        playersMoney[i] += (T.Prix[0]);
                    }
                }
                else if (state == "C")
                {
                    Companies C = (Companies)plateau.TheBoard[j];
                    if (C.IDOwner == i)
                    {
                        playersMoney[i] += (C.Prix[0]);
                    }
                }
            }
            displayPlayersMoney += players[i].Username + " has " + playersMoney[i] + "€\n";
        }
        textFieldPlayers.GetComponent<Text>().text = displayPlayersMoney;

        int richest = 0;
        int tmpMoney = 0;
        bool draw = true;
        for (int i = 0; i < playersMoney.Length; i++)
        {
            if(playersMoney[i] > tmpMoney)
            {
                tmpMoney = playersMoney[i];
                richest = i;
            }
            else if (playersMoney[i] < tmpMoney)
            {
                draw = false;
            }
        }

        if(draw) { textFieldWinner.GetComponent<Text>().text = "It's a draw!"; }
        else { textFieldWinner.GetComponent<Text>().text = "The winner is " + players[richest].Username + " with " + playersMoney[richest] + "€"; }
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
