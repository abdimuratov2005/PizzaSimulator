using DG.Tweening;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using Utils;

public enum NPC_STATE
{
    Walking,
    Buying,
    SearchingChair,
    Staying,
    Waiting,
    Eating,
    Return
}

public class NPSClient : TransformUtils
{
    [Header("Base")]
    private NavMeshAgent agent;
    public Transform currentTarget;
    private Clients clients;
    public TMP_Text countItemText;
    public NPC_STATE npcState;

    public Item handItem;
    public Transform stol;

    private float timer = 0;
    public float timeToBuy = 1;
    public float timeToTable = 5;
    public Transform itemPoint;
    private bool buying = false;
    public bool onTable = false;
    public int countItem;
    public List<Item> items;

    private GameManager _gameManager;
    private int returnIndexPoint = 0;

    private void Awake()
    {
        clients = FindObjectOfType<Clients>();
        agent = GetComponent<NavMeshAgent>();
        _gameManager = FindObjectOfType<GameManager>();
        SetDefaultPoint();
        SetRandomItemCount();
        countItemText.transform.parent.gameObject.SetActive(false);
    }

    protected override void Run()
    {
        #region Destination
        if (currentTarget != null)
        {
            clients.UpdateCurrentTargets(this, items, currentTarget, agent);
        }
        #endregion

        #region Buying Item
        if (countItem > 0)
        {
            if (buying && clients.currentZavod.playerStaying)
            {
                timer += Time.deltaTime;
                if (timer >= timeToBuy && clients.currentZavod.getItemsList.Count > 0) {
                    handItem = clients.currentZavod.getItemsList[^1];
                    handItem.FreezeRigibody();
                    handItem.SetParent(itemPoint);
                    items.Add(handItem);
                    countItem -= 1;
                    _gameManager.coins += 100;
                    UpdateCountItemText();
                    handItem.transform
                        .DOLocalMove(new Vector3(0, items.Count * handItem.transform.localScale.y), 0.2f)
                        .OnComplete(() =>
                        {
                            if (countItem <= 0)
                            {
                                countItem = 0;
                                timer = 0;
                                npcState = NPC_STATE.SearchingChair;
                            }
                        })
                        .SetLink(handItem.gameObject);
                    clients.currentZavod.getItemsList.Remove(handItem);
                }
            } else
            {
                timer = 0;
            }
        }
        #endregion

        #region Searching Chair
        if (npcState == NPC_STATE.SearchingChair)
        {
            if (clients.stulPoints.Count > 0)
            {
                buying = false;
                agent.isStopped = false;
                currentTarget = SearchFreePlace();
                npcState = NPC_STATE.Eating;
                clients.npcClientsBuyer.Remove(this);
                clients.npcClientsBought.Add(this);
                clients.UpdateCurrentTargets(this, items, currentTarget, agent);
                
            }
        }
        #endregion

        #region OnTable
        if (onTable) 
        {
            var table = stol.GetComponentInParent<Table>();
            if (table.items.Count > 0)
            {
                if (table.items[^1] == null) return;

                var lastItemOnTable = table.items[^1];
                timer += Time.deltaTime;

                if (timer >= timeToTable)
                {
                    table.items.Remove(lastItemOnTable);
                    items.Remove(lastItemOnTable);
                    if (lastItemOnTable != null)
                    {
                        Destroy(lastItemOnTable.gameObject);
                    }
                    timer = 0;
                }
            } else
            {
                onTable = false;
                npcState = NPC_STATE.Return;
            }
        }
        #endregion

        #region Return
        if (npcState == NPC_STATE.Return)
        {
            currentTarget = clients.returnPoints[returnIndexPoint].transform;
            if (Vector3.Distance(currentTarget.position, transform.position) < 0.5f)
            {
                returnIndexPoint++;
                if (returnIndexPoint >= clients.returnPoints.Count)
                {
                    npcState = NPC_STATE.Walking;
                    handItem = null;
                    stol = null;
                    SetDefaultPoint();
                    clients.npcClientsBuyer.Add(this);
                    clients.npcClientsBought.Remove(this);
                    items.Clear();
                    SetRandomItemCount();
                    clients.UpdateCurrentTargets(this, items, currentTarget, agent);
                    returnIndexPoint = 0;
                }
            }
        }
        #endregion
    }

    public Transform SearchFreePlace()
    {
        Chair RandomChair()
        {
            return clients.stulPoints[Random.Range(0, clients.stulPoints.Count)].GetComponent<Chair>();
        }

        Chair randomChair;

        do
        {
            randomChair = RandomChair();
        }
        while (randomChair.busy);

        stol = randomChair.transform;
        return randomChair.transform;
    }

    public void SetRandomItemCount()
    {
        countItem = Random.Range(1, 5);
    }

    public void SetDefaultPoint()
    {
        currentTarget = clients.clientPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ClienPoint"))
        {
            buying = true;
            UpdateCountItemText();
            npcState = NPC_STATE.Buying;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ClienPoint"))
        {
            buying = false;
        }
    }

    public void UpdateCountItemText()
    {
        countItemText.transform.parent.gameObject.SetActive(true);
        countItemText.text = countItem.ToString();
        
        if (countItem <= 0)
        {
            countItemText.transform.parent.gameObject.SetActive(false);
        }
    }
}
