using System.Collections;
using UnityEngine;

public class SkeletonArcher : Enemy
{

    [SerializeField]
    private GameObject arrowPrefabUp;
    [SerializeField]
    private GameObject arrowPrefabDown;
    [SerializeField]
    private GameObject arrowPrefabRight;
    [SerializeField]
    private GameObject arrowPrefabLeft;

    [SerializeField]
    private GameObject exclamationPoint;

    [SerializeField]
    private float arrowSpeed;

    private bool aiming = false;
    private bool notice = false;

    private new void Start()
    {
        base.Start();
        TurnController.instance.EnemyTurn += OnNextTurn;
        TurnController.instance.PlayerTurn += OnEndTurn;
    }

    private void OnDestroy()
    {
        TurnController.instance.EnemyTurn -= OnNextTurn;
        TurnController.instance.PlayerTurn -= OnEndTurn;
    }

    private void OnNextTurn(Player player)
    {
        GameObject playerOrCover = GetPlayerOrCoverOnPath(player, directionFaced);
        if (playerOrCover == null) return;

        if (aiming)
        {
            aiming = false;
            animator.SetTrigger("Shoot");

            StartCoroutine(ShootArrow(playerOrCover.transform.position, GridController.instance.GetPositionOfCoord(coords)));

            
            if (playerOrCover.GetComponent<Player>() != null)
            {
                HitPlayer(player);
                return;
            }

            Enemy enemy = playerOrCover.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.Die();
            }

            return;
        }


        if (dead) return;


        if (notice)
        {
            Aim();
            return;
        }

        if (playerOrCover.GetComponent<Player>() != null)
        {
            Notice();
        }
    }


    private void Notice()
    {
        notice = true;
        exclamationPoint.SetActive(true);
    }
    private void StopNotice()
    {
        notice = false;
        exclamationPoint.SetActive(false);
    }

    private void Aim()
    {
        StopNotice();

        aiming = true;
        animator.SetTrigger("Aim");
    }

    private void HitPlayer(Player player)
    {
        player.Die();
    }


    private GameObject GetPlayerOrCoverOnPath(Player player, Direction direction)
    {
        int y = coords.Y;
        int x = coords.X;

        while (y > -1 && y < GridController.instance.GetTilesCountY()
            && x > -1 && x < GridController.instance.GetTilesCountX())
        {

            switch (direction)
            {
                case Direction.UP:
                    y--;
                    break;
                case Direction.DOWN:
                    y++;
                    break;
                case Direction.LEFT:
                    x--;
                    break;
                case Direction.RIGHT:
                    x++;
                    break;
            }

            Coords currentCoords = new Coords(x, y);

            GameObject playerOrObject = GetPlayerOrCoverAtCoords(currentCoords, player);
            if (playerOrObject != null) return playerOrObject;
        }
        return null;
    }

    private GameObject GetPlayerOrCoverAtCoords(Coords coords, Player player)
    {
        Obstacle obstacle = GridController.instance.GetObstacleAtCoord(coords);
        if (obstacle != null && obstacle.IsCover()) return obstacle.gameObject;

        Tile tile = GridController.instance.GetTileAtCoord(coords);
        if (tile != null && tile.IsCover()) return tile.gameObject;


        if (coords.IsSame(player.GetCoords())) return player.gameObject;

        return null;
    }

    private IEnumerator ShootArrow(Vector3 goal, Vector3 startLocation)
    {
        GameObject arrowPrefab = null;

        switch (directionFaced)
        {
            case Direction.UP:
                arrowPrefab = arrowPrefabUp;
                break;
            case Direction.DOWN:
                arrowPrefab = arrowPrefabDown;
                break;
            case Direction.LEFT:
                arrowPrefab = arrowPrefabLeft;
                break;
            case Direction.RIGHT:
                arrowPrefab = arrowPrefabRight;
                break;
        }

        GameObject projectile = Instantiate(arrowPrefab, startLocation, Quaternion.identity);

        while (Vector3.Distance(projectile.transform.position, goal) > 0.01f)
        {

            var step = arrowSpeed * Time.deltaTime;
            projectile.transform.position = Vector3.MoveTowards(projectile.transform.position, goal, step);

            yield return null;
        }

        Destroy(projectile);
        yield return null;
    }

    public override void Die()
    {
        base.Die();

        StopNotice();
    }

    // fixes a bug, where the archer shoots on the next turn after death
    private void OnEndTurn(PlayerAction action)
    {
        if (dead) aiming = false;
    }

    public override State GetState()
    {
        SkeletonArcherState state = new(coords, dead, directionFaced, aiming, notice);

        return state;
    }

    public override void SetState(State state)
    {
        if (state == null || state is not SkeletonArcherState) return;

        SkeletonArcherState newState = (SkeletonArcherState)state;

        EnemyState enemyState = new(newState.coords, newState.dead, newState.directionFaced);
        base.SetState(enemyState);

        if (!newState.notice && !newState.aiming)
        {
            StopNotice();
        } else if (newState.notice)
        {
            Notice();
            aiming = false;
            animator.SetTrigger("StopAim");
        } else if (newState.aiming)
        {
            Aim();
        }
    }
}

public class SkeletonArcherState : EnemyState
{
    public bool aiming;
    public bool notice;

    public SkeletonArcherState(Coords coords, bool dead, Direction directionFaced, bool aiming, bool notice) : base(coords, dead, directionFaced)
    {
        this.aiming = aiming;
        this.notice = notice;
    }
}
