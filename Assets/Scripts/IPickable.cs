public interface IPickable
{
    void Pickup(OwnerType owner);
    void Drop();
    bool Picked();
}