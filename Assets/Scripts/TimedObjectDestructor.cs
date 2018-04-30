using System;
using UnityEngine;

namespace UnityStandardAssets.Utility
{
    public class TimedObjectDestructor : MonoBehaviour
    {
        [SerializeField] private float m_TimeOut = 1.0f;
        [SerializeField] private bool m_DetachChildren = false;
        [SerializeField] private bool m_PhotonView = false;



        private void Awake()
        {
            Invoke("DestroyNow", m_TimeOut);
        }


        private void DestroyNow()
        {
            if (m_DetachChildren)
            {
                transform.DetachChildren();
            }

            if (m_PhotonView)
            {
                if(gameObject.GetPhotonView().isMine)
                    PhotonNetwork.Destroy(gameObject);
            }
                
            else
                DestroyObject(gameObject);
        }
    }
}
