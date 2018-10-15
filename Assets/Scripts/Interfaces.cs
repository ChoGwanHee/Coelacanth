
public interface IInteractable
{
    void Interact(PlayerController pc);
    bool IsInteractable();
    int GetButtonType();
}