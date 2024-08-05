namespace fluttering_avian.view.gameplay_summary;

using System;
using System.Threading.Tasks;
using ViewManager.View;

public class GameplaySummaryStateView : View
{
    private readonly GameplaySummaryStateControl control;
    
    public event Action OnRestartButtonPressed;
    
    public GameplaySummaryStateView(GameplaySummaryStateControl control)
    {
        this.control = control;
        UpdateView(0);
    }
    
    public void UpdateView(int score)
    {
        control.ScoreLabel.Text = score.ToString();
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
        control.RestartButton.Pressed += OnRestartButtonPressedHandler;
    }

    private void UnregisterHandlers()
    {
        control.RestartButton.Pressed -= OnRestartButtonPressedHandler;
    }

    private void OnRestartButtonPressedHandler()
    {
        OnRestartButtonPressed?.Invoke();
    }
}