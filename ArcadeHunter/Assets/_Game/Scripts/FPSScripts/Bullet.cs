using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime = 5f;
    [SerializeField] float bulletSpeed = 9f;
    [SerializeField] int damage = 5;
    [SerializeField] float explosionForce = 100f;
    [SerializeField] bool gravity = false;
    [SerializeField] LayerMask mask;
    [SerializeField] GameObject hitEffectPrefab;
    [SerializeField] float damagePower = 10;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(CheckAndDestroy), lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(transform.forward * Time.deltaTime * 2);
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
        if (gravity)
            transform.localRotation *= Quaternion.Euler(Vector3.right * 0.15f);
        Debug.DrawRay(transform.position, transform.forward.normalized);
        if (Physics.Raycast(transform.position, transform.forward, out hit, 1f)) 
        {
            //Debug.Log("bullet HitSomething");

            //if (hit.collider.tag == "Player")
            //{
            //    CmdPlayerShoot(hit.collider.name, damage);
            //}
            //else if (hit.collider.tag == "Enemy")
            //{
            //    CmdEnemyShoot(hit.collider.name, damage);
            //}

            //Collider[] colliders = Physics.OverlapSphere(hit.point, 2.5f);
            //foreach(Collider c in colliders)
            //{

            //    Rigidbody rb = c.GetComponent<Rigidbody>();

            //    if (c.tag == "Enemy")   
            //    {
            //        if (gravity)
            //            CmdEnemyShoot(c.name, 10);
            //    }
            //    else if (rb != null)
            //    {
            //        rb.AddExplosionForce(explosionForce, hit.point, 5f);
            //    }
            //}
            if (hit.collider.gameObject.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                damageable.Damage(damagePower);
            }
            //GameObject hitEffect = Instantiate(hitEffectPrefab, hit.point, Quaternion.LookRotation(hit.normal));
            //hitAudio.Play();
            //Destroy(hitEffect, 1f);
            Destroy(this.gameObject);
        }
    }

    void CmdPlayerShoot(string _Id, int damage)
    {
        Debug.Log(_Id + "has been Shoot");

        //Player player = GameManager.GetPlayer(_Id);
        //player.RpcTakeDamage(damage);
    }

    void CmdEnemyShoot(string _Id, int damage)
    {
        Debug.Log(_Id + " has been Shoot");

        //Enemy enemy = GameManager.GetEnemy(_Id);
        //enemy.RpcTakeDamage(damage);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    Destroy(this.gameObject);
    //}

    void CheckAndDestroy()
    {
        if (this != null)
        {
            Destroy(this.gameObject);
        }
    }
}
