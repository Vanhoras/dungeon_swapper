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
    private float arrowSpeed;

    private bool aiming = false;

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
                Hit(player);
            }

            return;
        } 

        if (playerOrCover.GetComponent<Player>() != null)
        {
            aiming = true;
            animator.SetTrigger("Aim");
        }
    }



    private GameObject GetPlayerOrCoverOnPath(Player player, Direction direction)
    {
        int y;
        int x;

        switch (direction)
        {
            case Direction.UP:
                y = coords.Y - 1;
                while (y > -1)
                {
                    Coords currentCoords = new Coords(coords.X, y);

                    GameObject playerOrObject = GetPlayerOrCoverAtCoords(currentCoords, player);
                    if (playerOrObject != null) return playerOrObject;

                    y--;
                }
                break;
            case Direction.DOWN:
                y = coords.Y + 1;
                while (y < GridController.instance.GetTilesCountY())
                {
                    Coords currentCoords = new Coords(coords.X, y);

                    GameObject playerOrObject = GetPlayerOrCoverAtCoords(currentCoords, player);
                    if (playerOrObject != null) return playerOrObject;

                    y++;
                }
                break;
            case Direction.LEFT:
                x = coords.X - 1;
                while (x > -1)
                {
                    Coords currentCoords = new Coords(x, coords.Y);

                    GameObject playerOrObject = GetPlayerOrCoverAtCoords(currentCoords, player);
                    if (playerOrObject != null) return playerOrObject;

                     x--;
                }
                break;
            case Direction.RIGHT:
                x = coords.X + 1;
                while (x < GridController.instance.GetTilesCountX())
                {
                    Coords currentCoords = new Coords(x, coords.Y);

                    GameObject playerOrObject = GetPlayerOrCoverAtCoords(currentCoords, player);
                    if (playerOrObject != null) return playerOrObject;

                    x++;
                }
                break;
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

    private void Hit(Player player)
    {
        TurnController.instance.Stop();
        
        player.Kill();
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
