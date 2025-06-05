using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x020016D3 RID: 5843
[SerializationConfig(MemberSerialization.OptIn)]
[AddComponentMenu("KMonoBehaviour/scripts/OrbitalMechanics")]
public class OrbitalMechanics : KMonoBehaviour
{
	// Token: 0x0600788F RID: 30863 RVA: 0x000F3C69 File Offset: 0x000F1E69
	protected override void OnPrefabInit()
	{
		base.Subscribe<OrbitalMechanics>(-1298331547, this.OnClusterLocationChangedDelegate);
	}

	// Token: 0x06007890 RID: 30864 RVA: 0x0031FDB4 File Offset: 0x0031DFB4
	private void OnClusterLocationChanged(object data)
	{
		ClusterLocationChangedEvent clusterLocationChangedEvent = (ClusterLocationChangedEvent)data;
		this.UpdateLocation(clusterLocationChangedEvent.newLocation);
	}

	// Token: 0x06007891 RID: 30865 RVA: 0x0031FDD4 File Offset: 0x0031DFD4
	protected override void OnCleanUp()
	{
		if (this.orbitingObjects != null)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					Util.KDestroyGameObject(@ref.Get());
				}
			}
		}
	}

	// Token: 0x06007892 RID: 30866 RVA: 0x0031FE40 File Offset: 0x0031E040
	[ContextMenu("Rebuild")]
	private void Rebuild()
	{
		List<string> list = new List<string>();
		if (this.orbitingObjects != null)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					list.Add(@ref.Get().orbitalDBId);
					Util.KDestroyGameObject(@ref.Get());
				}
			}
			this.orbitingObjects = new List<Ref<OrbitalObject>>();
		}
		if (list.Count > 0)
		{
			for (int i = 0; i < list.Count; i++)
			{
				this.CreateOrbitalObject(list[i]);
			}
		}
	}

	// Token: 0x06007893 RID: 30867 RVA: 0x0031FEF8 File Offset: 0x0031E0F8
	private void UpdateLocation(AxialI location)
	{
		if (this.orbitingObjects.Count > 0)
		{
			foreach (Ref<OrbitalObject> @ref in this.orbitingObjects)
			{
				if (!@ref.Get().IsNullOrDestroyed())
				{
					Util.KDestroyGameObject(@ref.Get());
				}
			}
			this.orbitingObjects = new List<Ref<OrbitalObject>>();
		}
		ClusterGridEntity visibleEntityOfLayerAtCell = ClusterGrid.Instance.GetVisibleEntityOfLayerAtCell(location, EntityLayer.POI);
		if (visibleEntityOfLayerAtCell != null)
		{
			ArtifactPOIClusterGridEntity component = visibleEntityOfLayerAtCell.GetComponent<ArtifactPOIClusterGridEntity>();
			if (component != null)
			{
				ArtifactPOIStates.Instance smi = component.GetSMI<ArtifactPOIStates.Instance>();
				if (smi != null && smi.configuration.poiType.orbitalObject != null)
				{
					foreach (string orbit_db_name in smi.configuration.poiType.orbitalObject)
					{
						this.CreateOrbitalObject(orbit_db_name);
					}
				}
			}
			HarvestablePOIClusterGridEntity component2 = visibleEntityOfLayerAtCell.GetComponent<HarvestablePOIClusterGridEntity>();
			if (component2 != null)
			{
				HarvestablePOIStates.Instance smi2 = component2.GetSMI<HarvestablePOIStates.Instance>();
				if (smi2 != null && smi2.configuration.poiType.orbitalObject != null)
				{
					List<string> orbitalObject = smi2.configuration.poiType.orbitalObject;
					KRandom krandom = new KRandom();
					float num = smi2.poiCapacity / smi2.configuration.GetMaxCapacity() * (float)smi2.configuration.poiType.maxNumOrbitingObjects;
					int num2 = 0;
					while ((float)num2 < num)
					{
						int index = krandom.Next(orbitalObject.Count);
						this.CreateOrbitalObject(orbitalObject[index]);
						num2++;
					}
					return;
				}
			}
		}
		else
		{
			Clustercraft component3 = base.GetComponent<Clustercraft>();
			if (component3 != null)
			{
				if (component3.GetOrbitAsteroid() != null || component3.Status == Clustercraft.CraftStatus.Launching)
				{
					this.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.orbit.Id);
					return;
				}
				if (component3.Status == Clustercraft.CraftStatus.Landing)
				{
					this.CreateOrbitalObject(Db.Get().OrbitalTypeCategories.landed.Id);
				}
			}
		}
	}

	// Token: 0x06007894 RID: 30868 RVA: 0x00320128 File Offset: 0x0031E328
	public void CreateOrbitalObject(string orbit_db_name)
	{
		WorldContainer component = base.GetComponent<WorldContainer>();
		GameObject gameObject = Util.KInstantiate(Assets.GetPrefab(OrbitalBGConfig.ID), base.gameObject, null);
		OrbitalObject component2 = gameObject.GetComponent<OrbitalObject>();
		component2.Init(orbit_db_name, component, this.orbitingObjects);
		gameObject.SetActive(true);
		this.orbitingObjects.Add(new Ref<OrbitalObject>(component2));
	}

	// Token: 0x04005A90 RID: 23184
	[Serialize]
	private List<Ref<OrbitalObject>> orbitingObjects = new List<Ref<OrbitalObject>>();

	// Token: 0x04005A91 RID: 23185
	private EventSystem.IntraObjectHandler<OrbitalMechanics> OnClusterLocationChangedDelegate = new EventSystem.IntraObjectHandler<OrbitalMechanics>(delegate(OrbitalMechanics cmp, object data)
	{
		cmp.OnClusterLocationChanged(data);
	});
}
