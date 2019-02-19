﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityStandardAssets.Characters.ThirdPerson;

using SnowDay.Diego.CharacterController;



public class MoveModifier : MonoBehaviour {

    public enum MoveModes { Slow, Stop, Reset };
    private float OriginalMoveSpeed;
    private float OriginalTurnSpeed;
    private float OriginalStationTurn;
    public float SpeedOffset = 0.3f;
    public float TurnSpeedOffset=.045f;
    //refrences
    private SnowDayCharacter Player;
    

    // Use this for initialization
    void Start () {
    
        Player = GetComponentInParent<SnowDayCharacter>();
        Player.RunEnabled = false;
        //get charecter original move stats on start
        OriginalMoveSpeed = Player.m_MoveSpeedMultiplier;
        OriginalTurnSpeed = Player.m_MovingTurnSpeed;
        OriginalStationTurn = Player.m_StationaryTurnSpeed;


    }
	

    public void ModifyMovement(MoveModes mode , float WeightPercent)
    {
        switch (mode)
        {
            case MoveModes.Slow:
                //subtracts movement speed based on the percent the shovel is filled
                Player.m_MoveSpeedMultiplier = OriginalMoveSpeed-((WeightPercent - SpeedOffset) * OriginalMoveSpeed);
               
                //TurnSpeedModification
                Player.m_MovingTurnSpeed = OriginalTurnSpeed - ((WeightPercent - TurnSpeedOffset) * OriginalTurnSpeed);
                Player.m_StationaryTurnSpeed= OriginalStationTurn - ((WeightPercent - TurnSpeedOffset) * OriginalStationTurn);
                

                break;

            case MoveModes.Stop:
               Player. m_MoveSpeedMultiplier = 0;
               Player.m_StationaryTurnSpeed = 0;
               Player.m_MovingTurnSpeed = 0;
                break;

            case MoveModes.Reset:
                Player.m_MoveSpeedMultiplier = OriginalMoveSpeed;
                Player.m_StationaryTurnSpeed = OriginalStationTurn;
                Player.m_MovingTurnSpeed = OriginalTurnSpeed;
                break;
        }
    }


}
