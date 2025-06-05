using System;
using STRINGS;

// Token: 0x0200192E RID: 6446
public class ClustercraftInteriorDoor : KMonoBehaviour, ISidescreenButtonControl
{
	// Token: 0x170008A6 RID: 2214
	// (get) Token: 0x060085E4 RID: 34276 RVA: 0x000FC76C File Offset: 0x000FA96C
	public string SidescreenButtonText
	{
		get
		{
			return UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL;
		}
	}

	// Token: 0x170008A7 RID: 2215
	// (get) Token: 0x060085E5 RID: 34277 RVA: 0x000FC778 File Offset: 0x000FA978
	public string SidescreenButtonTooltip
	{
		get
		{
			return this.SidescreenButtonInteractable() ? UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.LABEL : UI.UISIDESCREENS.ROCKETMODULESIDESCREEN.BUTTONVIEWEXTERIOR.INVALID;
		}
	}

	// Token: 0x060085E6 RID: 34278 RVA: 0x000FC793 File Offset: 0x000FA993
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		Components.ClusterCraftInteriorDoors.Add(this);
	}

	// Token: 0x060085E7 RID: 34279 RVA: 0x000FC7A6 File Offset: 0x000FA9A6
	protected override void OnCleanUp()
	{
		Components.ClusterCraftInteriorDoors.Remove(this);
		base.OnCleanUp();
	}

	// Token: 0x060085E8 RID: 34280 RVA: 0x000AA7E7 File Offset: 0x000A89E7
	public bool SidescreenEnabled()
	{
		return true;
	}

	// Token: 0x060085E9 RID: 34281 RVA: 0x003575D4 File Offset: 0x003557D4
	public bool SidescreenButtonInteractable()
	{
		WorldContainer myWorld = base.gameObject.GetMyWorld();
		return myWorld.ParentWorldId != 255 && myWorld.ParentWorldId != myWorld.id;
	}

	// Token: 0x060085EA RID: 34282 RVA: 0x000FC7B9 File Offset: 0x000FA9B9
	public void OnSidescreenButtonPressed()
	{
		ClusterManager.Instance.SetActiveWorld(base.gameObject.GetMyWorld().ParentWorldId);
	}

	// Token: 0x060085EB RID: 34283 RVA: 0x000AFED1 File Offset: 0x000AE0D1
	public int ButtonSideScreenSortOrder()
	{
		return 20;
	}

	// Token: 0x060085EC RID: 34284 RVA: 0x000AFECA File Offset: 0x000AE0CA
	public void SetButtonTextOverride(ButtonMenuTextOverride text)
	{
		throw new NotImplementedException();
	}

	// Token: 0x060085ED RID: 34285 RVA: 0x000AFE89 File Offset: 0x000AE089
	public int HorizontalGroupID()
	{
		return -1;
	}
}
