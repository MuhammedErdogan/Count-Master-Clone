public enum EventKeys : uint
{
    OnGameStarted,
    OnGameEnded,
    OnGamePaused,
    OnGameResumed,

    OnNextLevelRequest,

    LevelLoaded,
    LevelCompleted,
    LevelFailed,

    Buttonclicked,

    OnGateContactEnter,
    PlayerOnEnemyContact,
    EnemyContactEnded,

    OnPlayerUnitCountChange,
    OnPlayerUnitFall,
    OnPlayerUnitHit,

    FinishTriggered,
    TowerIsCreating,
    TowerCompleted,
}

public enum GameState : byte
{
    Start,
    Playing,
    Paused,
    GameOver,
    Victory,
}

public enum PoolType : byte
{
    PlayerUnit,
    EnemyUnit,
    PlayerUnitProjectile,
    EnemyUnitProjectile,
}

public enum ParticleType : byte
{
    PlayerUnitAdd,
    PlayerUnitBlood,
}

public enum PlayerUnitType : byte
{
    Melee,
    Ranged,
    Magic,
}

public enum EnemyType : byte
{
    Melee,
    Ranged,
    Magic,
}

public enum EnemyState : byte
{
    Idle,
    Chase,
    Attack,
    Dead,
}

public enum PlayerState : byte
{
    Idle,
    Run,
    Attack,
}

public enum Operations : byte
{
    Add,
    Subtract,
    Multiply,
    Divide,
}

public enum ButtonType : byte
{
    Play,
    Pause,
    Resume,
    Restart,
    NextLevel,
}

public enum CamerasType : byte
{
    Start_CAM = 5,
    Follow_CAM = 10,
    Finish_CAM = 15,
    Finish_CAM_2 = 20,
}

public enum FinishType : byte
{
    HumanTower,
    KickBoks,
    Archery,
}

public enum Particles : byte
{
    RedBlood,
    BlueBlood,
}