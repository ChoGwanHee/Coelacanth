using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Item/HotSauce")]
public class ItemHotSauce : UtilItem
{
    public float damage;
    public float duration;
    public float addSpeed;
    public int gainScore;
    public float hitForce;
    public float hitRadius;


    public override void Execute(BuffController bc)
    {
        bc.Stat.AddScore(gainScore);
        bc.SetBuff(BuffType.HotSauce, Process(bc));
    }

    private IEnumerator Process(BuffController bc)
    {
        float elapsedTime = 0f;

        bc.photonView.RPC("ApplyBuff", PhotonTargets.All, (int)BuffType.HotSauce);
        bc.PC.maxSpeedFactor += addSpeed;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        bc.PC.maxSpeedFactor -= addSpeed;
        bc.photonView.RPC("RemoveBuff", PhotonTargets.All, (int)BuffType.HotSauce);

        Explosion(bc);
    }

    /// <summary>
    /// 폭발하여 일정 반경 안의 물체를 밀어내고 피해를 줍니다.
    /// </summary>
    private void Explosion(BuffController bc)
    {
        bc.GetComponent<Collider>().enabled = false;

        Collider[] effectedObjects = Physics.OverlapSphere(bc.transform.position, hitRadius, LayerMask.GetMask("DynamicObject"));

        int totalDamage = Mathf.RoundToInt(damage * bc.GetComponent<FireworkExecuter>().damageFactor);

        for (int i = 0; i < effectedObjects.Length; i++)
        {
            Vector3 direction = Vector3.Scale(effectedObjects[i].transform.position - bc.transform.position, new Vector3(1, 0, 1)).normalized;

            PhotonView objPhotonView = effectedObjects[i].GetComponent<PhotonView>();
            objPhotonView.RPC("Pushed", PhotonTargets.All, (direction * hitForce));

            if (effectedObjects[i].CompareTag("Player"))
            {
                objPhotonView.RPC("DamageShake", objPhotonView.owner, totalDamage,  6, bc.photonView.ownerId);
                Vector3 efxPos = effectedObjects[i].GetComponent<CapsuleCollider>().ClosestPointOnBounds(bc.transform.position);
                PhotonNetwork.Instantiate("Prefabs/Effect_base_Hit_fx", efxPos, Quaternion.identity, 0);

                effectedObjects[i].GetComponent<PlayerController>().Pushed(direction * hitForce * 0.5f);
            }
        }
        bc.GetComponent<Collider>().enabled = true;
        bc.photonView.RPC("Damage", bc.photonView.owner, bc.Stat.maxHP, -1);
    }

}
