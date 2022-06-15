using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageTextSpawner : MonoBehaviour
{
    [SerializeField] DamageText damageTextPrefab;

    DamageText damageText;

    public void SpawnDamageText(float damage)
    {
        damageText = Instantiate<DamageText>(damageTextPrefab, transform);
        damageText.SetDamageText(damage);
    }
}
