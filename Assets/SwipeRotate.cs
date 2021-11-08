using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeRotate : MonoBehaviour
{
    private Touch touch;
    private Vector2 touchPostion;
    private Quaternion rotationY;
    private float Speedmodifer=0.3f;
    private float zoommodifer=2;

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1)
        {
       
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Moved)
            {
                rotationY = Quaternion.Euler(0, -touch.deltaPosition.x * Speedmodifer, 0);
                GameManger.instance.Model.transform.rotation = rotationY * GameManger.instance.Model.transform.rotation;
              
            }
        }
        if (Input.touchCount == 2)
        {
            Touch tzero = Input.GetTouch(0);
            Touch tOne = Input.GetTouch(1);

            Vector2 tzeroPrev = tzero.position - tzero.deltaPosition;
            Vector2 tOnePrev = tOne.position - tOne.deltaPosition;

            float oldtouchdistance = Vector2.Distance(tzeroPrev, tOnePrev);
            float currentdistance= Vector2.Distance(tzero.position, tOne.position);

            float DeltaDistance = oldtouchdistance - currentdistance;
            Zoom(DeltaDistance, zoommodifer);
        }
        //float scc = Input.GetAxis("Mouse ScrollWheel");
        //Zoom(scc, Speedmodifer);
        if (Camera.main.fieldOfView <= 30)
        { Camera.main.fieldOfView = 30; }
        else if (Camera.main.fieldOfView >= 90)
        { Camera.main.fieldOfView = 90; }
    }
    void Zoom(float DeltaDistance, float speed)
    {
        Camera.main.fieldOfView += DeltaDistance * speed;
        Camera.main.fieldOfView = Mathf.Clamp(Camera.main.fieldOfView, 30, 90);
    }
}
