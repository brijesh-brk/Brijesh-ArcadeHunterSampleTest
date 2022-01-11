using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu(fileName ="weaponDatabase",menuName ="Database/Weapons")]
public class WeaponDatabase : ScriptableObject
{
    public PlayerWeapon[] allWeapons;

    //public List<weaponDictionary> weaponList;

    //[System.Serializable] public Dictionary<string, PlayerWeapon> dictionary;    
}
