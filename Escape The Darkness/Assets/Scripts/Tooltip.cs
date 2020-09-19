using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            TooltipUnpause();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            UIManager.MyInstance.ShowTooltip();
            TooltipPause();
            //StartCoroutine(TooltipPause());
        }
    }

    private void TooltipPause()
    {
        UIManager.MyInstance.ShowTooltip();
        Time.timeScale = 0;
    }

    private void TooltipUnpause()
    {
        Time.timeScale = 1;
        UIManager.MyInstance.HideTooltip();
    }

    //private IEnumerator TooltipPause()
    //{
    //    Time.timeScale = 0;
    //    float pauseEndTime = Time.realtimeSinceStartup + 2;

    //    while (Time.realtimeSinceStartup < pauseEndTime)
    //    {
    //        yield return 0;
    //    }

    //    Time.timeScale = 1;
    //    UIManager.MyInstance.HideTooltip();
    //}
}