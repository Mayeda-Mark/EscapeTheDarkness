using UnityEngine;

public class Star : MonoBehaviour
{
    #region Serilaized Fields
    [SerializeField]private GameObject explosion = default;
    [SerializeField]private Transform spawnPoint = default;
    #endregion

    private void OnTriggerEnter2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("Player"))
        {
            Instantiate(explosion, spawnPoint.position, spawnPoint.rotation);            
            Destroy(gameObject);
            PermanentUI.perm.startCount ++;
        }
    }
}