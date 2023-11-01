using UnityEngine;

public class SkeletonWarrior : Enemy
{
    private new void Start()
    {
        base.Start();
        TurnController.instance.EnemyTurn += OnNextTurn;
    }

    private void OnDestroy()
    {
        TurnController.instance.EnemyTurn -= OnNextTurn;
    }


    private void OnNextTurn(Player player)
    {
        if (dead) return;


        Coords playerCoords = player.GetCoords();

        if (playerCoords.X == coords.X && playerCoords.Y == coords.Y - 1)
        {
            directionFaced = Direction.UP;
            animator.SetBool("Up", true);
            animator.SetBool("Down", false);
            transform.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            Attack(player);
        }
        else if (playerCoords.X == coords.X && playerCoords.Y == coords.Y + 1)
        {
            directionFaced = Direction.DOWN;
            animator.SetBool("Up", false);
            animator.SetBool("Down", true);
            transform.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            Attack(player);
        }
        else if (playerCoords.X == coords.X - 1 && playerCoords.Y == coords.Y)
        {
            directionFaced = Direction.LEFT;
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            transform.transform.eulerAngles = new Vector3(transform.rotation.x, 180, transform.rotation.z);
            Attack(player);
        }
        else if (playerCoords.X == coords.X + 1 && playerCoords.Y == coords.Y)
        {
            directionFaced = Direction.RIGHT;
            animator.SetBool("Up", false);
            animator.SetBool("Down", false);
            transform.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
            Attack(player);
        }
    }

    private void Attack(Player player)
    {
        TurnController.instance.Stop();
        animator.SetTrigger("Attack");

        player.Die();
    }
}
