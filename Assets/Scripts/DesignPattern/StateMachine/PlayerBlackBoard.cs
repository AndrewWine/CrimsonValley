using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackBoard : EntityBlackboard
{
    [Header("State")]
    public IdleStatePlayer idlePlayer;
    public MoveState moveState;
    public SowState sowState;
    public HarvestState harvestState;
    public WaterState waterState;
    public HoeState hoeState;
    public CutState cutState;
    public MiningState miningState;
    public JumpState jumpState;


    [Header("Elements")]
    public PlayerToolSelector playerToolSelector;
    public Transform playerTransform;

    [Header("Attribute")]
    public float health = 100;
    public float maxHealth = 100;
    public float stamina = 100;
    public float maxStamina = 100;
    public float Axedamage = 1;
    public float Pickaxedamage = 1;
    public float speed = 10f;
    public float runThreshold = 0.5f;
    public float moveSpeedMultiplier = 100f;
    public float JumpForce = 100f;

    [Header("Crop & seed")]
    public ItemData seed;



    [Header("Check Variable")]
    public bool isGround = false;
    public bool sowButtonPressed = false;
    public bool harvestButtonPress = false;
    public bool waterButtonPressed = false;
    public bool hoeButtonPressed = false;
    public bool cutButtonPressed = false;
    public bool miningButtonPressed = false;
    public bool jumpButtonPressed = false;



}
