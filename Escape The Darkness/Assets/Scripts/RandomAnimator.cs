using System.Collections;
using UnityEngine;

public class RandomAnimator : MonoBehaviour
{
    #region Private Fields
    private Animator anim;
    private bool playAnim;
    #endregion

    private void Start()
    {
        anim = GetComponent<Animator>();
        playAnim = true;
    }

    private void Update()
    {
        if (playAnim)
        {
            StartCoroutine(WaitForSeconds());
        }
    }

    private IEnumerator WaitForSeconds()
    {
        playAnim = false;
        float randomWait = Random.Range(1, 10);
        yield return new WaitForSeconds(randomWait);
        anim.SetTrigger("play");
        playAnim = true;
    }
}
