using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnner : MonoBehaviour
{
    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        /*foreach(Transform t in transform)
        {
            Instantiate(enemyPrefab, t.position, Quaternion.identity, transform);
        }*/
        for(int i = 0; i < 5; i++)
        {
            Instantiate(enemyPrefab, new Vector3(Random.Range(0, 50), 0, Random.Range(0, 50)), Quaternion.identity, transform);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
