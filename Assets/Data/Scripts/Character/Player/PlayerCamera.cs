using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public Camera cameraObject;
    public PlayerManager playerManager;
    [SerializeField] Transform cameraPivotTransform;

    [Header("Camera Settings")]
    private float cameraSmoothTime = 0.2f;
    [SerializeField] float upAndDownRotationSpeed = 220;
    [SerializeField] float leftAndRightRotationSpeed = 220;
    [SerializeField] float minimumPivot = -30;
    [SerializeField] float maximumPivot = 60;
    [SerializeField] float cameraCollisonRadius = 0.2f;
    [SerializeField] LayerMask collideWithLayers;

    [Header("Camera Values")]
    private Vector3 cameraVelocity;
    private Vector3 cameraObjectPosition;
    [SerializeField] float upAndDownLookAngle;
    [SerializeField] float leftAndRightLookAngle;
    private float idealCameraZPosition;
    private float targetCameraZPosition;

    private void Awake() {
        if(instance == null){
            instance = this;
        }else{
            Destroy(gameObject);
        }
    }

    private void Start() {
        DontDestroyOnLoad(gameObject);
        idealCameraZPosition = cameraObject.transform.localPosition.z;
    }

    public void HandleAllCameraActions(){
        if(playerManager != null){
            // 1.跟随player
            HandleFollowPlayer();
            // 2.根据玩家进行旋转
            HandleRotations();
            // 3.碰撞
            HandleCollisions();
        }
    }

    private void HandleFollowPlayer(){
        Vector3 newPosition = Vector3.SmoothDamp(transform.position,
                                                     playerManager.transform.position,
                                                     ref cameraVelocity,
                                                     cameraSmoothTime);

        transform.position = newPosition;
    }

    private void HandleRotations(){
        leftAndRightLookAngle += PlayerInputManager.instance.horizontalCameraInput * leftAndRightRotationSpeed * Time.deltaTime;
        upAndDownLookAngle -= PlayerInputManager.instance.vertiCalcameralInput * upAndDownRotationSpeed * Time.deltaTime;
        upAndDownLookAngle = Mathf.Clamp( upAndDownLookAngle, minimumPivot, maximumPivot);

        Vector3 cameraRotation = Vector3.zero;
        Quaternion newRotation;
        // 水平方向旋转
        cameraRotation.y = leftAndRightLookAngle;
        newRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = newRotation;
        // 垂直方向旋转
        cameraRotation = Vector3.zero;
        cameraRotation.x = upAndDownLookAngle;
        newRotation = Quaternion.Euler(cameraRotation);
        cameraPivotTransform.localRotation = newRotation;

    }

    private void HandleCollisions(){
        targetCameraZPosition = idealCameraZPosition;

        RaycastHit hit;
        Vector3 direction = cameraObject.transform.position - cameraPivotTransform.position;
        direction.Normalize();

        

        if(Physics.SphereCast(cameraPivotTransform.position, cameraCollisonRadius, direction, out hit, Mathf.Abs(targetCameraZPosition), collideWithLayers)){
            float distanceFromHitObject = Vector3.Distance(cameraPivotTransform.position, hit.point);
            targetCameraZPosition = -(distanceFromHitObject - cameraCollisonRadius);
        }

        if(Mathf.Abs(targetCameraZPosition) < cameraCollisonRadius){
            targetCameraZPosition = -cameraCollisonRadius;
        }

        cameraObjectPosition.z = Mathf.Lerp(cameraObject.transform.localPosition.z, targetCameraZPosition, 0.2f);
        cameraObject.transform.localPosition = cameraObjectPosition;
    }

}
