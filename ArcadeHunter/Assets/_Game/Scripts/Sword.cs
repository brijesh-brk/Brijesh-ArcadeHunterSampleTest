using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    [SerializeField] private float damagePower;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(Constants.Strings.Player))
        {
            if (other.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.Damage(damagePower);
            }
            Destroy(this.gameObject);
        }
    }
}
