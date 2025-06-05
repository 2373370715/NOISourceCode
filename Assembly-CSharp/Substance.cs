using System;
using FMODUnity;
using Klei;
using UnityEngine;
using UnityEngine.Serialization;

// Token: 0x02001A21 RID: 6689
[Serializable]
public class Substance
{
	// Token: 0x06008B46 RID: 35654 RVA: 0x0036D630 File Offset: 0x0036B830
	public GameObject SpawnResource(Vector3 position, float mass, float temperature, byte disease_idx, int disease_count, bool prevent_merge = false, bool forceTemperature = false, bool manual_activation = false)
	{
		GameObject gameObject = null;
		PrimaryElement primaryElement = null;
		if (!prevent_merge)
		{
			int cell = Grid.PosToCell(position);
			GameObject gameObject2 = Grid.Objects[cell, 3];
			if (gameObject2 != null)
			{
				Pickupable component = gameObject2.GetComponent<Pickupable>();
				if (component != null)
				{
					Tag b = GameTagExtensions.Create(this.elementID);
					for (ObjectLayerListItem objectLayerListItem = component.objectLayerListItem; objectLayerListItem != null; objectLayerListItem = objectLayerListItem.nextItem)
					{
						KPrefabID component2 = objectLayerListItem.gameObject.GetComponent<KPrefabID>();
						if (component2.PrefabTag == b)
						{
							PrimaryElement component3 = component2.GetComponent<PrimaryElement>();
							if (component3.Mass + mass <= PrimaryElement.MAX_MASS)
							{
								gameObject = component2.gameObject;
								primaryElement = component3;
								temperature = SimUtil.CalculateFinalTemperature(primaryElement.Mass, primaryElement.Temperature, mass, temperature);
								position = gameObject.transform.GetPosition();
								break;
							}
						}
					}
				}
			}
		}
		if (gameObject == null)
		{
			gameObject = GameUtil.KInstantiate(Assets.GetPrefab(this.nameTag), Grid.SceneLayer.Ore, null, 0);
			primaryElement = gameObject.GetComponent<PrimaryElement>();
			primaryElement.Mass = mass;
		}
		else
		{
			global::Debug.Assert(primaryElement != null);
			Pickupable component4 = primaryElement.GetComponent<Pickupable>();
			if (component4 != null)
			{
				component4.TotalAmount += mass / primaryElement.MassPerUnit;
			}
			else
			{
				primaryElement.Mass += mass;
			}
		}
		primaryElement.Temperature = temperature;
		position.z = Grid.GetLayerZ(Grid.SceneLayer.Ore);
		gameObject.transform.SetPosition(position);
		if (!manual_activation)
		{
			this.ActivateSubstanceGameObject(gameObject, disease_idx, disease_count);
		}
		return gameObject;
	}

	// Token: 0x06008B47 RID: 35655 RVA: 0x000FFA73 File Offset: 0x000FDC73
	public void ActivateSubstanceGameObject(GameObject obj, byte disease_idx, int disease_count)
	{
		obj.SetActive(true);
		obj.GetComponent<PrimaryElement>().AddDisease(disease_idx, disease_count, "Substances.SpawnResource");
	}

	// Token: 0x06008B48 RID: 35656 RVA: 0x0036D7AC File Offset: 0x0036B9AC
	private void SetTexture(MaterialPropertyBlock block, string texture_name)
	{
		Texture texture = this.material.GetTexture(texture_name);
		if (texture != null)
		{
			this.propertyBlock.SetTexture(texture_name, texture);
		}
	}

	// Token: 0x06008B49 RID: 35657 RVA: 0x0036D7DC File Offset: 0x0036B9DC
	public void RefreshPropertyBlock()
	{
		if (this.propertyBlock == null)
		{
			this.propertyBlock = new MaterialPropertyBlock();
		}
		if (this.material != null)
		{
			this.SetTexture(this.propertyBlock, "_MainTex");
			float @float = this.material.GetFloat("_WorldUVScale");
			this.propertyBlock.SetFloat("_WorldUVScale", @float);
			if (ElementLoader.FindElementByHash(this.elementID).IsSolid)
			{
				this.SetTexture(this.propertyBlock, "_MainTex2");
				this.SetTexture(this.propertyBlock, "_HeightTex2");
				this.propertyBlock.SetFloat("_Frequency", this.material.GetFloat("_Frequency"));
				this.propertyBlock.SetColor("_ShineColour", this.material.GetColor("_ShineColour"));
				this.propertyBlock.SetColor("_ColourTint", this.material.GetColor("_ColourTint"));
			}
		}
	}

	// Token: 0x06008B4A RID: 35658 RVA: 0x000FFA8E File Offset: 0x000FDC8E
	internal AmbienceType GetAmbience()
	{
		if (this.audioConfig == null)
		{
			return AmbienceType.None;
		}
		return this.audioConfig.ambienceType;
	}

	// Token: 0x06008B4B RID: 35659 RVA: 0x000FFAA5 File Offset: 0x000FDCA5
	internal SolidAmbienceType GetSolidAmbience()
	{
		if (this.audioConfig == null)
		{
			return SolidAmbienceType.None;
		}
		return this.audioConfig.solidAmbienceType;
	}

	// Token: 0x06008B4C RID: 35660 RVA: 0x000FFABC File Offset: 0x000FDCBC
	internal string GetMiningSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.miningSound;
	}

	// Token: 0x06008B4D RID: 35661 RVA: 0x000FFAD7 File Offset: 0x000FDCD7
	internal string GetMiningBreakSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.miningBreakSound;
	}

	// Token: 0x06008B4E RID: 35662 RVA: 0x000FFAF2 File Offset: 0x000FDCF2
	internal string GetOreBumpSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.oreBumpSound;
	}

	// Token: 0x06008B4F RID: 35663 RVA: 0x000FFB0D File Offset: 0x000FDD0D
	internal string GetFloorEventAudioCategory()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.floorEventAudioCategory;
	}

	// Token: 0x06008B50 RID: 35664 RVA: 0x000FFB28 File Offset: 0x000FDD28
	internal string GetCreatureChewSound()
	{
		if (this.audioConfig == null)
		{
			return "";
		}
		return this.audioConfig.creatureChewSound;
	}

	// Token: 0x0400692E RID: 26926
	public string name;

	// Token: 0x0400692F RID: 26927
	public SimHashes elementID;

	// Token: 0x04006930 RID: 26928
	internal Tag nameTag;

	// Token: 0x04006931 RID: 26929
	public Color32 colour;

	// Token: 0x04006932 RID: 26930
	[FormerlySerializedAs("debugColour")]
	public Color32 uiColour;

	// Token: 0x04006933 RID: 26931
	[FormerlySerializedAs("overlayColour")]
	public Color32 conduitColour = Color.white;

	// Token: 0x04006934 RID: 26932
	[NonSerialized]
	internal bool renderedByWorld;

	// Token: 0x04006935 RID: 26933
	[NonSerialized]
	internal int idx;

	// Token: 0x04006936 RID: 26934
	public Material material;

	// Token: 0x04006937 RID: 26935
	public KAnimFile anim;

	// Token: 0x04006938 RID: 26936
	[SerializeField]
	internal bool showInEditor = true;

	// Token: 0x04006939 RID: 26937
	[NonSerialized]
	internal KAnimFile[] anims;

	// Token: 0x0400693A RID: 26938
	[NonSerialized]
	internal ElementsAudio.ElementAudioConfig audioConfig;

	// Token: 0x0400693B RID: 26939
	[NonSerialized]
	internal MaterialPropertyBlock propertyBlock;

	// Token: 0x0400693C RID: 26940
	public EventReference fallingStartSound;

	// Token: 0x0400693D RID: 26941
	public EventReference fallingStopSound;
}
