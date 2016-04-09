using UnityEngine;
using System.Collections;

public class SpawnObjects : MonoBehaviour {

	public GameObject objectToSpawn;
	public float spawnDelay = 1f;
    public int enemySupply;
    public float scrollSpeed = 1f;
    protected bool startedSpawning = false;
    public float homeYPosition = 5.16f; //location for spawners to warp to once they touch the level

	// Use this for initialization
	void Start () {

        Rigidbody2D rbody = GetComponent<Rigidbody2D>();
		rbody.velocity = new Vector2(0,-1) * scrollSpeed; //This is really down
	}
	
	// Update is called once per frame
    void Update()
    {
        if (transform.position.y < 5.16f)
        {
            transform.position = new Vector2(transform.position.x, homeYPosition);
            if (!startedSpawning)
            {
                Invoke("Spawn", spawnDelay);
                startedSpawning = true;
            }
        }
    }

	protected virtual GameObject Spawn()
	{
        enemySupply--;
		GameObject obj = (GameObject)Instantiate (objectToSpawn, transform.position, transform.rotation);
        if (enemySupply > 0)
        {
            Invoke("Spawn", spawnDelay);
        }
        else
        {
            Destroy(gameObject);
        }

		return obj;
    }
}
