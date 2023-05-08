public enum EventKeys
{
    OnGameStarted,
    OnGameEnded,
    OnGamePaused,
    OnGameResumed,

    LevelLoaded,
    LevelUnloaded,
    LevelCompleted,
    LevelFailed,

    Buttonclicked,

    OnGateContactEnter,
    PlayerOnEnemyContact,
    EnemyContactEnded,

    OnPlayerUnitCountChange,
    OnPlayerUnitHit
}

public enum GameState
{
    Start,
    Playing,
    Paused,
    GameOver,
    Victory,
}

public enum PoolType
{
    PlayerUnit,
    EnemyUnit,
    PlayerUnitProjectile,
    EnemyUnitProjectile,
}

public enum ParticleType
{
    PlayerUnitAdd,
    PlayerUnitBlood,
}

public enum PlayerUnitType
{
    Melee,
    Ranged,
    Magic,
}

public enum EnemyType
{
    Melee,
    Ranged,
    Magic,
}

public enum EnemyState
{
    Idle,
    Chase,
    Attack,
    Dead,
}

public enum PlayerState
{
    Idle,
    Run,
    Attack,
}

public enum Operations
{
    Add,
    Subtract,
    Multiply,
    Divide,
}

public enum ButtonType
{
    Play,
    Pause,
    Resume,
    Restart,
}