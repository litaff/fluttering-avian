namespace fluttering_avian.state_machine.states;

using System;
using character_controller;
using Godot;
using input_manager;
using obstacles;

[GlobalClass]
public partial class GameplayState : BaseCoreState
{
    [Export]
    private CharacterController character;
    [Export]
    private ObstacleManager obstacleManager;
    [Export]
    private Key inputKey;

    private InputManager inputManager;
    
    public override StateType StateType => StateType.Gameplay;

    public void Initialize(InputManager inputManager)
    {
        this.inputManager = inputManager;
    }
    
    public override void OnEnter()
    {
        inputManager.OnKeyPressed += OnFirstInputHandler;
        base.OnEnter();
    }

    public override void OnExit()
    {
        inputManager.OnKeyPressed -= OnFirstInputHandler;
        inputManager.OnKeyPressed -= OnInputHandler;
        base.OnExit();
    }

    private void OnFirstInputHandler(Key keycode)
    {
        if (keycode != inputKey) return;
        inputManager.OnKeyPressed -= OnFirstInputHandler;
        
        character.Freeze = false;
        obstacleManager.StartSpawning();
        character.RequestJump();
        inputManager.OnKeyPressed += OnInputHandler;
    }

    private void OnInputHandler(Key keycode)
    {
        if (keycode != inputKey) return;
        
        character.RequestJump();
    }
    
}