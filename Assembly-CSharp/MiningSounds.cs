using System;
using FMODUnity;
using UnityEngine;

// Token: 0x02000AC4 RID: 2756
[AddComponentMenu("KMonoBehaviour/scripts/MiningSounds")]
public class MiningSounds : KMonoBehaviour
{
	// Token: 0x06003257 RID: 12887 RVA: 0x000C51CE File Offset: 0x000C33CE
	protected override void OnPrefabInit()
	{
		base.Subscribe<MiningSounds>(-1762453998, MiningSounds.OnStartMiningSoundDelegate);
		base.Subscribe<MiningSounds>(939543986, MiningSounds.OnStopMiningSoundDelegate);
	}

	// Token: 0x06003258 RID: 12888 RVA: 0x002105E0 File Offset: 0x0020E7E0
	private void OnStartMiningSound(object data)
	{
		if (this.miningSound == null)
		{
			Element element = data as Element;
			if (element != null)
			{
				string text = element.substance.GetMiningSound();
				if (text == null || text == "")
				{
					return;
				}
				text = "Mine_" + text;
				string sound = GlobalAssets.GetSound(text, false);
				this.miningSoundEvent = RuntimeManager.PathToEventReference(sound);
				if (!this.miningSoundEvent.IsNull)
				{
					this.loopingSounds.StartSound(this.miningSoundEvent);
				}
			}
		}
	}

	// Token: 0x06003259 RID: 12889 RVA: 0x000C51F2 File Offset: 0x000C33F2
	private void OnStopMiningSound(object data)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.StopSound(this.miningSoundEvent);
			this.miningSound = null;
		}
	}

	// Token: 0x0600325A RID: 12890 RVA: 0x000C5219 File Offset: 0x000C3419
	public void SetPercentComplete(float progress)
	{
		if (!this.miningSoundEvent.IsNull)
		{
			this.loopingSounds.SetParameter(this.miningSoundEvent, MiningSounds.HASH_PERCENTCOMPLETE, progress);
		}
	}

	// Token: 0x04002275 RID: 8821
	private static HashedString HASH_PERCENTCOMPLETE = "percentComplete";

	// Token: 0x04002276 RID: 8822
	[MyCmpGet]
	private LoopingSounds loopingSounds;

	// Token: 0x04002277 RID: 8823
	private FMODAsset miningSound;

	// Token: 0x04002278 RID: 8824
	private EventReference miningSoundEvent;

	// Token: 0x04002279 RID: 8825
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStartMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStartMiningSound(data);
	});

	// Token: 0x0400227A RID: 8826
	private static readonly EventSystem.IntraObjectHandler<MiningSounds> OnStopMiningSoundDelegate = new EventSystem.IntraObjectHandler<MiningSounds>(delegate(MiningSounds component, object data)
	{
		component.OnStopMiningSound(data);
	});
}
