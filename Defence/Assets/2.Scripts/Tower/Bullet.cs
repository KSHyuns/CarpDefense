using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Pool;

public class Bullet : MonoBehaviour , IBullet
{
    public IObjectPool<Bullet> Ipool;

    public BungData bungData;

    private float speed;

    private void OnEnable()
    {
        deathDelay = 0f;
    }

    public void Damagedelivery(BungData data , float speed)
    {
        bungData = data;
        this.speed = speed;
    }

    private float deathDelay = 0f;
    public void OnUpdate()
    {
       transform.Translate(Vector3.right * Time.deltaTime * speed);

        if( 3 < (deathDelay +=Time.deltaTime) ) 
        {
            Release();
        }

        var cols = Physics2D.OverlapCircle(transform.position, 0.01f, 1 << LayerMask.NameToLayer("Enemy"));

        if( cols != null ) 
        {
            if(cols.TryGetComponent(out IEnemyUnit enemy))
            {
                (enemy as EnemyUnit).Health -= bungData.stkpoint;
                //(enemy as EnemyUnit).transform.GetChild(0)
                GameDataBase.Instance.Shake((enemy as EnemyUnit).transform, 0.1f, 0.05f).Forget();

                SoundMGR.Instance.SoundPlay(audioName.hit);
                Release();
            }
        }


    }

    public void Release()
    {
        GameManager.Instance.bulletList.Remove(this);
        Ipool.Release(this);
    }
}
