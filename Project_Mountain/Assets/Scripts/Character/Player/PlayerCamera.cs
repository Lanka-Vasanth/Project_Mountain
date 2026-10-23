using System.Collections.Generic;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    public PlayerManager player;

    public Camera cameraObject;
    [SerializeField] Transform cameraPivotTransform;

    [Header("Camera Settings")]
    private Vector3 cameraVelocity;
    private float cameraSmoothSpeed = 1; //BIGGER THIS VALUE, LONGER THE CAMERA TAKES TO REACH POSITION WHEN PLAYER MOVES
    [SerializeField] float leftAndRightRotationSpeed = 220;
    // [SerializeField] float upAnDownRotationSpeed = 220;
    // [SerializeField] float minimumPivot = -30;
    // [SerializeField] float maximumPivot = -50;
    [SerializeField] float leftAndRightLookAngle;
    // [SerializeField] float upAndDownLookAngle;

    [Header("Lock On")]
    [SerializeField] float lockOnRadius = 20; 
    [SerializeField] float minViewableAngle = -50;
    [SerializeField] float maxViewableAngle = 50;
    private List<CharacterManager> availableTargets = new List<CharacterManager>();
    public CharacterManager nearestLockOnTarget;
    public CharacterManager leftLockOnTarget;
    public CharacterManager rightLockOnTarget;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void HandleAllCameraActions()
    {
        if (player != null)
        {
            
        //FOLLOW THE PLAYER
        HandleFollowTarget();
        //ROTATE AROUND THE PLAYER
        // HandleRotations();
        //COLLIDE WITH SURROUNDINGS
        }
    }

    private void HandleFollowTarget()
    {
        Vector3 targetCameraPosition = Vector3.SmoothDamp(transform.position, player.transform.position, ref cameraVelocity, cameraSmoothSpeed*Time.deltaTime);
        transform.position = targetCameraPosition;
    }

    private void HandleRotations()
    {
        //IF LOCKED ON< FORCE ROTATION TOWARD TARGET 
        // if (player.isLockedOn)
        // {
        //     Vector3 rotationDirection = player.playerCombatManager.currentTarget.characterCombatManager.lockOnTransform.position - transform.position;
        //     rotationDirection.Normalize();
        //     rotationDirection.y = 0;

        //     Quaternion targetRotation = Quaternion.LookRotation(rotationDirection);
        //     transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, lockOnTargetFollowSpeed);

        //     // cameraPivotTransform.transform.rotation = Quaternion.Slerp(cameraPivotTransform.rotation, targetRotation, lockOnTargetFollowSpeed);

        //     leftAndRightLookAngle = transform.eulerAngles.y;
        // }
        // //ELSE ROTATE REGULARLY 
        // else
        // {
            //NOMRAL ROTATIONS
            leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput*leftAndRightRotationSpeed)*Time.deltaTime;

            Vector3 cameraRotation = Vector3.zero;
            Quaternion targetRotation;

            cameraRotation.y = leftAndRightLookAngle;
            targetRotation = Quaternion.Euler(cameraRotation);

            transform.rotation = targetRotation;

            cameraPivotTransform.localRotation = targetRotation;
        // }
    }

    public void HandleLocatingLockOnTargets()
    {
        float shortestDistance = Mathf.Infinity;
        float shortestDistanceToRightTarget = Mathf.Infinity;
        float shortestDistanceToLeftTarget = -Mathf.Infinity;

        Collider[] colliders = Physics.OverlapSphere(player.transform.position, lockOnRadius, WorldUtilityManager.instance.GetCharacterLayers());

        for(int i = 0; i< colliders.Length; i++)
        {
            CharacterManager lockOnTarget = colliders[i].GetComponent<CharacterManager>();

            if(lockOnTarget != null)
            {
                //CHECK IF THEY ARE WITHIN FIELD OF VIEW
                Vector3 lockOnTargetDirection = lockOnTarget.transform.position - player.transform.position;
                float distanceFromTarget = Vector3.Distance(player.transform.position, lockOnTarget.transform.position);
                float viewableAngle = Vector3.Angle(lockOnTargetDirection, player.transform.forward);

                if (lockOnTarget.isDead)
                {
                    continue;
                }
                if(lockOnTarget.transform.root == player.transform.root) // CANT LOCK ON TO SELF
                {
                    continue;
                }

                if(viewableAngle > minViewableAngle && viewableAngle < maxViewableAngle)
                {
                    RaycastHit hit;

                    //ONLY CHECK FOR ENVIRONMENT LAYER
                    if (Physics.Linecast(player.playerCombatManager.lockOnTransform.position, 
                                         lockOnTarget.characterCombatManager.lockOnTransform.position,
                                         out hit,
                                         WorldUtilityManager.instance.GetEnvironmentLayers()))
                    {
                        continue;
                    }
                    else
                    {
                        availableTargets.Add(lockOnTarget);
                    }
                }
            }
        }

        for(int k = 0; k < availableTargets.Count; k++)
        {
            if(availableTargets != null)
            {
                float distanceFromTarget = Vector3.Distance(player.transform.position, availableTargets[k].transform.position);
                
                if(distanceFromTarget < shortestDistance)
                {
                    shortestDistance = distanceFromTarget;
                    nearestLockOnTarget = availableTargets[k];
                }

                if (player.isLockedOn)
                {
                    Vector3 relativeEnemyPosition = player.transform.InverseTransformPoint(availableTargets[k].transform.position);
                    var distanceFromLeftTarget = relativeEnemyPosition.x;
                    var distanceFromRightTarget = relativeEnemyPosition.x;

                    if(availableTargets[k] == player.playerCombatManager.currentTarget)
                    {
                        continue;
                    }
                    if(relativeEnemyPosition.x <= 0.00 && distanceFromLeftTarget > shortestDistanceToLeftTarget)
                    {
                        shortestDistanceToLeftTarget = distanceFromLeftTarget;
                        leftLockOnTarget = availableTargets[k];
                    }else if (relativeEnemyPosition.x >= 0.00 && distanceFromRightTarget < shortestDistanceToRightTarget)
                    {
                        shortestDistanceToRightTarget = distanceFromRightTarget;
                        rightLockOnTarget = availableTargets[k];
                    }
                }
            }
            else
            {
                ClearLockOnTargets();
                player.isLockedOn = false;
            }
        }
    }

    public void ClearLockOnTargets()
    {
        nearestLockOnTarget = null;
        leftLockOnTarget = null;
        rightLockOnTarget = null;
        availableTargets.Clear();
    }

    public IEnumerator WaitThenFindNewTarget()
    {
        while (player.isPerformingAction)
        {
            yield return null;
        }

        ClearLockOnTargets();
        HandleLocatingLockOnTargets();

        if(nearestLockOnTarget != null)
        {
            player.playerCombatManager.SetTarget(nearestLockOnTarget);
            player.isLockedOn = true;
        }

        yield return null;
    }
}
