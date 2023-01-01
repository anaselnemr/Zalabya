using UnityEngine;
#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public bool isAiming;
		public bool isAimShoot;
		public bool isSwordSlash;
		public bool isShieldBlock;
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;

		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM && STARTER_ASSETS_PACKAGES_CHECKED
		public void OnMove(InputValue value)
		{
			if (!PlayerClimbing.isClimbing)
				MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				if (!PlayerClimbing.isClimbing)
					LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			if (!PlayerClimbing.isClimbing)
				JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			if (!PlayerClimbing.isClimbing)

				SprintInput(value.isPressed);
		}
#endif

		public void MoveInput(Vector2 newMoveDirection)
		{
			if (!PlayerClimbing.isClimbing)
				move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			if (!PlayerClimbing.isClimbing)
				look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			if (!PlayerClimbing.isClimbing)
				jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			if (!PlayerClimbing.isClimbing)
				sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

		/*		public void OnAiming(InputValue value)
				{
					isAiming = value.isPressed;
				}

				public void OnAimShoot(InputValue value)
				{
					isAimShoot = value.isPressed;
				}

				public void OnSwordSlash(InputValue value)
				{
					isSwordSlash = value.isPressed;
				}
				public void OnShieldBlock(InputValue value)
				{
					isShieldBlock = value.isPressed;
				}*/

	}


}