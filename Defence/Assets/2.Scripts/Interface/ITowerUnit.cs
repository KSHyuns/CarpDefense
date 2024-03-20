using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerUnit 
{
    public void managedUpdate();
    public void managedInitialize();

    public void OnDown();
    public void OnUp();
    public void OnDrag();
}
