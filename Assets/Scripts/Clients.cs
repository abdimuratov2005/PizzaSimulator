using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
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

    public List<NPSClient> allClients;
    public int clientIndex = 0;
    public NavMeshSurface meshSurface;

    private void Start()
    {
        StartCoroutine(SpawnAllClients());
    }

    IEnumerator SpawnAllClients()
    {
        if (clientIndex < allClients.Count)
        {
            allClients[clientIndex].gameObject.SetActive(true);
            yield return new WaitForSeconds(3);
            clientIndex++;
            StartCoroutine(SpawnAllClients());
        } else
        {
            StopCoroutine(SpawnAllClients());
        }
    }

    public void UpdateCurrentTargets(NPSClient client, List<Item> items, Transform currentTarget, NavMeshAgent agent)
    {
        if (client.countItem > 0)
        {
            var newPos = currentTarget.position;
            newPos.z -= npcClientsBuyer.IndexOf(client);
            agent.SetDestination(newPos);

            if (Vector3.Distance(newPos, client.transform.position) < 0.5f)
            {
                agent.isStopped = true;
                client.transform.eulerAngles = Vector3.Lerp(client.transform.eulerAngles, Vector3.zero, 1);
            }
        }
        else
        {
            agent.SetDestination(currentTarget.position);
            if (Vector3.Distance(currentTarget.position, client.transform.position) < 1f && !client.onTable)
            {
                if (client.stol == null) return;
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
