using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02002099 RID: 8345
[AddComponentMenu("KMonoBehaviour/scripts/TitleBarPortrait")]
public class TitleBarPortrait : KMonoBehaviour
{
	// Token: 0x0600B1FB RID: 45563 RVA: 0x00118409 File Offset: 0x00116609
	public void SetSaturation(bool saturated)
	{
		this.ImageObject.GetComponent<Image>().material = (saturated ? this.DefaultMaterial : this.DesatMaterial);
	}

	// Token: 0x0600B1FC RID: 45564 RVA: 0x0043B3BC File Offset: 0x004395BC
	public void SetPortrait(GameObject selectedTarget)
	{
		MinionIdentity component = selectedTarget.GetComponent<MinionIdentity>();
		if (component != null)
		{
			this.SetPortrait(component);
			return;
		}
		Building component2 = selectedTarget.GetComponent<Building>();
		if (component2 != null)
		{
			this.SetPortrait(component2.Def.GetUISprite("ui", false));
			return;
		}
		MeshRenderer componentInChildren = selectedTarget.GetComponentInChildren<MeshRenderer>();
		if (componentInChildren)
		{
			this.SetPortrait(Sprite.Create((Texture2D)componentInChildren.material.mainTexture, new Rect(0f, 0f, (float)componentInChildren.material.mainTexture.width, (float)componentInChildren.material.mainTexture.height), new Vector2(0.5f, 0.5f)));
		}
	}

	// Token: 0x0600B1FD RID: 45565 RVA: 0x0043B474 File Offset: 0x00439674
	public void SetPortrait(Sprite image)
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(true);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(true);
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(false);
		}
		if (image == null)
		{
			this.ClearPortrait();
			return;
		}
		this.ImageObject.GetComponent<Image>().sprite = image;
	}

	// Token: 0x0600B1FE RID: 45566 RVA: 0x0043B508 File Offset: 0x00439708
	private void SetPortrait(MinionIdentity identity)
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(true);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(false);
		}
		CrewPortrait component = base.GetComponent<CrewPortrait>();
		if (component != null)
		{
			component.SetIdentityObject(identity, true);
			return;
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(true);
			CrewPortrait.SetPortraitData(identity, this.AnimControllerObject.GetComponent<KBatchedAnimController>(), true);
		}
	}

	// Token: 0x0600B1FF RID: 45567 RVA: 0x0043B5A4 File Offset: 0x004397A4
	public void ClearPortrait()
	{
		if (this.PortraitShadow)
		{
			this.PortraitShadow.SetActive(false);
		}
		if (this.FaceObject)
		{
			this.FaceObject.SetActive(false);
		}
		if (this.ImageObject)
		{
			this.ImageObject.SetActive(false);
		}
		if (this.AnimControllerObject)
		{
			this.AnimControllerObject.SetActive(false);
		}
	}

	// Token: 0x04008C60 RID: 35936
	public GameObject FaceObject;

	// Token: 0x04008C61 RID: 35937
	public GameObject ImageObject;

	// Token: 0x04008C62 RID: 35938
	public GameObject PortraitShadow;

	// Token: 0x04008C63 RID: 35939
	public GameObject AnimControllerObject;

	// Token: 0x04008C64 RID: 35940
	public Material DefaultMaterial;

	// Token: 0x04008C65 RID: 35941
	public Material DesatMaterial;
}
