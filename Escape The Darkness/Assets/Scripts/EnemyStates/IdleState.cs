using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    private float idleTimer;
    private float idleDuration;

    public void Enter(Enemy _enemy)
    {
        
        idleDuration = Random.Range(1, 10);
        this.enemy = _enemy;

        enemy.FieldOfView.SetFoV(60f);
        enemy.FieldOfView.SetViewDistance(8f);

        enemy.movementSpeed = 4;
    }

    public void Execute()
    {
        Idle();

        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {
        
    }

    public void OntriggerEnter(Collider2D _collision)
    {
        
    }

    private void Idle()
    {
        enemy.animationState = Enemy.AnimationState.IDLE;
        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}
