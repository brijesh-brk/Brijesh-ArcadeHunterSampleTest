using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Utility { 
    
    public static void SetLayerRecursively(GameObject obj,int newlayer)
    {
        obj.layer = newlayer;

        foreach(Transform child in obj.transform)
        {
            if (child == null)
                continue;
            SetLayerRecursively(child.gameObject, newlayer);
        }
    }

    public static void ToogleRigidBodyKinematic(Transform rig, bool value)
    {

        foreach(Transform child in rig)
        {
            if (rig.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                //rig.GetComponent<Rigidbody>().isKinematic = value;
                rb.isKinematic = value;
            }

            ToogleRigidBodyKinematic(child, value);
        }
    }

    public static void GetRigidBody(Transform rig, Rigidbody[] rb, int i = 0)
    {
        foreach (Transform child in rig)
        {
            if (rig.TryGetComponent<Rigidbody>(out Rigidbody _rb))
            {
                rb[i] = _rb;
                i++;
            }

            GetRigidBody(child, rb, i);
        }
    }

    public static void GetRigidBody(Transform rig, List<Rigidbody> rb)
    {
        foreach (Transform child in rig)
        {
            if (rig.TryGetComponent<Rigidbody>(out Rigidbody _rb))
            {
                if (!rb.Contains(_rb))
                    rb.Add(_rb);
            }

            GetRigidBody(child, rb);
        }
    }

    public static IEnumerator ShowRepostioning(Transform target,Transform reachPoint)
    {
        Vector3 pos = reachPoint.position;
        while(target.position != pos)
        {
            target.position = Vector3.MoveTowards(target.position, pos, 10f*Time.deltaTime);
            target.rotation = Quaternion.Lerp(target.rotation, reachPoint.rotation, Time.deltaTime * 10f);
            yield return null;
        }
        MonoBehaviour.Destroy(target.gameObject);
        
    }
}
