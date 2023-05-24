using System.Collections;
using System.Collections.Generic;
using UnityEngine;



///这里手工实现协程的目的是为了执行顺序的保证，而且它只会在状态机使用
public class CoroutineManager
{
    public LinkedList<StateCoroutine> coroutines=new LinkedList<StateCoroutine>();
    public LinkedList<StateCoroutine> closeingCors = new LinkedList<StateCoroutine>();
    
    public StateCoroutine StartCoroutine(IEnumerator enumerator)
    {
        StateCoroutine coroutine = new StateCoroutine(enumerator);
        coroutines.AddLast(coroutine);
        return coroutine;
    }
    public void CloseCoroutine(StateCoroutine coroutine)
    {
        closeingCors.AddLast(coroutine);
    }

    public void Update()
    {
        var node = coroutines.First;
        while (node != null)
        {
            var a = node.Value;
            bool res = false;
            if (a != null)
            {
                bool closing=closeingCors.Contains(a);
                if (closing)
                    closeingCors.Remove(a);
                else
                    res=a.MoveNext();
            }
            if (!res)
                coroutines.Remove(node);
            node= node.Next;
        }
    }


}