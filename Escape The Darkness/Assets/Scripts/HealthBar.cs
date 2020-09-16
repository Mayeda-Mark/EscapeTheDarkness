using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]private Slider slider = default;

    public void SetMaxHealth(float _health)
    {
        slider.maxValue = _health;
        slider.value = _health;
    }

    public void SetHealth(float _health)
    {
        slider.value = _health;
    }
}
