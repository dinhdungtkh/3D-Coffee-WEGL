using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotGroupController : MonoBehaviour
{
    public BotMove[] BotList;
    public int BotCount;
   
    private void Start()
    {
        BotCount = BotList.Length;
    }
    private void Update()
    {
      for ( int i = 0; i < BotCount - 1; i++)
        {
            if (BotList[i].targetChair == BotList[i+1].targetChair) {
                BotList[i].targetChair = BotList[i].FindTargetChair();
            } 
        }
    }
}
