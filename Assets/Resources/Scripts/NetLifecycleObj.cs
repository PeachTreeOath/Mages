using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

//Other objects that wish to create or destroy a networked object should
//call through this interface on the specific GameObject.  Do not instantiate
//or Destroy networked objects directly!!
public abstract class NetLifecycleObj : NetworkBehaviour {

    abstract public void endLife();

    abstract public void createLife();

}