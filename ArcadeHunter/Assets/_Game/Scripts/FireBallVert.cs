using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallVert : MonoBehaviour
{
    [SerializeField] private float damagePower;
    [HideInInspector]
    public Vector3 hitPoint;
    public GameObject smallExplosion;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        //StartCoroutine("WaitAndDestroy");
        //rb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hitPoint != Vector3.zero)
        {
            Vector3 vel= BallisticVelocity(hitPoint, 45);
            //if (vel.magnitude == Mathf.Infinity)
            //{
            //    vel = vel.normalized * 5;
            //}
            rb.velocity = vel;
            rb.useGravity = true;
            hitPoint = Vector3.zero;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent< IDamageable>(out IDamageable damageable))
        {
            damageable.Damage(damagePower);
        }

        if (smallExplosion != null)
            Instantiate(smallExplosion, transform.position, Quaternion.identity, transform);
        StartCoroutine(nameof(WaitAndDestroy));
        //Destroy(gameObject);
    }

    Vector3 BallisticVelocity(Vector3 target, float angle)
    {
        Vector3 direction = target - transform.position;
        float heightDiff = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        /*print(distance);
        if (distance > 80)
            distance = 80;
        else if (distance < 20)
            distance = 200;*/
        float a = angle * Mathf.Deg2Rad;
        direction.y = distance * Mathf.Tan(a);
        distance += heightDiff/Mathf.Tan(a);
        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude / Mathf.Sin(2 * a));

        if (float.IsNaN(velocity)) velocity = 5;

        return velocity * direction.normalized;
    }   
    
    public void Throw(Vector3 point)
    {
        transform.parent = null;
        //rb.isKinematic = false;
        //rb.useGravity = true;
        hitPoint = point;
    }
    IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(1);
        Destroy(gameObject);
    }
}
