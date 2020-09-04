using UnityEngine;

public interface IEnemyState
{
    void Execute();

    void Enter(Enemy _enemy);

    void Exit();

    void OntriggerEnter(Collider2D _collision);
}
