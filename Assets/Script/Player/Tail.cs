using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour
{
    private Transform tailRoot;
    [SerializeField] private List<Color> dashColors;
    private LinkedList<Transform> tails=new LinkedList<Transform>();
    private List<Color>originColors = new List<Color>();
    [SerializeField] private List<SpriteRenderer> sprites = new List<SpriteRenderer>();
    [SerializeField] private int colorSplit;
    [SerializeField] private float intensity;
    private void Awake()
    {
        tailRoot = GetComponent<Transform>();
        foreach (Transform t in tailRoot.GetComponentInChildren<Transform>())
        {
            tails.AddLast(t);
            SpriteRenderer sprite = t.GetComponent<SpriteRenderer>();
            sprites.Add(sprite);
            if(!originColors.Contains(sprite.color))
                originColors.Add(sprite.color);
        }
        originRootPosX = tails.First.Value.localPosition.x;
    }

    private float originRootPosX;

    [SerializeField]private float growUpSpeed;
    private float scale=1;
    public void UpdateShape(Vector2 tailOffset, float speed)
    {
        //测试的数据：0.04对应1.3      0.015以下对应1
        float targetScale = 1 + Mathf.Lerp(0, 0.3f, (tailOffset.SqrMagnitude()-0.015f)/ 0.025f);
        scale = Mathf.MoveTowards(scale, targetScale, growUpSpeed);
        tailRoot.transform.localScale = new Vector3(scale, scale, 1);

        LinkedListNode<Transform> nodeToFollow = tails.First;

        float sign = Mathf.Sign(tailOffset.x);
        Vector3 rootPos = nodeToFollow.Value.localPosition;
        rootPos.x = originRootPosX * sign * -1;
        nodeToFollow.Value.localPosition = rootPos;

        if (nodeToFollow != null)
            while (nodeToFollow.Next != null)
            {
                Transform tail = nodeToFollow.Next.Value;
                Vector2 targetPos = (Vector2)nodeToFollow.Value.position + tailOffset;
                tail.position = Vector2.MoveTowards((Vector2)tail.position, targetPos, speed);
                nodeToFollow = nodeToFollow.Next;
            }
    }

    public void UpdateColor(int index)
    {
        Color c1, c2;
        Color lc1,lc2;
        lc1 = lc2 = Color.white;
        //bool light = false;
        if (index<0)
        {
            c1 = originColors[0];
            c2 = originColors[1];
        }
        else
        {
            c1 = dashColors[index];
            c2 = dashColors[index + 1];
            lc1 = Calc.GetLightColor(c1, intensity);
            lc2 = Calc.GetLightColor(c2, intensity);
            //light = true;
        }
        for (int i = 0; i < colorSplit; i++)
        {
            sprites[i].color = c1;
            sprites[i].material.SetColor("_Color", lc1);
        }
        for (int i = colorSplit; i < sprites.Count; i++)
        {
            sprites[i].color = c2;
            sprites[i].material.SetColor("_Color", lc2);
        }
            
    }

}
