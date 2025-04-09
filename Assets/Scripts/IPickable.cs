using UnityEngine;

public interface IPickable
{
    void Pickup(GameObject owner);
    void Drop();
    void Outline();
    void RemoveOutline();
    bool IsPicked();

    // add a bool function that returns if the item is picked by hovering with the mouse
    // or automatically by being close to it
}