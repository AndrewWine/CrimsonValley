using System.Collections;
using UnityEngine;

public class Chicken : Animals
{
    protected override void Start()
    {
        float randomAngle = Random.Range(0f, 360f); // Lấy góc ngẫu nhiên từ 0 đến 360 độ
        transform.Rotate(0, randomAngle, 0); // Xoay ngẫu nhiên quanh trục Y        // 🔹 Khai báo đúng cách
        animations = new string[] { "eat", "idle", "rotate" };

        base.Start();
    }

    protected override void OnStopAnimation()
    {
        base.OnStopAnimation();
    }

 
}
