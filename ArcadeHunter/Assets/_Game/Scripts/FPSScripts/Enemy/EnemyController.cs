using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;
using UnityStandardAssets.Characters.ThirdPerson;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(ThirdPersonCharacter))]
[RequireComponent(typeof(EnemyShoot))]
public class EnemyController : IEnemy,IDamageable
{
    public Transform gun;

    Enemy enemy;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public ThirdPersonCharacter character;
    [HideInInspector] public EnemyShoot enemyShoot;
    [SerializeField] float folllowDistance = 50f;
    Collider myCollider;
    Animator animator;
    public Transform spine;
    //public Transform hips;
    //Quaternion spineRot;
    public float maxAngle = 50f;

    bool folowPlayer = false;
    [SerializeField] LayerMask mask;
    RaycastHit hit;
    [SerializeField]Transform target;
    [SerializeField] ThirdPersonCharacter_edited player;
    Transform RHand;
    
    //Ragdoll
    public Transform rig;
    public List<Rigidbody> rigidbodies = new List<Rigidbody>();
    public List<Collider> colliders = new List<Collider>();

    private void OnEnable()
    {
        //spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        //spineRot = spine.rotation;
    }

    private void Start()
    {
        currentHealth = maxHealth;
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        //agent.velocity = Vector3.zero;
        //agent.isStopped = true;
        character = GetComponent<ThirdPersonCharacter>();
        enemyShoot = GetComponent<EnemyShoot>();
        agent.updateRotation = false;

        myCollider = gameObject.GetComponent<Collider>();
        animator = gameObject.GetComponent<Animator>();
        spine = animator.GetBoneTransform(HumanBodyBones.Spine);
        //hips = animator.GetBoneTransform(HumanBodyBones.Hips);
        RHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
        //spineRot = spine.transform.rotation;
        gun.transform.parent = RHand;
        GetRigidBody(rig);
        ToogleRagdoll(false);
        //animator.enabled = false;

        healthSliderOffset = transform.position - healthSlider.transform.position;
    }

    private void Update()
    {
        if (target == null)
        {
            //if (GameObject.FindGameObjectWithTag("Player") != null) 
            {
                //target = GameObject.FindGameObjectWithTag("Player").transform;
                myCollider.enabled = true;
                //agent.SetDestination(target.position);
            }
            folowPlayer = true;
        }
        else if (!enemy.isDead && player.GetPower() > 0)
        {
            Physics.Raycast(transform.position, target.position - transform.position, out hit, 100, mask, QueryTriggerInteraction.Ignore);
            //Debug.Log(hit.collider.name);

            if (folowPlayer)
            {
                agent.SetDestination(target.position);

                //spine.LookAt(target);

                if (agent.desiredVelocity == Vector3.zero)
                {
                    //animator.enabled = false;

                    Vector3 relativePos = target.position - spine.transform.position + new Vector3(0, -0.4f, 0);//spine transform earlier


                    // the second argument, upwards, defaults to Vector3.up
                    //relativePos = Quaternion.Euler(0, 0, 0) * relativePos;
                    Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);//Vector3.up

                    if (Quaternion.Angle(rotation, transform.rotation) <= maxAngle)
                    {
                        spine.transform.rotation = Quaternion.Slerp(spine.transform.rotation,
                            rotation, Time.fixedDeltaTime * 10f);
                    }
                    else
                    {
                        relativePos.y = 0f;//y
                        rotation = Quaternion.LookRotation(relativePos, Vector3.up);//Vector3.up
                        transform.rotation = Quaternion.Slerp(transform.rotation,
                            rotation, Time.fixedDeltaTime * 10f);//hips transform earlier
                    }
                    gun.rotation = rotation;
                    /*if (!enemyShoot.shooting ) 
                    {
                        InvokeRepeating("Shoot", 0, 2);
                        enemyShoot.shooting = true;
                    }*/
                }
                else
                {
                    if (enemyShoot.shooting)
                    {
                        CancelInvoke("Shoot");
                        enemyShoot.shooting = false;
                    }
                }

                //if (Vector3.Distance(transform.position, target.position) <= 5)                
                if (agent.remainingDistance < agent.stoppingDistance && hit.collider.tag == "Player")
                {
                    Debug.Log("Reach Player");
                    agent.stoppingDistance = 20f;
                    GetComponent<Rigidbody>().velocity = Vector3.zero;
                    character.Move(Vector3.zero, false, false);
                    animator.SetBool("Fire", true);
                    StartCoroutine(WaitAndDisable());
                    if (!enemyShoot.shooting)
                    {
                        InvokeRepeating("Shoot", 1, 1f / 1);
                        enemyShoot.shooting = true;
                    }
                    //agent.isStopped = true;
                }
                else
                {
                    Debug.Log("Reach Player not " + hit.collider.name + agent.remainingDistance + " " + agent.stoppingDistance);
                    if (hit.collider.tag != "Player")
                        agent.stoppingDistance = 5f;
                    else
                        agent.stoppingDistance = 20f;
                    StopAllCoroutines();
                    animator.enabled = true;
                    animator.SetBool("Fire", false);
                    character.Move(agent.desiredVelocity, false, false);
                    //agent.isStopped = false;
                    if (enemyShoot.shooting)
                    {
                        CancelInvoke("Shoot");
                        enemyShoot.shooting = false;
                    }
                }
            }
            else if (Vector3.Distance(a: target.position, b: transform.position) < folllowDistance && hit.collider != null)
            {
                Debug.DrawRay(transform.position, target.position - transform.position);
                if (hit.collider.tag == "Player")
                    folowPlayer = true;
            }
        }
        else
        {
            if (enemyShoot.shooting)
            {
                CancelInvoke("Shoot");
                enemyShoot.shooting = false;
            }

            animator.enabled = true;
            animator.SetBool("Fire", false);
        }

        healthSlider.transform.position = transform.position - healthSliderOffset;
    }
    void Shoot()
    {
        enemyShoot.Shoot(target.position);
    }

    public void ToogleRagdoll(bool isRagdoll)
    {
        if (isRagdoll)
        {
            myCollider.enabled = false;
            gun.GetChild(0).SendMessage("DropWeapon");
            //gun.GetChild(0).parent = null;
            //gun.GetComponentInChildren<BoxCollider>().enabled = true;
            //gun.GetComponentInChildren<Rigidbody>().isKinematic = false;
            //Utility.SetLayerRecursively(gun.gameObject, 14);
        }

        animator.enabled = !isRagdoll;

        //Utility.ToogleRigidBodyKinematic(rig, !isRagdoll);
        foreach(Rigidbody rb in rigidbodies)
        {
            rb.isKinematic = !isRagdoll;
        }
        foreach(Collider collider in colliders)
        {
            collider.enabled = isRagdoll;
        }
    }

    public void GetRigidBody(Transform rig)
    {
        //Transform _child=rig.GetChild(0).transform
        foreach (Transform child in rig)
        {
            if (rig.TryGetComponent<Rigidbody>(out Rigidbody _rb))
            {
                if (!rigidbodies.Contains(_rb))
                    rigidbodies.Add(_rb);                
            }
            if(rig.TryGetComponent<Collider>(out Collider _collider))
            {
                if (!colliders.Contains(_collider))
                    colliders.Add(_collider);
            }

            GetRigidBody(child);
        }
    }

    IEnumerator WaitAndDisable()
    {
        yield return new WaitForSeconds(.5f);
        animator.enabled = false;
    }

    [Header("Health")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float maxHealth = 100;
    [HideInInspector] public float currentHealth;
    Vector3 healthSliderOffset;


    public override float GetPower()
    {
        return currentHealth;
    }

    public override Transform GetTransform()
    {
        return transform;
    }

    public void Damage(float power)
    {
        currentHealth -= power;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthSlider.value = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            GetComponent<Enemy>().Die();
            Destroy(transform.parent.gameObject);
        }
    }

    public Vector3 GetPos()
    {
        throw new System.NotImplementedException();
    }
}
