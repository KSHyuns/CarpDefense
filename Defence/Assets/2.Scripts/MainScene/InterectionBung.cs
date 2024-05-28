using Cysharp.Threading.Tasks;
using DG.Tweening;
using DG.Tweening.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterectionBung : MonoBehaviour
{
    private Sequence mySequence;
    private Sequence moveSequence;

    public SpriteRenderer spriteRenderer;

    public MainSceneProcess mainSceneProcess;

    private float xPos = 3.5f;
    private float yPos = 6.5f;


    private void OnDisable ()
    {
        moveSequence?.Kill(transform);
    }


    public void OnMouseDown()
    {
        mySequence?.Rewind(); //Ãß°¡
        mySequence = DOTween.Sequence();
        mySequence.OnStart(() => { });
        mySequence.Append(transform.DOScale(1.2f, 0.2f));
        mySequence.Join(transform.DORotate(new Vector3(Random.Range(-1f , 1f), 0, Random.Range(-1f, 1f)*20f) , 0.2f));
        mySequence.Append(transform.DOScale(1f, 0.2f)).SetEase(Ease.OutBounce);
        mySequence.Join(transform.DORotate(new Vector3(0, 0, 0f), 0.2f));
    }


    public void MoveInterectionBung()
    {
        Vector3 rPos= new Vector3(Random.Range(-xPos, xPos), Random.Range(-yPos, yPos), 1);
        spriteRenderer.flipX = ( rPos.x >= transform.position.x ) ? true : false;
        float dis = (rPos - transform.position).magnitude;

        //mySequence?.Rewind();
        moveSequence = DOTween.Sequence();
        moveSequence.SetAutoKill(true);
        moveSequence.OnStart(onStart);
        moveSequence.Append(transform.DOMove(rPos , dis*0.7f));
        moveSequence.OnComplete(() => { MoveInterectionBung(); });

    }

    

    private void onStart() { }

}
