using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static Character_Controls;

[CreateAssetMenu(fileName = "InputReader", menuName = "Input/Input Reader")]
public class InputReader : ScriptableObject, IPlayerActions
{
    public event UnityAction<Vector2> Move = delegate { };
    public event UnityAction<bool> Jump = delegate { };
    public event UnityAction<bool> Dash = delegate { };
    public event UnityAction Attack = delegate { };
    public event UnityAction Pause = delegate { };
    public event UnityAction PickUp = delegate { };
    public event UnityAction Drop = delegate { };
    public event UnityAction InfiniteHealth = delegate { };
    public event UnityAction Nuke = delegate { };
    public event UnityAction Teleport = delegate { };
    public event UnityAction Flash = delegate { };

    private Character_Controls InputActions;

    public Vector3 Direction => InputActions.Player.Move.ReadValue<Vector2>();

    private void OnEnable()
    {
        if (InputActions == null)
        {
            InputActions = new Character_Controls();
            InputActions.Player.SetCallbacks(this);
        }

        InputActions.Enable();
    }

    public void OnMouse_Look(InputAction.CallbackContext context)
    {
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Move?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Jump?.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                Jump?.Invoke(false);
                break;
        }
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        switch (context.phase)
        {
            case InputActionPhase.Started:
                Dash?.Invoke(true);
                break;
            case InputActionPhase.Canceled:
                Dash?.Invoke(false);
                break;
        }
    }

    public void OnL_Click(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            Attack?.Invoke();
    }

    public void OnR_Click(InputAction.CallbackContext context)
    {
    }

    public void OnPickUp(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            PickUp?.Invoke();
    }

    public void OnDrop(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            Drop?.Invoke();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            Pause?.Invoke();
    }

    public void OnInfiniteHealth(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            InfiniteHealth?.Invoke();
    }

    public void OnNuke(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            Nuke?.Invoke();
    }

    public void OnFlash(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            Flash?.Invoke();
    }

    public void OnTeleport(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
            Teleport?.Invoke();
    }
}