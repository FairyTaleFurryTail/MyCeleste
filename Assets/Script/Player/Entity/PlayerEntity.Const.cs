using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class PlayerEntity : MonoBehaviour
{
    public static class Path
    {
        public static readonly string prefabPath = "Prefab/";
    }


    public static class PosSet
    {
        public static readonly Vector3 foot = new Vector3(0, -0.6f,0);
        public static readonly Vector3 front = new Vector3(0.45f , 0, 0);
    }

    public static class TailMove
    {
        public static readonly Vector2 
            idleOffset=new Vector2(0.05f,0.04f), runOffset=new Vector2(0.1f, 0.015f), 
            jumpUpOffset=new Vector2(0.001f, -0.1f), jumpDownOffset=new Vector2(0.001f, 0.15f), 
            duckOffset= new Vector2(0.03f, 0.07f), dashOffset=new Vector2(-0.2f, 0), 
            climbUpOffset=new Vector2(0.01f, -0.05f), climbDownOffset=new Vector2(0.025f, 0.08f);
        public static readonly float 
            idleSpeed = 0.01f, runSpeed = 0.012f,
            jumpUpSpeed = 0.015f, jumpDownSpeed = 0.015f, 
            duckSpeed = 0.012f, dashSpeed = 0.04f, 
            climbUpSpeed = 0.01f, climbDownSpeed = 0.01f;
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
        public const int DashCornerCorrection = 5;
        public const float CorrectXStep = 0.12f;
        public const float CorrectYStep = 0.09f;
    }

    public static class SpdSet
    {
        public const float DuckFriction = 62.5f;

        public const float DodgeSlideSpeedMult = 1.2f;
        public const float EndDashUpMult = 0.75f;

        public const float AirMult = 0.65f;
        public const float MaxFall= -16f;
        public const float FastMaxFall = -24f;
        public const float WallSlideStartMax = -2f;
        public const float FastMaxAccel = 30f;

        public const float ClimbHopX = 11;
        public const float ClimbHopY = 12;
        public const float ClimbAccel = 90;
        public const float ClimbUpSpeed = 4.5f;
        public const float ClimbDownSpeed = -8;

        public const float MinLaunchSpeed = 180;
    }

    public static class TimeSet
    {
        /// <summary>跳跃</summary>
        public const float VarJumpTime = 0.2f;
        public const float SuperWallJumpVarTime = 0.25f;
        public const float WallJumpForceTime = 0.16f;
        public const float JumpGraceTime = 0.1f;//土狼时间，离地0.1s还可以跳
        /// <summary>爬墙开始时的速度减少</summary>
        public const float ClimbGrabYMult = 0.2f;
        public const float ClimbNoMoveTime=.1f;
        public const float ClimbHopForceTime = 0.2f;
        public const float WallSlideTime = 1.5f;

        public const float DashAttackTime=0.3f;
        public const float DashEffectTime=0.2f;
        public const float DashTime = .15f;
        public const float DashCooldown = 0.2f;

        public const float FlashInterval = 0.05f;

        public const float launchTime = 1f;
    }

}
