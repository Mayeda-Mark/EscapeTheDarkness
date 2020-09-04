using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public enum AnimationState { IDLE, RUN, JUMP, FALL, HURT, CROUCH }

    //[SerializeField] private Transform wayPointLeft = default;
    //[SerializeField] private Transform wayPointRight = default;

    public AnimationState animationState;

    private IEnemyState currentState;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    private bool facingRight;
    public int movementSpeed;

    public bool isFound;
    public bool isInRange;

    public Vector2 startPosition;

    public Collider2D coll;

    [SerializeField] private PlayerController player;
    //[SerializeField] private Transform prefabFieldOFView;
    //[SerializeField] private float fov = 40f;
    //[SerializeField] private float viewDistance = 15f;

    //[SerilaizedField] private Vector3 aimDirection;
    [SerializeField]private FieldOfView fieldOfView;

    [SerializeField] Transform rayCastPoistion;

    [SerializeField] private LayerMask target;

    public GameObject Target { get; set; }

    public Animator Anim { get; private set; }

    public FieldOfView FieldOfView { get => fieldOfView; set => fieldOfView = value; }
    //public Transform PrefabFieldOFView { get => prefabFieldOFView; set => prefabFieldOFView = value; }

    private void Start()
    {
        ChangeState(new IdleState());
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        facingRight = true;
        movementSpeed = 3;
        Anim = GetComponent<Animator>();

        //fieldOfView = Instantiate(PrefabFieldOFView, null).GetComponent<FieldOfView>();
        //fieldOfView.SetFoV(fov);
        //fieldOfView.SetViewDistance(viewDistance);

        isFound = false;
        coll.enabled = false;

        startPosition = transform.position;

        Physics2D.queriesStartInColliders = false;
    }

    private void Update()
    {
        currentState.Execute();
        LookAtTarget();
        SetAnimation();

        if (fieldOfView != null)
        {
            //fieldOfView.SetOrigin(transform.position);
            //fieldOfView.SetAimDirection(GetDirection());

            FieldOfView.SetOrigin(transform.position);
            FieldOfView.SetAimDirection(GetDirection());
        }

        Debug.DrawLine(transform.position, transform.position + GetDirection() * 10f);

        FindTargetPlayer();

        if (isFound)
        {
            coll.enabled = true;
            ChangeState(new RangedState());
        }
        if (!isFound)
        {
            coll.enabled = false;
            //ChangeState(new IdleState());
        }
    }

    private void FindTargetPlayer()
    {
        if (Vector3.Distance(GetPosition(), player.GetPosition()) < FieldOfView.viewDistance)
        {
            // Player inside ViewDistance
            Debug.Log("Attack1");

            Vector3 dirToPlayer = (player.GetPosition() - GetPosition()).normalized;

            if (Vector3.Angle(GetDirection(), dirToPlayer) < FieldOfView.fov / 2f)// 
            {
                //Player inside FieldOfView
                Debug.Log("Attack2");

                RaycastHit2D raycastHit2D = Physics2D.Raycast(rayCastPoistion.position, dirToPlayer, FieldOfView.viewDistance);

                if (raycastHit2D.collider != null)
                {
                    Debug.DrawLine(rayCastPoistion.position, raycastHit2D.point, Color.red);
                    if (raycastHit2D.collider.CompareTag("Player"))
                    {
                        Debug.Log("RayHitPlayer");
                        //Destroy(raycastHit2D.collider.gameObject);
                        //movementSpeed = movementSpeed * 2;
                        isFound = true;
                    }
                }
                else
                {
                    Debug.DrawLine(transform.position, transform.position + GetDirection() * FieldOfView.viewDistance, Color.green);
                    Debug.Log("NoAttack");
                }
            }                     
        }
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    private void LookAtTarget()
    {
        if (Target != null)
        {
            float xDir = Target.transform.position.x - transform.position.x;

            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                ChangeDirection();
            }
        }
    }

    public void SetAnimation()
    {
        Anim.SetInteger("state", (int)animationState);
    }

    public void ChangeState(IEnemyState _newState)
    {
        if (currentState != null)
        {
            currentState.Exit();
        }

        currentState = _newState;
        currentState.Enter(this);

        //if (animationState == 0)
        //{
            
        //}
        //if (animationState == 1)
        //{

        //}
    }

    public void Move()
    {
        transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
    }

    public Vector3 GetDirection()
    {
        return facingRight ? Vector3.right : Vector3.left;
    }

    public void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, 1, 1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        currentState.OntriggerEnter(collision);

        //if (collision.CompareTag("Player"))
        //{
        //    isInRange = true;
        //}
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player")) 
        //{
        //    isInRange = false;
        //}
    }
}