using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerEntity : MonoBehaviour
{
    public static class Path
    {
        public static readonly string prefabPath = "Prefab/";
    }

    public static class ClimbSet
    {
        public const float ClimbMaxStamina=110;
        public const float ClimbTiredThreshold = 20;
        public const float ClimbUpCost = 100 / 2.2f;
        public const float ClimbStillCost = 100 / 10f;
        public const float ClimbJumpCost = ClimbMaxStamina / 4;
    }

    public static class ColSet
    {
        public const float WallJumpCheckDist = .6f;
        public const float OffsetDistance = 0.02f;
    }

    public static class SpdSet
    {
        public const float DuckFriction = 62.5f;

        public const float EndDashUpMult = .75f;

        public const float AirMult = .65f;
        public const float MaxFall= -16f;
        public const float FastMaxFall = -24f;
        public const float WallSlideStartMax = -2f;
        public const float FastMaxAccel = 30f;

        public const float ClimbHopX = 11;
        public const float ClimbHopY = 12;
        public const float ClimbAccel = 90;
        public const float ClimbUpSpeed = 4.5f;
        public const float ClimbDownSpeed = -8;
    }

    public static class TimeSet
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

        public const float FlashInterval = .05f;
    }

}
