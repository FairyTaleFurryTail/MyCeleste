using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private RectTransform black;
    [SerializeField] private float stY = -1472,edY= 1724;
    [SerializeField] private float blackSpeed;
    public IEnumerator SwitchScreen()
    {
        float y = stY;
        while(y!=edY)
        {
            y = Mathf.MoveTowards(y, edY, blackSpeed * Time.unscaledDeltaTime);
            black.anchoredPosition=new Vector3(0, y, 0);
            yield return null;
        }
        black.anchoredPosition = new Vector3(0, y, 0);
    }

}
