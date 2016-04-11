using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Use this class to specify weapon properties.  Usually combined on an object with the Weapon script.
public class WeaponProps : MonoBehaviour {

    [SerializeField]
    public GameObject bulletObj; //template for projectile sprite
    private Bullet bullet;
    [SerializeField]
    public float speed;
    [SerializeField]
    public float shotDelay;
    [SerializeField]
    public int numberOfBullets;
    [SerializeField]
    public int type;
    [SerializeField]
    public bool isPassable;
    [SerializeField]
    public bool useParentVelocity;
    [SerializeField]
    public float burstTime;
    [SerializeField]
    public float burstDelay;
    [SerializeField]
    public float perturbation;

    protected Rigidbody2D body;
    protected bool readyToShoot;
    private float burstStartTime;

    protected Weapon weaponInst;

    // Use this for initialization
    void Start() {
        //In general you do not want to put startup code here...put it in startInit();

        weaponInst = gameObject.AddComponent<Weapon>();
    }

    // Update is called once per frame
    void Update() {
        //In general you do not want to put update code here.  Put it in doUpdate()	
    }

    //Called once to initialize this weapon
    public virtual void startInit(Weapon weaponInstance) {
        Debug.Log("Weapon init called on " + gameObject.name);
        weaponInst = weaponInstance;
        if (useParentVelocity) {
            body = gameObject.GetComponentInParent<Rigidbody2D>();
        }
        bullet = bulletObj.GetComponent<Bullet>();
        ResetReadyToShoot();
    }

    //Called on update from parent object
    public virtual void doUpdate() {
        if (readyToShoot) {
            readyToShoot = false;
            sendFireCommand();
            Invoke("ResetReadyToShoot", burstDelay);
        }
    }

    void ResetReadyToShoot() {
        readyToShoot = true;
    }

    //Fire commands need to execute in a server context.  This means a NetworkBehaviour object must do the calling.
    protected void sendFireCommand() {
        weaponInst.CmdFire();
    }

    //called in server context, returns objects the server should spawn
    public virtual List<GameObject> doFireCallback() {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < numberOfBullets; i++) {

            Vector3 bulletPosition = transform.position;

            Quaternion bulletRotation = Quaternion.identity;

            GameObject bulletgo = ((GameObject)Instantiate(bulletObj, bulletPosition, bulletRotation));
            Bullet bullet = bulletgo.GetComponent<Bullet>();
            bullet.SetType(type);
            bullet.owner = GetComponentInParent<Player>();
            bullet.isPassable = isPassable;
            bullet.SetSpeed(speed, 0, 0);

            //bullet.transform.parent = transform;
            bullet.Fire();

            if (useParentVelocity) {
                bullet.GetComponent<Rigidbody2D>().velocity += body.velocity;
            }

            objs.Add(bulletgo);

        }
        return objs;

    }

}