using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts{
    public static class Path
    {
        public static readonly string prefabPath = "Prefab/";
    }

    public static class PhySet
    {
        public const float Gravity = 9.8f;
        public const float AirMult = .65f;
    }

    public static class SpdSet
    {
        public const float DuckFriction = 62.5f;

        public const float EndDashUpMult = .75f;
    }

    public static class Times
    {
        /// <summary>��Ծ</summary>
        public const float VarJumpTime = .2f;
        public const float WallJumpForceTime = .16f;
        /// <summary>����ʱ�䣬���0.1s��������</summary>
        public const float JumpGraceTime = .1f;
        /// <summary>��ǽ��ʼʱ���ٶȼ���</summary>
        public const float ClimbGrabYMult = .2f;
        public const float ClimbNoMoveTime=.1f;

        public const float DashTime = .15f;
    }

}
