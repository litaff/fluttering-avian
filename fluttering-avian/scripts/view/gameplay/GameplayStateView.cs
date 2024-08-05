namespace fluttering_avian.view.gameplay;

using System.Threading.Tasks;
using ViewManager.View;

public class GameplayStateView : View
{
    private readonly GameplayStateControl control;
    
    public GameplayStateView(GameplayStateControl control)
    {
        this.control = control;
    }
    
    public void UpdateScore(int score)
    {
        control.ScoreLabel.Text = score.ToString();
    }

    protected override Task BeforeDisplay()
    {
        UpdateScore(0);
        return base.BeforeDisplay();
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
}