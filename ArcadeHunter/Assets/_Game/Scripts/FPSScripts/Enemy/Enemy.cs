using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(EnemyController))]
public class Enemy : MonoBehaviour
{
     bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }

    [SerializeField] int maxHealth = 100;
    
    
    private int currentHealth;

    EnemyController enemyController;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        enemyController = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RpcTakeDamage(int amount)
    {
        if (isDead)
            return;

        currentHealth -= amount;
        //Debug.Log(transform.name + "now has" + currentHealth + "health");

        if (currentHealth<=0)
        {
            Die();
        }
    }

    public void Die()
    {
        isDead = true;

        enemyController.ToogleRagdoll(true);
        //if (enemyController.agent.remainingDistance <= 2)
        enemyController.agent.isStopped = true;
        enemyController.character.Move(Vector3.zero, false, false);
        enemyController.character.m_Rigidbody.isKinematic = true;
        if (enemyController.enemyShoot.shooting)
        {
            enemyController.CancelInvoke("Shoot");
            enemyController.enemyShoot.shooting = false;
        }
        //Debug.Log(transform.name + "is Dead");

    }
    //public override void OnStartClient()
    //{
    //    base.OnStartClient();

    //    string netID = GetComponent<NetworkIdentity>().netId.ToString();
    //    Enemy enemy = GetComponent<Enemy>();

    //    GameManager.RegisterEnemy(netID, enemy);
    //}
}
