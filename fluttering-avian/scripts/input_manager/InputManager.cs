namespace fluttering_avian.input_manager;

using System;
using Godot;

public class InputManager : Node
{
    public event Action<Key> OnKeyPressed;
    public event Action<Key> OnKeyHeld;
    public event Action<Key> OnKeyReleased;
    
    public override void _Input(InputEvent @event)
    {
        base._Input(@event);
		
        if(@event is InputEventKey inputEventKey)
        {
            HandleInputEventKey(inputEventKey);
        }
    }

    private void HandleInputEventKey(InputEventKey inputEventKey)
    {
        if (inputEventKey.Echo)
        {
            OnKeyHeld?.Invoke(inputEventKey.Keycode);
            return;
        }

        if (inputEventKey.Pressed)
        {
            OnKeyPressed?.Invoke(inputEventKey.Keycode);
        }
        else
        {
            OnKeyReleased?.Invoke(inputEventKey.Keycode);
        }
    }
}