using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PermanentUI : MonoBehaviour
{
    //Player stats
    public int collectable = 0;
    public int score = 0;
    public int lives = 3;
    public int startCount = 0;
    public TextMeshProUGUI collectableTxt = default;
    public TextMeshProUGUI scoreTxt = default;
    public TextMeshProUGUI livesTxt = default;
    public CanvasGroup star1;
    public CanvasGroup star2;
    public CanvasGroup star3;
    public static PermanentUI perm;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        //Singleton
        if (!perm)
        {
            perm = this;//this = current instance
        }
        else
        {
            Destroy(gameObject);
        }

        var tempColor = star1.alpha;
        star1.alpha = tempColor;
        var tempColor1 = star1.alpha;
        star2.alpha = tempColor;
        var tempColor2 = star1.alpha;
        star3.alpha = tempColor;
    }

    private void Update()
    {
        if (startCount == 1)
        {
            star1.alpha = 1f;
        }
        if (startCount == 2)
        {
            star2.alpha = 1f;
        }
        if (startCount == 3)
        {
            star3.alpha = 1f;
        }
    }

    public void Reset()
    {
        collectable = 0;
        collectableTxt.text = collectable.ToString();
    }
}
