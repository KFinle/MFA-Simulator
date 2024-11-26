
public interface IInteractable
{
    void Interact(LevelManager levelManager); // Pass LevelManager to handle the canvas switch
    void CancelInteraction(LevelManager levelManager);
}
