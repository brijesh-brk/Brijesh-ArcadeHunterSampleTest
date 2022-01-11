using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface IDamageable
{
    public void Damage(float power);

    public float GetPower();

    public Vector3 GetPos();
}
