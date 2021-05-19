using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
  Animator anim;

  void Awake()
  {
    anim = GetComponent<Animator>();
  }

  void OnEnable() => Invoke("Disable", 0.35f);
  void Disable() => gameObject.SetActive(false);

  public void StartExplosion(string target)
  {
    anim.SetTrigger("Explosion");
    transform.localScale = target switch
    {
      "S" => Vector3.one * 0.7f,
      "L" => Vector3.one * 2f,
      "B" => Vector3.one * 3f,
      _ => Vector3.one,
    };
  }
}
