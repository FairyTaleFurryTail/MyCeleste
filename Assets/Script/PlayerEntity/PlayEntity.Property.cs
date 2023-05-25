using System.Collections;
using UnityEngine;
using static Consts;

public partial class PlayerEntity: MonoBehaviour
{
    public bool onGround
    {
        get { return _onGround; }
        set
        {
            /*if (speed.y<0)
            if(value==true&& _onGround==false)
            {
                float squish = Mathf.Min(speed.y / SpdSet.FastMaxFall, 1);
                scale.x = Mathf.Lerp(1, 1.6f, squish);
                scale.y = Mathf.Lerp(1, .4f, squish);
            }*/
            _onGround = value;
        }
    }
    
}
