using System.Collections.Generic;
using System.Linq;
using MLAPI;
using UnityEngine;

namespace CameraScript
{
    public class CameraController : NetworkBehaviour
    {
        [SerializeField] private float dampTime = 0.2f;
        [SerializeField] private float screenEdgeBuffer = 4f;

        [SerializeField] private float minSize = 6.5f;

        public readonly Dictionary<ulong, Transform> Targets = new Dictionary<ulong, Transform>();

        private Camera _camera;
        private float _zoomSpeed;
        private Vector3 _moveVelocity;
        private Vector3 _desiredPosition;

        // Start is called before the first frame update
        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            Move();
            Zoom();
        }

        private void Move()
        {
            FindAveragePosition();

            transform.position =
                Vector3.SmoothDamp(transform.position, _desiredPosition, ref _moveVelocity, dampTime);
        }

        private void FindAveragePosition()
        {
            var averagePos = new Vector3();
            var numTargets = 0;

            foreach (var t in Targets.Values.Where(t => t.gameObject.activeSelf))
            {
                averagePos += t.position;
                numTargets++;
            }

            if (numTargets > 0)
                averagePos /= numTargets;

            averagePos.y = transform.position.y;

            _desiredPosition = averagePos;
        }

        private void Zoom()
        {
            var requiredSize = FindRequiredSize();
            _camera.orthographicSize =
                Mathf.SmoothDamp(_camera.orthographicSize, requiredSize, ref _zoomSpeed, dampTime);
        }

        private float FindRequiredSize()
        {
            var desiredLocalPos = transform.InverseTransformPoint(_desiredPosition);

            var size = 0f;

            foreach (var t in Targets.Values)
            {
                if (!t.gameObject.activeSelf)
                    continue;

                Vector3 targetLocalPos = transform.InverseTransformPoint(t.position);

                Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));

                size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / _camera.aspect);
            }

            size += screenEdgeBuffer;

            size = Mathf.Max(size, minSize);

            return size;
        }

        public void SetStartPositionAndSize()
        {
            FindAveragePosition();

            transform.position = _desiredPosition;

            _camera.orthographicSize = FindRequiredSize();
        }
    }
}