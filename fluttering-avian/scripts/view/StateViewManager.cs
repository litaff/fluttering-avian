namespace fluttering_avian.view;

using System.Collections.Generic;
using gameplay;
using gameplay_summary;
using Godot;
using idle;
using logger;
using ViewManager;
using ViewManager.View;

public partial class StateViewManager : Control
{
    [Export]
    private IdleStateControl idleStateControl;
    [Export]
    private GameplayStateControl gameplayStateControl;
    [Export]
    private GameplaySummaryStateControl gameplaySummaryStateControl;

    public ViewManager ViewManager { get; private set; }

    public override void _Ready()
    {
        var idleStateView = new IdleStateView(idleStateControl);
        var gameplayStateView = new GameplayStateView(gameplayStateControl);
        var gameplaySummaryStateView = new GameplaySummaryStateView(gameplaySummaryStateControl);
        
        ViewManager = new ViewManager(new List<IView> { idleStateView, gameplayStateView, gameplaySummaryStateView }, 
            new GodotLogger());
        
        base._Ready();
    }
}