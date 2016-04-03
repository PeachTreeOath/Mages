using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{
	
	public float speed;
    public bool isPassable; //Whether the bullet keeps going after it collides or not.
    public int type;

	// Use this for initialization
	void Start ()
	{

	}

	// Update is called once per frame
	void Update ()
	{

	}

    void OnDestroy()
    {

        
    }

	public void SetType (int newType)
	{
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();

		switch (newType) {
		case 0:
			sprite.material = Resources.Load<Material> ("Materials/blueMat");
			gameObject.layer = LayerMask.NameToLayer ("PlayerBullets");
            type = 0;
			break;
		case 1:
			sprite.material = Resources.Load<Material> ("Materials/redMat");
			gameObject.layer = LayerMask.NameToLayer ("EnemyBullets");
            type = 1;
			break;
		case 2:
			sprite.material = Resources.Load<Material> ("Materials/yellowMat");
			gameObject.layer = LayerMask.NameToLayer ("EnemyBullets");
            type = 2;
			break;
		}
	}

	public void SetSpeed (float newSpeed)
	{
		speed = newSpeed;
	}

	public void Fire ()
	{
		GetComponent<Rigidbody2D> ().velocity = transform.up * speed;
	}

    public void OnTriggerEnter2D(Collider2D col)
    {
        string name = col.gameObject.name;
        Enemy enemy = col.gameObject.GetComponent<Enemy>();
        Head playerHead = col.gameObject.GetComponent<Head>();

        switch (type)
        {
            case 0:
                //Friendly bullets
                
                if (enemy != null && !isPassable) 
                {
                    Destroy(gameObject);
                }
                break;
            case 1:
            case 2:
                //same case for 1 and 2;

                //Enemy bullets or pvp bullets
                if (playerHead != null)
                {
                    col.gameObject.GetComponent<Head>().Die();
                }

                break;
        }

    }
}
