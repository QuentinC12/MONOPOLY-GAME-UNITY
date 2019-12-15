using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Board
{
    // UTILISER LE DESIGN PATTERN SINGLETON POUR S'ASSURER QU'ON A QU'UNE INSTANCE DE CETTE CLASSE.
    private Case[] theBoard; //Plateau de jeu avec 39 cases.

    private TextAsset monopFile;

    public Board(TextAsset file)
    {
        this.monopFile = file;
        InitiateBoard();
    }
    public Case[] TheBoard
    {
        get { return this.theBoard; }
        set { theBoard = value; }
    }


    /// <summary>
    /// Fromat the data list we've created using the CSV file to the Case format
    /// Each of the Tiles are inserted in the board
    /// </summary>
    public void InitiateBoard()
    {
        List<string[]> data = ReadFile();
        theBoard = new Case[40];
        for (int i = 0; i < 40; i++)
        {
            string[] row = data[i];
            if (row[1] == "J") //1 corresponding to column in csv where we can find the type of case
            {
                theBoard[i] = new Jail(Convert.ToInt32(row[0]), row[1], row[2]);
            }
            else if (row[1] == "P")
            {
                int[] prices = { Convert.ToInt32(row[3]), Convert.ToInt32(row[4]), Convert.ToInt32(row[5]), Convert.ToInt32(row[6]), Convert.ToInt32(row[7]), Convert.ToInt32(row[8]), Convert.ToInt32(row[9]), Convert.ToInt32(row[10]) };
                theBoard[i] = new Property(Convert.ToInt32(row[0]), row[1], row[2], prices);
            }
            else if (row[1] == "L")
            {
                theBoard[i] = new Chance(Convert.ToInt32(row[0]), row[1], row[2]);
            }
            else if (row[1] == "E")
            {
                theBoard[i] = new Community(Convert.ToInt32(row[0]), row[1], row[2]);
            }
            else if (row[1] == "G")
            {
                theBoard[i] = new Train(Convert.ToInt32(row[0]), row[1], row[2]);
            }
            else if (row[1] == "C")
            {
                theBoard[i] = new Companies(Convert.ToInt32(row[0]), row[1], row[2]);
            }
            else if (row[1] == "PG")
            {
                theBoard[i] = new FreeParking(Convert.ToInt32(row[0]), row[1], row[2]);
            }
            else
            {
                theBoard[i] = new OtherTiles(Convert.ToInt32(row[0]), row[1], row[2]);
            }
        }
    }
    /// <summary>
    /// The csv file has been attached to the controller in the unity app
    /// By doing so we can easily read it as a TextAsset (a string basically)
    /// </summary>
    /// <returns>It return a list of tiles data: property, chance, community, train station, companies, prison and departure</returns>
    public List<string[]> ReadFile()
    {
        string data = monopFile.text;

        string[] dataLines = System.Text.RegularExpressions.Regex.Split(data, "\n");

        List<string[]> monopData = new List<string[]>();
        for (int i = 0; i < dataLines.Length; i++)
        {
            string[] row = System.Text.RegularExpressions.Regex.Split(dataLines[i], ",");

            monopData.Add(row);
        }
        return monopData;
    }


    public string nameCase(int numCase)
    {
        return theBoard[numCase].NameCase;
    }

    public string typeCase(int numCase)
    {
        return theBoard[numCase].StateCase;
    }
}
