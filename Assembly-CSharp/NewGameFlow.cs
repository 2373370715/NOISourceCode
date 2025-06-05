using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02001EA7 RID: 7847
[AddComponentMenu("KMonoBehaviour/scripts/NewGameFlow")]
public class NewGameFlow : KMonoBehaviour
{
	// Token: 0x0600A487 RID: 42119 RVA: 0x0010F262 File Offset: 0x0010D462
	public void BeginFlow()
	{
		this.currentScreenIndex = -1;
		this.Next();
	}

	// Token: 0x0600A488 RID: 42120 RVA: 0x0010F271 File Offset: 0x0010D471
	private void Next()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex++;
		this.ActivateCurrentScreen();
	}

	// Token: 0x0600A489 RID: 42121 RVA: 0x0010F28D File Offset: 0x0010D48D
	private void Previous()
	{
		this.ClearCurrentScreen();
		this.currentScreenIndex--;
		this.ActivateCurrentScreen();
	}

	// Token: 0x0600A48A RID: 42122 RVA: 0x0010F2A9 File Offset: 0x0010D4A9
	private void ClearCurrentScreen()
	{
		if (this.currentScreen != null)
		{
			this.currentScreen.Deactivate();
			this.currentScreen = null;
		}
	}

	// Token: 0x0600A48B RID: 42123 RVA: 0x003F6428 File Offset: 0x003F4628
	private void ActivateCurrentScreen()
	{
		if (this.currentScreenIndex >= 0 && this.currentScreenIndex < this.newGameFlowScreens.Count)
		{
			NewGameFlowScreen newGameFlowScreen = Util.KInstantiateUI<NewGameFlowScreen>(this.newGameFlowScreens[this.currentScreenIndex].gameObject, base.transform.parent.gameObject, true);
			newGameFlowScreen.OnNavigateForward += this.Next;
			newGameFlowScreen.OnNavigateBackward += this.Previous;
			if (!newGameFlowScreen.IsActive() && !newGameFlowScreen.activateOnSpawn)
			{
				newGameFlowScreen.Activate();
			}
			this.currentScreen = newGameFlowScreen;
		}
	}

	// Token: 0x040080A8 RID: 32936
	public List<NewGameFlowScreen> newGameFlowScreens;

	// Token: 0x040080A9 RID: 32937
	private int currentScreenIndex = -1;

	// Token: 0x040080AA RID: 32938
	private NewGameFlowScreen currentScreen;
}
