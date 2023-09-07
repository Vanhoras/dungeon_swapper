using UnityEngine;

public abstract class Enemy : Obstacle
{
    [SerializeField]
    protected Direction startDirectionFaced;

    protected Direction directionFaced;

    protected Animator animator;

    protected bool dead = false;

    protected new void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();

        FacceDirection(startDirectionFaced);
    }

    protected void FacceDirection(Direction newDirection)
    {
        directionFaced = newDirection;

        switch (newDirection)
        {
            case Direction.UP:
                animator.SetBool("Up", true);
                animator.SetBool("Down", false);
                transform.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                break;
            case Direction.DOWN:
                animator.SetBool("Up", false);
                animator.SetBool("Down", true);
                transform.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                break;
            case Direction.LEFT:
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                transform.transform.eulerAngles = new Vector3(transform.rotation.x, 180, transform.rotation.z);
                break;
            case Direction.RIGHT:
                animator.SetBool("Up", false);
                animator.SetBool("Down", false);
                transform.transform.eulerAngles = new Vector3(transform.rotation.x, 0, transform.rotation.z);
                break;
        }
    }
}
