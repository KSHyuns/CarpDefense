using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;


public class TowerUnit : MonoBehaviour , ITowerUnit
{
    public SpriteRenderer spriteRenderer;

    public Collider2D collision2D;

    public enum TouchSelected {catching , move , missing }

    public TouchSelected touchSelected = TouchSelected.missing;

    public bool isDown = false;

    public void OnCatch()
    {
        collision2D.enabled = false;
        spriteRenderer.color = GameManager.Instance.catchColor;
        touchSelected = TouchSelected.catching;
    }

    public void OnMissing(Action evt)
    {
        collision2D.enabled = true;
        spriteRenderer.color = GameManager.Instance.missingColor;
        evt.Invoke();
        touchSelected = TouchSelected.missing;
    }
    public void managedUpdate() { }
    public void Update()
    {
        OnDrag();
    }

    public void managedInitialize()
    {
        touchSelected = TouchSelected.missing;

    }

    private void OnMouseDown()
    {
        OnDown();
    }
    private void OnMouseUp()
    {
        OnUp();
    }


    public void OnDown()
    {
        isDown = true;
        OnCatch();
        GameManager.Instance.selectTower = this;
        GameManager.Instance.inputProcess.prevPos = transform.position;

    }

    public void OnUp()
    {
        isDown = false;
        if (GameManager.Instance.selectTower == null) return;

        var hit2D = Physics2D.Raycast(GameManager.Instance.inputProcess.mousePos, Vector2.zero);
        if (hit2D)
        {
            if (hit2D.collider.TryGetComponent(out TowerUnit tower))
            {
                GameManager.Instance.selectTower.transform.position = GameManager.Instance.inputProcess.prevPos;
                OnMissing(() => { GameManager.Instance.selectTower = null; });
            }
            else if (hit2D.collider.TryGetComponent(out SpawnBlock block))
            {
                GameManager.Instance.inputProcess.snapPosition(GameManager.Instance.inputProcess.mousePos, 6, 9, GameManager.Instance.selectTower.transform);
                OnMissing(() => { GameManager.Instance.selectTower = null; });
            }
        }
        else
        { 
            Destroy(GameManager.Instance.selectTower.gameObject);
        }
    }

    public void OnDrag()
    {
       
    }
}
