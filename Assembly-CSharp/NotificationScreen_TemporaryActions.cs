using System;
using System.Collections.Generic;
using STRINGS;
using UnityEngine;

// Token: 0x02001EB2 RID: 7858
public class NotificationScreen_TemporaryActions : KMonoBehaviour
{
	// Token: 0x17000A95 RID: 2709
	// (get) Token: 0x0600A4CF RID: 42191 RVA: 0x0010F543 File Offset: 0x0010D743
	// (set) Token: 0x0600A4D0 RID: 42192 RVA: 0x0010F54A File Offset: 0x0010D74A
	public static NotificationScreen_TemporaryActions Instance { get; private set; }

	// Token: 0x0600A4D1 RID: 42193 RVA: 0x0010F552 File Offset: 0x0010D752
	public static void DestroyInstance()
	{
		NotificationScreen_TemporaryActions.Instance = null;
	}

	// Token: 0x0600A4D2 RID: 42194 RVA: 0x0010F55A File Offset: 0x0010D75A
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		NotificationScreen_TemporaryActions.Instance = this;
		this.originalRow.gameObject.SetActive(false);
	}

	// Token: 0x0600A4D3 RID: 42195 RVA: 0x003F7A88 File Offset: 0x003F5C88
	private TemporaryActionRow CreateActionRow()
	{
		TemporaryActionRow temporaryActionRow = Util.KInstantiateUI<TemporaryActionRow>(this.originalRow.gameObject, this.originalRow.transform.parent.gameObject, false);
		temporaryActionRow.gameObject.SetActive(true);
		temporaryActionRow.transform.SetAsLastSibling();
		this.rows.Add(temporaryActionRow);
		return temporaryActionRow;
	}

	// Token: 0x0600A4D4 RID: 42196 RVA: 0x0010F579 File Offset: 0x0010D779
	private void RemoveRow(TemporaryActionRow row)
	{
		if (this.rows.Contains(row))
		{
			this.rows.Remove(row);
		}
		row.OnRowHidden = null;
		row.gameObject.DeleteObject();
	}

	// Token: 0x0600A4D5 RID: 42197 RVA: 0x003F7AE0 File Offset: 0x003F5CE0
	protected override void OnCleanUp()
	{
		if (this.rows != null)
		{
			foreach (TemporaryActionRow temporaryActionRow in this.rows.ToArray())
			{
				if (temporaryActionRow != null)
				{
					this.RemoveRow(temporaryActionRow);
				}
			}
			this.rows.Clear();
		}
		base.OnCleanUp();
	}

	// Token: 0x0600A4D6 RID: 42198 RVA: 0x003F7B34 File Offset: 0x003F5D34
	public void CreateCameraReturnActionButton(Vector3 positionToReturnTo)
	{
		if (this.cameraReturnRow == null)
		{
			this.cameraReturnRow = this.CreateActionRow();
			this.cameraReturnRow.Setup(UI.TEMPORARY_ACTIONS.CAMERA_RETURN.NAME, UI.TEMPORARY_ACTIONS.CAMERA_RETURN.TOOLTIP, Assets.GetSprite("action_follow_cam"));
			this.cameraReturnRow.gameObject.name = "TemporaryActionRow_CameraReturn";
			this.cameraPositionToReturnTo = positionToReturnTo;
			this.cameraReturnRow.OnRowHidden = new Action<TemporaryActionRow>(this.RemoveRow);
			this.cameraReturnRow.OnRowClicked = new Action<TemporaryActionRow>(this.OnCameraReturnActionButtonClicked);
		}
		this.cameraReturnRow.SetLifetime(10f);
	}

	// Token: 0x0600A4D7 RID: 42199 RVA: 0x0010F5A8 File Offset: 0x0010D7A8
	private void OnCameraReturnActionButtonClicked(TemporaryActionRow row)
	{
		if (this.cameraPositionToReturnTo != Vector3.zero)
		{
			GameUtil.FocusCamera(this.cameraPositionToReturnTo, 2f, true, false);
		}
	}

	// Token: 0x040080F1 RID: 33009
	public TemporaryActionRow originalRow;

	// Token: 0x040080F2 RID: 33010
	private List<TemporaryActionRow> rows = new List<TemporaryActionRow>();

	// Token: 0x040080F3 RID: 33011
	private TemporaryActionRow cameraReturnRow;

	// Token: 0x040080F4 RID: 33012
	private Vector3 cameraPositionToReturnTo = Vector3.zero;

	// Token: 0x040080F5 RID: 33013
	private const float CAMERA_RETURN_BUTTON_LIFETIME = 10f;
}
