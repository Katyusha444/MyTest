using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class AItactics 
{
    public int aiScore;

    public int baseScore;
    public int PutNodeScore;
    public int AroundNodeScore;
    public int occSmallScore;

    public HandBUI bui;

    public Node putNode;


    // Start is called before the first frame update
    public AItactics(HandBUI handbui,Node putNode, int score=-999)
    {
      
        
        this.putNode = putNode;
        bui = handbui;

        if (score == -999)
        {
            BaseCaculate();
        }
        else
        {
            aiScore = score;
        }
    }

    private void BaseCaculate()
    {
        baseScore += bui.bBase.atk.nowValue*1000;
        if (putNode.floorPlayer == null)
        {
            PutNodeScore += ((putNode.nodeScore.nowValue * 1000) + (bui.bBase.needOcc.nowValue * 300)) + (putNode.orgScore * 80);
        }
        else
        {
            PutNodeScore += bui.bBase.needOcc.nowValue * 100;
        }
        foreach (var item in putNode.aroundNode)
        {
           
            if (item!=null&&item.theB==null)
            {
                if (item.floorPlayer==null)
                {
                    ///占领分 加上 这个原始地块的价值分
                    AroundNodeScore += (item.nodeScore.nowValue * 1000) + (item.occScore * 60);
                }
                else
                {
                    if (item.floorPlayer != bui.pl)
                    {
                        if (item.occScore - bui.bBase.occ.nowValue <= 0)
                        {
                            AroundNodeScore += item.nodeScore.nowValue * 1000 * 2;
                        }

                    }
                }


                for (int i = 1; i <= bui.bBase.occ.nowValue; i++)
                { int vv=0;
                    if (item.floorPlayer==null)
                    {
                        vv = 200;
                    }
                    else if (item.floorPlayer!=bui.pl)
                    {
                        if (item.occScore - i >= 0)                        
                            vv = 200;
                        if (item.occScore - i < 0&& item.occScore - i >=-10)
                        {
                            vv = 200 + ((item.occScore - i) * 13);
                        }
                        else
                        {
                            vv = 50;
                        }


                    }

                    occSmallScore += vv;


                }
              
                aiScore = baseScore + occSmallScore + PutNodeScore + AroundNodeScore;


            }
        }
    }
}
