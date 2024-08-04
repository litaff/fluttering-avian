namespace fluttering_avian.view.idle;

using System;
using System.Threading.Tasks;
using ViewManager.View;

public class IdleStateView : View
{
    private readonly IdleStateControl control;
    
    public event Action OnStartButtonPressed;

    public IdleStateView(IdleStateControl control)
    {
        this.control = control;
    }

    protected override Task DisplaySequence(bool instant = false)
    {
        control.Visible = true;
        return Task.CompletedTask;
    }

    protected override Task HideSequence(bool instant = false)
    {        
        control.Visible = false;
        return Task.CompletedTask;
    }

    protected override async Task AfterDisplay()
    {
        await base.AfterDisplay();
        RegisterHandlers();
    }

    protected override Task BeforeHide()
    {
        UnregisterHandlers();
        return base.BeforeHide();
    }

    private void RegisterHandlers()
    {
        control.StartButton.Pressed += OnStartButtonPressedHandler;
    }

    private void UnregisterHandlers()
    {
        control.StartButton.Pressed -= OnStartButtonPressedHandler;
    }

    private void OnStartButtonPressedHandler()
    {
        OnStartButtonPressed?.Invoke();
    }
}