using UnityEngine;

namespace Game.Scripts
{
    public class CircleInputController : MonoBehaviour
    {
        private const float ScreenSizeOffset = 0.5f;

        private CircleMovementController _movementController;

        private Vector2 _relativePosition;

        void Awake()
        {
            _movementController = GetComponent<CircleMovementController>();
        }

        void Update()
        {
            CalculateRelativeScreenPosition();
            HandleInput();
        }

        private void CalculateRelativeScreenPosition()
        {
            _relativePosition.x = Input.mousePosition.x / Screen.width - ScreenSizeOffset;
            _relativePosition.y = Input.mousePosition.y / Screen.height - ScreenSizeOffset;

            //Debug.Log($"RELATIVE POSITION ON SCREEN: {_relativePosition.x}:{_relativePosition.y}");
        }

        private void HandleInput()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                var movementPosition = new Vector2(_relativePosition.x * Screen.width, _relativePosition.y * Screen.height);
                _movementController.AddMovementPosition(movementPosition);
            }
        }
    }
}
