using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Community : Case
{
    private int money;
    public Community(int id, string stateCase, string name) : base(id, stateCase, name)
    {
        this.money = 0;
    }

    public override int Id { get { return this.id; } }
    public override string StateCase { get { return this.stateCase; } }
    public override string NameCase { get { return this.nameCase; } }

    /// <summary>
    /// The first random is used to define if the player is going to win or loose money
    /// The second random is used to defin the amount he is going to win or loose
    /// The player can win a 100 to 200 € and loose 50 to a 100€
    /// </summary>
    /// <returns>Return a positive or a negative value: positive if you win money / negative if you loose money</returns>
    public int WinOrLoose()
    {
        money = 0;
        int plusOrMinus = UnityEngine.Random.Range(1, 3);

        if (plusOrMinus == 1)
        {
            money = UnityEngine.Random.Range(100, 200);
        }
        else
        {
            money = UnityEngine.Random.Range(-50, -100);
        }
        return money;
    }
}
