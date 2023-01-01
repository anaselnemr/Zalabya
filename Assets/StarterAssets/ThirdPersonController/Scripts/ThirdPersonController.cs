using System;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using TMPro;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.XR;
#endif

/* Note: animations are called via the controller for both the character and capsule using animator null checks
 */

namespace StarterAssets
{
	[RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
	[RequireComponent(typeof(PlayerInput))]
#endif
	public class ThirdPersonController : MonoBehaviour
	{
        public AudioSource arrowSound;
		public GameObject pnlGameover;
		public GameObject gliderObj;
		public GameObject arrowObj;
		public Transform arrowPoint;
		public GameObject playerFollowCamera;
		public GameObject playerAimCamera;
		public GameObject Swordhand;
		public GameObject Shieldhand;
		public GameObject Bowhand;
		public GameObject Sword;
		public GameObject Shield;

		public AudioSource DeathSound;
		/*		public GameObject Bow;
		*/
		private Boolean combatBow = false;

		[Header("Player")]
		[Tooltip("Move speed of the character in m/s")]
		public float MoveSpeed = 2.0f;

		[Tooltip("Sprint speed of the character in m/s")]
		public float SprintSpeed = 4.0f;

		[Tooltip("How fast the character turns to face movement direction")]
		[Range(0.0f, 0.3f)]
		public float RotationSmoothTime = 0.12f;

		[Tooltip("Acceleration and deceleration")]
		public float SpeedChangeRate = 10.0f;

		public AudioClip LandingAudioClip;
		public AudioClip[] FootstepAudioClips;
		[Range(0, 1)] public float FootstepAudioVolume = 0.5f;

		[Space(10)]
		[Tooltip("The height the player can jump")]
		public float JumpHeight = 1.2f;

		[Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
		public float Gravity = -15.0f;

		[Space(10)]
		[Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
		public float JumpTimeout = 0.50f;

		[Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
		public float FallTimeout = 0.15f;

		[Header("Player Grounded")]
		[Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
		public bool Grounded = true;

		[Tooltip("Useful for rough ground")]
		public float GroundedOffset = -0.14f;

		[Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
		public float GroundedRadius = 0.28f;

		[Tooltip("What layers the character uses as ground")]
		public LayerMask GroundLayers;

		[Header("Cinemachine")]
		[Tooltip("The follow target set in the Cinemachine Virtual Camera that the camera will follow")]
		public GameObject CinemachineCameraTarget;

		[Tooltip("How far in degrees can you move the camera up")]
		public float TopClamp = 70.0f;

		[Tooltip("How far in degrees can you move the camera down")]
		public float BottomClamp = -30.0f;

		[Tooltip("Additional degress to override the camera. Useful for fine tuning camera position when locked")]
		public float CameraAngleOverride = 0.0f;

		[Tooltip("For locking the camera position on all axis")]
		public bool LockCameraPosition = false;

		// cinemachine
		private float _cinemachineTargetYaw;
		private float _cinemachineTargetPitch;

		// player
		private float _speed;
		private float _animationBlend;
		private float _targetRotation = 0.0f;
		private float _rotationVelocity;
		private float _verticalVelocity;
		private float _terminalVelocity = 53.0f;

		// timeout deltatime
		private float _jumpTimeoutDelta;
		private float _fallTimeoutDelta;

		// animation IDs
		private int _animIDSpeed;
		private int _animIDGrounded;
		private int _animIDJump;
		private int _animIDFreeFall;
		private int _animIDMotionSpeed;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		private PlayerInput _playerInput;
#endif
		private Animator _animator;
		private CharacterController _controller;
		private StarterAssetsInputs _input;
		private GameObject _mainCamera;
		private int c = 0;
		private const float _threshold = 0.01f;

		private bool _hasAnimator;
		private bool once = true;
		private bool gliding = false;
		private float Pos1 = 0f;
		private bool mara = true;
		private int health = 24;
		public Sprite fullhealth;
		public Sprite emptyhealth;
		public Sprite halfhealthSprite;
		public Image[] hearts;
		public GameObject t;

		private float shieldBlockTime = 10f;
		private bool shieldCanBlock = true;
		public TMP_Text Text;


		private bool IsCurrentDeviceMouse
		{
			get
			{
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
				return _playerInput.currentControlScheme == "KeyboardMouse";
#else
				return false;
#endif
			}
		}


		private void Awake()
		{
			// get a reference to our main camera
			if (_mainCamera == null)
			{
				_mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
			}
		}

		private void Start()
		{
			GameObject.Find("Swordhand").SetActive(true);
			GameObject.Find("Shieldhand").SetActive(true);
			GameObject.Find("Sword").SetActive(false);
			GameObject.Find("Shield").SetActive(false);
			GameObject.Find("Bowhand").SetActive(false);
			_cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

			_hasAnimator = TryGetComponent(out _animator);
			_animator.GetComponent<Animator>();
			_controller = GetComponent<CharacterController>();
			_input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
			_playerInput = GetComponent<PlayerInput>();
#else
			Debug.LogError( "Starter Assets package is missing dependencies. Please use Tools/Starter Assets/Reinstall Dependencies to fix it");
#endif

			AssignAnimationIDs();

			// reset our timeouts on start
			_jumpTimeoutDelta = JumpTimeout;
			_fallTimeoutDelta = FallTimeout;
		}

		bool isPlaying(string stateName)
		{
			if (_animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
				return true;
			else
				return false;
		}

		public void TakeDamage(int damage)
		{
			if (_animator.GetBool("ShieldBlock"))
			{
				return;
			}
			_animator.PlayInFixedTime("hit");
			Debug.Log("I AM DYINGGGG");
			health -= damage;

			if (health <= 0) StartCoroutine(DestroyEnemy());
		}

		public void HealthUpdate()
		{

			int full = (int)Mathf.Floor(health / 2);
			int half = 0;
			int empty = 0;
			if (health % 2 != 0)
				half = 1;


			empty = hearts.Length - full - half;

			for (int i = (full + half); i < hearts.Length; i++)
			{
				hearts[i].sprite = emptyhealth;
			}

			if (half == 1)
			{
				hearts[full].sprite = halfhealthSprite;
			}

		}
		IEnumerator DestroyEnemy()
		{
			DeathSound.Play();
			_animator.PlayInFixedTime("dying");
			yield return new WaitForSeconds(2);

			gameObject.SetActive(false);
			Destroy(gameObject);
			PlayerClimbing.isClimbing = false;
			Abilities.activeStasis = false;
			Bokoblinagent.chased = false;
			Moblinagent.chased = false;
			pnlGameover.SetActive(true);
		}

		public void OnTriggerStay(Collider c)
		{

			if (c.gameObject.CompareTag("Bokoblin") && isPlaying("Sword Slash") && once)
			{
				once = false;
				MonoBehaviour w = c.gameObject.GetComponent<MonoBehaviour>();
				string type = w.GetType().Name;
				if (type == "Bokoblinagent")
				{
					c.gameObject.GetComponent<Bokoblinagent>().TakeDamage(10);
				}
				else
				{
					c.gameObject.GetComponent<Moblinagent>().TakeDamage(10);

				}
			}
			if (c.gameObject.CompareTag("Moblin") && isPlaying("Sword Slash") && once)
			{
				once = false;
				MonoBehaviour w = c.gameObject.GetComponent<MonoBehaviour>();
				string type = w.GetType().Name;
				if (type == "Bokoblinagent")
				{
					c.gameObject.GetComponent<Bokoblinagent>().TakeDamage(10);
				}
				else
				{
					c.gameObject.GetComponent<Moblinagent>().TakeDamage(10);

				}
			}
			if (c.gameObject.CompareTag("Boss1") && isPlaying("Sword Slash") && once)
			{
				once = false;

				c.gameObject.GetComponent<bossScript>().TakeDamage(10);

			}
            if (c.gameObject.CompareTag("Boss2") && isPlaying("Sword Slash") && once)
            {
                once = false;
                // double the dmg if he is in phase 2
                if (c.gameObject.GetComponent<boss2Script>().getPhase())
                {
                    Debug.Log("the dmg is doubled");
                    c.gameObject.GetComponent<boss2Script>().TakeDamage(10);
                    return;
                }
                else if (!c.gameObject.GetComponent<boss2Script>().getShieldStatus())
                { // dbl the dmg
                    c.gameObject.GetComponent<boss2Script>().TakeDamage(10 * 2);
                }



            }

        }
		public void OnTriggerEnter(Collider c)
        {
			if (c.gameObject.CompareTag("Next Level2"))
			{
                SceneManager.LoadScene(3);
            }
                if (c.gameObject.CompareTag("Next Level"))
			{
				Debug.Log("here");



				if (GameObject.Find("Bokoblin Attack").transform.childCount == 0 && GameObject.Find("Moblin Attack").transform.childCount == 0)
				{

					/*                    Debug.Log("DONE");
										GameObject[] gos;
										//get all the objects with the tag "myTag"
										gos = GameObject.FindGameObjectsWithTag("Door");
										//loop through the returned array of game objects and set each to active false
										for (int i = 0; i < gos.Length; i++)
										{
											gos[i].SetActive(false);
										}*/

					SceneManager.LoadScene(2);
					//Load level 2 
				}
				else
				{
					Debug.Log("Job not finish");
					StartCoroutine(DisplayText());
				}

				//show panel with clear the area 

			}
		}
		IEnumerator DisplayText()
		{
			t.SetActive(true);
			yield return new WaitForSeconds(3);
			t.SetActive(false);
		}

		private void Update()
		{
			if (combatBow)
			{
				Text.text = "Ranged Attack";
			}
			else
			{
				Text.text = "Melee Attack";

			}
			_hasAnimator = TryGetComponent(out _animator);
			if (!isPlaying("Sword Slash"))
				once = true;

			// if (!PlayerClimbing.isClimbing)
			JumpAndGravity();

			HealthUpdate();
			GroundedCheck();
			FallDamageCheck();
			Move();

			// Gliding animation and action with setting the active weapons

			if (Input.GetKey(KeyCode.Space) && _verticalVelocity < 0)
			{
				Gravity = -1.0f;
				gliderObj.SetActive(true);
				_animator.SetBool("Hanging", true);
				Swordhand.SetActive(false);
				Shieldhand.SetActive(false);
				Bowhand.SetActive(false);
				Sword.SetActive(true);
				Shield.SetActive(true);
				gliding = true;

			}
			else
			{
				gliding = false;
				Gravity = -15.0f;
				gliderObj.SetActive(false);
				_animator.SetBool("Hanging", false);
				if (combatBow == true)
				{
					// bow and arrow set to active true
					Swordhand.SetActive(false);
					Shieldhand.SetActive(false);
					Sword.SetActive(true);
					Shield.SetActive(true);
					Bowhand.SetActive(true);
					combatBow = true;

				}
				else
				{
					//sword and shield set to active true
					Swordhand.SetActive(true);
					Shieldhand.SetActive(true);
					Sword.SetActive(false);
					Shield.SetActive(false);
					Bowhand.SetActive(false);
					combatBow = false;
				}
			}

			if (Grounded)
			{
				// to disable glider when grounded
				gliderObj.SetActive(false);
			}
			if (Input.GetKeyDown(KeyCode.Tab))
			{
				combatBow = !combatBow;
			}


			// aimaing animation with camera and stay still (right click)
			// shoot only while aiming (left click)
			//sword slash  (right click)
			//or sword block (left click)
			// return animation after each trial

			if (_animator.GetBool("ShieldBlock") == false)
			{//reset block time
				shieldBlockTime = 10f;
			}


			_animator.SetBool("Aiming", false);
			_animator.SetBool("AimShoot", false);
			_animator.SetBool("ShieldBlock", false);
			playerFollowCamera.SetActive(true);
			playerAimCamera.SetActive(false);

			if (Input.GetMouseButton(1) && Grounded) // right click
			{
				if (combatBow)
				{
					_animator.SetBool("Aiming", true);
					playerFollowCamera.SetActive(false);
					playerAimCamera.SetActive(true);
				}

				else
				{
					if (shieldCanBlock && shieldBlockTime > 0)
					{
						shieldBlockTime -= Time.deltaTime;
						_animator.SetBool("ShieldBlock", true);
					}
					if (shieldCanBlock && shieldBlockTime <= 0)
					{
						StartCoroutine(WaitAndEnableShield());
					}

					if (!shieldCanBlock)
					{
						// TODO: SWORD BLOCK
						// _animator.SetTrigger("SwordBlock");
					}
				}
			}

			if (Input.GetMouseButton(0) && Grounded) // left click
			{

				if (combatBow && _animator.GetBool(("Aiming")))
				{
					_animator.SetBool("AimShoot", true);
				}

				if (!combatBow && _speed==0f)
				{
					_animator.SetTrigger("SwordSlash");
				}
			}



		}
		private void FallDamageCheck()
		{

			if (!Grounded && !gliding && mara)
			{
				Pos1 = transform.position.y;
				mara = false;
			}
			if (gliding)
			{
				mara = true;
				Pos1 = transform.position.y;
			}
			if (Grounded && !mara)
			{
				mara = true;
				// Debug.Log("Fall : " + (Pos1 - transform.position.y));
				if (Pos1 - transform.position.y > 10)
				{
					TakeDamage(24);
				}
			}

		}


		IEnumerator WaitAndEnableShield()
		{
			shieldCanBlock = false;
			yield return new WaitForSeconds(5f);
			shieldCanBlock = true;
		}

		public void ShootArrow()
		{
			//   Event Trigger by animation
			Debug.Log("ShootArrow");
			GameObject arrow = Instantiate(arrowObj, arrowPoint.position, transform.rotation);
            arrowSound.Play();
            arrow.GetComponent<Rigidbody>().AddForce(Camera.main.transform.forward * 50f, ForceMode.Impulse);
		}



		private void LateUpdate()
		{
			CameraRotation();
		}

		private void AssignAnimationIDs()
		{
			_animIDSpeed = Animator.StringToHash("Speed");
			_animIDGrounded = Animator.StringToHash("Grounded");
			_animIDJump = Animator.StringToHash("Jump");
			_animIDFreeFall = Animator.StringToHash("FreeFall");
			_animIDMotionSpeed = Animator.StringToHash("MotionSpeed");
		}

		private void GroundedCheck()
		{
			// set sphere position, with offset
			Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset,
				transform.position.z);
			Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
				QueryTriggerInteraction.Ignore);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetBool(_animIDGrounded, Grounded);
			}
		}

		private void CameraRotation()
		{
			// if there is an input and camera position is not fixed
			if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
			{
				//Don't multiply mouse input by Time.deltaTime;
				float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

				_cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
				_cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
			}

			// clamp our rotations so our values are limited 360 degrees
			_cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
			_cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

			// Cinemachine will follow this target
			CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
				_cinemachineTargetYaw, 0.0f);
		}

		private void Move()
		{
			// set target speed based on move speed, sprint speed and if sprint is pressed
			float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

			// a simplistic acceleration and deceleration designed to be easy to remove, replace, or iterate upon

			// note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is no input, set the target speed to 0
			if (_input.move == Vector2.zero) targetSpeed = 0.0f;

			// a reference to the players current horizontal velocity
			float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

			float speedOffset = 0.1f;
			float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

			// accelerate or decelerate to target speed
			if (currentHorizontalSpeed < targetSpeed - speedOffset ||
				currentHorizontalSpeed > targetSpeed + speedOffset)
			{
				// creates curved result rather than a linear one giving a more organic speed change
				// note T in Lerp is clamped, so we don't need to clamp our speed
				_speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
					Time.deltaTime * SpeedChangeRate);

				// round speed to 3 decimal places
				_speed = Mathf.Round(_speed * 1000f) / 1000f;
			}
			else
			{
				_speed = targetSpeed;
			}

			_animationBlend = Mathf.Lerp(_animationBlend, targetSpeed, Time.deltaTime * SpeedChangeRate);
			if (_animationBlend < 0.01f) _animationBlend = 0f;

			// normalise input direction
			Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

			// note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
			// if there is a move input rotate player when the player is moving
			if (_input.move != Vector2.zero)
			{
				_targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
								  _mainCamera.transform.eulerAngles.y;
				float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
					RotationSmoothTime);

				// rotate to face input direction relative to camera position
				transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
			}


			Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

			// move the player
			if (!PlayerClimbing.isClimbing)
				_controller.Move(targetDirection.normalized * (_speed * Time.deltaTime) +
								 new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);

			// update animator if using character
			if (_hasAnimator)
			{
				_animator.SetFloat(_animIDSpeed, _animationBlend);
				_animator.SetFloat(_animIDMotionSpeed, inputMagnitude);
			}
		}

		private void JumpAndGravity()
		{
			if (Grounded)
			{
				// reset the fall timeout timer
				_fallTimeoutDelta = FallTimeout;

				// update animator if using character
				if (_hasAnimator)
				{
					_animator.SetBool(_animIDJump, false);
					_animator.SetBool(_animIDFreeFall, false);
				}

				// stop our velocity dropping infinitely when grounded
				if (_verticalVelocity < 0.0f)
				{
					_verticalVelocity = -2f;
				}

				// Jump
				if (_input.jump && _jumpTimeoutDelta <= 0.0f)
				{
					_animator.PlayInFixedTime("JumpStart");
					// the square root of H * -2 * G = how much velocity needed to reach desired height
					_verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDJump, true);


					}
				}

				// jump timeout
				if (_jumpTimeoutDelta >= 0.0f)
				{
					_jumpTimeoutDelta -= Time.deltaTime;
				}
			}
			else
			{
				// reset the jump timeout timer
				_jumpTimeoutDelta = JumpTimeout;

				// fall timeout
				if (_fallTimeoutDelta >= 0.0f)
				{
					_fallTimeoutDelta -= Time.deltaTime;
				}
				else
				{
					// update animator if using character
					if (_hasAnimator)
					{
						_animator.SetBool(_animIDFreeFall, true);
					}
				}

				// if we are not grounded, do not jump
				_input.jump = false;
			}

			// apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
			if (_verticalVelocity < _terminalVelocity)
			{
				_verticalVelocity += Gravity * Time.deltaTime;
			}
		}

		private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
		{
			if (lfAngle < -360f) lfAngle += 360f;
			if (lfAngle > 360f) lfAngle -= 360f;
			return Mathf.Clamp(lfAngle, lfMin, lfMax);
		}

		private void OnDrawGizmosSelected()
		{
			Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
			Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

			if (Grounded) Gizmos.color = transparentGreen;
			else Gizmos.color = transparentRed;

			// when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
			Gizmos.DrawSphere(
				new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z),
				GroundedRadius);
		}

		private void OnFootstep(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight > 0.5f)
			{
				if (FootstepAudioClips.Length > 0)
				{
					var index = UnityEngine.Random.Range(0, FootstepAudioClips.Length);
					AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
				}
			}
		}

		private void OnLand(AnimationEvent animationEvent)
		{
			if (animationEvent.animatorClipInfo.weight > 0.5f)
			{
				AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
			}
		}
	}
}