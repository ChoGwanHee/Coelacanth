using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBoxUtil : BaseItemBox, IInteractable
{
    public void Interact(PlayerController pc)
    {
        pc.LiftUtilItem(this);
    }
}
