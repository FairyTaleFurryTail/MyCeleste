using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CoroutineManager:Singleton<CoroutineManager>
{
    public LinkedList<StateCoroutine> coroutines=new LinkedList<StateCoroutine>();
    public LinkedList<StateCoroutine> closeingCors = new LinkedList<StateCoroutine>();

    public void StartCoroutine(IEnumerator enumerator)
    {
        coroutines.AddLast(new StateCoroutine(enumerator));
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