using UnityEngine;

namespace Destroyers
{
    public class SelfDestroyerByTime : MonoBehaviour
    {
        [SerializeField] private float threshold;
        private float _time;

        // Update is called once per frame
        void Update()
        {
            _time += Time.deltaTime;
            if (_time > threshold)
            {
                Destroy(gameObject);
            }
        }
    }
}