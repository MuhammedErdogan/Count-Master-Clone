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

    OnGateContact,
    OnEnemyContact,

    OnPlayerUnitSpawned,
    OnPlayerUnitDestroyed,
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
    Chase,
    Attack,
    Dead,
}