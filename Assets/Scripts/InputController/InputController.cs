using UnityEngine;
using UnityEngine.Windows;
using UnityEngine.InputSystem;

public class InputController : MonoBehaviour
{

    public static InputController instance;
    public Vector2 moveDirection {  get; private set; }


    private InputSystem_Actions _inputController;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        _inputController = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        _inputController.Enable();
        _inputController.Player.Move.performed += PlayerMovePerfomed;
        _inputController.Player.Move.canceled += PlayerMoveCanceled;


    }

    private void OnDisable()
    {

        _inputController.Player.Move.performed -= PlayerMovePerfomed;
        _inputController.Player.Move.canceled -= PlayerMoveCanceled;

        _inputController.Disable();
    }

    private void PlayerMovePerfomed(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<Vector2>();
    }
    private void PlayerMoveCanceled(InputAction.CallbackContext context)
    {
        moveDirection = Vector2.zero;
    }
}
