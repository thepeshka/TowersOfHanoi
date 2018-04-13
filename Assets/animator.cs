using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class animator : MonoBehaviour {

    public class Point
    {
        public float x;
        public float y;
        public float z;

        public Point(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public float baseSpeed = 1f;
    public const string AT_SELECT_DISK = "selectDisk";
    public const string AT_DESELECT_DISK = "deselectDisk";
    public const string AT_MOVE_TO_TOWER = "moveToTower";
    const string TW_MOVE = "move";
    Queue<BasicTween> tweenQueue = new Queue<BasicTween>();
    static BasicTween currentTween = null;
    gameGlobal game;

    abstract class BasicTween
    {
        public abstract void Update();
    }

    class MoveTween : BasicTween
    {
        Vector3 destPoint;
        float speed;
        Transform target;
        float threshold = 0.05f;

        public MoveTween(Point destPoint, float speed, Transform target)
        {
            this.destPoint = new Vector3(destPoint.x, destPoint.y, destPoint.z);
            this.speed = speed;
            this.target = target;
        }

        public override void Update()
        {
            Vector3 direction = destPoint - target.position;
            if (direction.magnitude > threshold)
            {
                direction.Normalize();
                target.position = target.position + direction * speed * Time.deltaTime;
            }
            else
            {
                target.position = destPoint;
                if (currentTween == this)
                {
                    currentTween = null;
                }
            }
        }
    }
    // Use this for initialization
    void Start() {
        game = GameObject.Find("gameGlobal").GetComponent<gameGlobal>();
    }

    abstract class TweenData{
        
    }

    public void Animate(string animType, disk dsk = null, tower trgt = null)
    {
        Debug.Log("Animate ("+animType+","+dsk.ToString()+")");
        switch (animType)
        {
            case AT_SELECT_DISK:
                {
                    Debug.Log("AT_SELECT_DISK");
                    Transform dskTrans = dsk.GetComponent<Transform>();
                    createTween(TW_MOVE, new Point(dskTrans.position.x, 2.1f, dskTrans.position.z), baseSpeed, dskTrans);
                    break;
                }
            case AT_DESELECT_DISK:
                {
                    Debug.Log("AT_DESELECT_DISK");
                    Transform dskTrans = dsk.GetComponent<Transform>();
                    tower twr = game.GetTowerById(dsk.towerID);
                    createTween(TW_MOVE, new Point(dskTrans.position.x, (twr.diskCount - 1) * game.diskThickness, dskTrans.position.z), baseSpeed, dskTrans);
                    break;
                }
            case AT_MOVE_TO_TOWER:
                {
                    Debug.Log("AT_MOVE_TO_TOWER");
                    Transform dskTrans = dsk.GetComponent<Transform>();
                    createTween(TW_MOVE, new Point(trgt.GetComponent<Transform>().position.x, 2.1f, dskTrans.position.z), baseSpeed*2, dskTrans);
                    createTween(TW_MOVE, new Point(trgt.GetComponent<Transform>().position.x, (trgt.diskCount - 1) * game.diskThickness, dskTrans.position.z), baseSpeed*2, dskTrans);
                    break;
                }
        }
    }

    void createTween(string tweenType, Point destPoint = null, float speed = 0, Transform target = null)
    {
        switch (tweenType)
        {
            case TW_MOVE:
                {
                    tweenQueue.Enqueue(new MoveTween(destPoint, speed, target));
                    break;
                }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (tweenQueue.Count != 0 || currentTween != null)
        {
            game.blockInput(true);
            if (currentTween == null)
            {
                currentTween = tweenQueue.Dequeue();
            }
            currentTween.Update();
        } else
        {
            game.blockInput(false);
        }
    }
}
