using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkWalls : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private GameObject frontWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject backWall;
    [SerializeField] private GameObject leftWall;



    public void Configure(int configuration)
    {
        bool isFrontWallActive = IsKthBitSet(configuration, 0);
        bool isRightWallActive = IsKthBitSet(configuration, 1);
        bool isBackWallActive = IsKthBitSet(configuration, 2);
        bool isLeftWallActive = IsKthBitSet(configuration, 3);

        // Cập nhật trạng thái của các bức tường
        frontWall.SetActive(isFrontWallActive);
        rightWall.SetActive(isRightWallActive);
        backWall.SetActive(isBackWallActive);
        leftWall.SetActive(isLeftWallActive);

        // Ghi log trạng thái của các bức tường
        Debug.Log("Front Wall Active: " + isFrontWallActive);
        Debug.Log("Right Wall Active: " + isRightWallActive);
        Debug.Log("Back Wall Active: " + isBackWallActive);
        Debug.Log("Left Wall Active: " + isLeftWallActive);
    }

    public bool IsKthBitSet(int configuration, int k)
    {
        // Kiểm tra xem bit k có được bật (1) hay không
        if ((configuration & (1 << k)) > 0)
            return false; // Nếu bit k là 1, tắt tường
        else
            return true; // Nếu bit k là 0, bật tường
    }


}
