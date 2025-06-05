using System;
using UnityEngine;

// Token: 0x02000930 RID: 2352
[Serializable]
public class AnimEvent
{
	// Token: 0x17000137 RID: 311
	// (get) Token: 0x06002940 RID: 10560 RVA: 0x000BF49F File Offset: 0x000BD69F
	// (set) Token: 0x06002941 RID: 10561 RVA: 0x000BF4A7 File Offset: 0x000BD6A7
	[SerializeField]
	public string name { get; private set; }

	// Token: 0x17000138 RID: 312
	// (get) Token: 0x06002942 RID: 10562 RVA: 0x000BF4B0 File Offset: 0x000BD6B0
	// (set) Token: 0x06002943 RID: 10563 RVA: 0x000BF4B8 File Offset: 0x000BD6B8
	[SerializeField]
	public string file { get; private set; }

	// Token: 0x17000139 RID: 313
	// (get) Token: 0x06002944 RID: 10564 RVA: 0x000BF4C1 File Offset: 0x000BD6C1
	// (set) Token: 0x06002945 RID: 10565 RVA: 0x000BF4C9 File Offset: 0x000BD6C9
	[SerializeField]
	public int frame { get; private set; }

	// Token: 0x06002946 RID: 10566 RVA: 0x000AA024 File Offset: 0x000A8224
	public AnimEvent()
	{
	}

	// Token: 0x06002947 RID: 10567 RVA: 0x001E2080 File Offset: 0x001E0280
	public AnimEvent(string file, string name, int frame)
	{
		this.file = ((file == "") ? null : file);
		if (this.file != null)
		{
			this.fileHash = new KAnimHashedString(this.file);
		}
		this.name = name;
		this.frame = frame;
	}

	// Token: 0x06002948 RID: 10568 RVA: 0x001E20D4 File Offset: 0x001E02D4
	public void Play(AnimEventManager.EventPlayerData behaviour)
	{
		if (this.IsFilteredOut(behaviour))
		{
			return;
		}
		if (behaviour.previousFrame < behaviour.currentFrame)
		{
			if (behaviour.previousFrame < this.frame && behaviour.currentFrame >= this.frame)
			{
				this.OnPlay(behaviour);
				return;
			}
		}
		else if (behaviour.previousFrame > behaviour.currentFrame && (behaviour.previousFrame < this.frame || this.frame <= behaviour.currentFrame))
		{
			this.OnPlay(behaviour);
		}
	}

	// Token: 0x06002949 RID: 10569 RVA: 0x000AA038 File Offset: 0x000A8238
	private void DebugAnimEvent(string ev_name, AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x0600294A RID: 10570 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnPlay(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void OnUpdate(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x000AA038 File Offset: 0x000A8238
	public virtual void Stop(AnimEventManager.EventPlayerData behaviour)
	{
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x000BF4D2 File Offset: 0x000BD6D2
	protected bool IsFilteredOut(AnimEventManager.EventPlayerData behaviour)
	{
		return this.file != null && !behaviour.controller.HasAnimationFile(this.fileHash);
	}

	// Token: 0x04001C10 RID: 7184
	[SerializeField]
	private KAnimHashedString fileHash;

	// Token: 0x04001C12 RID: 7186
	public bool OnExit;
}
