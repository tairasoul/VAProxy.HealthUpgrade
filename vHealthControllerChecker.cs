using Invector;
using UnityEngine;

namespace HealthUpgrade
{
    internal class vHealthControllerChecker : MonoBehaviour
    {
        void Update()
        {
            if (GameObject.Find("S-105") != null)
            {
                Plugin.Instance.healthController = GameObject.Find("S-105").GetComponent<vHealthController>();
            }
        }
    }
}
