using System;
using KSerialization;
using UnityEngine;

// Token: 0x02000C71 RID: 3185
[AddComponentMenu("KMonoBehaviour/scripts/Baggable")]
public class Baggable : KMonoBehaviour
{
	// Token: 0x06003C7E RID: 15486 RVA: 0x0023C25C File Offset: 0x0023A45C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.minionAnimOverride = Assets.GetAnim("anim_restrain_creature_kanim");
		Pickupable pickupable = base.gameObject.AddOrGet<Pickupable>();
		pickupable.workAnims = new HashedString[]
		{
			new HashedString("capture"),
			new HashedString("pickup")
		};
		pickupable.workAnimPlayMode = KAnim.PlayMode.Once;
		pickupable.workingPstComplete = null;
		pickupable.workingPstFailed = null;
		pickupable.overrideAnims = new KAnimFile[]
		{
			this.minionAnimOverride
		};
		pickupable.trackOnPickup = false;
		pickupable.useGunforPickup = this.useGunForPickup;
		pickupable.synchronizeAnims = false;
		pickupable.SetWorkTime(3f);
		if (this.mustStandOntopOfTrapForPickup)
		{
			pickupable.SetOffsets(new CellOffset[]
			{
				default(CellOffset),
				new CellOffset(0, -1)
			});
		}
		base.Subscribe<Baggable>(856640610, Baggable.OnStoreDelegate);
		if (base.transform.parent != null)
		{
			if (base.transform.parent.GetComponent<Trap>() != null || base.transform.parent.GetSMI<ReusableTrap.Instance>() != null)
			{
				base.GetComponent<KBatchedAnimController>().enabled = true;
			}
			if (base.transform.parent.GetComponent<EggIncubator>() != null)
			{
				this.wrangled = true;
			}
		}
		if (this.wrangled)
		{
			this.SetWrangled();
		}
	}

	// Token: 0x06003C7F RID: 15487 RVA: 0x0023C3B8 File Offset: 0x0023A5B8
	private void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage != null || (data != null && (bool)data))
		{
			base.gameObject.AddTag(GameTags.Creatures.Bagged);
			if (storage && storage.HasTag(GameTags.BaseMinion))
			{
				this.SetVisible(false);
				return;
			}
		}
		else
		{
			if (!this.keepWrangledNextTimeRemovedFromStorage)
			{
				this.Free();
			}
			this.keepWrangledNextTimeRemovedFromStorage = false;
		}
	}

	// Token: 0x06003C80 RID: 15488 RVA: 0x0023C428 File Offset: 0x0023A628
	private void SetVisible(bool visible)
	{
		KAnimControllerBase component = base.gameObject.GetComponent<KAnimControllerBase>();
		if (component != null && component.enabled != visible)
		{
			component.enabled = visible;
		}
		KSelectable component2 = base.gameObject.GetComponent<KSelectable>();
		if (component2 != null && component2.enabled != visible)
		{
			component2.enabled = visible;
		}
	}

	// Token: 0x06003C81 RID: 15489 RVA: 0x0023C480 File Offset: 0x0023A680
	public static string GetBaggedAnimName(GameObject baggableObject)
	{
		string result = "trussed";
		Pickupable pickupable = baggableObject.AddOrGet<Pickupable>();
		if (pickupable != null && pickupable.storage != null)
		{
			IBaggedStateAnimationInstructions component = pickupable.storage.GetComponent<IBaggedStateAnimationInstructions>();
			if (component != null)
			{
				string baggedAnimationName = component.GetBaggedAnimationName();
				if (baggedAnimationName != null)
				{
					result = baggedAnimationName;
				}
			}
		}
		return result;
	}

	// Token: 0x06003C82 RID: 15490 RVA: 0x0023C4D0 File Offset: 0x0023A6D0
	public void SetWrangled()
	{
		this.wrangled = true;
		Navigator component = base.GetComponent<Navigator>();
		if (component && component.IsValidNavType(NavType.Floor))
		{
			component.SetCurrentNavType(NavType.Floor);
		}
		base.gameObject.AddTag(GameTags.Creatures.Bagged);
		base.GetComponent<KAnimControllerBase>().Play(Baggable.GetBaggedAnimName(base.gameObject), KAnim.PlayMode.Loop, 1f, 0f);
	}

	// Token: 0x06003C83 RID: 15491 RVA: 0x000CB82C File Offset: 0x000C9A2C
	public void Free()
	{
		base.gameObject.RemoveTag(GameTags.Creatures.Bagged);
		this.wrangled = false;
		this.SetVisible(true);
	}

	// Token: 0x040029F3 RID: 10739
	[SerializeField]
	private KAnimFile minionAnimOverride;

	// Token: 0x040029F4 RID: 10740
	public bool mustStandOntopOfTrapForPickup;

	// Token: 0x040029F5 RID: 10741
	[Serialize]
	public bool wrangled;

	// Token: 0x040029F6 RID: 10742
	[Serialize]
	public bool keepWrangledNextTimeRemovedFromStorage;

	// Token: 0x040029F7 RID: 10743
	public bool useGunForPickup;

	// Token: 0x040029F8 RID: 10744
	private static readonly EventSystem.IntraObjectHandler<Baggable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Baggable>(delegate(Baggable component, object data)
	{
		component.OnStore(data);
	});

	// Token: 0x040029F9 RID: 10745
	public const string DEFAULT_BAGGED_ANIM_NAME = "trussed";
}
