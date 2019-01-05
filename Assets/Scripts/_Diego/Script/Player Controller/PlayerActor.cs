﻿using UnityEngine;
using SnowDay.Diego.GameMode;
using SnowDay.Diego.CharacterController;

using RootMotion.Dynamics;
/// <summary>
/// Base Class for all player
/// Allows players to take Damage
/// </summary>
public class PlayerActor : MonoBehaviour
{
    TDMManager manager;
    public int Health = 3;

    public int TeamID;
    public bool isAlive = true;
    public void TakeDamage(int damage)
    {
        Health -= damage;
    }
    public void Start()
    {
        manager = FindObjectOfType<TDMManager>();
    }
    public void DecreaseHealth(int amout, int oppTeamID)
    {
        if (isAlive)
        {
            Health -= amout;
            if (Health <= 0)
            {
               //int oppTeamID = TeamID == 1 ? 0 : 1;
                manager.IncreaseTeamScore(1, oppTeamID);
                manager.playerDeadReport(TeamID);
                //Destroy(gameObject);
                GetComponentInParent<PlayerController>().gameObject.GetComponentInChildren<PuppetMaster>().Kill();
                isAlive = false;
            }
        }
        
    }
    private void Update()
    {
        
    }
}