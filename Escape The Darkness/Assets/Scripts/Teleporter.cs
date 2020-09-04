using UnityEngine;

public class Teleporter : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField] private Transform teleportTarget = default;
    [SerializeField] private GameObject player = default;
    #endregion

    private bool playerInZone;

    private void Start()
    {
        playerInZone = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W) && playerInZone || Input.GetButtonDown("Use") && playerInZone)
        {
            SoundManager.PlaySound("Teleport");
            player.transform.position = teleportTarget.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.tag == "Player")
        {
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.tag == "Player")
        {
            playerInZone = false;
        }
    }
}
