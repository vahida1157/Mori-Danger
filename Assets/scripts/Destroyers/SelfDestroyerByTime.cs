using UnityEngine;

namespace Destroyers
{
    public class SelfDestroyerByTime : MonoBehaviour
    {
        [SerializeField] private float threshold;

        private void Start()
        {
            Destroy(gameObject, threshold);
        }
    }
}    