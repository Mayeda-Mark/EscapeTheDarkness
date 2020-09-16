using System.Collections;
using System.Collections.Generic;
//using System.Management.Instrumentation;
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

            enemy.MoveTowardsTarget();

            //enemy.transform.position = Vector2.MoveTowards(enemy.transform.position, player.transform.position, enemy.movementSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("Reset");
            //enemy.isFound = false;
            enemy.movementSpeed = 0;
            enemy.transform.position = enemy.startPosition;
            enemy.rb.velocity = new Vector2(0, 0);
            enemy.patrolBar.SetMaxHealth(0);
            enemy.idleBar.SetMaxHealth(0);
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
