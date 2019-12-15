using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controller : MonoBehaviour
{
    private SetParameters GameParameters; //Allow to get the parameters set by the players
    public int nbPlayer = 2; //Number of players - the minimum by default is 2

    private int tour; //Lap we are at
    private int tourMax; //The maximum laps the players can do

    public Player[] listPlayer; //Players list
    private Board plateau;
    private bool playerHasProperties;
    
    private bool victory;



    //Flags for Unity
    private bool diceRolled = false;
    private int doubleCounter;
    private bool check1 = false;

    private bool haveToPay = false;


    //Objects for unity
    private DiceRoller theDiceRoller;
    private int initialPosition;
    private int idPlayerPlaying;
    private int idPlayerToMove;
    private string tileInfoText;
    private string infosToPlayer;

    //public GameObject button_roll;

    public TextAsset file;

    public GameObject rollDice;
    public GameObject button_buy;
    public GameObject button_pay;
    public GameObject button_ok_buy_house;
    public GameObject inputField_buy_house;
    public GameObject button_ok_sell_property;
    public GameObject inputField_sell_house;

    public GameObject textFieldLapNumber;
    public GameObject textFieldMaxLap;
    public GameObject textFieldPlayerTurn;
    public GameObject textFieldPlayerMoney;
    public GameObject textFieldPlayerProperty;
    public GameObject textFieldPlayersPlaying;
    public GameObject textFieldInfoPlayer;
    public GameObject textFieldDouble;

    public GameObject textFieldInfoCase;

    public GameObject inputField1; //ID to buy a house for a property
    public GameObject inputField2; //ID to sell a property

    void Start()
    {
        DeactivateButtons();

        theDiceRoller = GameObject.FindObjectOfType<DiceRoller>();

        nbPlayer = SetParameters.GetInstance().NotNullCounter();
        //We create the array of palyers
        listPlayer = new Player[nbPlayer];
        for (int i = 0; i < this.nbPlayer; i++)
        {
            listPlayer[i] = new Player(SetParameters.GetInstance().ListUsernames()[i]);
        }

        plateau = new Board(file);

        tourMax = SetParameters.GetInstance().LapNumber();
        tour = 1;

        idPlayerPlaying = 0;
        idPlayerToMove = 1;
        initialPosition = listPlayer[idPlayerPlaying].Position;
        doubleCounter = 0;
        infosToPlayer = " ";
        playerHasProperties = false;

        victory = false;

        UpdateDisplay();
    }
    // Update is called once per frame
    void Update()
    {
        UpdateDisplay();
        if (infosToPlayer.Split('\n').Length > 4)
        {
            infosToPlayer = " ";
        }
    }
    
    public int IdPlayerPlaying { get { return idPlayerPlaying; } }
    public bool DiceRolled { get { return diceRolled; } set { diceRolled = value; } }
    public int DoubleCounter { get { return doubleCounter; } set { doubleCounter = value; } }
    public bool Check1 { get { return check1; } set { check1 = value; } }
    public string InfosToPlayer {  get { return infosToPlayer; } set { infosToPlayer = value; } }
    public Board Plateau { get { return plateau; } }
    public bool HaveToPay { get { return haveToPay; } }


    public void DeactivateButtons()
    {
        button_buy.SetActive(false);
        button_pay.SetActive(false);
        button_ok_buy_house.SetActive(false);
        button_ok_sell_property.SetActive(false);
        inputField_buy_house.SetActive(false);
        inputField_sell_house.SetActive(false);
        textFieldInfoCase.SetActive(false);
    }
    private void UpdateDisplay()
    {
        textFieldLapNumber.GetComponent<Text>().text = "Lap " + Convert.ToString(tour);
        textFieldMaxLap.GetComponent<Text>().text = "Max " + Convert.ToString(tourMax) + " lap";
        textFieldPlayerTurn.GetComponent<Text>().text = "Turn of " + listPlayer[idPlayerPlaying].Username;
        textFieldPlayerMoney.GetComponent<Text>().text = Convert.ToString(listPlayer[idPlayerPlaying].Cash) + " €";
        textFieldInfoPlayer.GetComponent<Text>().text = infosToPlayer;
        DisplayProprety();
        DisplayPlayersPlaying();
        if (theDiceRoller.Double) { textFieldDouble.GetComponent<Text>().text = "! DOUBLE !"; }
        else { textFieldDouble.GetComponent<Text>().text = " "; }
    }
    private void DisplayProprety()
    {
        string text = "Your propreties:\n\n";

        for (int i = 0; i < plateau.TheBoard.Length; i++)
        {
            if (plateau.TheBoard[i].StateCase == "P")
            {
                Property P = (Property)plateau.TheBoard[i];
                if (P.IDOwner == idPlayerPlaying)
                {
                    text += Convert.ToString(P.Id) + " - " + P.NameCase + " - " + P.EtatCase + " house(s)\n";
                }
            }
            else if (plateau.TheBoard[i].StateCase == "G")
            {
                Train T = (Train)plateau.TheBoard[i];
                if (T.IDOwner == idPlayerPlaying)
                {
                    text += Convert.ToString(T.Id) + " - " + T.NameCase + "\n";
                }
            }
            else if (plateau.TheBoard[i].StateCase == "C")
            {
                Companies C = (Companies)plateau.TheBoard[i];
                if (C.IDOwner == idPlayerPlaying)
                {
                    text += Convert.ToString(C.Id) + " - " + C.NameCase + "\n";
                }
            }
        }

        textFieldPlayerProperty.GetComponent<Text>().text = text;
    }
    private void DisplayPlayersPlaying()
    {
        string text = "Players in game:\n\n";
        for (int i = 0; i < listPlayer.Length; i++)
        {
            if(listPlayer[i].Statut != -1)
            {
                text += listPlayer[i].Username + "\n";
            }
        }

        textFieldPlayersPlaying.GetComponent<Text>().text = text;
    }


    public void CheckStateAfterRoll()
    {
        textFieldInfoCase.SetActive(true);

        int playerState = listPlayer[idPlayerPlaying].Statut;
        int pos = listPlayer[idPlayerPlaying].Position;

        if (initialPosition > listPlayer[idPlayerPlaying].Position)
        {
            if (idPlayerPlaying == 0) { tour++; }
        }

        for (int i = 0; i < plateau.TheBoard.Length; i++)
        {
            string state = plateau.TheBoard[i].StateCase;
            if (state == "P")
            {
                Property P = (Property)plateau.TheBoard[i];
                if (P.IDOwner == idPlayerPlaying) { playerHasProperties = true; break; }
            }
            else if (state == "G")
            {
                Train T = (Train)plateau.TheBoard[i];
                if (T.IDOwner == idPlayerPlaying) { playerHasProperties = true; break; }
            }
            else if (state == "C")
            {
                Companies C = (Companies)plateau.TheBoard[i];
                if (C.IDOwner == idPlayerPlaying) { playerHasProperties = true; break; }
            }
        }

        //If the player is in prison he can't do anything so we deactivate all buttons
        if (playerState == 0)
        {
            button_ok_buy_house.SetActive(true);
            button_ok_sell_property.SetActive(true);
            inputField_buy_house.SetActive(true);
            inputField_sell_house.SetActive(true);
        }
        else { DeactivateButtons(); }


        tileInfoText = "Welcome on " + plateau.TheBoard[pos].NameCase + "!  ";
        string tileState = plateau.TheBoard[pos].StateCase;
        if (tileState == "P" || tileState == "G" || tileState == "C") { CheckIfBuyOrPay(tileState); }
        else if (tileState == "L") { ChanceTile(); }
        else if (tileState == "E") { CommunityTile(); }
        else if (tileState == "I") { TaxesTile(); }
        else if (tileState == "T") { LuxuryTaxTile(); }
        else if (tileState == "PG") { FreeParkingTile(); }
        else if (tileState == "J")
        {
            if (playerState == 0)
            {
                tileInfoText += "This is just a simple visit!";
            }
            else
            {
                if (playerState == 1) { tileInfoText = "Your are in prison! Make a double to get out or 2 turns left!"; }
                else if (playerState == 2) { tileInfoText = "Your are in prison! 2 turns left!"; }
                else if (playerState == 3) { tileInfoText = "Your are in prison! 1 turns left!"; }
                else if (playerState == 4) { tileInfoText = "You are know in simple visit !"; }
            }
        }
        else { }
        textFieldInfoCase.GetComponent<Text>().text = tileInfoText;
    }

    /// <summary>
    /// checking if the player can buy where he has landed (properties / train stations / companies)
    /// </summary>
    private void CheckIfBuyOrPay(string tileState)
    {
        int pos = listPlayer[idPlayerPlaying].Position;

        if (tileState == "P")
        {
            Property P = (Property)plateau.TheBoard[pos];

            if (P.Owned)
            {
                if (P.IDOwner == idPlayerPlaying)
                {
                    tileInfoText += "This is your property!";
                }
                else
                {
                    tileInfoText += "This is the property of " + listPlayer[P.IDOwner].Username + " with " + P.EtatCase + " house(s). ";
                    tileInfoText += "You have to pay " + P.Prix[P.EtatCase] + " €";
                    haveToPay = true;
                    button_pay.SetActive(true);
                }
            }
            else
            {
                tileInfoText += "This property is for sale! ";
                tileInfoText += "It cost " + P.Prix[6] + " €";
                button_buy.SetActive(true);
            }

        }
        else if (tileState == "G")
        {
            Train T = (Train)plateau.TheBoard[pos];
            if (T.Owned)
            {
                if (T.IDOwner == idPlayerPlaying)
                {
                    tileInfoText += "This is your train station!";
                }
                else
                {
                    tileInfoText += "This is the train station of " + listPlayer[T.IDOwner].Username + " ";
                    tileInfoText += "You have to pay " + T.PayTrain(plateau) + " €";
                    haveToPay = true;
                    button_pay.SetActive(true);
                }
            }
            else
            {
                tileInfoText += "This train station is for sale! ";
                tileInfoText += "It cost " + T.Prix[0] + " €";
                button_buy.SetActive(true);
            }
        }
        else if (tileState == "C")
        {
            Companies C = (Companies)plateau.TheBoard[pos];
            if (C.Owned)
            {
                if (C.IDOwner == idPlayerPlaying)
                {
                    tileInfoText += "This is your company!";
                }
                else
                {
                    tileInfoText += "This is the company of " + listPlayer[C.IDOwner].Username + " ";
                    tileInfoText += "You have to pay " + C.PayCompanies(plateau) + " €";
                    haveToPay = true;
                    button_pay.SetActive(true);
                }
            }
            else
            {
                tileInfoText += "This company is for sale! ";
                tileInfoText += "It cost " + C.Prix[0] + " €";
                button_buy.SetActive(true);
            }
        }
    }

    public void Buy()
    {
        int pos = listPlayer[idPlayerPlaying].Position;
        string tileState = plateau.TheBoard[pos].StateCase;

        if (tileState == "P")
        {
            Property P = (Property)plateau.TheBoard[pos];
            if (listPlayer[idPlayerPlaying].Cash >= P.Prix[6]) //Checking if the player has enough money
            {
                P.Owned = true;
                P.IDOwner = idPlayerPlaying;
                listPlayer[idPlayerPlaying].Cash -= P.Prix[6];
                plateau.TheBoard[pos] = P;
                button_buy.SetActive(false);
                infosToPlayer += "Property bought!\n";
            }
            else
            {
                infosToPlayer += "You don't have enough cash!\n";
                return;
            }
        }
        else if (tileState == "G")
        {
            Train T = (Train)plateau.TheBoard[pos];
            if (listPlayer[idPlayerPlaying].Cash >= T.Prix[0]) //Checking if the player has enough money
            {
                T.Owned = true;
                T.IDOwner = idPlayerPlaying;
                listPlayer[idPlayerPlaying].Cash -= T.Prix[0];
                plateau.TheBoard[pos] = T;
                button_buy.SetActive(false);
                infosToPlayer += "Train station bought!\n";
            }
            else
            {
                infosToPlayer += "You don't have enough cash!\n";
                return;
            }
        }
        else if (tileState == "C")
        {
            Companies C = (Companies)plateau.TheBoard[pos];
            if (listPlayer[idPlayerPlaying].Cash >= C.Prix[0]) //Checking if the player has enough money
            {
                C.Owned = true;
                C.IDOwner = idPlayerPlaying;
                listPlayer[idPlayerPlaying].Cash -= C.Prix[0];
                plateau.TheBoard[pos] = C;
                button_buy.SetActive(false);
                infosToPlayer += "Company bought!\n";
            }
            else
            {
                infosToPlayer += "You don't have enough cash!\n";
                return;
            }
        }

    }

    public void Pay()
    {
        int pos = listPlayer[idPlayerPlaying].Position;
        string tileState = plateau.TheBoard[pos].StateCase;

        
        if (tileState == "P")
        {
            Property P = (Property)plateau.TheBoard[pos];

            if (listPlayer[idPlayerPlaying].Cash >= P.Prix[P.EtatCase])
            {
                listPlayer[P.IDOwner].Cash += P.Prix[P.EtatCase];
                listPlayer[idPlayerPlaying].Cash -= P.Prix[P.EtatCase];
                button_pay.SetActive(false);
                haveToPay = false;
                infosToPlayer += "Bill paid!\n";
            }
            else
            {
                if (playerHasProperties)
                {
                    infosToPlayer += "Not enough cash! Sell properties!\n";
                }
                else
                {
                    infosToPlayer += "Not enough cash and no properties! You can't pay! You are eliminated!\n";
                    listPlayer[idPlayerPlaying].Cash -= P.Prix[P.EtatCase];
                    listPlayer[idPlayerPlaying].Playing = false;
                }
            }
        }
        else if (tileState == "G")
        {
            Train T = (Train)plateau.TheBoard[pos];
            int toPay = T.PayTrain(plateau);

            if (listPlayer[idPlayerPlaying].Cash >= toPay)
            {
                listPlayer[T.IDOwner].Cash += toPay;
                listPlayer[idPlayerPlaying].Cash -= toPay;
                button_pay.SetActive(false);
                haveToPay = false;
                infosToPlayer += "Bill paid!\n";
            }
            else
            {
                if (playerHasProperties)
                {
                    infosToPlayer += "Not enough cash! Sell properties!\n";
                }
                else
                {
                    infosToPlayer += "Not enough cash and no properties! You can't pay! You are eliminated!\n";
                    listPlayer[idPlayerPlaying].Cash -= toPay;
                    listPlayer[idPlayerPlaying].Playing = false;
                }
            }
        }
        else if (tileState == "C")
        {
            Companies C = (Companies)plateau.TheBoard[pos];
            int toPay = C.PayCompanies(plateau);

            if (listPlayer[idPlayerPlaying].Cash >= toPay)
            {
                listPlayer[C.IDOwner].Cash += toPay;
                listPlayer[idPlayerPlaying].Cash -= toPay;
                button_pay.SetActive(false);
                haveToPay = false;
                infosToPlayer += "Bill paid!\n";
            }
            else
            {
                if (playerHasProperties)
                {
                    infosToPlayer += "Not enough cash! Sell properties!\n";
                }
                else
                {
                    infosToPlayer += "Not enough cash and no properties! You can't pay! You are eliminated!\n";
                    listPlayer[idPlayerPlaying].Cash -= toPay;
                    listPlayer[idPlayerPlaying].Playing = false;
                }
            }
        }

    }

    private void ChanceTile()
    {
        int pos = listPlayer[idPlayerPlaying].Position;
        Chance C = (Chance)plateau.TheBoard[pos];

        int chanceMoney = C.WinOrLoose();
        listPlayer[idPlayerPlaying].Cash += chanceMoney;
        if(chanceMoney < 0) //If the player looses money it goes to the middle and can be won by any player landing on the free parking tile
        {
            FreeParking FP = (FreeParking)plateau.TheBoard[20];
            FP.CashAvailable += -chanceMoney;
            plateau.TheBoard[20] = FP;
            infosToPlayer += "Chance: you loose " + (-chanceMoney) + "€\n";
        }
        else { infosToPlayer += "Chance: you win " + chanceMoney + "€\n"; }
    }

    private void CommunityTile()
    {
        int pos = listPlayer[idPlayerPlaying].Position;
        Community C = (Community)plateau.TheBoard[pos];

        int communityMoney = C.WinOrLoose();
        listPlayer[idPlayerPlaying].Cash += communityMoney;
        if (communityMoney < 0) //If the player looses money it goes to the middle and can be won by any player landing on the free parking tile
        {
            FreeParking FP = (FreeParking)plateau.TheBoard[20];
            FP.CashAvailable += -communityMoney;
            plateau.TheBoard[20] = FP;
            infosToPlayer += "Community: you loose " + (-communityMoney) + "€\n";
        }
        else { infosToPlayer += "Community: you loose " + communityMoney + "€\n"; }
    }

    private void FreeParkingTile()
    {
        int pos = listPlayer[idPlayerPlaying].Position;

        FreeParking FP = (FreeParking)plateau.TheBoard[pos];

        if (FP.CashAvailable != 0)
        {
            infosToPlayer += "Free Parking: cash available! You win " + FP.CashAvailable + "€\n";
            listPlayer[idPlayerPlaying].Cash += FP.CashAvailable;
            FP.CashAvailable = 0;
            plateau.TheBoard[pos] = FP;
        }
        else
        {
            infosToPlayer += "Free parking: no cash available\n";
        }
    }

    private void TaxesTile()
    {
        listPlayer[idPlayerPlaying].Cash -= 200;
        infosToPlayer += "You paid 200€ of taxes!\n";
    }

    private void LuxuryTaxTile()
    {
        listPlayer[idPlayerPlaying].Cash -= 100;
        infosToPlayer += "You paid 100€ of luxury taxes!\n";
    }

    public void BuyHouse()
    {
        int id = Convert.ToInt32(inputField1.GetComponent<Text>().text);
        if (id > 0 && id < 40)
        {
            string tileState = plateau.TheBoard[id].StateCase;
            if (tileState == "P")
            {
                Property P = (Property)plateau.TheBoard[id];

                if (P.Owned && P.IDOwner == idPlayerPlaying)
                {
                    if (P.EtatCase < 5 && listPlayer[idPlayerPlaying].Cash >= P.Prix[7])
                    {
                        P.EtatCase++;
                        listPlayer[idPlayerPlaying].Cash -= P.Prix[7];
                        plateau.TheBoard[id] = P;
                        check1 = false;
                        infosToPlayer += "House bought for property " + id + "\n";
                    }
                    else if (P.EtatCase > 5 && listPlayer[idPlayerPlaying].Cash >= P.Prix[7])
                    {
                        infosToPlayer += "5 houses max per property!\n";
                    }
                    else
                    {
                        infosToPlayer += "Not enough cash!\n";
                    }
                }
                else
                {
                    infosToPlayer += "This is not your property";
                }
            }
            else
            {
                infosToPlayer += "Cannot put houses here!\n";
            }
        }
        else
        {
            infosToPlayer += "This tile does not exist!\n";
        }
    }

    public void SellProperty()
    {
        int id = Convert.ToInt32(inputField2.GetComponent<Text>().text);
        if (id > 0 && id < 40)
        {
            string tileState = plateau.TheBoard[id].StateCase;
            if (tileState == "P")
            {
                Property P = (Property)plateau.TheBoard[id];

                if (P.Owned && P.IDOwner == idPlayerPlaying)
                {
                    int money = P.Prix[6];
                    if (P.EtatCase > 0) { money += P.EtatCase * P.Prix[7]; }

                    listPlayer[idPlayerPlaying].Cash += money;

                    P.IDOwner = -1;
                    P.Owned = false;
                    P.EtatCase = 0;

                    plateau.TheBoard[id] = P;

                    infosToPlayer += "Property sold! +" + money + "€\n";
                }
                else
                {
                    infosToPlayer += "It's not your property!\n";
                }
            }
            else if (tileState == "G")
            {
                Train T = (Train)plateau.TheBoard[id];

                if (T.Owned && T.IDOwner == idPlayerPlaying)
                {
                    listPlayer[idPlayerPlaying].Cash += T.Prix[0];

                    T.IDOwner = -1;
                    T.Owned = false;

                    plateau.TheBoard[id] = T;

                    infosToPlayer += "Train station sold! +" + T.Prix[0] + "€\n";
                }
                else
                {
                    infosToPlayer += "It's not your train station!\n";
                }
            }
            else if (tileState == "C")
            {
                Companies C = (Companies)plateau.TheBoard[id];

                if (C.Owned && C.IDOwner == idPlayerPlaying)
                {
                    listPlayer[idPlayerPlaying].Cash += C.Prix[0];

                    C.IDOwner = -1;
                    C.Owned = false;

                    plateau.TheBoard[id] = C;

                    infosToPlayer += "Company sold! +" + C.Prix[0] + "€\n";
                }
                else
                {
                    infosToPlayer += "This is not your company!\n";
                }
            }
            else
            {
                infosToPlayer += "Can't be sold!\n";
            }
        }
        else
        {
            infosToPlayer += "This tile does not exist!\n";
        }
    }


    public void EndTurn()
    {
        int playing = nbPlayer;
        for (int i = 0; i < nbPlayer; i++) //Checking how many player still "alive" we need at least two
        {
            if (listPlayer[i].Cash < 1)
            {
                playing--;
            }
        }


        if (diceRolled && !haveToPay && playing > 1 && !theDiceRoller.Double && tour <= tourMax)
        {
            
            if (idPlayerPlaying + 1 == nbPlayer)
            {
                idPlayerPlaying = 0;
            }
            else
            {
                idPlayerPlaying++;
            }
            initialPosition = listPlayer[idPlayerPlaying].Position;
            idPlayerToMove = idPlayerPlaying + 1;

            diceRolled = false;
            doubleCounter = 0;
            haveToPay = false;
            check1 = false;
            infosToPlayer = " ";
            playerHasProperties = false;

            DeactivateButtons();
            rollDice.SetActive(true);

            if (tour == tourMax) { infosToPlayer = "DERNIER TOUR !\n"; }

            UpdateDisplay();
        }
        else if (DiceRolled && !haveToPay && tour > tourMax)
        {
            EndGame();
        }
        else if (DiceRolled && !haveToPay && playing < 2)
        {
            EndGame();
        }
        else if(!DiceRolled || theDiceRoller.Double)
        {
            infosToPlayer = "You have to roll the dice!!\n";
        }
        else if (haveToPay)
        {
            infosToPlayer = "You to pay the bill!!\n";
        }
    }

    public void EndGame()
    {
        DeactivateButtons();
        rollDice.SetActive(false);
        SceneManager.LoadScene(2, LoadSceneMode.Additive);
    }
}