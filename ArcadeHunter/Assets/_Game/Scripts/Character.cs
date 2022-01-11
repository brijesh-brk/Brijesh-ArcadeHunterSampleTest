using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class Character : MonoBehaviour
{
    [SerializeField] float movingTurnSpeed = 360;
    [SerializeField] float stationaryTurnSpeed = 180;
    [SerializeField] float jumpPower = 12f;
    [Range(1f, 4f)] [SerializeField] float gravityMultiplier = 2f;
    [SerializeField] float runCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    [SerializeField] float moveSpeedMultiplier = 1f;
    [SerializeField] float animSpeedMultiplier = 1f;
    [SerializeField] float groundCheckDistance = 0.1f;

    Rigidbody charRigidbody;
    bool isGrounded;
    float origGroundCheckDistance;
    const float half = 0.5f;
    float turnAmount;
    float forwardAmount;
    Vector3 groundNormal;
    float capsuleHeight;
    Vector3 capsuleCenter;
    CapsuleCollider capsule;
    bool crouching;


    void Start()
    {
        charRigidbody = GetComponent<Rigidbody>();
        capsule = GetComponent<CapsuleCollider>();
        capsuleHeight = capsule.height;
        capsuleCenter = capsule.center;

        charRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        origGroundCheckDistance = groundCheckDistance;
    }

    public void Move(Vector3 move, bool crouch, bool jump, bool Attack)
    {
        if (move.magnitude > 1f) move.Normalize();
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, groundNormal);
        //Debug.DrawRay(transform.position, move * 20, Color.blue);
        turnAmount = Mathf.Atan2(move.x, move.z);
        forwardAmount = move.z;

        ApplyExtraTurnRotation();
               

        ScaleCapsuleForCrouching(crouch);
        PreventStandingInLowHeadroom();

    }

    void ScaleCapsuleForCrouching(bool crouch)
    {
        if (isGrounded && crouch)
        {
            if (crouching) return;
            capsule.height = capsule.height / 2f;
            capsule.center = capsule.center / 2f;
            crouching = true;
        }
        else
        {
            Ray crouchRay = new Ray(charRigidbody.position + Vector3.up * capsule.radius * half, Vector3.up);
            float crouchRayLength = capsuleHeight - capsule.radius * half;
            if (Physics.SphereCast(crouchRay, capsule.radius * half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                crouching = true;
                return;
            }
            capsule.height = capsuleHeight;
            capsule.center = capsuleCenter;
            crouching = false;
        }
    }

    void PreventStandingInLowHeadroom()
    {
        // prevent standing up in crouch-only zones
        if (!crouching)
        {
            Ray crouchRay = new Ray(charRigidbody.position + Vector3.up * capsule.radius * half, Vector3.up);
            float crouchRayLength = capsuleHeight - capsule.radius * half;
            if (Physics.SphereCast(crouchRay, capsule.radius * half, crouchRayLength, Physics.AllLayers, QueryTriggerInteraction.Ignore))
            {
                crouching = true;
            }
        }
    }

    void HandleAirborneMovement()
    {
        // apply extra gravity from multiplier:
        Vector3 extraGravityForce = (Physics.gravity * gravityMultiplier) - Physics.gravity;
        charRigidbody.AddForce(extraGravityForce);
        //Debug.Log(m_Rigidbody.velocity.y);
        groundCheckDistance = charRigidbody.velocity.y < 0 ? origGroundCheckDistance : 0.01f;
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
        transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
    }
      

}
