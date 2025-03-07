using UnityEngine;

public enum TowerId //For Raz: This is an enum of all basic towers in the game. This can be used to determine which tower to build when the player clicks on a build spot.
{
    Empty,
    BasicTower,
    SlowTower,
    HeavyTower,
    ChainTower,
    SnipeTower,
    FastBasicTower,
    LongRangeBasicTower,
    DamageSlowTower,
    SplashHeavyTower,
    SuperChainTower,
    SuperSnipeTower
}
// Will be used for data presistance, deciding which tower to build when a game is loaded.