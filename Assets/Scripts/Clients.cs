using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clients : MonoBehaviour
{
    public List<Transform> stulPoints;
    public Zavod currentZavod;
    public List<NPSClient> npcClients;
    public NPSClient npcClient;
    public Transform clientPoint;

    private void Start()
    {
        StartCoroutine(SpawnClients());
    }

    IEnumerator SpawnClients()
    {
        yield return new WaitForSeconds(5);
        if (npcClients.Count < 5)
        {
            GameObject go = Instantiate(npcClient.gameObject, Vector3.zero, Quaternion.identity);
            npcClients.Add(go.GetComponent<NPSClient>());
            go.transform.SetParent(clientPoint);
            StartCoroutine(SpawnClients());
        }
    }
}
