using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script requires you to have setup your animator with 3 parameters, "InputMagnitude", "InputX", "InputZ"
//With a blend tree to control the inputmagnitude and allow blending between animations.
[RequireComponent(typeof(CharacterController))]
public class MovementInput : MonoBehaviour {

    public float Velocity;
    [Space]

	//private Animator anim;
	private Camera cam;
	private CharacterController controller;
	private Vector3 desiredMoveDirection;

	public bool blockRotationPlayer;
	public float desiredRotationSpeed = 0.1f;


	public float forwardSpeed = 25f, strafeSpeed = 7.5f, hoverSpeed = 5f;
    private float activeHoverSpeed;
    public float lookRateSpeed = 90f;
    private Vector2 lookInput, screenCenter, mouseDistance;

    private float rollInput;
    public float rollSpeed = 90f, rollAcceleration = 3.5f;


	void Start () {
		//anim = this.GetComponent<Animator> ();
		cam = Camera.main;
		controller = this.GetComponent<CharacterController> ();

		screenCenter.x = Screen.width * .5f;
        screenCenter.y = Screen.height * .5f;
	}
	
	void Update () {
		PlayerMoveAndRotation();
    }

	void PlayerMoveAndRotation() {
		// Giros de camara
        lookInput.x = Input.mousePosition.x;
        lookInput.y = Input.mousePosition.y;

        // Consistencia en el movimiento
        mouseDistance.x = (lookInput.x - screenCenter.x) / screenCenter.y;
        mouseDistance.y = (lookInput.y - screenCenter.y) / screenCenter.y;

        mouseDistance = Vector2.ClampMagnitude(mouseDistance, 1f);

        // Rotacion
        rollInput = Mathf.Lerp(rollInput, Input.GetAxisRaw("Roll"), rollAcceleration * Time.deltaTime);

        transform.Rotate(-mouseDistance.y * lookRateSpeed * Time.deltaTime, 0f/*mouseDistance.x * lookRateSpeed * Time.deltaTime*/, rollInput * rollSpeed * Time.deltaTime, Space.Self);

		var acceleration = (Input.GetAxisRaw("Vertical") != 1) ? 0 : 1;

        // Movimientos (arriba/abajo, delante/detras, derecha/izquierda)
        //activeStrafeSpeed = Input.GetAxisRaw("Horizontal") * strafeSpeed;
        activeHoverSpeed = hoverSpeed * Input.GetAxis("Mouse Y");

		var movement = transform.forward * (forwardSpeed + acceleration * forwardSpeed) * Time.deltaTime + 
					//transform.right * activeStrafeSpeed * Time.deltaTime + 
					transform.up * activeHoverSpeed * Time.deltaTime;
		controller.Move(movement);
	}

    public void LookAt(Vector3 pos)
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(pos), desiredRotationSpeed);
    }

    public void RotateToCamera(Transform t)
    {
        var forward = cam.transform.forward;

        desiredMoveDirection = forward;
		Quaternion lookAtRotation = Quaternion.LookRotation(desiredMoveDirection);
		Quaternion lookAtRotationOnly_Y = Quaternion.Euler(transform.rotation.eulerAngles.x, lookAtRotation.eulerAngles.y, -transform.rotation.eulerAngles.z);

		t.rotation = Quaternion.Slerp(transform.rotation, lookAtRotationOnly_Y, desiredRotationSpeed);
	}
}
