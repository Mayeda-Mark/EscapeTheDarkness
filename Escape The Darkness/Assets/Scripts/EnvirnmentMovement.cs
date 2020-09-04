using UnityEngine;

public class EnvirnmentMovement : MonoBehaviour
{
    [SerializeField] private ParticleSystem hideEffect = default;
    [SerializeField] private PlayerController player;

    private bool isHidable = false;
    private bool isHidding = false;

    //private Animator anim;

    private void Start()
    {
        //anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && isHidable)
        {
            player.gameObject.SetActive(false);
            //anim.SetTrigger("move");
            hideEffect.Play();
            isHidding = true;
        }
        if (Input.GetKeyDown(KeyCode.S) && isHidding)
        {
            player.gameObject.SetActive(true);
            //anim.SetTrigger("move");
            hideEffect.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.CompareTag("Player"))//
        {
            isHidable = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isHidable = false;
            isHidding = false;
        }
    }
}
