using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleculeController : MonoBehaviour {

    private Molecula currentObject;

    // For Rotation
    float t = 0.0f;
    float rotationVariance = 90;
    Quaternion targetRotation;
    Quaternion originalRotation;


    // Use this for initialization
    void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (this.currentObject == null)
        {
            try
            {
                this.currentObject = TrackableController.compoundMolecules[this.gameObject.name];
                originalRotation = transform.localRotation;
                t = 0;
                Debug.Log("QUEM ME CHAMOU :" + this.currentObject.name);
            }
            catch { }
        }
        else
        {
            this.UpdateRotation();
        }
	}

    private void UpdateRotation()
    {
        /*
        Debug.Log("UPDATE ROTATION");
        var newRotation = new Vector3(originalRotation.x + UnityEngine.Random.Range(rotationVariance * -1, rotationVariance), originalRotation.y + UnityEngine.Random.Range(rotationVariance * -1, rotationVariance), originalRotation.z + UnityEngine.Random.Range(rotationVariance * -1, rotationVariance));
        //targetRotation = new Quaternion(newRotation.x, newRotation.y, newRotation.z, 0);
        targetRotation = UnityEngine.Random.rotation;
        t = Mathf.Clamp01(100 + Time.deltaTime);
        this.currentObject.atomo.transform.localRotation = Quaternion.Slerp(originalRotation, targetRotation, Mathf.Clamp01(Time.deltaTime * 10));

        if (Quaternion.Angle(this.currentObject.atomo.transform.rotation, targetRotation) < 1)
        {
            targetRotation = UnityEngine.Quaternion.identity;
        }

        Debug.Log(this.currentObject.atomo.transform.localRotation);
        */

        this.currentObject.atomo.transform.Rotate(Time.deltaTime*8, 0, 0);
        this.currentObject.atomo.transform.Rotate(0, Time.deltaTime*8, 0);
    }
}


