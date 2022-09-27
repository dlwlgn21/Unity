using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float turnSpeed = 20;

    Vector3 mMovement;
    Quaternion mRotation = Quaternion.identity;
    Animator mAnimator;
    Rigidbody mRigidBody;

    void Start()
    {
        mAnimator = GetComponent<Animator>();
        mRigidBody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        mMovement.Set(horizontal, 0f, vertical);
        mMovement.Normalize();

        bool hasHorizontalInput = !Mathf.Approximately(horizontal, 0f);
        bool hasVerticalInput = !Mathf.Approximately(vertical, 0f);

        bool isWalking = hasHorizontalInput || hasVerticalInput;
        mAnimator.SetBool("IsWalking", isWalking);

        Vector3 desiredForward = Vector3.RotateTowards(transform.forward, mMovement, turnSpeed * Time.deltaTime, 0f);
        mRotation = Quaternion.LookRotation(desiredForward);
    }

    void OnAnimatorMove()
    {
        mRigidBody.MovePosition(mRigidBody.position + mMovement * mAnimator.deltaPosition.magnitude);           // Animator의 deltaPosition이란 루트모션으로 인한 프레임당 포지션의 이동량을 말한다.
                                                                                                                // 여기서는 deltaPosition의 크기에 캐릭터의 이동방향을 나타내는 이동 벡터를 곱했음.
        mRigidBody.MoveRotation(mRotation);

    }
}
