using UnityEngine;

[System.Serializable]
public class PlayerWeapon 
{
    public int index = 0;
    public string name = "Pistol";
    public float range = 100f;
    public int damage = 10;
    public float fireRate = 0f;
    public GameObject weaponGFX;
}
