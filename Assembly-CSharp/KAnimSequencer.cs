using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000947 RID: 2375
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/KAnimSequencer")]
public class KAnimSequencer : KMonoBehaviour, ISaveLoadable
{
	// Token: 0x06002A17 RID: 10775 RVA: 0x000BFD92 File Offset: 0x000BDF92
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.kbac = base.GetComponent<KBatchedAnimController>();
		this.mb = base.GetComponent<MinionBrain>();
		if (this.autoRun)
		{
			this.PlaySequence();
		}
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x000BFDC0 File Offset: 0x000BDFC0
	public void Reset()
	{
		this.currentIndex = 0;
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x001E4DC4 File Offset: 0x001E2FC4
	public void PlaySequence()
	{
		if (this.sequence != null && this.sequence.Length != 0)
		{
			if (this.mb != null)
			{
				this.mb.Suspend("AnimSequencer");
			}
			this.kbac.onAnimComplete += this.PlayNext;
			this.PlayNext(null);
		}
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x001E4E24 File Offset: 0x001E3024
	private void PlayNext(HashedString name)
	{
		if (this.sequence.Length > this.currentIndex)
		{
			this.kbac.Play(new HashedString(this.sequence[this.currentIndex].anim), this.sequence[this.currentIndex].mode, this.sequence[this.currentIndex].speed, 0f);
			this.currentIndex++;
			return;
		}
		this.kbac.onAnimComplete -= this.PlayNext;
		if (this.mb != null)
		{
			this.mb.Resume("AnimSequencer");
		}
	}

	// Token: 0x04001C92 RID: 7314
	[Serialize]
	public bool autoRun;

	// Token: 0x04001C93 RID: 7315
	[Serialize]
	public KAnimSequencer.KAnimSequence[] sequence = new KAnimSequencer.KAnimSequence[0];

	// Token: 0x04001C94 RID: 7316
	private int currentIndex;

	// Token: 0x04001C95 RID: 7317
	private KBatchedAnimController kbac;

	// Token: 0x04001C96 RID: 7318
	private MinionBrain mb;

	// Token: 0x02000948 RID: 2376
	[SerializationConfig(MemberSerialization.OptOut)]
	[Serializable]
	public class KAnimSequence
	{
		// Token: 0x04001C97 RID: 7319
		public string anim;

		// Token: 0x04001C98 RID: 7320
		public float speed = 1f;

		// Token: 0x04001C99 RID: 7321
		public KAnim.PlayMode mode = KAnim.PlayMode.Once;
	}
}
