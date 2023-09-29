using Godot;

namespace Game;

public partial class Mob : RigidBody2D
{
  AnimatedSprite2D _animatedSprite2D;

  public override void _Ready()
  {
    _animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    string[] mobTypes = _animatedSprite2D.SpriteFrames.GetAnimationNames();
    _animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
  }

  void OnVisibleOnScreenNotifier2DScreenExited() => QueueFree();
}
