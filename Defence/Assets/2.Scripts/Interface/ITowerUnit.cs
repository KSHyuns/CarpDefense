using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITowerUnit 
{
    public void managedInitialize();

    public void RangeLound();

    public void Attacked();

    public void MyDestroy();

    public bool DragOn();

}
public interface IEnemyUnit
{
    public void OnSerchPath();
    public void OnbufferPath();
    public void overlapCircleLogic();
    public void isStopped(bool value);
    public void OnUpdate();

    public void sliderPositionUpdate();

    public void idle();
    public void move();
    public void death();
    

}

public interface IAttaker
{
    public void ManagedUpdate();

    public GameObject getEnemy();

    public void SetEnemy(GameObject obj);
}

public interface IBullet
{
    public void OnUpdate();
    public void Release();
}