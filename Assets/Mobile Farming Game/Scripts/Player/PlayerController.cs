/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
    [Header(" Elements ")]
    [SerializeField] private MobileJoystick joystick;
    private PlayerAnimator playerAnimator;
    private CharacterController characterController;

    [Header(" Settings ")]
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    // Update is called once per frame
    void Update()
    {
        ManageMovement();
    }

    private void ManageMovement()
    {
        Vector3 moveVector = joystick.GetMoveVector() * moveSpeed * Time.deltaTime;

        moveVector.z = moveVector.y;
        moveVector.y = 0;

        characterController.Move(moveVector);

        playerAnimator.ManageAnimations(moveVector);
    }
}
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerAnimator))]
public class PlayerController : MonoBehaviour
{
	[Header(" Elements ")]
	[SerializeField] private MobileJoystick joystick;
	private PlayerAnimator playerAnimator;
	private CharacterController characterController;

	[Header(" Settings ")]
	[SerializeField] private float moveSpeed;
	[SerializeField] private float gravity = -9.81f;
	[SerializeField] private float groundDistance = 0.4f;
	[SerializeField] private LayerMask groundMask;

	private Vector3 velocity;
	private bool isGrounded;

	public Transform groundCheck;

	// Start is called before the first frame update
	void Start()
	{
		characterController = GetComponent<CharacterController>();
		playerAnimator = GetComponent<PlayerAnimator>();
	}

	// Update is called once per frame
	void Update()
	{
		ManageMovement();
	}

	private void ManageMovement()
	{
		isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

		if (isGrounded && velocity.y < 0)
		{
			velocity.y = -2f;
		}

		Vector3 moveVector = joystick.GetMoveVector() * moveSpeed * Time.deltaTime;

		moveVector.z = moveVector.y;
		moveVector.y = 0;

		characterController.Move(moveVector);

		playerAnimator.ManageAnimations(moveVector);

		// Apply gravity
		velocity.y += gravity * Time.deltaTime;
		characterController.Move(velocity * Time.deltaTime);
	}
}
