﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using System.Collections.Generic;
using System.Collections;
using SnowDay.Diego.GameMode;

public class BallSpawnerModifiedDC : MonoBehaviour
{
    //private Vector3 offset = new Vector3(1, 0, 1);
    //public GameObject prefab;
    //public GameObject spawnPoint;
    public ProjectileLauncher projectileLauncher;
    public PlayerActor mySelf;
   // public float LaunchVelocity;
    //public float throwDelay;
    private float timeFired;
    public PierInputManager.ButtonName ShootButton = PierInputManager.ButtonName.X;
    private PierInputManager playerInputController;
    public Animator animator;
    //SnowPick Up
    //   private bool BallPickedUp;
    //public FullBodyBipedIK IK;
    //private bool IsReaching;
    //public float lerpTime = 1f;
    //   float currentLerpTime;
    //public Transform ReachPoint;
    //   private bool DoneReaching;

    //correct within x degrees
    public float autoAimAngle = 25.0f;



    // Use this for initialization
    void Start ()
    {
        playerInputController = gameObject.GetComponentInParent<PierInputManager>();
        animator = GetComponentInParent<Animator>();
       // IK = gameObject.GetComponentInParent<FullBodyBipedIK>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (playerInputController.GetButtonDown(ShootButton))
        {
            if(projectileLauncher.CurrentplayerAmmo > 0)
            {
                animator.SetTrigger("ThrowHigh");

            }



            //if (!BallPickedUp)
            //{
            //   // print("Hit A Picking Up");
            //    PickUpBall();
            //}
            ////spawns in ball at spawn point and launches ball in faced direction
            //else
            //{
            //    print("Hit A Throwing");

            //}

            //timeFired = Time.time;

        }

  //      if (IsReaching==true)
  //      {
		//	print("LerpingIN");
		//	currentLerpTime += Time.deltaTime;
		//	if (currentLerpTime > lerpTime)
  //          {
  //              currentLerpTime = lerpTime;
  //          }

  //          float t = currentLerpTime / lerpTime;
  //          IK.solver.rightHandEffector.target = ReachPoint;
  //          IK.solver.rightHandEffector.positionWeight = Mathf.Lerp(IK.solver.rightHandEffector.positionWeight, 1.0f, t);

		//	if(IK.solver.rightHandEffector.positionWeight==1)

		//	{
		//		DoneReaching = true;
		//		IsReaching = false;

		//		print("Reached Ground");

		//	}

		//}


		//if(DoneReaching==true)
		//{
		//	print("Lerp Out");
		//	currentLerpTime = 0f;
		//	currentLerpTime += Time.deltaTime;
  //          if (currentLerpTime > lerpTime)
  //          {
  //              currentLerpTime = lerpTime;
  //          }

  //          float t = currentLerpTime / lerpTime;
            
  //          IK.solver.rightHandEffector.positionWeight = Mathf.Lerp(IK.solver.rightHandEffector.positionWeight, 0f, t);




  //          if (IK.solver.rightHandEffector.positionWeight <= 0.1)
		//	{
		//		print("PICKED UP");
		//		IK.solver.rightHandEffector.target = null;
		//		BallPickedUp = true;
		//		DoneReaching = false;

				
		//	}
		//}	   
    }



    //private void PickUpBall()
    //{
    //	IsReaching = true;
    //}
    public void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Vector3 tempPos = transform.position;
        tempPos += Vector3.up;
        Gizmos.DrawLine(tempPos, tempPos + transform.forward * 5);

        Gizmos.DrawLine(tempPos, (tempPos + (Quaternion.AngleAxis(autoAimAngle, Vector3.up) * transform.forward) * 5));
        Gizmos.DrawLine(tempPos,  (tempPos + (Quaternion.AngleAxis(-autoAimAngle, Vector3.up) *transform.forward )* 5));

        
        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(projectileLauncher.transform.position, projectileLauncher.transform.position + projectileLauncher.transform.forward * 5);

    }
    //now called from SnoDayCharacter.cs line 63
    public void ThrowBall()
    {


        //get reference to all players
        var AllPlayers = GamemodeManagerBase.Instance.Players;
           

        Debug.Log(AllPlayers.Count);

        for (int i = 0; i < AllPlayers.Count; i++)
        {
            if(AllPlayers[i].GetComponentInChildren<PlayerActor>().TeamID != mySelf.TeamID)
            {
                Debug.Log("check enemies");
                float angleBetweenPlayers = Vector3.Angle((AllPlayers[i].GetComponentInChildren<PlayerActor>().transform.position) - mySelf.transform.position, mySelf.transform.forward);
                //Debug.Log(AllPlayers[i].name);

                //   debug.log("angle between players: " + anglebetweenplayers);
                if (angleBetweenPlayers < autoAimAngle)
                {

                    // defaultshot.angle = anglebetweenplayers; 
                    Debug.Log("aim corrected " + angleBetweenPlayers);
                    Vector3 temp = mySelf.transform.InverseTransformPoint(AllPlayers[i].GetComponentInChildren<PlayerActor>().transform.position);
                    if(temp.x > 0)
                    {
                        projectileLauncher.transform.localRotation = Quaternion.Euler(0, angleBetweenPlayers, 0);

                    }
                    else
                    {
                        projectileLauncher.transform.localRotation = Quaternion.Euler(0, -angleBetweenPlayers, 0);

                    }
                }
                else
                {
                    projectileLauncher.transform.localRotation = Quaternion.Euler(0, 0, 0);
                    Debug.Log("standard aim " + angleBetweenPlayers);
                }
            }
        }

        projectileLauncher.LaunchProjectile(mySelf);
       // BallPickedUp = false;
    }



}
