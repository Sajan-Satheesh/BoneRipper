
using System;

public enum BoatMovementStates {load, stationary, moving, sinking, hidden, withPlayer}

public enum PlayerState {load, running, dead, jumping, inBoat, acting }

public enum EnemyState { load, chasing, attacking, resting, walkingToInn }

public enum Weapons { load, boneBreaker, enthiGuns, kaiMiniGun, spear }

public enum PlayerAnimationStates { idle, weaponBWwalk, restInBoat, jumpStart, aboutToLand, landing }
