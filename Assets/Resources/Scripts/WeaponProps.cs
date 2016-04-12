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
    [SerializeField]
    public float clockAngle; //0 is tranform.up, values go clockwise to 360 probably
    [SerializeField]
    public int ammoCount = -1; //-1 is infinite ammo

    protected Rigidbody2D body;
    protected bool readyToShoot;
    private bool inBurst = false;
    private float burstTimer;

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
        if(ammoCount <= 0 && ammoCount != -1) {
            //no ammo
            inBurst = false;
            readyToShoot = false;
            return;
        }
        if (!inBurst && Time.time > burstTimer + burstDelay) {
            inBurst = true;
            burstTimer = Time.time;
        }
        if (inBurst && Time.time > burstTimer + burstTime) {
            inBurst = false;
            burstTimer = Time.time;
        }
        if (inBurst && readyToShoot) {
            readyToShoot = false;
            sendFireCommand();
            Invoke("ResetReadyToShoot", shotDelay);
        }
    }

    void ResetReadyToShoot() {
        readyToShoot = true;
    }

    //Fire commands need to execute in a server context.  This means a NetworkBehaviour object must do the calling.
    //This should probably not be invoked directly?
    public void sendFireCommand() {
        weaponInst.CmdFire();
    }

    //creates a generic bullet assigned to this player
    protected virtual GameObject createBullet(Vector3 pos, Quaternion rot) {
            GameObject bulletgo = ((GameObject)Instantiate(bulletObj, pos, rot));
            Bullet bullet = bulletgo.GetComponent<Bullet>();
            bullet.SetType(type);
            bullet.owner = GetComponentInParent<Player>();
            bullet.isPassable = isPassable;
            bullet.SetSpeed(speed, 0, 0, clockAngle);
        return bulletgo;
    }

    //called in server context, returns objects the server should spawn
    public virtual List<GameObject> doFireCallback() {
        List<GameObject> objs = new List<GameObject>();
        for (int i = 0; i < numberOfBullets; i++) {

            GameObject bulletgo = createBullet(transform.position, Quaternion.AngleAxis(clockAngle, transform.up));
            Bullet bullet = bulletgo.GetComponent<Bullet>();

            bullet.Fire();

            if (useParentVelocity) {
                bullet.GetComponent<Rigidbody2D>().velocity += body.velocity;
            }

            objs.Add(bulletgo);

        }

        decrementAmmo(); //-1 for each bullet or for each burst?
        return objs;

    }

    protected virtual void decrementAmmo() {
        if(ammoCount > 0) {
            ammoCount -= 1;
        }
    }
}