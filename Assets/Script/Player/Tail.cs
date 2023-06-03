using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private Transform tailRoot;
    [SerializeField] private List<Color> transColors;
     private LinkedList<Transform> tails=new LinkedList<Transform>();
    private List<Color>originColors = new List<Color>();
    [SerializeField]private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    private void Awake()
    {
        tailRoot = GetComponent<Transform>();
        foreach (Transform t in tailRoot.GetComponentInChildren<Transform>())
        {
            tails.AddLast(t);
            SpriteRenderer sprite = t.GetComponent<SpriteRenderer>();
            sprites.Add(sprite);
            originColors.Add(sprite.color);
        }
        originRootPosX = tails.First.Value.localPosition.x;
    }

    private float originRootPosX;

    public void UpdateShape(Vector2 partOffset, float speed)
    {
        LinkedListNode<Transform> nodeToFollow = tails.First;

        float sign = Mathf.Sign(partOffset.x);
        Vector3 rootPos = nodeToFollow.Value.localPosition;
        rootPos.x = originRootPosX * sign * -1;
        nodeToFollow.Value.localPosition = rootPos;

        if (nodeToFollow != null)
            while (nodeToFollow.Next != null)
            {
                Transform tail = nodeToFollow.Next.Value;
                Vector2 targetPos = (Vector2)nodeToFollow.Value.position + partOffset;
                tail.position = Vector2.MoveTowards((Vector2)tail.position, targetPos, speed);
                nodeToFollow = nodeToFollow.Next;
            }
    }

}
