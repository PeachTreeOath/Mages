using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour
{

    public float speed;
    public bool isPassable; //Whether the bullet keeps going after it collides or not.
    public int type;
    public Player owner;
    public float angularVelocity = 0f;

	private Rigidbody2D body;

    // Use this for initialization
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

		transform.up = body.velocity;
        transform.Rotate(0f, 0f, 50f * angularVelocity * Time.deltaTime, Space.Self);
    }

    void OnDestroy()
    {


    }

    public void SetType(int newType)
    {
        SpriteRenderer sprite = GetComponent<SpriteRenderer>();

        switch (newType)
        {
            case 0:
                sprite.material = Resources.Load<Material>("Materials/blueMat");
                gameObject.layer = LayerMask.NameToLayer("PlayerBullets");
                type = 0;
                break;
            case 1:
                sprite.material = Resources.Load<Material>("Materials/redMat");
                gameObject.layer = LayerMask.NameToLayer("EnemyBullets");
                type = 1;
                break;
            case 2:
                sprite.material = Resources.Load<Material>("Materials/orangeMat");
                gameObject.layer = LayerMask.NameToLayer("EnemyBullets");
                type = 2;
                break;
        }
    }

    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    public void Fire()
    {
		body = GetComponent<Rigidbody2D> ();
        body.velocity = transform.up * speed;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (!isPassable)
        {
            if (type == 2)
            {
                Head playerHead = col.gameObject.GetComponent<Head>();
                if (playerHead != null && owner.Equals(col.gameObject.GetComponentInParent<Player>()))
                {
                    //This is a collision between a player and its own negative bullets.
                    //We do not want to destroy the bullet in this case.
                }
                else
                {
                    //Collided with another player, go ahead and destroy the bullet in this case."
                    Destroy(gameObject);
                }
            }
            else { 
                //Destroy on collisions with most things.
                Destroy(gameObject);
            }
        }

    }
}
