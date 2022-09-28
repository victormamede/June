using Godot;
using System;
using System.Collections;
using System.Collections.Generic;

public class DialogueBox : Control, IDialogueBox
{
    [Export]
    public float HideOffset = 64;

    private Timer _timer;
    private Label _label;
    private Tween _tween;
    private Queue<Dialogue> _texts = new Queue<Dialogue>();
    private Vector2 _initialPosition;


    private enum State
    {
        Writing,
        Waiting,
        Hidden
    }
    private State _state = State.Hidden;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _label = GetNode<Label>("Label");
        _tween = GetNode<Tween>("Tween");
        _initialPosition = RectPosition;

        Visible = false;
    }

    public override void _Input(InputEvent @event)
    {
        if (_state == State.Hidden) return;

        if (@event.IsActionPressed("action"))
        {
            DialogueAction();
            GetTree().SetInputAsHandled();
        }
    }

    public void ShowDialogue(Dialogue[] texts)
    {
        Visible = true;
        GetTree().Paused = true;
        Enter();

        _state = State.Waiting;
        _texts = new Queue<Dialogue>(texts);
        DialogueAction();
    }

    private void DialogueAction()
    {
        if (_state == State.Waiting)
        {
            if (_texts.Count > 0)
            {
                FillText(_texts.Dequeue());
            }
            else
            {
                _state = State.Hidden;
                Leave();
                GetTree().Paused = false;
            }
        }
        else if (_state == State.Writing)
        {
            _label.VisibleCharacters = _label.GetTotalCharacterCount();
        }

    }

    private async void FillText(Dialogue text)
    {
        _label.Text = text.Value;
        switch (text.Effect)
        {
            case Dialogue.TextEffect.Jiggle:
                (_label.Material as ShaderMaterial).SetShaderParam("jiggle", true);
                break;
            default:
                (_label.Material as ShaderMaterial).SetShaderParam("jiggle", false);
                break;
        }

        _state = State.Writing;
        for (_label.VisibleCharacters = 0; _label.VisibleCharacters < _label.GetTotalCharacterCount(); _label.VisibleCharacters++)
        {
            _timer.Start(0.05f);
            await ToSignal(_timer, "timeout");
        }
        _state = State.Waiting;
    }

    private void Enter()
    {
        _tween.InterpolateProperty(this,
            "rect_position",
            _initialPosition + new Vector2(0, HideOffset),
            _initialPosition,
            1f,
            Tween.TransitionType.Circ,
            Tween.EaseType.Out);
        _tween.Start();
    }
    private void Leave()
    {
        _tween.InterpolateProperty(this,
            "rect_position",
            _initialPosition,
            _initialPosition + new Vector2(0, HideOffset),
            1f,
            Tween.TransitionType.Circ,
            Tween.EaseType.Out);
        _tween.Start();
    }

}
