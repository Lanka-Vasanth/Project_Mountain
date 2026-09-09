using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager instance;
    private PlayerControls playerControls;

    [SerializeField] private bool LeftClick = false; 
    [SerializeField] public float mouseX;
    [SerializeField] public float mouseY;

    [Header("MOVEMENT INPUT")]
    [SerializeField] bool enableWASDMovement = true;
    [SerializeField] Vector2 movementInput;
    [SerializeField] public float verticalInput;
    [SerializeField] public float horizontalInput;
    [SerializeField] public float moveAmount;
 
     [Header("CAMERA MOVEMENT INPUT")]
    [SerializeField] public Vector2 cameraInput;
    public float cameraHorizontalInput;
    public float cameraVerticalInput;
 
    private void Awake()
    {   
        if(instance == null){
            instance = this;
        }
        else{
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    //FOR WHEN MENU SCENES COME IN
    // private void Start()
    // {
    //     SceneManager.activeSceneChanged += OnSceneChanged;
    //     gameObject.SetActive(false);
    // }

    // private void OnSceneChanged(Scene currentScene, Scene nextScene)
    // {
    //     Scene activeScene = SceneManager.GetActiveScene();

    //     // DISABLE PLAYER INPUT WHEN ON THE MENU SCENE, ENABLE ON ANY OTHER SCENE 
    //     if(activeScene.buildIndex == WorldSaveGameManager.instance.menuScreenIndex)
    //     {
    //         gameObject.SetActive(false);
    //     }
    //     else
    //     {
    //         gameObject.SetActive(true);
    //     }
    // }

    void OnEnable()
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();
        }

        playerControls.Enable();
        
        //MOUSE INPUTS 
        // playerControls.Player.LeftClick.performed += i => LeftClick = true;

        playerControls.PlayerMovement.Movement.performed += i => movementInput = i.ReadValue<Vector2>();
        playerControls.PlayerCamera.Movement.performed += i => cameraInput = i.ReadValue<Vector2>();
    }

    void OnDisable()
    {
        playerControls.Disable(); 
    }

    private void Update()
    {
        HandleInputActions();
    }

    // WILL BE CALLED EACH FRAME IN UPDATE, RECORDS ALL INPUT ACTIONS
    private void HandleInputActions()
    {
        HandleLeftClickAction();
        HandlePlayerMovementInput();
        HandleCameraMovementInput();
    }

    private void HandleLeftClickAction()
    {
        if(LeftClick){
            LeftClick = false;

            //RUN CLICK LOGIC
        }
    }

    private void HandlePlayerMovementInput()
    {
        if (!enableWASDMovement)
        {
            return;
        }

        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + Mathf.Abs(horizontalInput));
    
        if(moveAmount <= 0.5f && moveAmount > 0)
        {
            moveAmount = 0.5f;
        }else if(moveAmount > 0.5f && moveAmount <= 1)
        {
            moveAmount = 1;
        }
    }

    private void HandleCameraMovementInput()
    {
        cameraVerticalInput = cameraInput.y;
        cameraHorizontalInput = cameraInput.x;
    }
}
