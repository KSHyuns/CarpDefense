using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using static UnityEditor.PlayerSettings;
using ETouch = UnityEngine.InputSystem.EnhancedTouch;
public class InputProcess : MonoBehaviour
{
    private MapGenarator level;
    public int layermask;
    public Vector2 prevPos;
    public Vector2 mousePos;

    public GameInput gameInput;

    public List<ITowerUnit> ItowerUnitList = new List<ITowerUnit>();

    private void Awake()
    {
        level = FindObjectOfType<MapGenarator>();
        layermask = (1 << LayerMask.NameToLayer("TowerUnit") | 1 << LayerMask.NameToLayer("SpawnBlock") | 1 << LayerMask.NameToLayer("Enemy"));
        ItowerUnitList.ForEach(x => x.managedInitialize());
    }
    private void OnEnable()
    {
        gameInput = new GameInput();
        gameInput.Enable();
        //gameInput.Input.Drag.performed += Drag_performed;
        //gameInput.Input.Press.performed += Press_performed;
        gameInput.Input.Pos.performed += Pos_performed;

        EnhancedTouchSupport.Enable();
        ETouch.Touch.onFingerDown += onFingerDown;
        ETouch.Touch.onFingerMove += onFingerMove;
        ETouch.Touch.onFingerUp += onFingerUp;
    }
    private void OnDisable()
    {
        ETouch.Touch.onFingerDown -= onFingerDown;
        ETouch.Touch.onFingerMove -= onFingerMove;
        ETouch.Touch.onFingerUp -= onFingerUp;
        EnhancedTouchSupport.Disable();
    }
    #region PC
    private void Pos_performed(InputAction.CallbackContext obj)
    {
       mousePos = Camera.main.ScreenToWorldPoint( obj.ReadValue<Vector2>());
    }

   
    #endregion
    #region Mobile
    private void onFingerMove(Finger finger)
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);
        var hit2D = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, layermask);

        if (GameManager.Instance.selectTower == null) return;
        GameManager.Instance.selectTower.transform.position = mousePos;
    }
    private void onFingerUp(Finger finger)
    {
        if (GameManager.Instance.selectTower == null) return;
        Vector2 pos = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);
        var hit2D = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, layermask);

        if (hit2D)
        {
            if (hit2D.collider.TryGetComponent(out TowerUnit tower))
            {
                tower.isDown = false;
                GameManager.Instance.selectTower.transform.position = GameManager.Instance.inputProcess.prevPos;
                tower.OnMissing(() => { GameManager.Instance.selectTower = null; });
            }
            else if (hit2D.collider.TryGetComponent(out SpawnBlock block) && !hit2D.collider.TryGetComponent(out TowerUnit elsetower))
            {
                GameManager.Instance.inputProcess.snapPosition(GameManager.Instance.inputProcess.mousePos, 6, 9, GameManager.Instance.selectTower.transform);
                elsetower.OnMissing(() => { GameManager.Instance.selectTower = null; });
            }
        }
        else
        {
            Destroy(GameManager.Instance.selectTower.gameObject);
        }

    }
    private void onFingerDown(Finger finger)
    {
        Debug.Log("Down");
        Vector2 pos = Camera.main.ScreenToWorldPoint(finger.currentTouch.screenPosition);
        var hit2D = Physics2D.Raycast(pos, Vector2.zero, Mathf.Infinity, layermask);
        if (hit2D)
        {
            if (hit2D.collider.TryGetComponent(out TowerUnit tower))
            {
                tower.isDown = true;
                tower.OnCatch();
                GameManager.Instance.selectTower = tower;
                GameManager.Instance.inputProcess.prevPos = tower.transform.position;
            }
        }

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
        tower.transform.position = new Vector2(pos.x + valX, Mathf.Round(pos.y));
    }

   

    private void Update()
    {
        ItowerUnitList.ForEach(x => x.managedUpdate());
    }
}