using System;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target1;
    public Transform target2;
    public Transform target3;
    public Transform target4;

    private Controller theController;

    public float smoothSpeed = 0.08f;

    public Vector3 offset;

    void Start()
    {
        theController = GameObject.FindObjectOfType<Controller>();
    }

    private void FixedUpdate()
    {
        if(theController.IdPlayerPlaying == 0)
        {
            MovePawn(target1, target1.eulerAngles);
        }
        else if (theController.IdPlayerPlaying == 1)
        {
            MovePawn(target2, target2.eulerAngles);
        }
        else if (theController.IdPlayerPlaying == 2)
        {
            MovePawn(target3, target3.eulerAngles);
        }
        else if (theController.IdPlayerPlaying == 3)
        {
            MovePawn(target4, target4.eulerAngles);
        }
    }

    private void MovePawn(Transform target, Vector3 angles)
    {
        int yAngle = Convert.ToInt32(angles.y);

        Vector3 desiredPosition = target.position;

        if (yAngle == 270)
        {
            this.transform.rotation = Quaternion.Euler(45, 0, 0);
            desiredPosition = new Vector3(target.transform.position.x, 2, (float)(-1.5));
        }
        else if (yAngle == 315)
        {
            this.transform.rotation = Quaternion.Euler(45, 45, 0);
            desiredPosition = new Vector3(target.transform.position.x - (float)1.5, 2, (float)(-1.5));
        }
        else if (yAngle == 0)
        {
            this.transform.rotation = Quaternion.Euler(45, 90, 0);
            desiredPosition = new Vector3(target.position.x - (float)1.5, 2, target.position.z);
        }
        else if (yAngle == 45)
        {
            this.transform.rotation = Quaternion.Euler(45, 135, 0);
            desiredPosition = new Vector3(target.position.x - (float)1.5, 2, target.position.z + (float)1.5);
        }
        else if (yAngle == 90)
        {
            this.transform.rotation = Quaternion.Euler(45, 180, 0);
            desiredPosition = new Vector3(target.position.x, 2, target.position.z + (float)1.5);
        }
        else if (yAngle == 135)
        {
            this.transform.rotation = Quaternion.Euler(45, 225, 0);
            desiredPosition = new Vector3(target.position.x + (float)1.5, 2, target.position.z + (float)(1.5));
        }
        else if (yAngle == 180)
        {
            this.transform.rotation = Quaternion.Euler(45, 270, 0);
            desiredPosition = new Vector3(target.position.x + (float)1.5, 2, target.position.z);
        }
        else if (yAngle == 225)
        {
            this.transform.rotation = Quaternion.Euler(45, 315, 0);
            desiredPosition = new Vector3(target.position.x + (float)1.5, 2, target.position.z - (float)(1.5));
        }
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;
    }
}
