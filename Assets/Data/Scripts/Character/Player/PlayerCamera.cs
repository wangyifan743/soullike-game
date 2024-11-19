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

    [Header("Lock On")]
    [SerializeField] float loackOnRadius = 20f;
    [SerializeField] float minimumViewableAngle = -50;
    [SerializeField] float maximumViewableAngle = 50;
    [SerializeField] float maximumLockOnDistance = 20;
    [SerializeField] float lockOnTargetFollowSpeed = 0.2f;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;

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
        // 如果锁定目标，相机的旋转
        if(playerManager.playerNetworkManager.isLockedOn.Value){
            Vector3 rotateDirection = playerManager.playerCombatManager.currentAttackTarget.characterCombatManager.lockOnTransform.position - transform.position;
            rotateDirection.Normalize();
            rotateDirection.y = 0;
            Quaternion targetRotation = Quaternion.LookRotation(rotateDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

            // ????
            rotateDirection = playerManager.playerCombatManager.currentAttackTarget.characterCombatManager.lockOnTransform.position - cameraPivotTransform.position;
            rotateDirection.Normalize();
            targetRotation = Quaternion.LookRotation(rotateDirection);
            cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

            // ???
            leftAndRightLookAngle = transform.eulerAngles.y;
            upAndDownLookAngle = transform.eulerAngles.x;
        }
        // 没有锁定目标，相机的旋转
        else
        {
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

    public void HandleLocatingLockOnTarget(){
      
        float shortDistance = Mathf.Infinity;
        float shortDistanceOfRightTarget = Mathf.Infinity;
        float shortDistanceOfLeftTarget = -Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(playerManager.transform.position, loackOnRadius, WorldUtilityManager.instance.GetCharacterLayers());

        for (int i = 0; i < colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();
            if(lockOnTarget != null){
                Vector3 lockOnDirection = lockOnTarget.transform.position - playerManager.transform.position;
                lockOnDirection.Normalize();
                float distanceFromTarget = Vector3.Distance(playerManager.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnDirection, cameraObject.transform.forward);

                if(lockOnTarget.isDead.Value)
                    continue;
                
                if(lockOnTarget.transform.root == playerManager.transform.root)
                    continue;

                if(distanceFromTarget > maximumLockOnDistance)
                    continue;
                
                if(viewableAngle >= minimumViewableAngle && viewableAngle <= maximumViewableAngle){
                    RaycastHit hit;


                    if(Physics.Linecast(playerManager.playerCombatManager.lockOnTransform.position, lockOnTarget.characterCombatManager.lockOnTransform.position, out hit, WorldUtilityManager.instance.GetEnviromentLayers())){
                        continue;
                    }
                    else{
                        availableTargets.Add(lockOnTarget);
                    }
                }
            }
        }

        for (int k = 0; k < availableTargets.Count; k++)
        {
            if(availableTargets[k]!=null){
                float distanceFromTarget = Vector3.Distance(playerManager.transform.position, availableTargets[k].transform.position);

                if(distanceFromTarget < shortDistance){
                    shortDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }
            }

        }


    }

    public void ClearLockOnTargets(){
        nearestLockOnTarget = null;
        availableTargets.Clear();
    }

}
