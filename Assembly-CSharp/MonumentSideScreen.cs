using System;
using System.Collections.Generic;
using Database;
using UnityEngine;

// Token: 0x02001FF1 RID: 8177
public class MonumentSideScreen : SideScreenContent
{
	// Token: 0x0600ACDA RID: 44250 RVA: 0x00114C68 File Offset: 0x00112E68
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<MonumentPart>() != null;
	}

	// Token: 0x0600ACDB RID: 44251 RVA: 0x0041FF4C File Offset: 0x0041E14C
	protected override void OnSpawn()
	{
		base.OnSpawn();
		this.debugVictoryButton.onClick += delegate()
		{
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Thriving.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Clothe8Dupes.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.Build4NatureReserves.Id);
			SaveGame.Instance.GetComponent<ColonyAchievementTracker>().DebugTriggerAchievement(Db.Get().ColonyAchievements.ReachedSpace.Id);
			GameScheduler.Instance.Schedule("ForceCheckAchievements", 0.1f, delegate(object data)
			{
				Game.Instance.Trigger(395452326, null);
			}, null, null);
		};
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.flipButton.onClick += delegate()
		{
			this.target.GetComponent<Rotatable>().Rotate();
		};
	}

	// Token: 0x0600ACDC RID: 44252 RVA: 0x0041FFC8 File Offset: 0x0041E1C8
	public override void SetTarget(GameObject target)
	{
		base.SetTarget(target);
		this.target = target.GetComponent<MonumentPart>();
		this.debugVictoryButton.gameObject.SetActive(DebugHandler.InstantBuildMode && this.target.part == MonumentPartResource.Part.Top);
		this.GenerateStateButtons();
	}

	// Token: 0x0600ACDD RID: 44253 RVA: 0x00420018 File Offset: 0x0041E218
	public void GenerateStateButtons()
	{
		for (int i = this.buttons.Count - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.buttons[i]);
		}
		this.buttons.Clear();
		using (List<MonumentPartResource>.Enumerator enumerator = Db.GetMonumentParts().GetParts(this.target.part).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				MonumentPartResource state = enumerator.Current;
				GameObject gameObject = Util.KInstantiateUI(this.stateButtonPrefab, this.buttonContainer.gameObject, true);
				string state2 = state.State;
				string symbolName = state.SymbolName;
				gameObject.GetComponent<KButton>().onClick += delegate()
				{
					this.target.SetState(state.Id);
				};
				this.buttons.Add(gameObject);
				gameObject.GetComponent<KButton>().fgImage.sprite = Def.GetUISpriteFromMultiObjectAnim(state.AnimFile, state2, false, symbolName);
			}
		}
	}

	// Token: 0x04008813 RID: 34835
	private MonumentPart target;

	// Token: 0x04008814 RID: 34836
	public KButton debugVictoryButton;

	// Token: 0x04008815 RID: 34837
	public KButton flipButton;

	// Token: 0x04008816 RID: 34838
	public GameObject stateButtonPrefab;

	// Token: 0x04008817 RID: 34839
	private List<GameObject> buttons = new List<GameObject>();

	// Token: 0x04008818 RID: 34840
	[SerializeField]
	private RectTransform buttonContainer;
}
