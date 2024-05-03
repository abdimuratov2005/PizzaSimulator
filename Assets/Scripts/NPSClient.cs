using DG.Tweening;
using NTC.MonoCache;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPSClient : MonoCache
{
    [Header("Base")]
    private NavMeshAgent agent;
    private Transform currentTarget;
    private Clients clients;

    public Item handItem;

    private float timer = 0;
    public float timeToBuy = 3;
    public Transform itemPoint;

    private void Awake()
    {
        clients = FindObjectOfType<Clients>();
        agent = GetComponent<NavMeshAgent>();
        currentTarget = clients.clientPoint;
    }

    protected override void Run()
    {
        #region Destination
        if (currentTarget != null)
        {
            var newPos = currentTarget.position;
            newPos.z -= clients.npcClients.Count * transform.localScale.y;
            agent.SetDestination(newPos);

            if (Vector3.Distance(newPos, transform.position) < 0.4f)
            {
                agent.isStopped = true;
            }
        }
        #endregion

        if (handItem == null)
        {
            if (agent.remainingDistance < 0.1f)
            {
                timer += Time.deltaTime;
                if (timer >= timeToBuy && clients.currentZavod.spawnedItemsList.Count > 0) {
                    handItem = clients.currentZavod.spawnedItemsList[^1];
                    handItem.FreezeRigibody();
                    handItem.SetParent(itemPoint);

                    handItem.transform
                        .DOLocalMove(Vector3.zero, 0.2f)
                        .OnComplete(() =>
                        {
                            currentTarget = clients.stulPoints[Random.Range(0, clients.stulPoints.Count)];
                        })
                        .SetLink(handItem.gameObject);

                    clients.currentZavod.spawnedItemsList.Remove(handItem);
                }
            }
            agent.SetDestination(currentTarget.position);
        }
    }
}
