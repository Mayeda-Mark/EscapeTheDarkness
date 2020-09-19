using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SignTrigger : MonoBehaviour
{
    Collider2D myCollider;
    //[SerializeField]  TMParent;
    [SerializeField] GameObject tooltip;
    [SerializeField] TextMeshProUGUI tutorialText;
    [SerializeField] string tutorialString;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }
    void Update () {
        ToggleSign();
    }
    private void ToggleSign() {
        if(myCollider.IsTouchingLayers(LayerMask.GetMask("Player"))) {
            if (Input.GetKeyDown(KeyCode.W)) {
                tooltip.SetActive(true);
                //tooltip.GetComponentInChildren<TextMesh>().text = tutorialString;
                //TextMeshPro tmp = GetComponentInChildren<TextMeshPro>();
                tutorialText.text = tutorialString;
            } else if (Input.GetKeyDown(KeyCode.S)) {
                tooltip.SetActive(false);
            }
        }
    }
    // private void OnTriggerStay2D(Collider2D collision){
    //     if (collision.gameObject.CompareTag("Player")) {
    //         if (Input.GetKeyDown(KeyCode.S)) {
    //             tooltip.SetActive(true);
    //             tooltip.GetComponentInChildren<TextMesh>().text = tutorialString;
    //         } else if (Input.GetKeyDown(KeyCode.W)) {
    //             tooltip.SetActive(false);
    //         }
    //     }
    // }
    // private void ChangeText() {
    //     if()
    // }
}
