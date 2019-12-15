using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPawn : MonoBehaviour
{
    private DiceRoller theDiceRoller;
    private Controller theController;
    public Tile StartingTile;
    private Tile currentTile;

    //Vector3 allows to get and manage the posotion of an object in the 3 dimensions (x, y, z)
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;
    private float sommthTime = 0.08f;
    private float smoothDistance = 0.0001f;

    private bool pawnTeleportation = false;

    //Array of tile to have a smooth pawn movement
    private Tile[] moveQueue;
    //To save the tile we are using for the animation to get its rotation and use it to rotate the pawn
    private Tile tmp;
    private int moveQueueIndex = -1;

    // Start is called before the first frame update
    void Start()
    {
        theDiceRoller = GameObject.FindObjectOfType<DiceRoller>();
        theController = GameObject.FindObjectOfType<Controller>();
        if (theController.listPlayer.Length < 4)
        {
            if (this.tag == "4") { Destroy(this.gameObject); }
            if (theController.listPlayer.Length < 3)
            {
                if (this.tag == "3") { Destroy(this.gameObject); }
            }
        }
        currentTile = StartingTile;
        targetPosition = this.transform.position;
    }

    

    // Update is called once per frame
    /// <summary>
    /// This function is run each time the program update the screen so 60 times per second for a 60Hz screen
    /// </summary>
    void Update()
    {
        if(pawnTeleportation)
        {
            this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, currentTile.transform.eulerAngles.y - 90, this.transform.eulerAngles.z);
        }
        else
        {
            //Each frame we check the position and unpdate it if there is a new value to reach
            if (Vector3.Distance(this.transform.position, targetPosition) < smoothDistance)
            {
                //We ahve reached the last set desired position.? DOwe have another one in the queue?

                if (moveQueue != null && moveQueueIndex < moveQueue.Length)
                {
                    tmp = moveQueue[moveQueueIndex];
                    SetNewTargetPosition(moveQueue[moveQueueIndex].transform.position);
                    moveQueueIndex++;
                }
            }
            else
            {
                this.transform.position = Vector3.SmoothDamp(this.transform.position, targetPosition, ref velocity, sommthTime);
                //When we have reach the tile we chack the rotation to see of we have to rotate the pawn or not.
                this.transform.eulerAngles = new Vector3(this.transform.eulerAngles.x, tmp.transform.eulerAngles.y - 90, this.transform.eulerAngles.z);
            }
        }
        
    }
    
    /// <summary>
    /// Called when we have to move a player pawn
    /// It takes as parameter the new position and will then set the target position
    /// Then when set the pawn will be animated by yhe update function runing each frame
    /// </summary>
    /// <param name="pos"></param>
    void SetNewTargetPosition (Vector3 pos)
    {
        targetPosition = pos;
        velocity = Vector3.zero;
    }

    /// <summary>
    /// Called when the palyer has rolled the dice to make his pawn moove
    /// It searchs for the destination tile and then call the SetNewTargetPosition function
    /// </summary>
    public void Move ()
    {
        pawnTeleportation = false;
        int spacesToMove = theDiceRoller.DiceTotal;
        moveQueue = new Tile[spacesToMove];
        Tile finalTile = currentTile;


        for (int i = 0; i < spacesToMove; i++)
        {
            finalTile = finalTile.NextTile;
            moveQueue[i] = finalTile;
        }

        currentTile = finalTile;
        moveQueueIndex = 0;
    }

    public void MoveToTile()
    {
        pawnTeleportation = true;
        int spacesToMove = theDiceRoller.DiceTotal;
        Tile finalTile = currentTile;

        for (int i = 0; i < spacesToMove; i++)
        {
            finalTile = finalTile.NextTile;
        }

        currentTile = finalTile;
        //Teleporting the pawn to the target tile
        this.transform.position = currentTile.transform.position;
    }
}
