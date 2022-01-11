using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof( Character))]
public class CharacterController : MonoBehaviour
{
    Joystick joystick;

    private Character character; // A reference to the ThirdPersonCharacter on the object
    private Transform mCamera;                  // A reference to the main camera in the scenes transform
    private Vector3 camForward;             // The current forward direction of the camera
    private Vector3 move;
    private bool jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    bool isAttack = true;

    private void Start()
    {

        joystick = FindObjectOfType<Joystick>();
        // get the transform of the main camera
        if (Camera.main != null)
        {
            mCamera = null;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        character = GetComponent<Character>();
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h;//= Input.GetAxis("Horizontal");
        float v;// = Input.GetAxis("Vertical");
        h = joystick.Horizontal;
        v = joystick.Vertical;
        bool crouch = Input.GetKey(KeyCode.C);
        //m_Jump = Input.GetButton("Jump");
        //m_Attack = Input.GetMouseButtonDown(1);
        //m_Attack = attackButton.pressed;

        // calculate move direction to pass to character
        if (mCamera != null)
        {
            // calculate camera relative direction to move:
            camForward = Vector3.Scale(mCamera.forward, new Vector3(1, 0, 1)).normalized;
            move = v * camForward + h * mCamera.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            move = v * Vector3.forward + h * Vector3.right;
        }
#if !MOBILE_INPUT
        // walk speed multiplier
        if (Input.GetKey(KeyCode.LeftShift)) move *= 0.5f;
#endif

        // pass all parameters to the character control script
        character.Move(move, crouch, jump, isAttack);
        jump = false;
        isAttack = false;
    }
}
