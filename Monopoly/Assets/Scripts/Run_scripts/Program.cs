using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Program : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    static void Main(string[] args)
    {


        Controller jeu = new Controller();

        /*while (!jeu.VictoryConditions)
        {
            for (int id = 0; id < jeu.NbPlayer; id++)
            {
                jeu.VerifStatut(id);

            }
            jeu.Tour++;
        }*/
    }

    static void Restart()//Redemarrer une partie
    {

    }


}
