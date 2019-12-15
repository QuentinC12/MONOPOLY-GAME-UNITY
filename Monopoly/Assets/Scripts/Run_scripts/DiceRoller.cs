using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceRoller : MonoBehaviour
{
    // Start is called before the first frame update

    private int[] diceValues;
    private int diceTotal;
    private bool aDouble = false;

    // The images of each one of the die faces. They are associated to the variables in Unity
    public Sprite[] DiceImageOne;
    public Sprite[] DiceImageTwo;
    public Sprite[] DiceImageThree;
    public Sprite[] DiceImageFour;
    public Sprite[] DiceImageFive;
    public Sprite[] DiceImageSix;

    public PlayerPawn player1;
    public PlayerPawn player2;
    public PlayerPawn player3;
    public PlayerPawn player4;
    private Controller theController;

    void Start()
    {
        theController = GameObject.FindObjectOfType<Controller>();
    }

    public int[] DiceValues { get { return diceValues; } }
    public int DiceTotal { get { return diceTotal; } set { diceTotal = value; } }
    public bool Double { get { return this.aDouble; } }

    public void RollTheDice()
    {
        if ((!theController.DiceRolled || theController.DoubleCounter > 0) && !theController.HaveToPay)
        {
            diceValues = new int[2];
            //In monopoly you roll two dice.
            //Each have numbers from one to six
            //Each number on each dy has the same probability
            /*diceValues[0] = 5;
            diceValues[1] = 0;*/
            diceTotal = 0;
            for (int i = 0; i < diceValues.Length; i++) //Rolling the dice
            {
                diceValues[i] = UnityEngine.Random.Range(1, 7);
                diceTotal += diceValues[i];

                UpdateDiceVisual(i);
            }


            aDouble = false;
            if (diceValues != null) //Checking if there is a double or not.
            {
                int multiplyTest = diceValues[0] * diceValues.Length;
                int sumTest = 0;
                for (int i = 0; i < diceValues.Length; i++)
                {
                    sumTest += diceValues[i];
                }
                if (multiplyTest == sumTest) { aDouble = true; }
            }
            if (aDouble) //If double incresing the player double count
            {
                theController.DoubleCounter++;
            }
            else { theController.DoubleCounter = 0; }

            UpdatePlayerState(); //Updating the player state
        }

    }

    /// <summary>
    /// Using the immages added to unity we display the dice total
    /// </summary>
    /// <param name="i">Id of the dice result to consider in the diceValues tab</param>
    private void UpdateDiceVisual(int i)
    {
        if (diceValues[i] == 1)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite =
                DiceImageOne[UnityEngine.Random.Range(0, DiceImageOne.Length)];
        }
        else if (diceValues[i] == 2)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite =
                DiceImageTwo[UnityEngine.Random.Range(0, DiceImageTwo.Length)];
        }
        else if (diceValues[i] == 3)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite =
                DiceImageThree[UnityEngine.Random.Range(0, DiceImageThree.Length)];
        }
        else if (diceValues[i] == 4)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite =
                DiceImageFour[UnityEngine.Random.Range(0, DiceImageFour.Length)];
        }
        else if (diceValues[i] == 5)
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite =
                DiceImageFive[UnityEngine.Random.Range(0, DiceImageFive.Length)];
        }
        else
        {
            this.transform.GetChild(i).GetComponent<Image>().sprite =
                DiceImageSix[UnityEngine.Random.Range(0, DiceImageSix.Length)];
        }
    }

    private void UpdatePlayerState()
    {
        int idPlayer = theController.IdPlayerPlaying;
        int statut = theController.listPlayer[idPlayer].Statut;
        theController.DiceRolled = true;

        if ((statut == 0 || statut == 4 || aDouble) && theController.DoubleCounter < 3) //Check if the player is in prison or not. If he is check if it's his last time. If he is check if its a double.
        {
            if (!aDouble) { theController.rollDice.SetActive(false); }
            
            theController.listPlayer[idPlayer].Statut = 0;
            int tmp = diceTotal;
            diceTotal = theController.listPlayer[idPlayer].Forward(diceTotal);

            //Moving the player pawn if the Forward function of player hasn't a return a different value that would mean that the player has landed on the Go to Jail tile
            if (tmp != diceTotal)
            {
                theController.rollDice.SetActive(false);
                aDouble = false;
                theController.InfosToPlayer = "You land on Go to Prison!! You are now prison!!";
                Teleport();
            }
            else
            {
                MoveThePawn();
            }
            theController.CheckStateAfterRoll();
        }
        else if (statut == 0 && theController.DoubleCounter > 2)
        {
            theController.rollDice.SetActive(false);
            aDouble = false;
            //The state of the player is changed in the DoubleToPrison function
            //We are also getting the number of tiles to go to prison
            diceTotal = theController.listPlayer[idPlayer].DoubleToPrison();
            theController.InfosToPlayer = "Three double in a row!! You are now prison!!";
            theController.CheckStateAfterRoll();
            Teleport();
        }
        else
        {
            theController.rollDice.SetActive(false);
            theController.listPlayer[idPlayer].Statut ++;
            theController.CheckStateAfterRoll();
        }
    }


    /// <summary>
    /// To make the player pawn move
    /// </summary>
    private void MoveThePawn()
    {
        int id = theController.IdPlayerPlaying + 1;
        if (id == 1) { player1.Move(); }
        else if(id == 2) { player2.Move(); }
        else if(id == 3) { player3.Move(); }
        else { player4.Move(); }
    }

    public void Teleport()
    {
        int id = theController.IdPlayerPlaying + 1;
        if (id == 1) { player1.MoveToTile(); }
        else if (id == 2) { player2.MoveToTile(); }
        else if (id == 3) { player3.MoveToTile(); }
        else { player4.MoveToTile(); }
    }

}
