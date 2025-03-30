using UnityEngine;

public interface IPickable
{
    void Pickup(GameObject owner);
    void Drop();
    void Activate();
    void Disable();
    void Outline();
    void RemoveOutline();
    bool IsPicked();
}