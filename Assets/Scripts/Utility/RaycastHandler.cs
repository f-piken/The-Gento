using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastHandler : Singleton<RaycastHandler>
{
    [SerializeField]
    private LayerMask mLayerMask;

    [SerializeField]
    private Camera mCam;

    private void Awake()
    {
        mCam = Camera.main;
    }

    //private void Update()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        Transform obj = RayObject;
    //        if (obj != null)
    //        {
    //            Debug.Log(obj.name);
    //        }
    //    }
    //}

    public Transform RayObject
    {
        get
        {
            RaycastHit hit;
            Ray ray = mCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, mLayerMask))
            {
                //Debug.DrawRay(mCam.transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                //Debug.Log($"Did Hit {hit.collider.gameObject}");

                return hit.collider.transform;
            }
            else
            {
                //Debug.DrawRay(mCam.transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
                //Debug.Log("Did not Hit");

                return null;
            }
        }
    }
}