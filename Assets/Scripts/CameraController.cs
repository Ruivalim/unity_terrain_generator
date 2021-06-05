using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float speed = 10f;
    public float sensibility = 5f;

    float angleX = 0f;
    float angleY = 0f;

    void Update()
    {
        float movementX = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float movementZ = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        transform.Translate(new Vector3(movementX, 0, movementZ));

        angleX += Input.GetAxis("Mouse X") * sensibility;
        angleY -= Input.GetAxis("Mouse Y") * sensibility;

        transform.eulerAngles = new Vector3(angleY, angleX, 0);
    }
}
