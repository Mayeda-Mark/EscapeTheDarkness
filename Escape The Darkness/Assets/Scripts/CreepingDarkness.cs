using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreepingDarkness : MonoBehaviour
{
    [SerializeField] float scrollSpeed = 0.05f;
    [SerializeField] bool isBorder = false;
    void Update()
    {
        Move();
    }
    private void Move() {
        transform.Translate(new Vector2(scrollSpeed * Time.deltaTime, 0f));
    }
    private void OnTriggerEnter2D(Collider2D otherCollider) {
        PlayerController player = otherCollider.GetComponent<PlayerController>(); 
        if(player && !isBorder){
            Debug.Log("You Dead"); // Insert kill method here
        }
    }
}
