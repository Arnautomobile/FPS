public interface IPickable
{
    void Pickup();
    void Drop();
    void Activate();
    void Disable();
    void Outline();
    void RemoveOutline();
    bool IsPicked();
}