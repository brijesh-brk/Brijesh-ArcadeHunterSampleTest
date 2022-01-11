using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyThrowerController : IEnemy, IDamageable
{
    [SerializeField] private Transform throwerBaseTransform;
    [SerializeField] private IDamageable target;
    [SerializeField] private FireBallVert ballPrefab;
    [SerializeField] private Transform ballPosition;
    [SerializeField] private float hitDelay, startDelay;
    float t;

    [Header("Health")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private float maxHealth = 100;
    [HideInInspector]public float currentHealth;

    Vector3 hitPoint;
    //public GameObject death;

    FireBallVert ball;
    Vector3 direction;
    Quaternion rot;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<ThirdPersonCharacter_edited>();
        currentHealth = maxHealth;
        healthSlider.value = 1;
        t = startDelay;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0 || target.GetPower()<=0) return;
        if (hitPoint != Vector3.zero)
        {
            direction = hitPoint - transform.position;
            direction.y = 0;
            Quaternion rot = Quaternion.LookRotation(direction);
            if (throwerBaseTransform.rotation != rot)
                throwerBaseTransform.rotation = Quaternion.Slerp(throwerBaseTransform.rotation,
                    rot, Time.deltaTime * 5);
            //if (throwerBaseTransform.rotation == rot)
            if (Quaternion.Angle(throwerBaseTransform.rotation, rot) < 0.1f)
                ThrowFireBall();
        }
        else
        {
            if (t > 0)
            {
                t -= Time.deltaTime;
            }
            else
            {
                t = hitDelay;
                SetHitPoint(target.GetPos());
            }
        }
    }

    void ThrowFireBall()
    {
        //Quaternion rot = Quaternion.Euler(60, 0, 0);
        //throwerTransform.localRotation = Quaternion.Slerp(throwerTransform.localRotation, 
        //    rot, 20 * Time.deltaTime);
        //if (throwerTransform.localRotation == rot && ball != null) 
        //{
        ball = Instantiate(ballPrefab);
        ball.transform.position = ballPosition.position;
        ball.enabled = true;
        ball.Throw(hitPoint);
        ball = null;
        ResetThrowerRotation();
        //StartCoroutine(nameof(WaitAndReset));
    }

    public void SetHitPoint(Vector3 pos)
    {
        pos += Vector3.up * .5f;
        hitPoint = pos;
    }

    void ResetThrowerRotation()
    {
        //throwerTransform.localRotation = Quaternion.Euler(-40, 0, 0);
        //hitPoint = Vector3.zero;
        //GameObject newBAll = Instantiate(ballPrefab, /*ballPosition, Quaternion.identity,*/ throwerTransform);
        //newBAll.transform.localPosition = ballPosition;
        //newBAll.transform.localRotation = Quaternion.identity;
        //ball = newBAll.GetComponent<FireBallVert>();
        hitPoint = Vector3.zero;

    }

    IEnumerator WaitAndDead()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }

    public void Damage(float power)
    {
        currentHealth -= power;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        healthSlider.value = currentHealth / maxHealth;

        if (currentHealth <= 0)
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public override float GetPower()
    {
        return currentHealth;
    }

    public Vector3 GetPos()
    {
        throw new System.NotImplementedException();
    }

    public override Transform GetTransform()
    {
        return transform;
    }
}
