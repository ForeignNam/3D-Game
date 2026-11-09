namespace RPG.Control
{ 

public interface IRaycastable
{

    public CursorType GetCursorType();
    bool HandleRaycast(PlayerController callingController);
}

}
