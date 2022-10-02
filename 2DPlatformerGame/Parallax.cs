using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform[] backgrounds;
    public float smoothParallaxingAmount;       // How smooth parallaxing is going to be. Make sure to set this avobe Zero.



    private Transform mCamera;
    private Vector3 mPreviosFrameCameraPostion;
    private float[] mParallaxScales;             // The proportion of the Camera's movement to the move backgrounds by.
                                                // 카메라 움직임의 비율에 따라서 배경을 이동시키는 스케일을 저장하는 변수 

    void Awake()
    {
        mCamera = Camera.main.transform;
    }
    void Start()
    {
        mPreviosFrameCameraPostion = mCamera.position;

        mParallaxScales = new float[backgrounds.Length];

        for (int i = 0; i < backgrounds.Length; ++i)
        {
            mParallaxScales[i] = -backgrounds[i].position.z;
        }
    }
    void Update()
    {
        Debug.Assert(smoothParallaxingAmount > float.Epsilon);
        // for each background
        for (int i = 0; i < backgrounds.Length; ++i)
        {
            // The parallax is the opposite of the camera movement because the previous frame multiplied by the scale
            float parallax = (mPreviosFrameCameraPostion.x - mCamera.position.x) * mParallaxScales[i];

            // Set a target x position which is the current position plus the parallax
            float backgroundTargetPositionX = backgrounds[i].position.x + parallax;

            // Create target position which is the backgroud's current position which it's target x postion. 
            Vector3 backgroundTargetPosition = new Vector3(backgroundTargetPositionX, backgrounds[i].position.y, backgrounds[i].position.z);

            // Fade between current position and the target position using lerp;
            backgrounds[i].position = Vector3.Lerp(backgrounds[i].position, backgroundTargetPosition, smoothParallaxingAmount * Time.deltaTime);
        }

        mPreviosFrameCameraPostion = mCamera.position;
    }
}
