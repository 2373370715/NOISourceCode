using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using ImGuiNET;
using UnityEngine;

// Token: 0x02000BD2 RID: 3026
public class DevToolChoreDebugger : DevTool
{
	// Token: 0x06003962 RID: 14690 RVA: 0x000C99A0 File Offset: 0x000C7BA0
	protected override void RenderTo(DevPanel panel)
	{
		this.Update();
	}

	// Token: 0x06003963 RID: 14691 RVA: 0x0022B9B4 File Offset: 0x00229BB4
	public void Update()
	{
		if (!Application.isPlaying || SelectTool.Instance == null || SelectTool.Instance.selected == null || SelectTool.Instance.selected.gameObject == null)
		{
			return;
		}
		GameObject gameObject = SelectTool.Instance.selected.gameObject;
		if (this.Consumer == null || (!this.lockSelection && this.selectedGameObject != gameObject))
		{
			this.Consumer = gameObject.GetComponent<ChoreConsumer>();
			this.selectedGameObject = gameObject;
		}
		if (this.Consumer != null)
		{
			ImGui.InputText("Filter:", ref this.filter, 256U);
			this.DisplayAvailableChores();
			ImGui.Text("");
		}
	}

	// Token: 0x06003964 RID: 14692 RVA: 0x0022BA7C File Offset: 0x00229C7C
	private void DisplayAvailableChores()
	{
		ImGui.Checkbox("Lock selection", ref this.lockSelection);
		ImGui.Checkbox("Show Last Successful Chore Selection", ref this.showLastSuccessfulPreconditionSnapshot);
		ImGui.Text("Available Chores:");
		ChoreConsumer.PreconditionSnapshot target_snapshot = this.Consumer.GetLastPreconditionSnapshot();
		if (this.showLastSuccessfulPreconditionSnapshot)
		{
			target_snapshot = this.Consumer.GetLastSuccessfulPreconditionSnapshot();
		}
		this.ShowChores(target_snapshot);
	}

	// Token: 0x06003965 RID: 14693 RVA: 0x0022BADC File Offset: 0x00229CDC
	private void ShowChores(ChoreConsumer.PreconditionSnapshot target_snapshot)
	{
		ImGuiTableFlags flags = ImGuiTableFlags.RowBg | ImGuiTableFlags.BordersInnerH | ImGuiTableFlags.BordersOuterH | ImGuiTableFlags.BordersInnerV | ImGuiTableFlags.BordersOuterV | ImGuiTableFlags.SizingFixedFit | ImGuiTableFlags.ScrollX | ImGuiTableFlags.ScrollY;
		this.rowIndex = 0;
		if (ImGui.BeginTable("Available Chores", this.columns.Count, flags))
		{
			foreach (object obj in this.columns.Keys)
			{
				ImGui.TableSetupColumn(obj.ToString(), ImGuiTableColumnFlags.WidthFixed);
			}
			ImGui.TableHeadersRow();
			for (int i = target_snapshot.succeededContexts.Count - 1; i >= 0; i--)
			{
				this.ShowContext(target_snapshot.succeededContexts[i]);
			}
			if (target_snapshot.doFailedContextsNeedSorting)
			{
				target_snapshot.failedContexts.Sort();
				target_snapshot.doFailedContextsNeedSorting = false;
			}
			for (int j = target_snapshot.failedContexts.Count - 1; j >= 0; j--)
			{
				this.ShowContext(target_snapshot.failedContexts[j]);
			}
			ImGui.EndTable();
		}
	}

	// Token: 0x06003966 RID: 14694 RVA: 0x0022BBE0 File Offset: 0x00229DE0
	private void ShowContext(Chore.Precondition.Context context)
	{
		string text = "";
		Chore chore = context.chore;
		if (!context.IsSuccess())
		{
			text = context.chore.GetPreconditions()[context.failedPreconditionId].condition.id;
		}
		string text2 = "";
		if (chore.driver != null)
		{
			text2 = chore.driver.name;
		}
		string text3 = "";
		if (chore.overrideTarget != null)
		{
			text3 = chore.overrideTarget.name;
		}
		string text4 = "";
		if (!chore.isNull)
		{
			text4 = chore.gameObject.name;
		}
		if (Chore.Precondition.Context.ShouldFilter(this.filter, chore.GetType().ToString()) && Chore.Precondition.Context.ShouldFilter(this.filter, chore.choreType.Id) && Chore.Precondition.Context.ShouldFilter(this.filter, text) && Chore.Precondition.Context.ShouldFilter(this.filter, text2) && Chore.Precondition.Context.ShouldFilter(this.filter, text3) && Chore.Precondition.Context.ShouldFilter(this.filter, text4))
		{
			return;
		}
		this.columns["Id"] = chore.id.ToString();
		this.columns["Class"] = chore.GetType().ToString().Replace("`1", "");
		this.columns["Type"] = chore.choreType.Id;
		this.columns["PriorityClass"] = context.masterPriority.priority_class.ToString();
		this.columns["PersonalPriority"] = context.personalPriority.ToString();
		this.columns["PriorityValue"] = context.masterPriority.priority_value.ToString();
		this.columns["Priority"] = context.priority.ToString();
		this.columns["PriorityMod"] = context.priorityMod.ToString();
		this.columns["ConsumerPriority"] = context.consumerPriority.ToString();
		this.columns["Cost"] = context.cost.ToString();
		this.columns["Interrupt"] = context.interruptPriority.ToString();
		this.columns["Precondition"] = text;
		this.columns["Override"] = text3;
		this.columns["Assigned To"] = text2;
		this.columns["Owner"] = text4;
		this.columns["Details"] = "";
		ImGui.TableNextRow();
		string format = "ID_row_{0}";
		int num = this.rowIndex;
		this.rowIndex = num + 1;
		ImGui.PushID(string.Format(format, num));
		for (int i = 0; i < this.columns.Count; i++)
		{
			ImGui.TableSetColumnIndex(i);
			ImGui.Text(this.columns[i].ToString());
		}
		ImGui.PopID();
	}

	// Token: 0x06003967 RID: 14695 RVA: 0x000AA038 File Offset: 0x000A8238
	public void ConsumerDebugDisplayLog()
	{
	}

	// Token: 0x040027A7 RID: 10151
	private string filter = "";

	// Token: 0x040027A8 RID: 10152
	private bool showLastSuccessfulPreconditionSnapshot;

	// Token: 0x040027A9 RID: 10153
	private bool lockSelection;

	// Token: 0x040027AA RID: 10154
	private ChoreConsumer Consumer;

	// Token: 0x040027AB RID: 10155
	private GameObject selectedGameObject;

	// Token: 0x040027AC RID: 10156
	private OrderedDictionary columns = new OrderedDictionary
	{
		{
			"BP",
			""
		},
		{
			"Id",
			""
		},
		{
			"Class",
			""
		},
		{
			"Type",
			""
		},
		{
			"PriorityClass",
			""
		},
		{
			"PersonalPriority",
			""
		},
		{
			"PriorityValue",
			""
		},
		{
			"Priority",
			""
		},
		{
			"PriorityMod",
			""
		},
		{
			"ConsumerPriority",
			""
		},
		{
			"Cost",
			""
		},
		{
			"Interrupt",
			""
		},
		{
			"Precondition",
			""
		},
		{
			"Override",
			""
		},
		{
			"Assigned To",
			""
		},
		{
			"Owner",
			""
		},
		{
			"Details",
			""
		}
	};

	// Token: 0x040027AD RID: 10157
	private int rowIndex;

	// Token: 0x02000BD3 RID: 3027
	public class EditorPreconditionSnapshot
	{
		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06003969 RID: 14697 RVA: 0x000C99A8 File Offset: 0x000C7BA8
		// (set) Token: 0x0600396A RID: 14698 RVA: 0x000C99B0 File Offset: 0x000C7BB0
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> SucceededContexts { get; set; }

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x0600396B RID: 14699 RVA: 0x000C99B9 File Offset: 0x000C7BB9
		// (set) Token: 0x0600396C RID: 14700 RVA: 0x000C99C1 File Offset: 0x000C7BC1
		public List<DevToolChoreDebugger.EditorPreconditionSnapshot.EditorContext> FailedContexts { get; set; }

		// Token: 0x02000BD4 RID: 3028
		public struct EditorContext
		{
			// Token: 0x170002A1 RID: 673
			// (get) Token: 0x0600396E RID: 14702 RVA: 0x000C99CA File Offset: 0x000C7BCA
			// (set) Token: 0x0600396F RID: 14703 RVA: 0x000C99D2 File Offset: 0x000C7BD2
			public string Chore { readonly get; set; }

			// Token: 0x170002A2 RID: 674
			// (get) Token: 0x06003970 RID: 14704 RVA: 0x000C99DB File Offset: 0x000C7BDB
			// (set) Token: 0x06003971 RID: 14705 RVA: 0x000C99E3 File Offset: 0x000C7BE3
			public string ChoreType { readonly get; set; }

			// Token: 0x170002A3 RID: 675
			// (get) Token: 0x06003972 RID: 14706 RVA: 0x000C99EC File Offset: 0x000C7BEC
			// (set) Token: 0x06003973 RID: 14707 RVA: 0x000C99F4 File Offset: 0x000C7BF4
			public string FailedPrecondition { readonly get; set; }

			// Token: 0x170002A4 RID: 676
			// (get) Token: 0x06003974 RID: 14708 RVA: 0x000C99FD File Offset: 0x000C7BFD
			// (set) Token: 0x06003975 RID: 14709 RVA: 0x000C9A05 File Offset: 0x000C7C05
			public int WorldId { readonly get; set; }
		}
	}
}
