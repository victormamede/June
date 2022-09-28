using Godot;
using System;
using System.Linq;

public class SimpleInspect : Node2D, IInteractable
{
    [Export(PropertyHint.MultilineText)]
    public string[] texts;

    private IDialogueBox _dialogueBox;

    public override void _Ready()
    {
        _dialogueBox = GetNode<IDialogueBox>("/root/Hud/DialogueBox");
    }

    public void Interact(Node interactor)
    {
        _dialogueBox.ShowDialogue(texts.Select(text => Dialogue.FromString(text)).ToArray());
    }
}
