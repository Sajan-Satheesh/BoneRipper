
using UnityEngine;
using System;
using System.Threading;
using System.Threading.Tasks;

[Serializable]
public class TowerEnemyController : EnemyController 
{
    [SerializeField] TowerEnemyModel enemyModel;

    public TowerEnemyController(SO_TowerEnemy towerEnemyDetails, Vector3 spawnPosition) : base(towerEnemyDetails, spawnPosition)
    {
        enemyModel = new TowerEnemyModel(towerEnemyDetails, enemyView);
        setState(TowerEnemyState.resting);
    }

    override public void onUpdate()
    {
        updateStateMachine();
    }
    private void setAnimationState(
    EnemyAnimationStates state,
    bool blend = false,
    float transitionTime = 1f)
    {
        if (enemyModel.currAnimationState == state) return;

        enemyModel.currAnimationState = state;

        if (!blend)
            enemyModel.enemy.GetComponent<Animator>().Play(state.ToString());
        else enemyModel.enemy.GetComponent<Animator>().CrossFade(state.ToString(), transitionTime);

    }
    public void setState(TowerEnemyState _enemyState)
    {
        if (enemyModel.currState == _enemyState) return;
        prepareState(_enemyState);
        enemyModel.currState= _enemyState;
    }
    private void prepareState(TowerEnemyState _enemyState)
    {
        switch (_enemyState)
        {
            case TowerEnemyState.attacking:
                prepareAttack();
                break;
            case TowerEnemyState.resting:
                prepareResting();
                break;
            case TowerEnemyState.load:
                break;
            default:
                break;
        }
    }

    private void prepareResting()
    {
        setAnimationState(EnemyAnimationStates.idle,true);
    }

    private void updateStateMachine()
    {
        switch (enemyModel.currState)
        {
            case TowerEnemyState.attacking:
                turnTowardsTarget();
                break;
            case TowerEnemyState.resting:
                turnTowardsTarget();
                break;
            case TowerEnemyState.load:
                break;
            default:
                break;
        }
    }

    

    public void prepareAttack()
    {
        float playerSpeed = EnemyService.instance.requestPlayerSpeed();
        float normArrowSpeed = 10 / playerSpeed;
        if (normArrowSpeed <= 1)
        {
            Debug.LogWarning("trying to shoot inefficient Arrow");
            return;
        }
        shootAtTarget();
        setState(TowerEnemyState.resting);
    }

    private void resetProjections()
    {
        enemyModel.shootIteration = 1;
        enemyModel.projectedTarget = Vector3.zero;
        enemyModel.projectedShootReach = Vector3.zero;
        enemyModel.projectedShootDirection = Vector3.zero;
    }

    private async void shootAtTarget()
    {
        enemyModel.ct_shooting = new CancellationTokenSource();
        resetProjections();
        float playerSpeed = EnemyService.instance.requestPlayerSpeed();
        Vector3 playerPos = EnemyService.instance.requestPlayerLocation();
        Vector3 playerDirection = EnemyService.instance.requestPlayerDirection();
        float normArrowSpeed = 10 / playerSpeed;
        while(!canArrowReachPlayer(enemyModel.projectedTarget, enemyModel.projectedShootReach))
        {
            calculateTarget(playerPos, playerDirection, normArrowSpeed);
            ++enemyModel.shootIteration;
            await Task.Yield();
        }
        setAnimationState(EnemyAnimationStates.throwing,true);
        releaseArrow();
    }

    bool canArrowReachPlayer(Vector3 projectedTarget, Vector3 projectedReach)
    {
        float dist_shooter_target = Vector3.Distance(projectedTarget, enemyModel.enemy.transform.position);
        float dist_shooter_reach = Vector3.Distance(projectedReach, enemyModel.enemy.transform.position);
        return dist_shooter_reach > dist_shooter_target;
    }

    private void calculateTarget(Vector3 playerPos, Vector3 playerDirection, float normArrowSpeed)
    {
        
        calculateProjectedTarget(playerPos, playerDirection);
        calculateprojetedShoot(normArrowSpeed);
    }
    private void calculateProjectedTarget(Vector3 targetPos, Vector3 targetDirection)
    {
        targetDirection = targetDirection.normalized;
        enemyModel.projectedTarget = targetPos + targetDirection * enemyModel.shootIteration;
    }

    private void calculateprojetedShoot(float arrowSpeed)
    {
        enemyModel.projectedShootDirection = (enemyModel.projectedTarget - enemyModel.releaseHand.position).normalized;
        enemyModel.projectedShootReach = (enemyModel.releaseHand.position + enemyModel.projectedShootDirection * arrowSpeed * enemyModel.shootIteration);
    }
    private void releaseArrow()
    {
        WeaponServices.instance.shootArrows(enemyModel.releaseHand.position,enemyModel.projectedShootDirection);
    }

    private void turnTowardsTarget()
    {
        orient(EnemyService.instance.requestPlayerLocation());
    }

    void orient(Vector3 toLookPosition)
    {
        Quaternion toLookDirection = Quaternion.LookRotation(base.flatPosDirection(toLookPosition) - base.flatPosDirection(enemyModel.enemy.transform.position), Vector3.up);
        Quaternion finalRotation = Quaternion.Slerp(enemyModel.enemy.transform.rotation, toLookDirection, 0.1f);
        enemyModel.enemy.transform.rotation = finalRotation;
    }

    Vector3 getEnemyLoc()
    {
        return enemyModel.enemy.transform.position;
    }

    public override void destructEnemy()
    {
        base.destructEnemy();
        enemyModel.destroyModel();
        enemyModel.enemy = null;

    }
    ~TowerEnemyController()
    {
        Debug.Log("destroyed Tower Enemy Controller");
    }

}
