﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SnowDay.Diego.CharacterController;
using RootMotion.Demos;
using RootMotion.FinalIK;
using RootMotion.Dynamics;
using SnowDay.Diego.GameMode;

public class TobogganManager : ModeManager
{
    public Transform boardVisualPrefab;
    // Use this for initialization
    public BaseTeam[] Teams;

    public new IEnumerator Start ()
    {
        GameModeController controller = GameModeController.GetInstance();
        while (controller.GetLoadingStatus() == true)
        {
            yield return null;
        }

        AllPlayers = GameModeController.GetInstance().GetActivePlayers();
        if (cam != null)
        {
            cam.SetTargetPlayers(AllPlayers);
            cam.Initialize();
        }

        for (int i = 0; i < AllPlayers.Count; i++)
        {
            Debug.Log(AllPlayers[i].gameObject.name);

            Instantiate(ScriptsPrefab, AllPlayers[i].gameObject.transform.GetChild(0).GetChild(2), false);

            SnowDayCharacter s = AllPlayers[i].gameObject.GetComponentInChildren<SnowDayCharacter>();
            PuppetMaster puppet = AllPlayers[i].gameObject.GetComponentInChildren<PuppetMaster>();

            Transform boardVisual = Instantiate(boardVisualPrefab, this.transform.position, boardVisualPrefab.rotation);
            Transform board = Instantiate(ScriptsPrefab, this.transform.position, ScriptsPrefab.rotation);
           // Debug.Log("---------");
            //Debug.Log(boardVisual);
            //Debug.Log("---------");

            board.SetParent(s.transform.parent);
            s.transform.SetParent(board);
            TobbogganTestCarl carl = boardVisual.GetComponentInChildren<TobbogganTestCarl>();
            carl.animator = s.GetComponent<Animator>();
            carl.manager = AllPlayers[i].GetInputController();
            s.GetComponent<Animator>().runtimeAnimatorController = Resources.Load("Tobogganning") as RuntimeAnimatorController;
            s.GetComponent<Animator>().applyRootMotion = false;
            Rigidbody r = s.GetComponent<Rigidbody>();
            board.GetComponentInChildren<SpringJoint>().connectedBody = puppet.GetMuscle(s.GetComponent<FullBodyBipedIK>().references.spine[1]).joint.gameObject.GetComponent<Rigidbody>();
            // Debug.Log(puppet.GetMuscle(s.GetComponent<FullBodyBipedIK>().references.spine[1]).joint.gameObject.GetComponent<Rigidbody>());
            //  board.GetComponentInChildren<SpringJoint>().connectedAnchor = new Vector3(0, 0, -8);
            s.transform.localPosition = Vector3.zero;
            Destroy(s.GetComponent<CapsuleCollider>());
            Destroy(s);
            Destroy(r);
            puppet.mode = PuppetMaster.Mode.Disabled;
            //Debug.Log(board.GetComponentInChildren<PuppetBoard>());
            //board.GetComponentInChildren<PuppetBoard>().target = ;
            board.GetComponentInChildren<PuppetBoard>().Setup(boardVisual.GetComponentInChildren<ToboganVisual>().transform.GetComponent<Rigidbody>());
            //Debug.Log(board.GetComponentInChildren<PuppetBoard>().target);
        }
        // teamSplit(getTeam());


        //foreach(BaseTeam t in getTeam())
        //{
        //    for(int i = 0; i < t.players.Count; i++)
        //    {

        //    }
        //}
        //  Time.timeScale = 0;
       
     //   Debug.Break();
       // Time.timeScale = 1;
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
