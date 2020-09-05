using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState
{
    private Enemy enemy;
    private PlayerController player;

    public void Enter(Enemy _enemy)
    {
        this.enemy = _enemy;
        enemy.movementSpeed = 8;
    }

    public void Execute()
    {
        if (enemy.Target != null)
        {
            enemy.Move();

            //enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, player.transform.position, enemy.movementSpeed * Time.deltaTime);
        }
        else
        {
            //enemy.isFound = false;
            enemy.transform.position = enemy.startPosition;
            enemy.ChangeState(new IdleState());
        }
    }

    public void Update()
    {
        //if (enemy.Target == null)
        //{
        //    enemy.isFound = false;
        //    enemy.coll.enabled = false;
        //    enemy.ChangeState(new IdleState());
        //}
    }

    public void Exit()
    {
        
    }

    public void OntriggerEnter(Collider2D _collision)
    {
        
    }
}
