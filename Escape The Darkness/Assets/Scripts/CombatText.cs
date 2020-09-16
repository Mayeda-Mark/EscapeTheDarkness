using System.Collections;
using TMPro;
using UnityEngine;

public class CombatText : MonoBehaviour
{
    #region Serialized Fields
    [SerializeField]private float speed = default;
    [SerializeField]private TextMeshProUGUI text = default;
    [SerializeField]private float lifeTime = default;
    #endregion

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    public IEnumerator FadeOut()
    {
        float startAlpha = text.color.a;
        float rate = 1.0f / lifeTime;
        float progress = 0.0f;

        while(progress < 1.0)
        {
            Color tmp = text.color;
            tmp.a = Mathf.Lerp(startAlpha, 0, progress);
            text.color = tmp;
            progress += rate * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
