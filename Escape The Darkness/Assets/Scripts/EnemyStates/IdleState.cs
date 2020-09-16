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
        //enemy.transform.position = enemy.startPosition;
        //enemy.idleBar.SetMaxHealth(0f);
        //enemy.patrolBar.SetHealth(0);
        idleDuration = Random.Range(1, 10);
        this.enemy = _enemy;

        enemy.FieldOfView.SetFoV(60f);
        enemy.FieldOfView.SetViewDistance(8f);

        enemy.movementSpeed = 4;

        //enemy.idleBar.enabled = true;
        enemy.idleBar.SetMaxHealth(idleDuration);
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
        //idleTimer -= Time.deltaTime;
        //CombtTextManager.MyInstance.CreateText(enemy.combatTxtPosition.position, idleTimer.ToString(), SCT_TYPE.HEAL, true);
        enemy.idleBar.SetHealth(idleTimer);

        if (idleTimer >= idleDuration)
        {
            //enemy.idleBar.enabled = false;
            enemy.idleBar.SetMaxHealth(0f);
            enemy.ChangeState(new PatrolState());            
        }
    }
}
