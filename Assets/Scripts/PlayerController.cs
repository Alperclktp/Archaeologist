using Sirenix.OdinInspector;
using UnityEngine;
using TMPro;

enum MovementMode { Keyboard, Joystick };

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    [TabGroup("Options")][SerializeField] private float movementSpeed;
    [TabGroup("Options")][SerializeField] private float rotationSpeed;

    [TabGroup("References")] public FloatingJoystick joystick;

    [FoldoutGroup("Debug")][SerializeField] private bool canMove;

    [FoldoutGroup("Debug")][SerializeField] private MovementMode movementMode;

    private float horizontal, vertical;

    private Vector3 moveDirection;

    private Animator anim;

    private void Awake()
    {
        Instance = this;

        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    private void HandleMovement()
    {
        canMove = moveDirection.magnitude != 0 ? true : false;

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        if (movementMode == MovementMode.Keyboard)
        {
            moveDirection = new Vector3(horizontal, 0, vertical);
            joystick.gameObject.SetActive(false);
        }
        else if (movementMode == MovementMode.Joystick)
        {
            moveDirection = new Vector3(joystick.Horizontal, 0, joystick.Vertical);
            joystick.gameObject.SetActive(true);
        }

        moveDirection.Normalize();

        transform.Translate(moveDirection * movementSpeed * Time.deltaTime, Space.World);

        UpdatePlayerAnimations();   
    }

    private void HandleRotation()
    {
        if (moveDirection != Vector3.zero)
        {
            Quaternion rotation = Quaternion.LookRotation(moveDirection, Vector3.up);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void UpdatePlayerAnimations()
    {
        PlayerInteraction playerInteraction = PlayerInteraction.Instance;

        anim.SetFloat("Speed", moveDirection.magnitude);

        if (playerInteraction.canDig || playerInteraction.canClean)
        {
            anim.SetBool("canDig", true);
            anim.SetBool("canScan", false);
        }
        else if (playerInteraction.canScan)
        {
            anim.SetBool("canScan", true);
            anim.SetBool("canDig", false);
        }
        else
        {
            anim.SetBool("canDig", false);
            anim.SetBool("canScan", false); 
        }

        if (playerInteraction.hasHold)
            anim.SetBool("canHold", true);
        else
            anim.SetBool("canHold", false);
    }
}

