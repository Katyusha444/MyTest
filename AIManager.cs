using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AIManager : MonoSingleton<AIManager>
{
    public bool 干涉 = false;
    public string AI干涉;

    public Player nowPl;

    public List<Node> dangerList = new List<Node>();

    public List<Node> targetNodeList = new List<Node>();

  //  [SerializeField]
    public  List<AItactics> allAITactics = new List<AItactics>();

    public NodeMap nodeMap;

    //=============================节约计算=================================\
  //  public bool random=true;


   
    public AItactics bestAI;
    public void Think()
    {
        nowPl= ControlManager.instance.nowControl;
        nodeMap = BattleManager.instance.battleNodeMap;


        allAITactics.Clear();
        bestAI = null;

        foreach (var card in nowPl.userHand.handCards)
        {
          //  var v = ;
           allAITactics.AddRange(OneCardThink(card));
            
        }
      //  List<AItactics> all = new List<AItactics>();
      //  foreach (var item in allAITactics)
      //  {
      //      all.AddRange(item);
      //  }
        AItactics[] array = allAITactics.ToArray();
        if (array.Length==0)
        {
            Debug.LogError("没有策略，这里直接结束这个玩家的所有后续可行动能力  不太严谨");
            
            nowPl.over=true;
            ControlManager.instance.ChangeControl();

        }
        else
        {
            ControlManager.instance.ordering = true;

                bestAI = ArrayHelper.Max(array, new ArrayHelper.SelectHandler<AItactics, int>(p => p.aiScore));

            StartCoroutine(StartAI());
        }


        
    }
    private float waitTime =0.5f;
    IEnumerator StartAI()
    {
        yield return new WaitForSeconds(waitTime);
        ControlManager.instance.nowhandBUI = bestAI.bui;

        bestAI.putNode.WindowEffect(bestAI.bui);

    }

    //============================================================================================

    private List<AItactics> OneCardThink(HandBUI card)
    {
       // card.AIs.Clear();
        List<AItactics> tempAIs = new List<AItactics>();

      var l=  nodeMap.CheckCanUse(card.bBase);

        foreach (var item in l)
        {
           tempAIs.Add(new AItactics(card, item));
        }
        
        return tempAIs;
    }

    public List<Node> FindPath(Node node,Node tar)
    {
        findTar = false;
        List<Node> list=new List<Node>();


        GetOneNodePath(node, tar, list);

        return list;
    }

    bool findTar=false;
    private void GetOneNodePath(Node node, Node tar, List<Node> list)
    {
        float min = 999;
        Node theNode = null;
        if (node==tar)
        {
            findTar=true;
            list.Add(node);
            return;
        }

        foreach (var item in node.aroundNode)
        {
            if (item == null)
                continue;
            if (item==tar)
            {
                findTar = true;
                theNode = item;
                break;
            }
           
            var n = Vector3.Distance(item.transform.position, tar.transform.position);
            if (n < min)
            {
                min = n;
                theNode = item;
            }
        }

        list.Add(theNode);
        if (!findTar)
        {
            GetOneNodePath(theNode, tar, list);
        }


    }
    
}
