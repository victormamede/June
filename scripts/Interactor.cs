using Godot;
using System;
using System.Collections.Generic;

public class Interactor : Area2D
{
    private List<Node2D> _interactables = new List<Node2D>();

    public override void _Ready()
    {
        Connect("body_entered", this, nameof(OnBodyEntered));
        Connect("body_exited", this, nameof(OnBodyExited));
    }

    public void Interact(Vector2 lookDirection)
    {
        float lowestDot = Mathf.Inf;
        IInteractable currentInteractable = null;
        foreach (Node2D node in _interactables)
        {
            Vector2 nodeToThis = (GlobalPosition - node.GlobalPosition).Normalized();
            float dot = lookDirection.Dot(nodeToThis);
            if (dot >= 0 || dot > lowestDot) continue;

            lowestDot = dot;
            currentInteractable = node as IInteractable;
        }

        if (currentInteractable is null) return;

        currentInteractable.Interact(this);
    }

    private void OnBodyEntered(Node2D body)
    {
        if (body is IInteractable)
            _interactables.Add(body);
    }
    private void OnBodyExited(Node2D body)
    {
        _interactables.Remove(body);
    }
}
