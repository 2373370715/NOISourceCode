using System;
using KSerialization;

// Token: 0x02001CD3 RID: 7379
[SerializationConfig(MemberSerialization.OptIn)]
public class CreatureBait : StateMachineComponent<CreatureBait.StatesInstance>
{
	// Token: 0x060099E8 RID: 39400 RVA: 0x000B74E6 File Offset: 0x000B56E6
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
	}

	// Token: 0x060099E9 RID: 39401 RVA: 0x003C5B08 File Offset: 0x003C3D08
	protected override void OnSpawn()
	{
		base.OnSpawn();
		Tag[] constructionElements = base.GetComponent<Deconstructable>().constructionElements;
		this.baitElement = ((constructionElements.Length > 1) ? constructionElements[1] : constructionElements[0]);
		base.gameObject.GetSMI<Lure.Instance>().SetActiveLures(new Tag[]
		{
			this.baitElement
		});
		base.smi.StartSM();
	}

	// Token: 0x04007819 RID: 30745
	[Serialize]
	public Tag baitElement;

	// Token: 0x02001CD4 RID: 7380
	public class StatesInstance : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.GameInstance
	{
		// Token: 0x060099EB RID: 39403 RVA: 0x00108843 File Offset: 0x00106A43
		public StatesInstance(CreatureBait master) : base(master)
		{
		}
	}

	// Token: 0x02001CD5 RID: 7381
	public class States : GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait>
	{
		// Token: 0x060099EC RID: 39404 RVA: 0x003C5B74 File Offset: 0x003C3D74
		public override void InitializeStates(out StateMachine.BaseState default_state)
		{
			default_state = this.idle;
			this.idle.ToggleMainStatusItem(Db.Get().BuildingStatusItems.Baited, null).Enter(delegate(CreatureBait.StatesInstance smi)
			{
				KAnim.Build build = ElementLoader.FindElementByName(smi.master.baitElement.ToString()).substance.anim.GetData().build;
				KAnim.Build.Symbol symbol = build.GetSymbol(new KAnimHashedString(build.name));
				HashedString target_symbol = "snapTo_bait";
				smi.GetComponent<SymbolOverrideController>().AddSymbolOverride(target_symbol, symbol, 0);
			}).TagTransition(GameTags.LureUsed, this.destroy, false);
			this.destroy.PlayAnim("use").EventHandler(GameHashes.AnimQueueComplete, delegate(CreatureBait.StatesInstance smi)
			{
				Util.KDestroyGameObject(smi.master.gameObject);
			});
		}

		// Token: 0x0400781A RID: 30746
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State idle;

		// Token: 0x0400781B RID: 30747
		public GameStateMachine<CreatureBait.States, CreatureBait.StatesInstance, CreatureBait, object>.State destroy;
	}
}
