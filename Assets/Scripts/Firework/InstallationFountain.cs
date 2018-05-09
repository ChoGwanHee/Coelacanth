using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstallationFountain : BaseInstallation {

    [HideInInspector]
    public int damage;
    [HideInInspector]
    public float hitForce;
    [HideInInspector]
    public float hitRadius;
    [FMODUnity.EventRef]
    public string duringSound;

    private bool enable = false;
    public float enableTime = 1.2f;

    private float tickTime;
    private float tickElapsedTime = 0;

    private ParticleSystem particle;

    private void Start()
    {
        tickTime = GameManagerPhoton._instance.gameTick;
        particle = GetComponentInChildren<ParticleSystem>();
    }

    protected override void Update()
    {
        base.Update();

        
        if(enable && gameObject.GetPhotonView().isMine)
        {
            if (tickElapsedTime >= tickTime)
            {
                Sprinkle();
                tickElapsedTime = 0;
            }
            else
            {
                tickElapsedTime += Time.deltaTime;
            }
        }
        else
        {
            if(elapsedTime >= enableTime)
            {
                enable = true;
                elapsedTime = 0;
                particle.Play();
                FMODUnity.RuntimeManager.PlayOneShot(duringSound);
            }
        }
    }

    private void Sprinkle()
    {
        Collider[] effectedObjects = Physics.OverlapSphere(transform.position, hitRadius, LayerMask.GetMask("DynamicObject"));

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            Vector3 direction = Vector3.Scale(effectedObjects[i].transform.position - transform.position, new Vector3(1, 0, 1)).normalized;

            effectedObjects[i].gameObject.GetPhotonView().RPC("Pushed", PhotonTargets.All, (direction * hitForce));

            if (effectedObjects[i].CompareTag("Player"))
            {
                effectedObjects[i].gameObject.GetPhotonView().RPC("Damage", PhotonTargets.All, damage);
                Vector3 centerPos = transform.position;
                centerPos.y += 0.7f;
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(centerPos);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);
            }
        }

    }
}
