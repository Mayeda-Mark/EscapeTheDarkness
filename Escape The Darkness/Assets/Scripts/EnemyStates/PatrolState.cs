using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;

    private float patrolTimer;
    private float patrolDuration;

    public void Enter(Enemy _enemy)
    {
        patrolDuration = Random.Range(1, 10);
        this.enemy = _enemy;

        enemy.FieldOfView.SetFoV(40f);
        enemy.FieldOfView.SetViewDistance(15f);
    }

    public void Execute()
    {
        Patrol();
        enemy. Move();

        //if (enemy.Target != null)
        //{
        //    enemy.ChangeState(new RangedState());
        //}
    }

    public void Exit()
    {
        
    }

    public void OntriggerEnter(Collider2D _collision)
    {
        if (_collision.tag == "Edge")
        {
            enemy.ChangeDirection();
        }
    }

    private void Patrol()
    {
        enemy.animationState = Enemy.AnimationState.RUN;
        patrolTimer += Time.deltaTime;

        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}
