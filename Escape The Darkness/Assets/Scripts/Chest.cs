using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField]private ParticleSystem explosion = default;
    [SerializeField]private Transform spawnPoint = default;
    [SerializeField]private GameObject drop = default;

    private Animator anim;
    private bool hasOpened = false;
    
    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player"))
        {          
            anim.SetBool("open", true);
            anim.SetBool("close", false);
        }
        if (_collision.gameObject.CompareTag("Player") && hasOpened == false)
        {
            Instantiate(drop, spawnPoint.position, spawnPoint.rotation);
            explosion.Play();
            SoundManager.PlaySound("PowerUp");
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player"))
        {
            hasOpened = true;
            anim.SetBool("open", false);
            anim.SetBool("close", true);
        }
    }
}
