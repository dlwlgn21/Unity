using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof(SpriteRenderer))]
public class Tiler : MonoBehaviour
{
    public bool bHasRightBuddy;
    public bool bHasLeftBuddy;

    public bool bIsRreveseScale;            // It is used if the object is not tilable.
    
    
    private int mXoffset = 1;               // For Don't get any weird errors. 
    private float mSpriteWidth = 0.0f;
    private Camera mCamera;
    private Transform mMyTransform;

    void Awake()
    {
        mCamera = Camera.main;
        mMyTransform = transform;
    }

    void Start()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        mSpriteWidth = spriteRenderer.sprite.bounds.size.x;
    }

    void Update()
    {
        if (!bHasLeftBuddy || !bHasRightBuddy)
        {
            // calculate the cameras extend (half the width) of what the camera can see in world coodinates
            float camHorizontalExtend = mCamera.orthographicSize * Screen.width / Screen.height;

            // Calculate the x position where the camera can see the edge of the sprite (element)
            float edgeVisiblePositionRight = (mMyTransform.position.x + mSpriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (mMyTransform.position.x - mSpriteWidth / 2) + camHorizontalExtend;
        
            // Checking If Player Can see edge of the element and then calling makeNewBuddy if it can
            if (mCamera.transform.position.x >= edgeVisiblePositionRight - mXoffset && !bHasRightBuddy)
            {
                makeNewBuddy(1);
                bHasRightBuddy = true;
            }
            else if (mCamera.transform.position.x <= edgeVisiblePositionLeft + mXoffset && !bHasLeftBuddy)
            {
                makeNewBuddy(-1);
                bHasLeftBuddy = true;
            }
        }
    }

    private void makeNewBuddy(int rightOrLeft)
    {
        Debug.Assert(rightOrLeft == 1 || rightOrLeft == -1);
        Vector3 newPosition = new Vector3(mMyTransform.position.x + (mSpriteWidth * rightOrLeft), mMyTransform.position.y, mMyTransform.position.z);
        Transform newBuddy = Instantiate(mMyTransform, newPosition, mMyTransform.rotation) as Transform;
        
        // If not tilable let's revese the x size og object to get rid of ugly seams
        if (bIsRreveseScale == true)
        {
            newBuddy.localScale = new Vector3(-newBuddy.localScale.x, newBuddy.localScale.y, newBuddy.localScale.z);
        }

        newBuddy.parent = mMyTransform.parent;
        if (rightOrLeft > 0)
        {
            newBuddy.GetComponent<Tiler>().bHasLeftBuddy = true;
        }
        else
        {
            newBuddy.GetComponent<Tiler>().bHasRightBuddy = true;

        }

    }
}
