using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProcess : MonoBehaviour
{
  
    public int layermask;
    public Vector2 prevPos;
    public Vector2 mousePos;

    public GameInput gameInput;


    private void Awake()
    {
        layermask = (1 << LayerMask.NameToLayer("TowerUnit") | 1 << LayerMask.NameToLayer("SpawnBlock") | 1 << LayerMask.NameToLayer("Enemy"));
    }
    private void OnEnable()
    {
        gameInput = new GameInput();
        gameInput.Enable();
        gameInput.Input.Press.performed += Press_performed;
        gameInput.Input.Drag.performed += Drag_performed;
        gameInput.Input.Pos.performed += Pos_performed;
    }

    private void OnDisable()
    {
        gameInput.Input.Press.performed -= Press_performed;
        gameInput.Input.Drag.performed -= Drag_performed;
        gameInput.Input.Pos.performed -= Pos_performed;
        gameInput.Disable();

    
    }
   
    #region Controller
    private void Press_performed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.machine.curStare != ECurStare.RUN) return;

        bool down = context.ReadValue<float>() == 1 ? true : false;
        var hit2D = Physics2D.Raycast((Vector3)mousePos - Vector3.forward, Vector2.zero, Mathf.Infinity, layermask);
        if (down)
        {
            GameManager.Instance.PathReSearch();
            if (hit2D)
            {
                if (hit2D.collider.TryGetComponent(out TowerUnit tower))
                {
                    Debug.Log(tower.name);
                    GameManager.Instance.selectTower = tower;
                    tower.isDown = true;
                    tower.OnCatch();
                    GameManager.Instance.inputProcess.prevPos = tower.transform.position;
                }
                else if (hit2D.collider.TryGetComponent(out SpawnBlock block))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        for (int j = 0; j < 2; j++)
                        { 
                            if (block.points.x == j + 2 && block.points.y == i + GameManager.Instance.spawner.level.y - 2)
                            {
                                GameDataBase.Instance.logManager.Show("골타워에는 타워를 놓을수없습니다.").Forget();
                                return;
                            }
                        
                        }
                    }



                    if (GameManager.Instance.bungStoreRoom.Count <= 0)
                    {
                        GameDataBase.Instance.logManager.Show("저장고에 붕빵이가 없습니다 뽑기를 시도해주세요").Forget();
                        return;
                    }

                    GameManager.Instance.SpawnTower(block);
                    
                }
            }
        }
        else
        {
            GameManager.Instance.PathReSearch();
            if (hit2D && GameManager.Instance.selectTower)
            {
                if (hit2D.collider.TryGetComponent(out TowerUnit tower) || hit2D.collider.TryGetComponent(out EnemyUnit enemy))
                {
                    GameManager.Instance.selectTower.isDown = false;
                    GameManager.Instance.selectTower.transform.position = GameManager.Instance.inputProcess.prevPos;
                    GameManager.Instance.selectTower.OnMissing();
                }
                else if (hit2D.collider.TryGetComponent(out SpawnBlock block) /*&& !hit2D.collider.TryGetComponent(out TowerUnit elsetower)*/)
                {
                    for (int i = 0; i < 2; i++)
                    for (int j = 0; j < 2; j++)
                    {
                        if (block.points.x == j + 2 && block.points.y == i + GameManager.Instance.spawner.level.y-2)
                        {
                                GameManager.Instance.selectTower.isDown = false;
                                GameManager.Instance.selectTower.transform.position = GameManager.Instance.inputProcess.prevPos;
                                GameDataBase.Instance.logManager.Show("골타워에는 타워를 놓을수없습니다.").Forget();
                                GameManager.Instance.selectTower.OnMissing();
                                return;
                         }

                    }

                    GameManager.Instance.selectTower.isDown = false;
                    GameManager.Instance.inputProcess.snapPosition(GameManager.Instance.inputProcess.mousePos, 6, 13, GameManager.Instance.selectTower.transform);
                    GameManager.Instance.selectTower.OnMissing();
                }
            }
            else if (!hit2D && GameManager.Instance.selectTower)
            {
                GameManager.Instance.towerUnitList.Remove(GameManager.Instance.selectTower);
                Destroy(GameManager.Instance.selectTower.gameObject);
                GameManager.Instance.DoughCnt += 5;
            }
        }
    }
    private void Drag_performed(InputAction.CallbackContext context)
    {
        if (GameManager.Instance.selectTower == null) return;
        GameManager.Instance.selectTower.transform.position = (Vector3)mousePos - Vector3.forward;
    }
    private void Pos_performed(InputAction.CallbackContext obj)
    {
       mousePos = Camera.main.ScreenToWorldPoint( obj.ReadValue<Vector2>());
       mousePos = new Vector3(mousePos.x ,mousePos.y , -1f);
        //GameManager.Instance.PathReSearch();
    }
    #endregion
    public void snapPosition(Vector2 pos, int x, int y , Transform tower)
    {
        //float val;
        float vx = pos.x % 1;
        float vy = pos.y % 1;


        float valX = vx < 0 == true ? -0.5f + Mathf.Abs(vx) : 0.5f - vx;
        float valY = vy < 0 == true ? -0.5f + Mathf.Abs(vy) : 0.5f - vy;

        if(tower != null)
        tower.transform.position = new Vector3(pos.x + valX, Mathf.Round(pos.y) , -1f);
    }
   
}