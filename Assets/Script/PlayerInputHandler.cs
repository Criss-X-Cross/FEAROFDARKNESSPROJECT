using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.InputSystem.InputAction;

public class PlayerInputHandler : MonoBehaviour
{
    [Header("Input Action Asset")]
    [SerializeField] private InputActionAsset playerControls;

    [Header("Action Map Name Reference")]
    [SerializeField] private string actionMapName = "Player";

    [Header("Action Name References")]
    [SerializeField] private string movement = "Movement";
    [SerializeField] private string rotation = "Rotation";
    [SerializeField] private string jump = "Jump";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string menu = "Menu";


    private InputAction movementAction;
    private InputAction rotationAction;
    private InputAction jumpAction;
    private InputAction sprintAction;
    private InputAction interactAction;
    private InputAction menuAction;

    public Vector2 MovementInput {get; private set; }
    public Vector2 RotationInput {get; private set; }
    public bool JumpTrigerred {get; private set; }
    public bool SprintTriggered {get; private set; }
    public bool InteractTriggered {get; private set; }
    public bool MenuTriggered {get; private set; }

    private void Awake()
    {
        InputActionMap mapReference = playerControls.FindActionMap(actionMapName);

        movementAction = mapReference.FindAction(movement);
        rotationAction = mapReference.FindAction(rotation);
        jumpAction = mapReference.FindAction(jump);
        sprintAction = mapReference.FindAction(sprint);
        interactAction = mapReference.FindAction(interact);
        menuAction = mapReference.FindAction(menu);

        SubscribeActionValuesToInputEvents();

    }

    //private void InputPerformed(CallbackContext ctx)
    //{
    //    MovementInput = ctx.ReadValue<Vector2>();
    //    Debug.Log(MovementInput);
    //}

    private void SubscribeActionValuesToInputEvents()
    {
        //movementAction.performed += InputPerformed; // Alternative way to log input values
        movementAction.performed += inputInfo => MovementInput = inputInfo.ReadValue<Vector2>();
        movementAction.canceled += inputInfo => MovementInput = Vector2.zero;

        rotationAction.performed += inputInfo => RotationInput = inputInfo.ReadValue<Vector2>();
        rotationAction.canceled += inputInfo => RotationInput = Vector2.zero;

        jumpAction.performed += inputInfo => JumpTrigerred = true;
        jumpAction.canceled += inputInfo => JumpTrigerred = false;

        sprintAction.performed += inputInfo => SprintTriggered = true;
        sprintAction.canceled += inputInfo => SprintTriggered = false;

        interactAction.performed += inputInfo => InteractTriggered = true;
        interactAction.canceled += inputInfo => InteractTriggered = false;

        menuAction.performed += inputInfo => MenuTriggered = true;
        menuAction.canceled += inputInfo => MenuTriggered = false;
    }

    private void OnEnable()
    {
        playerControls.FindActionMap(actionMapName).Enable();
    }

    private void OnDisable()
    {
        playerControls.FindActionMap(actionMapName).Disable();
    }

}
