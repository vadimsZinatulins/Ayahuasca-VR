using UnityEngine;

namespace MyUtility.ObjectPool
{
    public class PoolTimer : MonoBehaviour
    {
        private bool _startedCounting;
        private float timeAlive;

        public void StartTimer(float t)
        {
            timeAlive = t;
            _startedCounting = true;
        }

        private void Update()
        {
            if (!_startedCounting) return;
            timeAlive -= Time.deltaTime;
            if (timeAlive <= 0)
            {
                gameObject.ReturnToPool();
                Destroy(this);
            }
            
        }
    }
}