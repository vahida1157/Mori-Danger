using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class PlayerUsername : MonoBehaviour
    {
        [SerializeField] private Text playerUsername;

        private Camera _camera;

        void Start()
        {
            _camera = Camera.main;
        }

        void Update()
        {
            playerUsername.text = MoriNetworkManager.username;
            var rotation = _camera.transform.rotation;
            transform.LookAt(transform.position + rotation * Vector3.forward,
                rotation * Vector3.up);
        }
    }
}