using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour {

	public GameObject objectToSpawn;
	public float spawnDelay = 1f;
    public int enemySupply;
    public float scrollSpeed;
    private bool startSpawning = false;

	// Use this for initialization
	void Start () {
        if (enemySupply > 0)
        {
            enemySupply--;
            Invoke("Spawn", spawnDelay);
        }
        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
        rbody.velocity = transform.up * scrollSpeed; //This is really down
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void LateUpdate()
    {
        if (transform.position.y < 5.16f)
        {
            transform.position = new Vector2(transform.position.x, 5.16f);
            startSpawning = true;
        }
    }

    private void Spawn()
	{
        enemySupply--;
		Instantiate (objectToSpawn, transform.position, transform.rotation);
        if (enemySupply > 0 && startSpawning)
        {
            Invoke("Spawn", spawnDelay);
        }
        else
        {
            Destroy(gameObject);
        }

    }
}
