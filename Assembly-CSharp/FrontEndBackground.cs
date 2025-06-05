using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001D2B RID: 7467
public class FrontEndBackground : UIDupeRandomizer
{
	// Token: 0x06009BEE RID: 39918 RVA: 0x003CEB58 File Offset: 0x003CCD58
	protected override void Start()
	{
		this.tuning = TuningData<FrontEndBackground.Tuning>.Get();
		base.Start();
		for (int i = 0; i < this.anims.Length; i++)
		{
			int minionIndex = i;
			KBatchedAnimController kbatchedAnimController = this.anims[i].minions[0];
			if (kbatchedAnimController.gameObject.activeInHierarchy)
			{
				kbatchedAnimController.onAnimComplete += delegate(HashedString name)
				{
					this.WaitForABit(minionIndex, name);
				};
				this.WaitForABit(i, HashedString.Invalid);
			}
		}
		this.dreckoController = base.transform.GetChild(0).Find("startmenu_drecko").GetComponent<KBatchedAnimController>();
		if (this.dreckoController.gameObject.activeInHierarchy)
		{
			this.dreckoController.enabled = false;
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minFirstDreckoInterval, this.tuning.maxFirstDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x06009BEF RID: 39919 RVA: 0x00109F59 File Offset: 0x00108159
	protected override void Update()
	{
		base.Update();
		this.UpdateDrecko();
	}

	// Token: 0x06009BF0 RID: 39920 RVA: 0x003CEC48 File Offset: 0x003CCE48
	private void UpdateDrecko()
	{
		if (this.dreckoController.gameObject.activeInHierarchy && Time.unscaledTime > this.nextDreckoTime)
		{
			this.dreckoController.enabled = true;
			this.dreckoController.Play("idle", KAnim.PlayMode.Once, 1f, 0f);
			this.nextDreckoTime = UnityEngine.Random.Range(this.tuning.minDreckoInterval, this.tuning.maxDreckoInterval) + Time.unscaledTime;
		}
	}

	// Token: 0x06009BF1 RID: 39921 RVA: 0x00109F67 File Offset: 0x00108167
	private void WaitForABit(int minion_idx, HashedString name)
	{
		base.StartCoroutine(this.WaitForTime(minion_idx));
	}

	// Token: 0x06009BF2 RID: 39922 RVA: 0x00109F77 File Offset: 0x00108177
	private IEnumerator WaitForTime(int minion_idx)
	{
		this.anims[minion_idx].lastWaitTime = UnityEngine.Random.Range(this.anims[minion_idx].minSecondsBetweenAction, this.anims[minion_idx].maxSecondsBetweenAction);
		yield return new WaitForSecondsRealtime(this.anims[minion_idx].lastWaitTime);
		base.GetNewBody(minion_idx);
		using (List<KBatchedAnimController>.Enumerator enumerator = this.anims[minion_idx].minions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KBatchedAnimController kbatchedAnimController = enumerator.Current;
				kbatchedAnimController.ClearQueue();
				kbatchedAnimController.Play(this.anims[minion_idx].anim_name, KAnim.PlayMode.Once, 1f, 0f);
			}
			yield break;
		}
		yield break;
	}

	// Token: 0x040079FC RID: 31228
	private KBatchedAnimController dreckoController;

	// Token: 0x040079FD RID: 31229
	private float nextDreckoTime;

	// Token: 0x040079FE RID: 31230
	private FrontEndBackground.Tuning tuning;

	// Token: 0x02001D2C RID: 7468
	public class Tuning : TuningData<FrontEndBackground.Tuning>
	{
		// Token: 0x040079FF RID: 31231
		public float minDreckoInterval;

		// Token: 0x04007A00 RID: 31232
		public float maxDreckoInterval;

		// Token: 0x04007A01 RID: 31233
		public float minFirstDreckoInterval;

		// Token: 0x04007A02 RID: 31234
		public float maxFirstDreckoInterval;
	}
}
