using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts{
    public static class Path
    {
        public static readonly string prefabPath = "Prefab/";
    }

    public static class Times
    {
        public const float AirMult = .65f;
        /// <summary>��Ծ</summary>
        public const float VarJumpTime = .2f;
        public const float WallJumpForceTime = .16f;
        /// <summary>����ʱ�䣬���0.1s��������</summary>
        public const float JumpGraceTime = .1f;
        /// <summary>��ǽ��ʼʱ���ٶȼ���</summary>
        public const float ClimbGrabYMult = .2f;
        public const float ClimbNoMoveTime=.1f;
    }

}
