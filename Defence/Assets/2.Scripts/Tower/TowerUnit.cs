using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using Cysharp.Threading.Tasks;


public class TowerUnit : MonoBehaviour , ITowerUnit
{
    public SpriteRenderer spriteRenderer;

    public Collider2D collision2D;

    public ParticleSystem rangeParticle;
    public enum TouchSelected {catching , move , missing }

    public TouchSelected touchSelected = TouchSelected.missing;

    public bool isDown = false;

    public BungData bungData;

    ParticleSystem.MainModule particle;

    public IAttaker attaker;
    private void Awake()
    {
        particle = rangeParticle.main;
        attaker??= GetComponent<IAttaker>();

    }
    public void OnCatch()
    {
        collision2D.enabled = false;
        touchSelected = TouchSelected.catching;
        spriteRenderer.color = GameManager.Instance.catchColor;
        rangeParticle.gameObject.SetActive(true);
    }

    public void OnMissing()
    {
        collision2D.enabled = true;
        touchSelected = TouchSelected.missing;
        spriteRenderer.color = GameManager.Instance.missingColor;
        rangeParticle.gameObject.SetActive(false);
        GameManager.Instance.selectTower = null;
    //    evt.Invoke();
    }
    public void managedInitialize()
    {
        touchSelected = TouchSelected.missing;
    }

    public void bungBBangSetting(BungData data)
    {
        bungData = data;
        spriteRenderer.sprite = bungData.mySprite;
    }

    public void RangeLound()
    {
        // (반경)원형으로 공격범위 형성
        float range = (bungData.stkrange + 3.5f) + (bungData.stkrange - 2);
        particle.startSize = range;
        rangeParticle.gameObject.SetActive(false);
    }

    private float timeRate = 0;
    public void Attacked()
    {
        if (attaker .getEnemy() == null || isDown || GameManager.Instance.blockUnWay) return;
        //공격속도 
        float stkspeed =  1.5f/ bungData.stkspeed ;
        if (stkspeed <= (timeRate += Time.deltaTime))
        { 
            Vector2 enemy = attaker.getEnemy().transform.position;
        
            
            timeRate = 0f;
            var bullet =GameManager.Instance.spawner.bulletPool.Get();
            bullet.transform.position = transform.position;
            bullet.Damagedelivery(bungData,10);

            
           float angle = Mathf.Atan2(enemy.y - bullet.transform.position.y , enemy.x - bullet.transform.position.x) * Mathf.Rad2Deg;
            bullet.transform.rotation = Quaternion.Euler( 0, 0,angle);
            GameManager.Instance.bulletList.Add(bullet);
        }
    }

    async UniTask enemyDamage(EnemyUnit Getenemy , float time)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(time), DelayType.UnscaledDeltaTime);
        if (Getenemy == null) return;
        Getenemy.Health -= bungData.stkpoint;
       
    }

    public bool DragOn()
    {
        return isDown;
    }
    public void MyDestroy()
    {
        Destroy(gameObject);
    }


    

}
