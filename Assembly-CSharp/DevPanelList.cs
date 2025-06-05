using System;
using System.Collections.Generic;

// Token: 0x02000BCB RID: 3019
public class DevPanelList
{
	// Token: 0x06003939 RID: 14649 RVA: 0x000C97A4 File Offset: 0x000C79A4
	public DevPanel AddPanelFor<T>() where T : DevTool, new()
	{
		return this.AddPanelFor(Activator.CreateInstance<T>());
	}

	// Token: 0x0600393A RID: 14650 RVA: 0x0022A664 File Offset: 0x00228864
	public DevPanel AddPanelFor(DevTool devTool)
	{
		DevPanel devPanel = new DevPanel(devTool, this);
		this.activePanels.Add(devPanel);
		return devPanel;
	}

	// Token: 0x0600393B RID: 14651 RVA: 0x0022A688 File Offset: 0x00228888
	public Option<T> GetDevTool<T>() where T : DevTool
	{
		foreach (DevPanel devPanel in this.activePanels)
		{
			T t = devPanel.GetCurrentDevTool() as T;
			if (t != null)
			{
				return t;
			}
		}
		return Option.None;
	}

	// Token: 0x0600393C RID: 14652 RVA: 0x0022A700 File Offset: 0x00228900
	public T AddOrGetDevTool<T>() where T : DevTool, new()
	{
		bool flag;
		T t;
		this.GetDevTool<T>().Deconstruct(out flag, out t);
		bool flag2 = flag;
		T t2 = t;
		if (!flag2)
		{
			t2 = Activator.CreateInstance<T>();
			this.AddPanelFor(t2);
		}
		return t2;
	}

	// Token: 0x0600393D RID: 14653 RVA: 0x000C97B6 File Offset: 0x000C79B6
	public void ClosePanel(DevPanel panel)
	{
		if (this.activePanels.Remove(panel))
		{
			panel.Internal_Uninit();
		}
	}

	// Token: 0x0600393E RID: 14654 RVA: 0x0022A738 File Offset: 0x00228938
	public void Render()
	{
		if (this.activePanels.Count == 0)
		{
			return;
		}
		using (ListPool<DevPanel, DevPanelList>.PooledList pooledList = ListPool<DevPanel, DevPanelList>.Allocate())
		{
			for (int i = 0; i < this.activePanels.Count; i++)
			{
				DevPanel devPanel = this.activePanels[i];
				devPanel.RenderPanel();
				if (devPanel.isRequestingToClose)
				{
					pooledList.Add(devPanel);
				}
			}
			foreach (DevPanel panel in pooledList)
			{
				this.ClosePanel(panel);
			}
		}
	}

	// Token: 0x0600393F RID: 14655 RVA: 0x000C97CC File Offset: 0x000C79CC
	public void Internal_InitPanelId(Type initialDevToolType, out string panelId, out uint idPostfixNumber)
	{
		idPostfixNumber = this.Internal_GetUniqueIdPostfix(initialDevToolType);
		panelId = initialDevToolType.Name + idPostfixNumber.ToString();
	}

	// Token: 0x06003940 RID: 14656 RVA: 0x0022A7EC File Offset: 0x002289EC
	public uint Internal_GetUniqueIdPostfix(Type initialDevToolType)
	{
		uint result;
		using (HashSetPool<uint, DevPanelList>.PooledHashSet pooledHashSet = HashSetPool<uint, DevPanelList>.Allocate())
		{
			foreach (DevPanel devPanel in this.activePanels)
			{
				if (!(devPanel.initialDevToolType != initialDevToolType))
				{
					pooledHashSet.Add(devPanel.idPostfixNumber);
				}
			}
			for (uint num = 0U; num < 100U; num += 1U)
			{
				if (!pooledHashSet.Contains(num))
				{
					return num;
				}
			}
			Debug.Assert(false, "Something went wrong, this should only assert if there's over 100 of the same type of debug window");
			uint num2 = this.fallbackUniqueIdPostfixNumber;
			this.fallbackUniqueIdPostfixNumber = num2 + 1U;
			result = num2;
		}
		return result;
	}

	// Token: 0x04002796 RID: 10134
	private List<DevPanel> activePanels = new List<DevPanel>();

	// Token: 0x04002797 RID: 10135
	private uint fallbackUniqueIdPostfixNumber = 300U;
}
