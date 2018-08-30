using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectCtrl : EffectCtrlBase {

	ParticleSystem[] ps;

	void Awake () {
		ps = gameObject.GetComponentsInChildren<ParticleSystem> ();
		gameObject.SetActive (false);
	}

	public override void Play ()
	{
		if (!gameObject.activeSelf) {
			gameObject.SetActive (true);
		}
		for (int i = 0; i < ps.Length; i++) {
			ps [i].Play ();
		}
	}

	public override void Stop ()
	{
		for (int i = 0; i < ps.Length; i++) {
			ps [i].Stop ();
		}
	}

	public override void Destroy ()
	{
		Destroy (gameObject);
	}

}
