using UnityEngine;

public abstract class Enemy : Obstacle, ISaveable
{
    [SerializeField]
    protected Direction startDirectionFaced;

    protected Animator animator;

    public bool dead = false;
    public Direction directionFaced;

    private SpriteRenderer sprite;

    protected new void Start()
    {
        base.Start();

        StepController.instance.AddSaveable(this, GetInstanceID());

        animator = GetComponent<Animator>();

        FaceDirection(startDirectionFaced);
        sprite = GetComponent<SpriteRenderer>();
    }

    public virtual void Die()
    {
        dead = true;
        animator.SetTrigger("Death");

        RemoveAsObstacle();

        sprite.sortingLayerName = "corpse";
    }

    public virtual void Revive()
    {
        dead = false;
        animator.SetTrigger("Revive");

        ReAddAsObstacle();

        sprite.sortingLayerName = "Default";
    }

    protected void FaceDirection(Direction newDirection)
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

    public virtual State GetState()
    {
        EnemyState state = new(coords, dead, directionFaced);

        return state;
    }

    public virtual void SetState(State state)
    {
        if (state == null || state is not EnemyState) return;

        EnemyState newState = (EnemyState)state;


        this.coords = newState.coords;
        this.directionFaced = newState.directionFaced;

        if (this.dead && !newState.dead)
        {
            Revive();
        }
        

        MoveTo(this.coords);
        FaceDirection(this.directionFaced);
    }
}

public class EnemyState : ObstacleState
{
    public bool dead { get; set; }
    public Direction directionFaced { get; set; }

    public EnemyState(Coords coords, bool dead, Direction directionFaced) : base(coords)
    {
        this.dead = dead;
        this.directionFaced = directionFaced;
    }
}
