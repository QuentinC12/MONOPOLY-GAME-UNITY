using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherTiles : Case
{
    public OtherTiles(int id, string stateCase, string name) : base(id, stateCase, name)
    {
    }
    public override int Id { get { return this.id; } }
    public override string StateCase { get { return this.stateCase; } }
    public override string NameCase { get { return this.nameCase; } }


}
