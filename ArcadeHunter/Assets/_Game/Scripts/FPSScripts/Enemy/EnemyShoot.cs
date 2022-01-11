using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    //[SerializeField] Transform bulletSpawnPos;
    [SerializeField] GameObject bulletPrefab;

    private WeaponManager weaponManager;
    public bool shooting;    

    // Start is called before the first frame update
    void Start()
    {
        weaponManager = GetComponent<WeaponManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Shoot(Vector3 pos)
    {
        WeaponGraphics weaponGraphics = weaponManager.GetCurrentGraphics();
        //Debug.Log("Shoot Player");
        //GameObject bullet = Instantiate(bulletPrefab, weaponGraphics.bulletSpawnPos.position, weaponGraphics.bulletSpawnPos.rotation);
        //Debug.Log(bullet.transform.forward);

        //weaponGraphics.muzzleFlash.Play();
        //weaponGraphics.fireSound.Play();
        pos = new Vector3(pos.x, transform.position.y-2, pos.z);
        Vector3 dir = pos - transform.position;
        dir.y = 0;
        Quaternion rot = Quaternion.LookRotation(dir);
        Instantiate(weaponGraphics.bulletPrefab, weaponGraphics.bulletSpawnPos.position, rot);
    }
}
