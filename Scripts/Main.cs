using Godot;

namespace Game;

public partial class Main : Node
{
  HUD _hud;
  Timer _mobTimer;
  Timer _scoreTimer;
  Timer _startTimer;
  AudioStreamPlayer _music;
  AudioStreamPlayer _deathSound;
  Player _player;
  Marker2D _startPosition;
  PathFollow2D _mobSpawnPath;

  [Export]
  public PackedScene MobScene { get; set; }

  int _score;

  public override void _Ready()
  {
    _hud = GetNode<HUD>("HUD");
    _mobTimer = GetNode<Timer>("MobTimer");
    _scoreTimer = GetNode<Timer>("ScoreTimer");
    _startTimer = GetNode<Timer>("StartTimer");
    _player = GetNode<Player>("Player");
    _startPosition = GetNode<Marker2D>("StartPosition");
    _mobSpawnPath = GetNode<PathFollow2D>("MobPath/MobSpawnLocation");
    _music = GetNode<AudioStreamPlayer>("Music");
    _deathSound = GetNode<AudioStreamPlayer>("DeathSound");
  }

  public void NewGame()
  {
    _score = 0;

    _player.Start(_startPosition.Position);

    _startTimer.Start();

    _hud.UpdateScore(_score);
    _hud.ShowMessage("Get Ready!");

    GetTree().CallGroup("mobs", Node.MethodName.QueueFree);

    _music.Play();
  }

  public void GameOver()
  {
    _scoreTimer.Stop();
    _mobTimer.Stop();

    _hud.ShowGameOver();

    _deathSound.Play();
    _music.Stop();
  }

  void OnScoreTimerTimeout()
  {
    _score++;

    _hud.UpdateScore(_score);
  }

  void OnStartTimerTimeout()
  {
    _mobTimer.Start();
    _scoreTimer.Start();
  }

  void OnMobTimerTimeout()
  {
    var newMob = MobScene.Instantiate<Mob>();

    _mobSpawnPath.ProgressRatio = GD.Randf();

    var direction = _mobSpawnPath.Rotation + Mathf.Pi * 0.5f;

    newMob.Position = _mobSpawnPath.Position;

    direction += (float)GD.RandRange(-Mathf.Pi * 0.25f, Mathf.Pi / 0.25f);
    newMob.Rotation = direction;

    var velocity = new Vector2((float)GD.RandRange(150f, 250f), 0);
    newMob.LinearVelocity = velocity.Rotated(direction);

    AddChild(newMob);
  }
}
