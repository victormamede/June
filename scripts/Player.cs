using Godot;
using System;

public class Player : KinematicBody2D
{
    private Vector2 _direction = new Vector2();
    private Vector2 _lookDirection = new Vector2(1f, 0f);
    private Vector2 _velocity = new Vector2();

    private AnimationPlayer _animationPlayer;
    private Sprite _sprite;
    private Interactor _interactor;

    [Export]
    private float _speed = 50f;

    public override void _Ready()
    {
        _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        _sprite = GetNode<Sprite>("Sprite");
        _interactor = GetNode<Interactor>("Interactor");
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("action"))
        {
            _interactor.Interact(_lookDirection);
        }
    }

    public override void _Process(float delta)
    {
        _direction.x = Input.GetAxis("left", "right");
        _direction.y = Input.GetAxis("up", "down");

        if (_direction.LengthSquared() > 0f)
        {
            _direction = _direction.Normalized();
            _lookDirection = _direction;

            if (_lookDirection.y >= 0f)
                _animationPlayer.CurrentAnimation = "walk-front";
            else
                _animationPlayer.CurrentAnimation = "walk-back";


            if (_direction.x != 0f)
                _sprite.FlipH = _direction.x < 0f;
        }
        else
        {
            if (_lookDirection.y >= 0f)
                _animationPlayer.CurrentAnimation = "idle-front";
            else
                _animationPlayer.CurrentAnimation = "idle-back";
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        _velocity = MoveAndSlide(_direction * _speed);
    }
}

