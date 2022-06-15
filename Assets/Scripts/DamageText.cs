using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] Text damageText;

    public void SetDamageText(float damage)
    {
        damageText.text = damage.ToString("F0");
    }

    public void DestroyText()
    {
        Destroy(gameObject);
    }
}
