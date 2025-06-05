using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002019 RID: 8217
public class RelatedEntitiesSideScreen : SideScreenContent, ISim1000ms
{
	// Token: 0x0600ADED RID: 44525 RVA: 0x00115761 File Offset: 0x00113961
	protected override void OnShow(bool show)
	{
		base.OnShow(show);
		this.rowPrefab.SetActive(false);
		if (show)
		{
			this.RefreshOptions(null);
		}
	}

	// Token: 0x0600ADEE RID: 44526 RVA: 0x00115780 File Offset: 0x00113980
	public override bool IsValidForTarget(GameObject target)
	{
		return target.GetComponent<IRelatedEntities>() != null;
	}

	// Token: 0x0600ADEF RID: 44527 RVA: 0x0011578B File Offset: 0x0011398B
	public override void SetTarget(GameObject target)
	{
		this.target = target;
		this.targetRelatedEntitiesComponent = target.GetComponent<IRelatedEntities>();
		this.RefreshOptions(null);
		this.uiRefreshSubHandle = Game.Instance.Subscribe(1980521255, new Action<object>(this.RefreshOptions));
	}

	// Token: 0x0600ADF0 RID: 44528 RVA: 0x001157C8 File Offset: 0x001139C8
	public override void ClearTarget()
	{
		if (this.uiRefreshSubHandle != -1 && this.targetRelatedEntitiesComponent != null)
		{
			Game.Instance.Unsubscribe(this.uiRefreshSubHandle);
			this.uiRefreshSubHandle = -1;
		}
	}

	// Token: 0x0600ADF1 RID: 44529 RVA: 0x00424AEC File Offset: 0x00422CEC
	private void RefreshOptions(object data = null)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		this.ClearRows();
		foreach (KSelectable entity in this.targetRelatedEntitiesComponent.GetRelatedEntities())
		{
			this.AddRow(entity);
		}
	}

	// Token: 0x0600ADF2 RID: 44530 RVA: 0x00424B58 File Offset: 0x00422D58
	private void ClearRows()
	{
		for (int i = this.rowContainer.childCount - 1; i >= 0; i--)
		{
			Util.KDestroyGameObject(this.rowContainer.GetChild(i));
		}
		this.rows.Clear();
	}

	// Token: 0x0600ADF3 RID: 44531 RVA: 0x00424B9C File Offset: 0x00422D9C
	private void AddRow(KSelectable entity)
	{
		GameObject gameObject = Util.KInstantiateUI(this.rowPrefab, this.rowContainer.gameObject, true);
		gameObject.GetComponent<KButton>().onClick += delegate()
		{
			SelectTool.Instance.SelectAndFocus(entity.transform.position, entity);
		};
		HierarchyReferences component = gameObject.GetComponent<HierarchyReferences>();
		component.GetReference<LocText>("label").SetText((SelectTool.Instance.selected == entity) ? ("<b>" + entity.GetProperName() + "</b>") : entity.GetProperName());
		component.GetReference<Image>("icon").sprite = Def.GetUISprite(entity.gameObject, "ui", false).first;
		this.rows.Add(entity, gameObject);
		this.RefreshMainStatus(entity);
	}

	// Token: 0x0600ADF4 RID: 44532 RVA: 0x00424C84 File Offset: 0x00422E84
	private void RefreshMainStatus(KSelectable entity)
	{
		if (entity.IsNullOrDestroyed())
		{
			return;
		}
		if (!this.rows.ContainsKey(entity))
		{
			return;
		}
		HierarchyReferences component = this.rows[entity].GetComponent<HierarchyReferences>();
		StatusItemGroup.Entry statusItem = entity.GetStatusItem(Db.Get().StatusItemCategories.Main);
		LocText reference = component.GetReference<LocText>("status");
		if (statusItem.data != null)
		{
			reference.gameObject.SetActive(true);
			reference.SetText(statusItem.item.GetName(statusItem.data));
			return;
		}
		reference.gameObject.SetActive(false);
		reference.SetText("");
	}

	// Token: 0x0600ADF5 RID: 44533 RVA: 0x00424D20 File Offset: 0x00422F20
	public void Sim1000ms(float dt)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		foreach (KeyValuePair<KSelectable, GameObject> keyValuePair in this.rows)
		{
			this.RefreshMainStatus(keyValuePair.Key);
		}
	}

	// Token: 0x040088ED RID: 35053
	private GameObject target;

	// Token: 0x040088EE RID: 35054
	private IRelatedEntities targetRelatedEntitiesComponent;

	// Token: 0x040088EF RID: 35055
	public GameObject rowPrefab;

	// Token: 0x040088F0 RID: 35056
	public RectTransform rowContainer;

	// Token: 0x040088F1 RID: 35057
	public Dictionary<KSelectable, GameObject> rows = new Dictionary<KSelectable, GameObject>();

	// Token: 0x040088F2 RID: 35058
	private int uiRefreshSubHandle = -1;
}
