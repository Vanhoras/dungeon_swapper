// Case that Saveable is not found is not handled. Saveable Objects may not be deleted.
public interface ISaveable
{
    public State GetState();

    public void SetState(State state);
}

public interface State {}