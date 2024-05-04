using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Utils;

public class Clients : TransformUtils
{
    public List<Transform> stulPoints;
    public List<Transform> returnPoints;
    public Zavod currentZavod;
    public List<NPSClient> npcClientsBuyer;
    public List<NPSClient> npcClientsBought;
    public Transform clientPoint;

    public void UpdateCurrentTargets(NPSClient client, List<Item> items, Transform currentTarget, NavMeshAgent agent)
    {
        if (client.countItem > 0)
        {
            var newPos = currentTarget.position;
            newPos.z -= npcClientsBuyer.IndexOf(client) + 1;
            agent.SetDestination(newPos);

            if (Vector3.Distance(newPos, transform.position) < 0.5f)
            {
                agent.isStopped = true;
            }
            if (agent.isStopped)
            {
                /*transform.eulerAngles = Vector3.Lerp(transform.eulerAngles, Vector3.zero, 1);*/
            }
        }
        else
        {
            agent.SetDestination(currentTarget.position);
            if (agent.remainingDistance < 0.1f && !client.onTable)
            {
                var table = client.stol.GetComponentInParent<Table>();
                foreach (var handItem in items) {
                    table.items.Add(handItem);
                    handItem.SetParent(table.transform);
                    TransformItem(table.items, handItem.transform, () =>
                    {
                        handItem.transform.rotation = Quaternion.identity;
                    });
                }
                client.onTable = true;
            }
        }
    }
}
