
public enum BoatMovementStates {load, stationary, moveTowards, sinking, hidden, moveAway}
public enum PlayerState {load, running, dead, jumping, inBoat, levelEnd }
public enum GroundEnemyState { load, chasing, attacking, resting, walkingToInn, walkFromInn }
public enum TowerEnemyState { load, attacking, resting }
public enum Weapons { load, boneBreaker, enthiGuns, kaiMiniGun, spear }
public enum Destructables { player, groundEnemy, towerEnemy}
public enum PlayerAnimationStates { idle, weaponBWwalk, restInBoat, jumpStart, aboutToLand, landing }
public enum EnemyAnimationStates { idle, chasing, throwing  }

public interface IDestructable
{
    public Controller getController();
    public Destructables getDestructableType();
}
