using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AnimationAndMovementController : MonoBehaviour
{
    //declare reference variables
    PlayerInput _playerInput; 
    CharacterController _characterController;
    Animator _animator;

    // variables to store optimized setter/getter parameter 
    int _isWalkingHash;
    int _isRunningHash;

    //variables to store player input values
    Vector2 _currentMovementInput;
    Vector3 _currentMovement;
    Vector3 _currentRunMovement;
    Vector3 _appliedMovement;
    bool _isMovementPressed;
    bool _isRunPressed;

    // constants
    float _rotationFactorPerFrame = 15.0f;
    float _runMultiplier = 5.0f;
    float _walkMultiplier = 2.0f;
    int _zero = 0;

    // gravity variables
    float _gravity = -9.8f;
    float _groundedGravity = -.05f;

    //jumping variables
    bool _isJumpPressed = false;
    float _initialJumpVelocity;
    float _maxJumpHeight = 2.0f;
    float _maxJumpTime = 0.75f;
    bool _isJumping = false;
    int _isJumpingHash;
    bool _isJumpAnimating = false;
    int _jumpCountHash;
    int _jumpCount = 0;
    Dictionary<int, float> initialJumpVelocities = new Dictionary<int, float>();
    Dictionary<int, float> jumpGravities = new Dictionary<int, float>();
    Coroutine currentJumpResetRoutine = null;   

    //bool isAttackPressed = false;
    //int isAttackingHash;
    //bool isAttackAnimating = false;
    //bool isAttacking = false;



    


    // Awake is called earlier than Start in Unity's event life cycle
    void Awake()
    {
        //initially set reference variables
        _playerInput = new PlayerInput();
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        //set the parameter hash references
        _isWalkingHash = Animator.StringToHash("isWalking");
        _isRunningHash = Animator.StringToHash("isRunning");
        _isJumpingHash = Animator.StringToHash("isJumping");
        _jumpCountHash = Animator.StringToHash("jumpCount");
        //isAttackingHash = Animator.StringToHash("isAttacking");

        // set the player input callbacks
        _playerInput.CharacterControls.Move.started += onMovementInput;
        _playerInput.CharacterControls.Move.canceled += onMovementInput;
        _playerInput.CharacterControls.Move.performed += onMovementInput;
        _playerInput.CharacterControls.Run.started += onRun;
        _playerInput.CharacterControls.Run.canceled += onRun;
        _playerInput.CharacterControls.Jump.started += onJump;
        _playerInput.CharacterControls.Jump.canceled += onJump;

        //playerInput.CharacterControls.Attack.started += onAttack;
        //playerInput.CharacterControls.Attack.canceled += onAttack;

        setupJumpVariables();

    }

    // set the initial velocity and gravity using jump heights and durations
    void setupJumpVariables()
    {
        float timeToApex = _maxJumpTime / 2;
        _gravity = ( - 2 * _maxJumpHeight) / Mathf.Pow(timeToApex, 2);
        _initialJumpVelocity = (2 * _maxJumpHeight) / timeToApex;
        float secondJumpGravity = (-2 * (_maxJumpHeight + 2 )) / Mathf.Pow((timeToApex * 1.25f), 2);
        float secondJumpInitialVelocity = (2 * (_maxJumpHeight + 2)) / (timeToApex * 1.25f);
        float thirdJumpGravity = (-2 * (_maxJumpHeight + 4)) / Mathf.Pow((timeToApex * 1.5f), 2);
        float thirdJumpInitialVelocity = (2 * (_maxJumpHeight + 4)) / (timeToApex * 1.5f);

        initialJumpVelocities.Add(1, _initialJumpVelocity);
        initialJumpVelocities.Add(2, secondJumpInitialVelocity);
        initialJumpVelocities.Add(3, thirdJumpInitialVelocity);

        jumpGravities.Add(0, _gravity);
        jumpGravities.Add(1, _gravity);
        jumpGravities.Add(2, secondJumpGravity);
        jumpGravities.Add(3, thirdJumpGravity);
    }

    // launch character into the air with initial verical velocity if conditions met
    void handleJump()
    {
        if(!_isJumping && _characterController.isGrounded && _isJumpPressed)
        {
            if(_jumpCount < 3 && currentJumpResetRoutine != null)
            {
                StopCoroutine(currentJumpResetRoutine);
            }
            _animator.SetBool(_isJumpingHash, true);
            _isJumpAnimating = true;
            _isJumping = true;
            _jumpCount += 1;
            _animator.SetInteger(_jumpCountHash, _jumpCount);
            _currentMovement.y = initialJumpVelocities[_jumpCount] ;
            _appliedMovement.y = initialJumpVelocities[_jumpCount] ;
        } else if (!_isJumpPressed && _isJumping && _characterController.isGrounded)
        {
            _isJumping = false;
        }
    }

    //void handleAttack()
    //{
    //    if (!isAttacking)
    //    { 
    //        animator.SetBool(isAttackingHash, true);
    //        isAttackAnimating = true;

    //    }
    //}

    IEnumerator jumpResetRoutine()
    {
        yield return new WaitForSeconds(.2f);
        _jumpCount = 0;
    }

    //void onAttack(InputAction.CallbackContext context)
    //{
    //    isAttackPressed = context.ReadValueAsButton();
    //}

    // callback handler function for jump buttons
    void onJump (InputAction.CallbackContext context)
    {
        _isJumpPressed = context.ReadValueAsButton();
        //Debug.Log(isJumpPressed);
    }

    // callback handler function for run buttons
    void onRun (InputAction.CallbackContext context)
    {
        _isRunPressed = context.ReadValueAsButton();
        //Debug.Log(isRunPressed);
    }

    void HandleRotation()
    {
        Vector3 positionToLookAt;
        // the change in position our character should point to
        positionToLookAt.x = _currentMovement.x;
        positionToLookAt.y = _zero;
        positionToLookAt.z = _currentMovement.z; 
        // the current rotation of our character
        Quaternion currentRotation = transform.rotation;

        if(_isMovementPressed )
        { 
        // creates a new roatation based on where the player is currently pressing
        Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        // rotate character to face the positionToLookAt
        transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        }

    }

    // callback handler function to set the player input values
    void onMovementInput (InputAction.CallbackContext context)
    {
        _currentMovementInput = context.ReadValue<Vector2>();
        _currentMovement.x = _currentMovementInput.x * _walkMultiplier;
        _currentMovement.z = _currentMovementInput.y * _walkMultiplier;
        _currentRunMovement.x = _currentMovementInput.x * _runMultiplier;
        _currentRunMovement.z = _currentMovementInput.y * _runMultiplier;
        _isMovementPressed = _currentMovementInput.x != _zero || _currentMovementInput.y != _zero;
    }
    void Start()
    {
        
    }

    void HandleAnimations()
    {
        // get parameter values from animator
        bool isWalking = _animator.GetBool(_isWalkingHash);
        bool isRunning = _animator.GetBool(_isRunningHash);

        // start walking if movement pressed is true and not already walking
        if(_isMovementPressed && !isWalking)
        {
            _animator.SetBool(_isWalkingHash, true);
        }

        // stop walking if isMovementPressed is false and not already walking
        else if(!_isMovementPressed && isWalking)
        {
            _animator.SetBool(_isWalkingHash, false);
        }

        // run if movement and run pressed are true and not currently running
        if ((_isMovementPressed && _isRunPressed) && !isRunning)
        {
            _animator.SetBool(_isRunningHash, true);

        }

        // stop running if movement or run pressed are false and currently running
        else if ((!_isMovementPressed || !_isRunPressed) && isRunning)
        {
            _animator.SetBool(_isRunningHash, false);
        }
    }

    // apply proper gravity if the player is grounded or not
    void handleGravity()
    {
        bool isFalling = _currentMovement.y <= 0.0f || !_isJumpPressed;
        float fallMultiplier = 2.0f;
        if (_characterController.isGrounded) { 
            if (_isJumpAnimating)
            {
                _animator.SetBool(_isJumpingHash, false);
                _isJumpAnimating = false;
                currentJumpResetRoutine = StartCoroutine(jumpResetRoutine());
                if (_jumpCount == 3) {
                    _jumpCount = 0;
                    _animator.SetInteger(_jumpCountHash, _jumpCount);
                }
            }
        
            _currentMovement.y = _groundedGravity;
            _appliedMovement.y = _groundedGravity;
        }
        else if (isFalling)
        {
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = _currentMovement.y + (jumpGravities[_jumpCount] * fallMultiplier * Time.deltaTime);
            _appliedMovement.y = Mathf.Max( (previousYVelocity + _currentMovement.y) * .5f, -20.0f);
        }

        else
        {
            float previousYVelocity = _currentMovement.y;
            _currentMovement.y = _currentMovement.y + (jumpGravities[_jumpCount] * Time.deltaTime);
            _appliedMovement.y = (previousYVelocity + _currentMovement.y) * .5f;

        }
    }

    void Update()
    {
        HandleRotation();
        HandleAnimations();

        if(_isRunPressed)
        {
            _appliedMovement.x = _currentRunMovement.x;
            _appliedMovement.z = _currentRunMovement.z;
        }
        else
        { 
            _appliedMovement.x = _currentMovement.x;
            _appliedMovement.z = _currentMovement.z;
        }

        _characterController.Move(_appliedMovement * Time.deltaTime); 


        handleGravity();
        handleJump();
        //handleAttack();
    }

    void OnEnable()
    {
        // enable the character controls action map
        _playerInput.CharacterControls.Enable();
    }

    void OnDisable()
    {
        // disable the character controls action map
        _playerInput.CharacterControls.Disable();
    }
}
