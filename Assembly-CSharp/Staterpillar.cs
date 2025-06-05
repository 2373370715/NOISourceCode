using System;
using System.Collections.Generic;
using Klei.AI;
using KSerialization;
using STRINGS;
using UnityEngine;

// Token: 0x02001158 RID: 4440
public class Staterpillar : KMonoBehaviour
{
	// Token: 0x06005AA3 RID: 23203 RVA: 0x000DF63A File Offset: 0x000DD83A
	protected override void OnPrefabInit()
	{
		this.dummyElement = new List<Tag>
		{
			SimHashes.Unobtanium.CreateTag()
		};
		this.connectorDef = Assets.GetBuildingDef(this.connectorDefId);
	}

	// Token: 0x06005AA4 RID: 23204 RVA: 0x000DF668 File Offset: 0x000DD868
	public void SpawnConnectorBuilding(int targetCell)
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			this.SpawnGenerator(targetCell);
			return;
		}
		this.SpawnConduitConnector(targetCell);
	}

	// Token: 0x06005AA5 RID: 23205 RVA: 0x002A4018 File Offset: 0x002A2218
	public void DestroyOrphanedConnectorBuilding()
	{
		KPrefabID building = this.GetConnectorBuilding();
		if (building != null)
		{
			this.connectorRef.Set(null);
			this.cachedGenerator = null;
			this.cachedConduitDispenser = null;
			GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Connector building", delegate(object o)
			{
				if (building != null)
				{
					Util.KDestroyGameObject(building.gameObject);
				}
			}, null, null);
		}
	}

	// Token: 0x06005AA6 RID: 23206 RVA: 0x000DF683 File Offset: 0x000DD883
	public void EnableConnector()
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			this.EnableGenerator();
			return;
		}
		this.EnableConduitConnector();
	}

	// Token: 0x06005AA7 RID: 23207 RVA: 0x000DF69C File Offset: 0x000DD89C
	public bool IsConnectorBuildingSpawned()
	{
		return this.GetConnectorBuilding() != null;
	}

	// Token: 0x06005AA8 RID: 23208 RVA: 0x000DF6AA File Offset: 0x000DD8AA
	public bool IsConnected()
	{
		if (this.conduitLayer == ObjectLayer.Wire)
		{
			return this.GetGenerator().CircuitID != ushort.MaxValue;
		}
		return this.GetConduitDispenser().IsConnected;
	}

	// Token: 0x06005AA9 RID: 23209 RVA: 0x000DF6D7 File Offset: 0x000DD8D7
	public KPrefabID GetConnectorBuilding()
	{
		return this.connectorRef.Get();
	}

	// Token: 0x06005AAA RID: 23210 RVA: 0x002A4080 File Offset: 0x002A2280
	private void SpawnConduitConnector(int targetCell)
	{
		if (this.GetConduitDispenser() == null)
		{
			GameObject gameObject = this.connectorDef.Build(targetCell, Orientation.R180, null, this.dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature, true, -1f);
			this.connectorRef = new Ref<KPrefabID>(gameObject.GetComponent<KPrefabID>());
			gameObject.SetActive(true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
		}
	}

	// Token: 0x06005AAB RID: 23211 RVA: 0x000DF6E4 File Offset: 0x000DD8E4
	private void EnableConduitConnector()
	{
		ConduitDispenser conduitDispenser = this.GetConduitDispenser();
		conduitDispenser.GetComponent<BuildingCellVisualizer>().enabled = true;
		conduitDispenser.storage = base.GetComponent<Storage>();
		conduitDispenser.SetOnState(true);
	}

	// Token: 0x06005AAC RID: 23212 RVA: 0x002A40EC File Offset: 0x002A22EC
	public ConduitDispenser GetConduitDispenser()
	{
		if (this.cachedConduitDispenser == null)
		{
			KPrefabID kprefabID = this.connectorRef.Get();
			if (kprefabID != null)
			{
				this.cachedConduitDispenser = kprefabID.GetComponent<ConduitDispenser>();
			}
		}
		return this.cachedConduitDispenser;
	}

	// Token: 0x06005AAD RID: 23213 RVA: 0x002A4130 File Offset: 0x002A2330
	private void DestroyOrphanedConduitDispenserBuilding()
	{
		ConduitDispenser dispenser = this.GetConduitDispenser();
		if (dispenser != null)
		{
			this.connectorRef.Set(null);
			GameScheduler.Instance.ScheduleNextFrame("Destroy Staterpillar Dispenser", delegate(object o)
			{
				if (dispenser != null)
				{
					Util.KDestroyGameObject(dispenser.gameObject);
				}
			}, null, null);
		}
	}

	// Token: 0x06005AAE RID: 23214 RVA: 0x002A4188 File Offset: 0x002A2388
	private void SpawnGenerator(int targetCell)
	{
		StaterpillarGenerator generator = this.GetGenerator();
		GameObject gameObject = null;
		if (generator != null)
		{
			gameObject = generator.gameObject;
		}
		if (!gameObject)
		{
			gameObject = this.connectorDef.Build(targetCell, Orientation.R180, null, this.dummyElement, base.gameObject.GetComponent<PrimaryElement>().Temperature, true, -1f);
			StaterpillarGenerator component = gameObject.GetComponent<StaterpillarGenerator>();
			component.parent = new Ref<Staterpillar>(this);
			this.connectorRef = new Ref<KPrefabID>(component.GetComponent<KPrefabID>());
			gameObject.SetActive(true);
			gameObject.GetComponent<BuildingCellVisualizer>().enabled = false;
			component.enabled = false;
		}
		Attributes attributes = gameObject.gameObject.GetAttributes();
		bool flag = base.gameObject.GetSMI<WildnessMonitor.Instance>().wildness.value > 0f;
		if (flag)
		{
			attributes.Add(this.wildMod);
		}
		bool flag2 = base.gameObject.GetComponent<Effects>().HasEffect("Unhappy");
		CreatureCalorieMonitor.Instance smi = base.gameObject.GetSMI<CreatureCalorieMonitor.Instance>();
		if (smi.IsHungry() || flag2)
		{
			float calories0to = smi.GetCalories0to1();
			float num = 1f;
			if (calories0to <= 0f)
			{
				num = (flag ? 0.1f : 0.025f);
			}
			else if (calories0to <= 0.3f)
			{
				num = 0.5f;
			}
			else if (calories0to <= 0.5f)
			{
				num = 0.75f;
			}
			if (num < 1f)
			{
				float num2;
				if (flag)
				{
					num2 = Mathf.Lerp(0f, 25f, 1f - num);
				}
				else
				{
					num2 = (1f - num) * 100f;
				}
				AttributeModifier modifier = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -num2, BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.HUNGRY, false, false, true);
				attributes.Add(modifier);
			}
		}
	}

	// Token: 0x06005AAF RID: 23215 RVA: 0x000DF70A File Offset: 0x000DD90A
	private void EnableGenerator()
	{
		StaterpillarGenerator generator = this.GetGenerator();
		generator.enabled = true;
		generator.GetComponent<BuildingCellVisualizer>().enabled = true;
	}

	// Token: 0x06005AB0 RID: 23216 RVA: 0x002A4348 File Offset: 0x002A2548
	public StaterpillarGenerator GetGenerator()
	{
		if (this.cachedGenerator == null)
		{
			KPrefabID kprefabID = this.connectorRef.Get();
			if (kprefabID != null)
			{
				this.cachedGenerator = kprefabID.GetComponent<StaterpillarGenerator>();
			}
		}
		return this.cachedGenerator;
	}

	// Token: 0x0400408A RID: 16522
	public ObjectLayer conduitLayer;

	// Token: 0x0400408B RID: 16523
	public string connectorDefId;

	// Token: 0x0400408C RID: 16524
	private IList<Tag> dummyElement;

	// Token: 0x0400408D RID: 16525
	private BuildingDef connectorDef;

	// Token: 0x0400408E RID: 16526
	[Serialize]
	private Ref<KPrefabID> connectorRef = new Ref<KPrefabID>();

	// Token: 0x0400408F RID: 16527
	private AttributeModifier wildMod = new AttributeModifier(Db.Get().Attributes.GeneratorOutput.Id, -75f, BUILDINGS.PREFABS.STATERPILLARGENERATOR.MODIFIERS.WILD, false, false, true);

	// Token: 0x04004090 RID: 16528
	private ConduitDispenser cachedConduitDispenser;

	// Token: 0x04004091 RID: 16529
	private StaterpillarGenerator cachedGenerator;
}
