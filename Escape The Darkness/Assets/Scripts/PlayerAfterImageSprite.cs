using UnityEngine;

public class PlayerAfterImageSprite : MonoBehaviour
{
    [SerializeField] private float activeTime = 0.1f;
    [SerializeField] private float alphaSet = 0.8f;
    [SerializeField] private float alphaDecay = default;
  
    private Transform player;
    private SpriteRenderer sr;
    private SpriteRenderer playerSr;
    private Color theColor;
    private float timeActivated;
    private float alpha;

    private void OnEnable()//works like a start function, but gets called when the object is enabled
    {
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerSr = player.GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        sr.sprite = playerSr.sprite;
        transform.position = player.position;
        transform.rotation = player.rotation;
        timeActivated = Time.time;
    }

    private void Update()
    {
        alpha -= alphaDecay * Time.deltaTime;
        theColor = new Color(1f, 1f, 1f, alpha);
        sr.color = theColor;

        if (Time.time >= (timeActivated + activeTime))
        {
            //Add back to the pool
            PlayerAfterImagePool.Instance.AddToPool(gameObject);
        }
    }
}
