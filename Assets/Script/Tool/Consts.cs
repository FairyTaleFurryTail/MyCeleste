using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts{
    public static class Path
    {
        public static readonly string prefabPath = "Prefab/";
    }

    public static class PlaySet
    {
        public const float ClimbMaxStamina=110;
    }

    public static class ColSet
    {
        public const float WallJumpCheckDist = .6f;
        public const float OffsetDistance = 0.02f;
    }

    public static class PhySet
    {
        //public const float Gravity = 9.8f;
        public const float AirMult = .65f;
        
    }

    public static class SpdSet
    {
        public const float DuckFriction = 62.5f;

        public const float EndDashUpMult = .75f;

        public const float MaxFall= -16f;
        public const float FastMaxFall = -24f;
        public const float WallSlideStartMax = -2f;
        public const float FastMaxAccel = 30f;

        public const float ClimbHopX = 10;
        public const float ClimbHopY = 12;
    }

    public static class Times
    {
        /// <summary>跳跃</summary>
        public const float VarJumpTime = .2f;
        public const float SuperWallJumpVarTime = .25f;
        public const float WallJumpForceTime = .16f;
        public const float JumpGraceTime = .1f;//土狼时间，离地0.1s还可以跳
        /// <summary>爬墙开始时的速度减少</summary>
        public const float ClimbGrabYMult = .2f;
        public const float ClimbNoMoveTime=.1f;
        public const float ClimbHopForceTime = .2f;
        public const float WallSlideTime = 1.5f;

        public const float DashAttackTime=.3f;
        public const float DashTime = .15f;
    }

}
