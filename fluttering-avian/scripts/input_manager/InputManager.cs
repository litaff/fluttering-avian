namespace fluttering_avian.input_manager;

using System;
using Godot;

public class InputManager
{
    private const string JUMP = "jump";

    public event Action OnJumpPressed;
    
    public void HandleInputEvent(InputEvent @event)
    {
        if(@event.IsAction(JUMP))
        {
            HandleJumpInput(@event);
        }
    }

    private void HandleJumpInput(InputEvent @event)
    {
        if (@event.IsActionPressed(JUMP)) OnJumpPressed?.Invoke();
    }
}