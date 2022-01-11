using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class IEnemy:MonoBehaviour
{
    public abstract float GetPower();

    public abstract Transform GetTransform();
}
