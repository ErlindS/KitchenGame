using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private Gameinput gameinput;
    [SerializeField] private LayerMask CountersLayerMask;

    private bool isWalking;


    private void Update()
    {
        HandleMovement();
        HandleIteractions();
    }



    public bool IsWalking() {
        return isWalking;
    }

    private void HandleIteractions() {

        Vector2 inputVector = gameinput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float interactDistance = 2f;
        if (Physics.Raycast(transform.position, moveDir, out RaycastHit raycasthit, interactDistance, CountersLayerMask))
        {
            if (raycasthit.transform.TryGetComponent(out ClearCounter clearCounter)) {
                //Has ClearCounter
                clearCounter.Interact();
            }
        }
    }

    private void HandleMovement() {

        Vector2 inputVector = gameinput.GetMovementVectorNormalized();

        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float moveDistance = moveSpeed * Time.deltaTime;
        float playerRadius = .7f;
        float playerHeight = 2f;
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        if (!canMove)
        {

            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

            if (canMove)
            {
                moveDir = moveDirX;
            }
            else
            {

                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);

                if (canMove)
                {

                    moveDir = moveDirZ;
                }
                else
                {
                }
            }
        }


        if (canMove)
        {
            transform.position += moveDir * moveSpeed * Time.deltaTime;
        }

        isWalking = moveDir != Vector3.zero;

        float rotateSpeed = 10f;
        transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotateSpeed);
        //Debug.Log(inputVector);
    }
}
