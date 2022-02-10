using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/*Implimentation Steps
 - Create a capsule to be your player object.
 - Add a CharacterController component to your player.
 - Parent the camera to the player, and position it in their head.
 - Add this script to that camera.
 - Adjust settings according to your needs.
*/

public class FirstPersonCharacterController : MonoBehaviour
{
    #region Serialized Settings
    [Header("Speed Settings")]
    [SerializeField] float movementSpeed = 6.5f;
    [SerializeField] float strafeSpeed = 4f;
    [SerializeField] float sprintSpeedMod = 2;
    [SerializeField] float crouchSpeedMod = .5f;
    
    [Space]
    [Header("Crouch Settings")]
    [SerializeField, Range(.01f, 1)] float crouchHeight = .6f;
    [SerializeField, Range(.01f, 1)] float moveToCrouchSpeed = .1f;
    [SerializeField, Range(.01f, 1)] float moveFromCrouchSpeed = .2f;
    
    [Space]
    [Header("Jump Settings")]
    [SerializeField] float gravity = .98f;
    [SerializeField] float jumpForce = 14;
    public int airJumps = 0;

    [Space]
    [Header("View Settings")]
    [SerializeField] float mouseSensitivityX = 2.2f;
    [SerializeField] float mouseSensitivityY = 2.2f;
    [SerializeField] Vector2 yawBounds = new Vector2(-90, 35);
     

    [Space]
    [Header("Audio Settings")]
    [SerializeField] float defaultAudioVolume;
    [SerializeField] float defaultAuioPitch;
    [SerializeField] AudioClip footstepSoundClip;
    [SerializeField] Vector2 footstepsVolumeRandomRange;
    [SerializeField] Vector2 footstepsPitchRandomRange;
    [SerializeField] float footstepFrequency;
    [SerializeField] AudioClip jumpSoundClip;
    [SerializeField] AudioClip doubleJumpSoundClip;
    [SerializeField] float timeInAirToPlayLandingAudio = .2f;
    [SerializeField] AudioClip landingAudioClip;

    [Space]
    [Header("Debug Settings")]
    public GameObject sprintMarker;
    #endregion
    
    #region Local Variables
    Transform player;
    GameObject playerGO;
    CharacterController controller;
    CollisionFlags collisions;
    AudioSource audioSource;
    Transform tweenTarget;
    FOVAnimator cameraFOVAnimator;

    float pitch = 0;
    float yaw = 0;

    float airJumpCounter = 0;
    
    float crouchT = 0;
    bool crouched = false;

    bool pWasInAir = false;
    float timeInAir = 0;
    bool footstepCooldown = false;
    bool doSequence = false;
    bool lockMovement = false;
    bool sprinting = false;
    public Vector3 velocity;
    float lastHorzInput;
    bool lastSpaceInput;
    float lastVertInput;
    #endregion

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        player = transform.parent;
        playerGO = player.gameObject;
        controller = playerGO.GetComponent<CharacterController>();

        audioSource = GetComponent<AudioSource>();
        tweenTarget = transform.Find("TweenTarget");
        cameraFOVAnimator = GetComponent<FOVAnimator>();
    }
    Vector3 pVelocity;
    bool jumpUsed = false;
    private void Update()
    {
        if (Player.instance.Dead || Time.timeScale == 0)
            return;
        #region Rotations
        pitch += Input.GetAxis("Mouse X") * mouseSensitivityX;
        yaw -= Input.GetAxis("Mouse Y") * mouseSensitivityY;

        yaw = Mathf.Clamp(yaw, yawBounds.x, yawBounds.y);

        Vector3 targetPlayerRotation = new Vector3(0, pitch);
        player.eulerAngles = targetPlayerRotation;

        Vector3 targetHeadRotation = new Vector3(yaw, pitch);
        transform.eulerAngles = targetHeadRotation;
        #endregion

        if (doSequence)
        {
            controller.Move(tweenTarget.position - player.transform.position);
        }

        if (Cursor.lockState == CursorLockMode.Locked && !lockMovement && Time.timeScale != 0)
        {

            #region Movement
            
            

            velocity = new Vector3(0, velocity.y, 0);

            float forwardInput = Input.GetAxis("Vertical") * movementSpeed;
            float sideInput = Input.GetAxis("Horizontal") * strafeSpeed;

            //Handle Sprinting
            bool pSprintingState = sprinting;
            if (Input.GetKey(KeyCode.LeftShift) && (Mathf.Abs(pVelocity.x) > 0 || Mathf.Abs(pVelocity.z) > 0))
            {
                forwardInput *= sprintSpeedMod;

                sprinting = true;
            }
            else
            {
                sprinting = false;
            }

            if (sprinting != pSprintingState)
            {
                cameraFOVAnimator.SprintStateChanged();
            }

            //If we're holding crouch, crouch, if we're not holding crouch, uncrouch
            //If we're standing, simply return.
            //Removing this because it's glitchy.
            //handleCrouch();

            //Handle Jumping
            if (controller.isGrounded)
            {
                sprintMarker.SetActive(true);
                if (pWasInAir && timeInAir > timeInAirToPlayLandingAudio)
                {
                    ResetAudioSettings();
                    audioSource.PlayOneShot(landingAudioClip);
                    EventSystem.OnPlayerLandEvent();
                }

                jumpUsed = false;

                timeInAir = 0f;

                velocity.y = -.01f;
                airJumpCounter = 0;
                

            }
             else
            {
                sprintMarker.SetActive(false);
            }

            if (!controller.isGrounded)
            {
                timeInAir += Time.deltaTime;
            }
            pWasInAir = !controller.isGrounded;


            velocity += (player.forward * forwardInput) + (player.right * sideInput);
            collisions = controller.Move(velocity * .5f * Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
            {
                EventSystem.OnPlayerJumpEvent();

                velocity.y = jumpForce;

                ResetAudioSettings();
                audioSource.PlayOneShot(jumpSoundClip, 2.2f);
                jumpUsed = true;
            }
            else if (Input.GetKeyDown(KeyCode.Space) && airJumpCounter < airJumps)
            {
                velocity.y = jumpForce;
                airJumpCounter++;

                ResetAudioSettings();
                audioSource.PlayOneShot(jumpSoundClip, 1.25f);

            }

            velocity += Vector3.down * gravity * Time.deltaTime;

            collisions = controller.Move(velocity * .5f * Time.deltaTime);

            if ((controller.collisionFlags & CollisionFlags.Above) != 0)
            {
                velocity.y = 0;
            }

            if (forwardInput != 0 && controller.isGrounded)
            {
                audioSource.volume = Random.Range(footstepsVolumeRandomRange.x, footstepsVolumeRandomRange.y);
                audioSource.pitch = Random.Range(footstepsPitchRandomRange.x, footstepsPitchRandomRange.y);
                if (!footstepCooldown && lastVertInput > 0)
                {
                    audioSource.PlayOneShot(footstepSoundClip);
                    StartCoroutine(footstepDelay());
                }

            }

            #endregion
        }
        //Shortcut for locking & unlocking the cursor
        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = (Cursor.lockState == CursorLockMode.Locked) ? CursorLockMode.None : CursorLockMode.Locked;
        pVelocity = velocity;

    }

    void FixedUpdate()
    {
    }


    void handleCrouch() 
    {
        if (Input.GetKey(KeyCode.LeftControl) && !Input.GetKey(KeyCode.LeftShift) && controller.isGrounded) 
        {
            crouched = true;
           crouchT += moveToCrouchSpeed;
        } else if(player.localScale.y < 1) 
        {
            Ray headRay = new Ray(new Vector3(player.transform.position.x, controller.bounds.max.y, player.transform.position.z), Vector3.up * .1f);
            
            if(!Physics.Raycast(headRay))
            crouchT -= moveFromCrouchSpeed;
        } else 
        {
            crouched = false;
            return;
        }

        crouchT = Mathf.Clamp(crouchT, 0, 1);

        float y = Mathf.Lerp(1, crouchHeight, crouchT);
        player.localScale = new Vector3(player.localScale.x, y, player.localScale.z);

        if(controller.isGrounded)
            controller.Move(Vector3.down);
    }


    public void MovementSequence(List<TweenSegment> tweens) 
    {
        tweenTarget.SetParent(null);
        doSequence = true;

        lockMovement = tweens[0].lockMovement;

        Sequence s = DOTween.Sequence();

        Vector3 sumMovement = Vector3.zero;
        Vector3 sumRotation = Vector3.zero;
        
        foreach(TweenSegment tween in tweens) 
        {
            Vector3 swivelDir = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, new Vector3(player.transform.forward.x, 0, player.transform.forward.z), Vector3.up), Vector3.up) * tween.relativeMovement;

            s.SetEase(Ease.OutSine);
            s.Append(tweenTarget.DOMove(tweenTarget.position + swivelDir + sumMovement, tween.segmentLength));
            s.Join(tweenTarget.DORotate(tweenTarget.eulerAngles + tween.relativeRotation + sumRotation, tween.segmentLength));
            sumMovement += swivelDir;
            sumRotation += tween.relativeRotation;
        }

        s.AppendCallback(unlockMovement);
        
    }

    void unlockMovement() 
    {
        doSequence = false;
        lockMovement = false;

        tweenTarget.SetParent(transform);
        tweenTarget.localPosition = Vector3.zero;
        tweenTarget.transform.forward = player.transform.forward;
    }

    //Audio Methods
    IEnumerator footstepDelay() 
    {
        footstepCooldown = true;
        yield return new WaitForSeconds(footstepFrequency);
        footstepCooldown = false;
    }

    void ResetAudioSettings() 
    {
        audioSource.volume = defaultAudioVolume;
        audioSource.pitch = defaultAuioPitch;
    }

    public bool GetGroundState() 
    {
        return pWasInAir;
    }
}

[System.Serializable]
public struct TweenSegment 
{
    public float segmentLength;
    public Vector3 relativeMovement;
    public Vector3 relativeRotation;

    public bool lockMovement;
}