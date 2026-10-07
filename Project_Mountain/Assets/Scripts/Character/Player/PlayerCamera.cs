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
        HandleRotations();
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
        //ELSE ROTATE REGULARLY 

        //NOMRAL ROTATIONS
        leftAndRightLookAngle += (PlayerInputManager.instance.cameraHorizontalInput*leftAndRightRotationSpeed)*Time.deltaTime;

        Vector3 cameraRotation = Vector3.zero;
        Quaternion targetRotation;

        cameraRotation.y = leftAndRightLookAngle;
        targetRotation = Quaternion.Euler(cameraRotation);
        transform.rotation = targetRotation;

        cameraPivotTransform.localRotation = targetRotation;
    }
}
