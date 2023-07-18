using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Model;

public class Player : Character
{
    public Player(Transform root) : base(root)
    {
        ChType = CharacterType.Player;
    }

    public override void Updata()
    {
        base.Updata();
    }

    protected override void Move()
    {
        base.Move();
    }
}

public class Enemy : Character
{
    public Enemy(Transform root) : base(root)
    {
        ChType = CharacterType.Enemy;
    }

    public override void Updata()
    {
        base.Updata();
    }

    protected override void Move()
    {
        base.Move();
    }

}
