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
        TurnController.instance.PlayerEndTurn += OnNextTurn;
    }

    private void OnDestroy()
    {
        TurnController.instance.PlayerEndTurn -= OnNextTurn;
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

    private void Aim()
    {
        notice = false;
        exclamationPoint.SetActive(false);

        aiming = true;
        animator.SetTrigger("Aim");
    }

    private void HitPlayer(Player player)
    {
        TurnController.instance.Stop();

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
}
