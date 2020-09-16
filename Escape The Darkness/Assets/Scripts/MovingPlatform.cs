using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]private float speed = default;
    [SerializeField]private Transform pos1 = default;
    [SerializeField]private Transform pos2 = default;
    [SerializeField]private Transform startPos = default;

    private Vector3 nextPos;

    public Vector3 NextPos { get => nextPos; set => nextPos = value; }

    void Start()
    {
        NextPos = startPos.position;
    }

    void Update()
    {
        if (transform.position == pos1.position)
        {
            NextPos = pos2.position;
        }
        if (transform.position == pos2.position)
        {
            NextPos = pos1.position;
        }

        transform.position = Vector3.MoveTowards(transform.position, NextPos, speed * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(pos1.position, pos2.position);
    }
}
