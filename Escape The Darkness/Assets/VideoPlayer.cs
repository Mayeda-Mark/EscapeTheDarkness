using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VideoPlayer : MonoBehaviour
{
    //[SerializeField] private string sceneName = default;

    private void Update()
    {
        StartCoroutine(StartNextScene());      
    }

    public IEnumerator StartNextScene()
    {
        yield return new WaitForSeconds(11);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        //SceneManager.LoadScene(sceneName);
    }
}
