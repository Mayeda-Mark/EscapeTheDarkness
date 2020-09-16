using TMPro;
using UnityEngine;

public enum SCT_TYPE { DAMAGE, HEAL }

public class CombtTextManager : MonoBehaviour
{
    private static CombtTextManager instance;

    public static CombtTextManager MyInstance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<CombtTextManager>();
            }
            return instance;
        }
    }

    [SerializeField]private GameObject combatTextPrefab = default;

    public void CreateText(Vector2 _position, string _text, SCT_TYPE _type, bool _crit)
    {
        TextMeshProUGUI sct = Instantiate(combatTextPrefab, transform).GetComponent<TextMeshProUGUI>();
        sct.transform.position = _position;

        string operation = string.Empty;

        switch (_type)
        {
            case SCT_TYPE.DAMAGE:
                operation += "-";
                sct.color = Color.red;
                break;

            case SCT_TYPE.HEAL:
                operation += "+";
                sct.color = Color.green;
                break;
        }

        sct.text = operation + _text;

        if (_crit)
        {
            sct.GetComponent<Animator>().SetBool("crit", true);
        }
    }
}
