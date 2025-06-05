using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000950 RID: 2384
public class KBatchedAnimUpdater : Singleton<KBatchedAnimUpdater>
{
	// Token: 0x06002A97 RID: 10903 RVA: 0x001E74E8 File Offset: 0x001E56E8
	public void InitializeGrid()
	{
		this.Clear();
		Vector2I visibleSize = this.GetVisibleSize();
		int num = (visibleSize.x + 32 - 1) / 32;
		int num2 = (visibleSize.y + 32 - 1) / 32;
		this.controllerGrid = new Dictionary<int, KBatchedAnimController>[num, num2];
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				this.controllerGrid[j, i] = new Dictionary<int, KBatchedAnimController>();
			}
		}
		this.visibleChunks.Clear();
		this.previouslyVisibleChunks.Clear();
		this.previouslyVisibleChunkGrid = new bool[num, num2];
		this.visibleChunkGrid = new bool[num, num2];
		this.controllerChunkInfos.Clear();
		this.movingControllerInfos.Clear();
	}

	// Token: 0x06002A98 RID: 10904 RVA: 0x001E759C File Offset: 0x001E579C
	public Vector2I GetVisibleSize()
	{
		if (CameraController.Instance != null)
		{
			Vector2I vector2I;
			Vector2I vector2I2;
			CameraController.Instance.GetWorldCamera(out vector2I, out vector2I2);
			return new Vector2I((int)((float)(vector2I2.x + vector2I.x) * KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x), (int)((float)(vector2I2.y + vector2I.y) * KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y));
		}
		return new Vector2I((int)((float)Grid.WidthInCells * KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.x), (int)((float)Grid.HeightInCells * KBatchedAnimUpdater.VISIBLE_RANGE_SCALE.y));
	}

	// Token: 0x14000009 RID: 9
	// (add) Token: 0x06002A99 RID: 10905 RVA: 0x001E7628 File Offset: 0x001E5828
	// (remove) Token: 0x06002A9A RID: 10906 RVA: 0x001E7660 File Offset: 0x001E5860
	public event System.Action OnClear;

	// Token: 0x06002A9B RID: 10907 RVA: 0x001E7698 File Offset: 0x001E5898
	public void Clear()
	{
		foreach (KBatchedAnimController kbatchedAnimController in this.updateList)
		{
			if (kbatchedAnimController != null)
			{
				UnityEngine.Object.DestroyImmediate(kbatchedAnimController);
			}
		}
		this.updateList.Clear();
		foreach (KBatchedAnimController kbatchedAnimController2 in this.alwaysUpdateList)
		{
			if (kbatchedAnimController2 != null)
			{
				UnityEngine.Object.DestroyImmediate(kbatchedAnimController2);
			}
		}
		this.alwaysUpdateList.Clear();
		this.queuedRegistrations.Clear();
		this.visibleChunks.Clear();
		this.previouslyVisibleChunks.Clear();
		this.controllerGrid = null;
		this.previouslyVisibleChunkGrid = null;
		this.visibleChunkGrid = null;
		System.Action onClear = this.OnClear;
		if (onClear == null)
		{
			return;
		}
		onClear();
	}

	// Token: 0x06002A9C RID: 10908 RVA: 0x001E779C File Offset: 0x001E599C
	public void UpdateRegister(KBatchedAnimController controller)
	{
		switch (controller.updateRegistrationState)
		{
		case KBatchedAnimUpdater.RegistrationState.Registered:
			break;
		case KBatchedAnimUpdater.RegistrationState.PendingRemoval:
			controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Registered;
			return;
		case KBatchedAnimUpdater.RegistrationState.Unregistered:
			((controller.visibilityType == KAnimControllerBase.VisibilityType.Always) ? this.alwaysUpdateList : this.updateList).AddLast(controller);
			controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Registered;
			break;
		default:
			return;
		}
	}

	// Token: 0x06002A9D RID: 10909 RVA: 0x001E77F0 File Offset: 0x001E59F0
	public void UpdateUnregister(KBatchedAnimController controller)
	{
		switch (controller.updateRegistrationState)
		{
		case KBatchedAnimUpdater.RegistrationState.Registered:
			controller.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.PendingRemoval;
			break;
		case KBatchedAnimUpdater.RegistrationState.PendingRemoval:
		case KBatchedAnimUpdater.RegistrationState.Unregistered:
			break;
		default:
			return;
		}
	}

	// Token: 0x06002A9E RID: 10910 RVA: 0x001E7820 File Offset: 0x001E5A20
	public void VisibilityRegister(KBatchedAnimController controller)
	{
		this.queuedRegistrations.Add(new KBatchedAnimUpdater.RegistrationInfo
		{
			transformId = controller.transform.GetInstanceID(),
			controllerInstanceId = controller.GetInstanceID(),
			controller = controller,
			register = true
		});
	}

	// Token: 0x06002A9F RID: 10911 RVA: 0x001E7870 File Offset: 0x001E5A70
	public void VisibilityUnregister(KBatchedAnimController controller)
	{
		if (App.IsExiting)
		{
			return;
		}
		this.queuedRegistrations.Add(new KBatchedAnimUpdater.RegistrationInfo
		{
			transformId = controller.transform.GetInstanceID(),
			controllerInstanceId = controller.GetInstanceID(),
			controller = controller,
			register = false
		});
	}

	// Token: 0x06002AA0 RID: 10912 RVA: 0x001E78C8 File Offset: 0x001E5AC8
	private Dictionary<int, KBatchedAnimController> GetControllerMap(Vector2I chunk_xy)
	{
		Dictionary<int, KBatchedAnimController> result = null;
		if (this.controllerGrid != null && 0 <= chunk_xy.x && chunk_xy.x < this.controllerGrid.GetLength(0) && 0 <= chunk_xy.y && chunk_xy.y < this.controllerGrid.GetLength(1))
		{
			result = this.controllerGrid[chunk_xy.x, chunk_xy.y];
		}
		return result;
	}

	// Token: 0x06002AA1 RID: 10913 RVA: 0x001E7934 File Offset: 0x001E5B34
	public void LateUpdate()
	{
		this.ProcessMovingAnims();
		this.UpdateVisibility();
		this.ProcessRegistrations();
		this.CleanUp();
		float num = Time.unscaledDeltaTime;
		int count = this.alwaysUpdateList.Count;
		KBatchedAnimUpdater.UpdateRegisteredAnims(this.alwaysUpdateList, num);
		if (this.DoGridProcessing())
		{
			num = Time.deltaTime;
			if (num > 0f)
			{
				int count2 = this.updateList.Count;
				KBatchedAnimUpdater.UpdateRegisteredAnims(this.updateList, num);
			}
		}
	}

	// Token: 0x06002AA2 RID: 10914 RVA: 0x001E79A8 File Offset: 0x001E5BA8
	private static void UpdateRegisteredAnims(LinkedList<KBatchedAnimController> list, float dt)
	{
		LinkedListNode<KBatchedAnimController> next;
		for (LinkedListNode<KBatchedAnimController> linkedListNode = list.First; linkedListNode != null; linkedListNode = next)
		{
			next = linkedListNode.Next;
			KBatchedAnimController value = linkedListNode.Value;
			if (value == null)
			{
				list.Remove(linkedListNode);
			}
			else if (value.updateRegistrationState != KBatchedAnimUpdater.RegistrationState.Registered)
			{
				value.updateRegistrationState = KBatchedAnimUpdater.RegistrationState.Unregistered;
				list.Remove(linkedListNode);
			}
			else if (value.forceUseGameTime)
			{
				value.UpdateAnim(Time.deltaTime);
			}
			else
			{
				value.UpdateAnim(dt);
			}
		}
	}

	// Token: 0x06002AA3 RID: 10915 RVA: 0x000C032A File Offset: 0x000BE52A
	public bool IsChunkVisible(Vector2I chunk_xy)
	{
		return this.visibleChunkGrid[chunk_xy.x, chunk_xy.y];
	}

	// Token: 0x06002AA4 RID: 10916 RVA: 0x000C0343 File Offset: 0x000BE543
	public void GetVisibleArea(out Vector2I vis_chunk_min, out Vector2I vis_chunk_max)
	{
		vis_chunk_min = this.vis_chunk_min;
		vis_chunk_max = this.vis_chunk_max;
	}

	// Token: 0x06002AA5 RID: 10917 RVA: 0x000C035D File Offset: 0x000BE55D
	public static Vector2I PosToChunkXY(Vector3 pos)
	{
		return KAnimBatchManager.CellXYToChunkXY(Grid.PosToXY(pos));
	}

	// Token: 0x06002AA6 RID: 10918 RVA: 0x001E7A18 File Offset: 0x001E5C18
	private void UpdateVisibility()
	{
		if (!this.DoGridProcessing())
		{
			return;
		}
		Vector2I vector2I;
		Vector2I vector2I2;
		Grid.GetVisibleCellRangeInActiveWorld(out vector2I, out vector2I2, 4, 1.5f);
		this.vis_chunk_min = new Vector2I(vector2I.x / 32, vector2I.y / 32);
		this.vis_chunk_max = new Vector2I(vector2I2.x / 32, vector2I2.y / 32);
		this.vis_chunk_max.x = Math.Min(this.vis_chunk_max.x, this.controllerGrid.GetLength(0) - 1);
		this.vis_chunk_max.y = Math.Min(this.vis_chunk_max.y, this.controllerGrid.GetLength(1) - 1);
		bool[,] array = this.previouslyVisibleChunkGrid;
		this.previouslyVisibleChunkGrid = this.visibleChunkGrid;
		this.visibleChunkGrid = array;
		Array.Clear(this.visibleChunkGrid, 0, this.visibleChunkGrid.Length);
		List<Vector2I> list = this.previouslyVisibleChunks;
		this.previouslyVisibleChunks = this.visibleChunks;
		this.visibleChunks = list;
		this.visibleChunks.Clear();
		for (int i = this.vis_chunk_min.y; i <= this.vis_chunk_max.y; i++)
		{
			for (int j = this.vis_chunk_min.x; j <= this.vis_chunk_max.x; j++)
			{
				this.visibleChunkGrid[j, i] = true;
				this.visibleChunks.Add(new Vector2I(j, i));
				if (!this.previouslyVisibleChunkGrid[j, i])
				{
					foreach (KeyValuePair<int, KBatchedAnimController> keyValuePair in this.controllerGrid[j, i])
					{
						KBatchedAnimController value = keyValuePair.Value;
						if (!(value == null))
						{
							value.SetVisiblity(true);
						}
					}
				}
			}
		}
		for (int k = 0; k < this.previouslyVisibleChunks.Count; k++)
		{
			Vector2I vector2I3 = this.previouslyVisibleChunks[k];
			if (!this.visibleChunkGrid[vector2I3.x, vector2I3.y])
			{
				foreach (KeyValuePair<int, KBatchedAnimController> keyValuePair2 in this.controllerGrid[vector2I3.x, vector2I3.y])
				{
					KBatchedAnimController value2 = keyValuePair2.Value;
					if (!(value2 == null))
					{
						value2.SetVisiblity(false);
					}
				}
			}
		}
	}

	// Token: 0x06002AA7 RID: 10919 RVA: 0x001E7CC4 File Offset: 0x001E5EC4
	private void ProcessMovingAnims()
	{
		foreach (KBatchedAnimUpdater.MovingControllerInfo movingControllerInfo in this.movingControllerInfos.Values)
		{
			if (!(movingControllerInfo.controller == null))
			{
				Vector2I vector2I = KBatchedAnimUpdater.PosToChunkXY(movingControllerInfo.controller.PositionIncludingOffset);
				if (movingControllerInfo.chunkXY != vector2I)
				{
					KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = default(KBatchedAnimUpdater.ControllerChunkInfo);
					DebugUtil.Assert(this.controllerChunkInfos.TryGetValue(movingControllerInfo.controllerInstanceId, out controllerChunkInfo));
					DebugUtil.Assert(movingControllerInfo.controller == controllerChunkInfo.controller);
					DebugUtil.Assert(controllerChunkInfo.chunkXY == movingControllerInfo.chunkXY);
					Dictionary<int, KBatchedAnimController> controllerMap = this.GetControllerMap(controllerChunkInfo.chunkXY);
					if (controllerMap != null)
					{
						DebugUtil.Assert(controllerMap.ContainsKey(movingControllerInfo.controllerInstanceId));
						controllerMap.Remove(movingControllerInfo.controllerInstanceId);
					}
					controllerMap = this.GetControllerMap(vector2I);
					if (controllerMap != null)
					{
						DebugUtil.Assert(!controllerMap.ContainsKey(movingControllerInfo.controllerInstanceId));
						controllerMap[movingControllerInfo.controllerInstanceId] = controllerChunkInfo.controller;
					}
					movingControllerInfo.chunkXY = vector2I;
					controllerChunkInfo.chunkXY = vector2I;
					this.controllerChunkInfos[movingControllerInfo.controllerInstanceId] = controllerChunkInfo;
					if (controllerMap != null)
					{
						controllerChunkInfo.controller.SetVisiblity(this.visibleChunkGrid[vector2I.x, vector2I.y]);
					}
					else
					{
						controllerChunkInfo.controller.SetVisiblity(false);
					}
				}
			}
		}
	}

	// Token: 0x06002AA8 RID: 10920 RVA: 0x001E7E64 File Offset: 0x001E6064
	private void ProcessRegistrations()
	{
		for (int i = 0; i < this.queuedRegistrations.Count; i++)
		{
			KBatchedAnimUpdater.RegistrationInfo registrationInfo = this.queuedRegistrations[i];
			if (registrationInfo.register)
			{
				if (!(registrationInfo.controller == null))
				{
					int instanceID = registrationInfo.controller.GetInstanceID();
					DebugUtil.Assert(!this.controllerChunkInfos.ContainsKey(instanceID));
					KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = new KBatchedAnimUpdater.ControllerChunkInfo
					{
						controller = registrationInfo.controller,
						chunkXY = KBatchedAnimUpdater.PosToChunkXY(registrationInfo.controller.PositionIncludingOffset)
					};
					this.controllerChunkInfos[instanceID] = controllerChunkInfo;
					bool flag = false;
					if (Singleton<CellChangeMonitor>.Instance != null)
					{
						flag = Singleton<CellChangeMonitor>.Instance.IsMoving(registrationInfo.controller.transform);
						Singleton<CellChangeMonitor>.Instance.RegisterMovementStateChanged(registrationInfo.controller.transform, new Action<Transform, bool>(this.OnMovementStateChanged));
					}
					Dictionary<int, KBatchedAnimController> controllerMap = this.GetControllerMap(controllerChunkInfo.chunkXY);
					if (controllerMap != null)
					{
						DebugUtil.Assert(!controllerMap.ContainsKey(instanceID));
						controllerMap.Add(instanceID, registrationInfo.controller);
					}
					if (flag)
					{
						DebugUtil.DevAssertArgs(!this.movingControllerInfos.ContainsKey(instanceID), new object[]
						{
							"Readding controller which is already moving",
							registrationInfo.controller.name,
							controllerChunkInfo.chunkXY,
							this.movingControllerInfos.ContainsKey(instanceID) ? this.movingControllerInfos[instanceID].chunkXY.ToString() : null
						});
						this.movingControllerInfos[instanceID] = new KBatchedAnimUpdater.MovingControllerInfo
						{
							controllerInstanceId = instanceID,
							controller = registrationInfo.controller,
							chunkXY = controllerChunkInfo.chunkXY
						};
					}
					if (controllerMap != null && this.visibleChunkGrid[controllerChunkInfo.chunkXY.x, controllerChunkInfo.chunkXY.y])
					{
						registrationInfo.controller.SetVisiblity(true);
					}
				}
			}
			else
			{
				KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo2 = default(KBatchedAnimUpdater.ControllerChunkInfo);
				if (this.controllerChunkInfos.TryGetValue(registrationInfo.controllerInstanceId, out controllerChunkInfo2))
				{
					if (registrationInfo.controller != null)
					{
						Dictionary<int, KBatchedAnimController> controllerMap2 = this.GetControllerMap(controllerChunkInfo2.chunkXY);
						if (controllerMap2 != null)
						{
							DebugUtil.Assert(controllerMap2.ContainsKey(registrationInfo.controllerInstanceId));
							controllerMap2.Remove(registrationInfo.controllerInstanceId);
						}
						registrationInfo.controller.SetVisiblity(false);
					}
					this.movingControllerInfos.Remove(registrationInfo.controllerInstanceId);
					Singleton<CellChangeMonitor>.Instance.UnregisterMovementStateChanged(registrationInfo.transformId, new Action<Transform, bool>(this.OnMovementStateChanged));
					this.controllerChunkInfos.Remove(registrationInfo.controllerInstanceId);
				}
			}
		}
		this.queuedRegistrations.Clear();
	}

	// Token: 0x06002AA9 RID: 10921 RVA: 0x001E8120 File Offset: 0x001E6320
	public void OnMovementStateChanged(Transform transform, bool is_moving)
	{
		if (transform == null)
		{
			return;
		}
		KBatchedAnimController component = transform.GetComponent<KBatchedAnimController>();
		if (component == null)
		{
			return;
		}
		int instanceID = component.GetInstanceID();
		KBatchedAnimUpdater.ControllerChunkInfo controllerChunkInfo = default(KBatchedAnimUpdater.ControllerChunkInfo);
		DebugUtil.Assert(this.controllerChunkInfos.TryGetValue(instanceID, out controllerChunkInfo));
		if (is_moving)
		{
			DebugUtil.DevAssertArgs(!this.movingControllerInfos.ContainsKey(instanceID), new object[]
			{
				"Readding controller which is already moving",
				component.name,
				controllerChunkInfo.chunkXY,
				this.movingControllerInfos.ContainsKey(instanceID) ? this.movingControllerInfos[instanceID].chunkXY.ToString() : null
			});
			this.movingControllerInfos[instanceID] = new KBatchedAnimUpdater.MovingControllerInfo
			{
				controllerInstanceId = instanceID,
				controller = component,
				chunkXY = controllerChunkInfo.chunkXY
			};
			return;
		}
		this.movingControllerInfos.Remove(instanceID);
	}

	// Token: 0x06002AAA RID: 10922 RVA: 0x001E8214 File Offset: 0x001E6414
	private void CleanUp()
	{
		if (!this.DoGridProcessing())
		{
			return;
		}
		int length = this.controllerGrid.GetLength(0);
		for (int i = 0; i < 16; i++)
		{
			int num = (this.cleanUpChunkIndex + i) % this.controllerGrid.Length;
			int num2 = num % length;
			int num3 = num / length;
			Dictionary<int, KBatchedAnimController> dictionary = this.controllerGrid[num2, num3];
			ListPool<int, KBatchedAnimUpdater>.PooledList pooledList = ListPool<int, KBatchedAnimUpdater>.Allocate();
			foreach (KeyValuePair<int, KBatchedAnimController> keyValuePair in dictionary)
			{
				if (keyValuePair.Value == null)
				{
					pooledList.Add(keyValuePair.Key);
				}
			}
			foreach (int key in pooledList)
			{
				dictionary.Remove(key);
			}
			pooledList.Recycle();
		}
		this.cleanUpChunkIndex = (this.cleanUpChunkIndex + 16) % this.controllerGrid.Length;
	}

	// Token: 0x06002AAB RID: 10923 RVA: 0x000C036A File Offset: 0x000BE56A
	private bool DoGridProcessing()
	{
		return this.controllerGrid != null && Camera.main != null;
	}

	// Token: 0x04001CDA RID: 7386
	private const int VISIBLE_BORDER = 4;

	// Token: 0x04001CDB RID: 7387
	public static readonly Vector2I INVALID_CHUNK_ID = Vector2I.minusone;

	// Token: 0x04001CDC RID: 7388
	private Dictionary<int, KBatchedAnimController>[,] controllerGrid;

	// Token: 0x04001CDD RID: 7389
	private LinkedList<KBatchedAnimController> updateList = new LinkedList<KBatchedAnimController>();

	// Token: 0x04001CDE RID: 7390
	private LinkedList<KBatchedAnimController> alwaysUpdateList = new LinkedList<KBatchedAnimController>();

	// Token: 0x04001CDF RID: 7391
	private bool[,] visibleChunkGrid;

	// Token: 0x04001CE0 RID: 7392
	private bool[,] previouslyVisibleChunkGrid;

	// Token: 0x04001CE1 RID: 7393
	private List<Vector2I> visibleChunks = new List<Vector2I>();

	// Token: 0x04001CE2 RID: 7394
	private List<Vector2I> previouslyVisibleChunks = new List<Vector2I>();

	// Token: 0x04001CE3 RID: 7395
	private Vector2I vis_chunk_min = Vector2I.zero;

	// Token: 0x04001CE4 RID: 7396
	private Vector2I vis_chunk_max = Vector2I.zero;

	// Token: 0x04001CE5 RID: 7397
	private List<KBatchedAnimUpdater.RegistrationInfo> queuedRegistrations = new List<KBatchedAnimUpdater.RegistrationInfo>();

	// Token: 0x04001CE6 RID: 7398
	private Dictionary<int, KBatchedAnimUpdater.ControllerChunkInfo> controllerChunkInfos = new Dictionary<int, KBatchedAnimUpdater.ControllerChunkInfo>();

	// Token: 0x04001CE7 RID: 7399
	private Dictionary<int, KBatchedAnimUpdater.MovingControllerInfo> movingControllerInfos = new Dictionary<int, KBatchedAnimUpdater.MovingControllerInfo>();

	// Token: 0x04001CE8 RID: 7400
	private const int CHUNKS_TO_CLEAN_PER_TICK = 16;

	// Token: 0x04001CE9 RID: 7401
	private int cleanUpChunkIndex;

	// Token: 0x04001CEA RID: 7402
	private static readonly Vector2 VISIBLE_RANGE_SCALE = new Vector2(1.5f, 1.5f);

	// Token: 0x02000951 RID: 2385
	public enum RegistrationState
	{
		// Token: 0x04001CED RID: 7405
		Registered,
		// Token: 0x04001CEE RID: 7406
		PendingRemoval,
		// Token: 0x04001CEF RID: 7407
		Unregistered
	}

	// Token: 0x02000952 RID: 2386
	private struct RegistrationInfo
	{
		// Token: 0x04001CF0 RID: 7408
		public bool register;

		// Token: 0x04001CF1 RID: 7409
		public int transformId;

		// Token: 0x04001CF2 RID: 7410
		public int controllerInstanceId;

		// Token: 0x04001CF3 RID: 7411
		public KBatchedAnimController controller;
	}

	// Token: 0x02000953 RID: 2387
	private struct ControllerChunkInfo
	{
		// Token: 0x04001CF4 RID: 7412
		public KBatchedAnimController controller;

		// Token: 0x04001CF5 RID: 7413
		public Vector2I chunkXY;
	}

	// Token: 0x02000954 RID: 2388
	private class MovingControllerInfo
	{
		// Token: 0x04001CF6 RID: 7414
		public int controllerInstanceId;

		// Token: 0x04001CF7 RID: 7415
		public KBatchedAnimController controller;

		// Token: 0x04001CF8 RID: 7416
		public Vector2I chunkXY;
	}
}
