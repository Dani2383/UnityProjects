using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiDeathState : AiState
{
    public AiStateId GetId()
    {
        return AiStateId.Death;
    }
    public void Enter(AiAgent agent)
    {
        agent.GetComponent<PathfindingA>().chasing = false;
        if(!agent.isDead){
            agent.animator.SetTrigger("Dead");
            if(agent.transform.name == "Skeleton") AudioManager.instance.PlaySound("Death Skeleton");
            else if (agent.transform.name == "Boss") AudioManager.instance.PlaySound("Death Boss");
            else AudioManager.instance.PlaySound("Death Mushroom");

            agent.transform.GetChild(agent.transform.tag == "Enemy" ? 3: 2).gameObject.SetActive(false);
            ItemAssets.Instance.DropRandomItem(agent.transform.position);
            if(agent.transform.tag == "Boss") {
                GameObject.FindGameObjectWithTag("UI").GetComponent<VictoryMenu>().YouWin();
            }
            agent.Die();
        }
    }

    public void Exit(AiAgent agent)
    {
        
    }


    public void Update(AiAgent agent)
    {
        
    }

}
