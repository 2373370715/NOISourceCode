using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001A48 RID: 6728
[AddComponentMenu("KMonoBehaviour/scripts/Trappable")]
public class Trappable : KMonoBehaviour, IGameObjectEffectDescriptor
{
	// Token: 0x06008C34 RID: 35892 RVA: 0x00100370 File Offset: 0x000FE570
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.Register();
		this.OnCellChange();
	}

	// Token: 0x06008C35 RID: 35893 RVA: 0x00100384 File Offset: 0x000FE584
	protected override void OnCleanUp()
	{
		this.Unregister();
		base.OnCleanUp();
	}

	// Token: 0x06008C36 RID: 35894 RVA: 0x0037103C File Offset: 0x0036F23C
	private void OnCellChange()
	{
		int cell = Grid.PosToCell(this);
		GameScenePartitioner.Instance.TriggerEvent(cell, GameScenePartitioner.Instance.trapsLayer, this);
	}

	// Token: 0x06008C37 RID: 35895 RVA: 0x00100392 File Offset: 0x000FE592
	protected override void OnCmpEnable()
	{
		base.OnCmpEnable();
		this.Register();
	}

	// Token: 0x06008C38 RID: 35896 RVA: 0x001003A0 File Offset: 0x000FE5A0
	protected override void OnCmpDisable()
	{
		this.Unregister();
		base.OnCmpDisable();
	}

	// Token: 0x06008C39 RID: 35897 RVA: 0x00371068 File Offset: 0x0036F268
	private void Register()
	{
		if (this.registered)
		{
			return;
		}
		base.Subscribe<Trappable>(856640610, Trappable.OnStoreDelegate);
		Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange), "Trappable.Register");
		this.registered = true;
	}

	// Token: 0x06008C3A RID: 35898 RVA: 0x001003AE File Offset: 0x000FE5AE
	private void Unregister()
	{
		if (!this.registered)
		{
			return;
		}
		base.Unsubscribe<Trappable>(856640610, Trappable.OnStoreDelegate, false);
		Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(base.transform, new System.Action(this.OnCellChange));
		this.registered = false;
	}

	// Token: 0x06008C3B RID: 35899 RVA: 0x001003ED File Offset: 0x000FE5ED
	public List<Descriptor> GetDescriptors(GameObject go)
	{
		return new List<Descriptor>
		{
			new Descriptor(UI.BUILDINGEFFECTS.CAPTURE_METHOD_TRAP, UI.BUILDINGEFFECTS.TOOLTIPS.CAPTURE_METHOD_TRAP, Descriptor.DescriptorType.Effect, false)
		};
	}

	// Token: 0x06008C3C RID: 35900 RVA: 0x003710B8 File Offset: 0x0036F2B8
	public void OnStore(object data)
	{
		Storage storage = data as Storage;
		if (storage && (storage.GetComponent<Trap>() != null || storage.GetSMI<ReusableTrap.Instance>() != null))
		{
			base.gameObject.AddTag(GameTags.Trapped);
			return;
		}
		base.gameObject.RemoveTag(GameTags.Trapped);
	}

	// Token: 0x040069DE RID: 27102
	private bool registered;

	// Token: 0x040069DF RID: 27103
	private static readonly EventSystem.IntraObjectHandler<Trappable> OnStoreDelegate = new EventSystem.IntraObjectHandler<Trappable>(delegate(Trappable component, object data)
	{
		component.OnStore(data);
	});
}
