using Unity.Netcode;
using UnityEngine;

public class TowerSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject jengaBlockPrefab;
    [SerializeField] private Vector3 startingPosition = new Vector3(0, 1.05f, 0);

    [SerializeField] private int numberOfLayers = 6;

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

        for (int layer = 0; layer < numberOfLayers; layer++)

        {

            //Check if we are on an odd layer

            bool isOddLayer = layer % 2 == 1;

            //Calculate the Y position for the new Jenga blocks on this layer 

            //based on the starting position, the height of the block and the layer number

            float newBlockHeight = startingPosition.y + layer * jengaBlockPrefab.gameObject.transform.localScale.y;

            // On this layer, Loop over 3 times and Spawn 3 blocks

            // Center them so that the middle block is at 0

            // and the left/right blocks are at ±blockWidth.

            for (int i = 0; i < 3; i++)

            {

                Vector3 newBlockPosition;

                Quaternion newBlockRotation;

                // Position and rotate blocks based on if odd or even layer 

                //If even layer is oriented along the X axis, use Z offset

                if (!isOddLayer)

                {

                    float zOffset = (1 - i) * jengaBlockPrefab.gameObject.transform.localScale.z;

                    newBlockPosition = new Vector3(startingPosition.x, newBlockHeight, startingPosition.z + zOffset);

                    newBlockRotation = Quaternion.Euler(0, 0, 0);

                }

                //else if odd layer and riented along the Z axis, use X offset

                else

                {

                    float xOffset = (1 - i) * jengaBlockPrefab.gameObject.transform.localScale.z;

                    newBlockPosition = new Vector3(startingPosition.x + xOffset, newBlockHeight, startingPosition.z);

                    newBlockRotation = Quaternion.Euler(0, 90, 0);

                }

                // Instantiate new Jenga block using position and rotation 

                //calculated above

                GameObject newBlock = Instantiate(jengaBlockPrefab, newBlockPosition, newBlockRotation);

                //Grab new Jenga block's NetworkObject and spawn it

                NetworkObject nObj = newBlock.GetComponent<NetworkObject>();

                nObj.Spawn();

            }

        }
    }
}
