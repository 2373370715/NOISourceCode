using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02000CF8 RID: 3320
public class CarePackage : StateMachineComponent<CarePackage.SMInstance>
{
	// Token: 0x06003FB5 RID: 16309 RVA: 0x000CDD45 File Offset: 0x000CBF45
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.smi.StartSM();
		if (this.info != null)
		{
			this.SetAnimToInfo();
		}
		this.reactable = this.CreateReactable();
	}

	// Token: 0x06003FB6 RID: 16310 RVA: 0x002467F8 File Offset: 0x002449F8
	public Reactable CreateReactable()
	{
		return new EmoteReactable(base.gameObject, "UpgradeFX", Db.Get().ChoreTypes.Emote, 15, 8, 0f, 20f, float.PositiveInfinity, 0f).SetEmote(Db.Get().Emotes.Minion.Cheer);
	}

	// Token: 0x06003FB7 RID: 16311 RVA: 0x000CDD72 File Offset: 0x000CBF72
	protected override void OnCleanUp()
	{
		this.reactable.Cleanup();
		base.OnCleanUp();
	}

	// Token: 0x06003FB8 RID: 16312 RVA: 0x000CDD85 File Offset: 0x000CBF85
	public void SetInfo(CarePackageInfo info)
	{
		this.info = info;
		this.SetAnimToInfo();
	}

	// Token: 0x06003FB9 RID: 16313 RVA: 0x000CDD94 File Offset: 0x000CBF94
	public void SetFacade(string facadeID)
	{
		this.facadeID = facadeID;
		this.SetAnimToInfo();
	}

	// Token: 0x06003FBA RID: 16314 RVA: 0x0024685C File Offset: 0x00244A5C
	private void SetAnimToInfo()
	{
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab("Meter".ToTag()), base.gameObject, null);
		GameObject prefab = Assets.GetPrefab(this.info.id);
		KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
		KBatchedAnimController component2 = prefab.GetComponent<KBatchedAnimController>();
		SymbolOverrideController component3 = prefab.GetComponent<SymbolOverrideController>();
		KBatchedAnimController component4 = gameObject.GetComponent<KBatchedAnimController>();
		component4.transform.SetLocalPosition(Vector3.forward);
		component4.AnimFiles = component2.AnimFiles;
		component4.isMovable = true;
		component4.animWidth = component2.animWidth;
		component4.animHeight = component2.animHeight;
		if (component3 != null)
		{
			SymbolOverrideController symbolOverrideController = SymbolOverrideControllerUtil.AddToPrefab(gameObject);
			foreach (SymbolOverrideController.SymbolEntry symbolEntry in component3.GetSymbolOverrides)
			{
				symbolOverrideController.AddSymbolOverride(symbolEntry.targetSymbol, symbolEntry.sourceSymbol, 0);
			}
		}
		component4.initialAnim = component2.initialAnim;
		component4.initialMode = KAnim.PlayMode.Loop;
		if (!string.IsNullOrEmpty(this.facadeID))
		{
			component4.SwapAnims(new KAnimFile[]
			{
				Db.GetEquippableFacades().Get(this.facadeID).AnimFile
			});
			base.GetComponentsInChildren<KBatchedAnimController>()[1].SetSymbolVisiblity("object", false);
		}
		KBatchedAnimTracker component5 = gameObject.GetComponent<KBatchedAnimTracker>();
		component5.controller = component;
		component5.symbol = new HashedString("snapTO_object");
		component5.offset = new Vector3(0f, 0.5f, 0f);
		gameObject.SetActive(true);
		component.SetSymbolVisiblity("snapTO_object", false);
		new KAnimLink(component, component4);
	}

	// Token: 0x06003FBB RID: 16315 RVA: 0x002469FC File Offset: 0x00244BFC
	private void SpawnContents()
	{
		if (this.info == null)
		{
			global::Debug.LogWarning("CarePackage has no data to spawn from. Probably a save from before the CarePackage info data was serialized.");
			return;
		}
		GameObject gameObject = null;
		GameObject prefab = Assets.GetPrefab(this.info.id);
		Element element = ElementLoader.GetElement(this.info.id.ToTag());
		Vector3 position = base.transform.position + Vector3.up / 2f;
		if (element == null && prefab != null)
		{
			int num = 0;
			while ((float)num < this.info.quantity)
			{
				gameObject = Util.KInstantiate(prefab, position);
				if (gameObject != null)
				{
					if (!this.facadeID.IsNullOrWhiteSpace())
					{
						EquippableFacade.AddFacadeToEquippable(gameObject.GetComponent<Equippable>(), this.facadeID);
					}
					gameObject.SetActive(true);
				}
				num++;
			}
		}
		else if (element != null)
		{
			float quantity = this.info.quantity;
			gameObject = element.substance.SpawnResource(position, quantity, element.defaultValues.temperature, byte.MaxValue, 0, false, true, false);
		}
		else
		{
			global::Debug.LogWarning("Can't find spawnable thing from tag " + this.info.id);
		}
		if (gameObject != null)
		{
			gameObject.SetActive(true);
		}
	}

	// Token: 0x04002C04 RID: 11268
	[Serialize]
	public CarePackageInfo info;

	// Token: 0x04002C05 RID: 11269
	private string facadeID;

	// Token: 0x04002C06 RID: 11270
	private Reactable reactable;

	// Token: 0x02000CF9 RID: 3321
	public class SMInstance : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.GameInstance
	{
		// Token: 0x06003FBD RID: 16317 RVA: 0x000CDDAB File Offset: 0x000CBFAB
		public SMInstance(CarePackage master) : base(master)
		{
		}

		// Token: 0x04002C07 RID: 11271
		public List<Chore> activeUseChores;
	}

	// Token: 0x02000CFA RID: 3322
	public class States : GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage>
	{
		// Token: 0x06003FBE RID: 16318 RVA: 0x00246B2C File Offset: 0x00244D2C
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.spawn;
			base.serializable = StateMachine.SerializeType.ParamsOnly;
			this.spawn.PlayAnim("portalbirth").OnAnimQueueComplete(this.open).ParamTransition<bool>(this.spawnedContents, this.pst, GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.IsTrue);
			this.open.PlayAnim("portalbirth_pst").QueueAnim("object_idle_loop", false, null).Exit(delegate(CarePackage.SMInstance smi)
			{
				smi.master.SpawnContents();
				this.spawnedContents.Set(true, smi, false);
			}).ScheduleGoTo(1f, this.pst);
			this.pst.PlayAnim("object_idle_pst").ScheduleGoTo(5f, this.destroy);
			this.destroy.Enter(delegate(CarePackage.SMInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x04002C08 RID: 11272
		public StateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.BoolParameter spawnedContents;

		// Token: 0x04002C09 RID: 11273
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State spawn;

		// Token: 0x04002C0A RID: 11274
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State open;

		// Token: 0x04002C0B RID: 11275
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State pst;

		// Token: 0x04002C0C RID: 11276
		public GameStateMachine<CarePackage.States, CarePackage.SMInstance, CarePackage, object>.State destroy;
	}
}
