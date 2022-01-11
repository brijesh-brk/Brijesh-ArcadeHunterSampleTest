using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class WeaponManager : MonoBehaviour
{
    [SerializeField]
    private Transform weaponHolder;

    [SerializeField] Camera camera;
    [SerializeField] WeaponDatabase weaponsDatabase;
    [SerializeField] private PlayerWeapon primaryWeapon;

    private PlayerWeapon currentWeapon;
    int currentWeaponIndex = 0;
    private WeaponGraphics currentGraphics;

    [SerializeField]
    private string weaponLayerName = "Weapon";

    RaycastHit hit;
    // Start is called before the first frame update
    void Start()
    {
        //if (isLocalPlayer)
        //    primaryWeapon = weaponsDatabase.allWeapons[0];
        EnquipWeapon(primaryWeapon);
    }

    private void Update()
    {
        //if (!isLocalPlayer)
            return;
        if (Input.GetKeyDown(KeyCode.N)) 
        {
            currentWeaponIndex++;
            if (currentWeaponIndex > 5)
                currentWeaponIndex = 0;
            EnquipWeapon(weaponsDatabase.allWeapons[currentWeaponIndex]);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (weaponHolder.childCount > 0)
                DropWeapon();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            float minDistance = 0f;
            string pickupItemName = "No Item";
            int pickUpIndex = -1;
            int i = 0;
            Collider[] colliders = Physics.OverlapBox(transform.position, Vector3.one * 3f, Quaternion.identity, 1 << 14);
            if (colliders.Length > 0)
            {
                //Debug.Log(colliders.Length);
                minDistance = Vector3.Distance(transform.position, colliders[0].ClosestPoint(transform.position));
                pickupItemName = colliders[0].name;
                pickUpIndex = 0;

                foreach (Collider c in colliders)
                {                    
                    float distance = Vector3.Distance(transform.position, c.ClosestPoint(transform.position));
                    //Debug.Log(c.transform.name);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        pickupItemName = c.name;
                        pickUpIndex = i;
                    }
                    i++;
                }
                //Debug.Log(pickUpIndex);
                //Destroy(colliders[pickUpIndex].gameObject);
                PickUpWeapon(colliders[pickUpIndex].transform);
            }
            //Debug.Log("Pick up " + pickupItemName);
        }
    }

    void PickUpWeapon(Transform pickWeapon)
    {
        bool found = false;
        if (weaponHolder.childCount > 0)
            DropWeapon();

        ShowPickUP(pickWeapon);

        foreach(PlayerWeapon weapon in weaponsDatabase.allWeapons)
        {
            if (weapon.name == pickWeapon.name)
            {
                currentWeaponIndex = weapon.index;
                found = true;
            }
        }
        if(!found)
            {
            Debug.LogError(pickWeapon.name + " not found");
        }

        //EnquipWeapon(weaponsDatabase.allWeapons[currentWeaponIndex]);        

        {/*
            foreach (Transform t in weaponHolder)
            {
                Destroy(t.gameObject);
            }
            Utility.SetLayerRecursively(weapon.gameObject, LayerMask.NameToLayer(weaponLayerName));
            weapon.parent = weaponHolder;
            weapon.GetComponent<Collider>().enabled = false;
            weapon.GetComponent<Rigidbody>().isKinematic = true;
            weapon.transform.position = weaponHolder.position;
            weapon.transform.rotation = weaponHolder.rotation;
            
            currentGraphics = weapon.GetComponent<WeaponGraphics>();
            currentGraphics.bulletSpawnPos.LookAt(camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 20f)));*/
        }
    }
    void DropWeapon()
    {
        Transform t = weaponHolder.GetChild(0);
        t.SendMessage("DropWeapon");
        currentGraphics = null;
    }
    void ShowPickUP(Transform pickWeapon)
    {
        pickWeapon.SendMessage("PickUp");
        StartCoroutine(ShowRepostioning(pickWeapon, weaponHolder));
    }
    IEnumerator ShowRepostioning(Transform target, Transform reachPoint)
    {
        Vector3 pos = reachPoint.position;
        while (target.position != pos)
        {
            target.position = Vector3.MoveTowards(target.position, pos, 10f * Time.deltaTime);
            target.rotation = Quaternion.Lerp(target.rotation, reachPoint.rotation, Time.deltaTime * 10f);
            yield return null;
        }
        Destroy(target.gameObject);
        EnquipWeapon(weaponsDatabase.allWeapons[currentWeaponIndex]);
    }


    public PlayerWeapon GetWeapon()
    {
        return currentWeapon;
    }
    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }
    void EnquipWeapon(PlayerWeapon weapon)
    {
        foreach(Transform t in weaponHolder)
        {
            Destroy(t.gameObject);
        }

        currentWeapon = weapon;

        GameObject weaponIns = Instantiate(weapon.weaponGFX, weaponHolder.position,
            weaponHolder.rotation, weaponHolder);
        weaponIns.transform.name = weapon.name;
        
        currentGraphics = weaponIns.GetComponent<WeaponGraphics>();
        currentGraphics.isEnquiped = true;
        if (currentGraphics == null)
        {
            Debug.LogError("No WeaponGraphics Component on the weapon Object: " + weaponIns.name);
        }
        if (currentWeaponIndex != 5)  
        {
            //weaponIns.layer = LayerMask.NameToLayer(weaponLayerName);            
            //Utility.SetLayerRecursively(weaponIns, LayerMask.NameToLayer(weaponLayerName));
            currentGraphics.bulletSpawnPos.LookAt(camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 20f)));
        }   
        
    }

    public void Shoot()
    {
        if (currentWeaponIndex != 5) 
        {            
            currentGraphics.muzzleFlash.Play();
            currentGraphics.fireSound.Play();

            GameObject bullet = Instantiate(currentGraphics.bulletPrefab, currentGraphics.bulletSpawnPos.position, currentGraphics.bulletSpawnPos.rotation);
            if (currentWeaponIndex == 1)
            {
                bullet.transform.localRotation *= Quaternion.Euler(Vector3.forward * Random.Range(0f, 10f) * 10f);
            }
        }
    }
}
