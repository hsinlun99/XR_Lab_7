using Unity.Netcode;
using UnityEngine;

public class TowerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject jengaBlockPrefab;
    [SerializeField] private Vector3 startingPosition = new Vector3(0, 1.05f, 0);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnNetworkSpawn()
    {
        if (!IsServer) 
        {
            return;
        }

        GameObject newBlock = Instantiate(jengaBlockPrefab, startingPosition, Quaternion.identity);
        NetworkObject nObj = newBlock.GetComponent<NetworkObject>();
        nObj.Spawn();
    }
}
