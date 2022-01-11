using UnityEngine;

[RequireComponent(typeof(Rigidbody),typeof(BoxCollider))]
public class WeaponGraphics : MonoBehaviour
{
    public bool isEnquiped = false;
    public Transform bulletSpawnPos;
    public GameObject bulletPrefab;//may be as batman hook for grappling gun in fututre
    public ParticleSystem muzzleFlash;
    public AudioSource fireSound;
    //public GameObject hitEffectPrefab;


    [Header("Drop and Pickup")]
    Collider _collider;
    Rigidbody rb;       

    protected void Start()
    {
        _collider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    public void DropWeapon()
    {
        isEnquiped = false;
        transform.parent = null;
        _collider.enabled = true;
        rb.isKinematic = false;
        rb.AddForce(transform.forward * 5, ForceMode.Impulse);
        Utility.SetLayerRecursively(gameObject, 14);
    }
    public void PickUp()
    {
        _collider.enabled = false;
        rb.isKinematic = true;
    }
}
