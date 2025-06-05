using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000832 RID: 2098
public abstract class Reactable
{
	// Token: 0x17000117 RID: 279
	// (get) Token: 0x060024F8 RID: 9464 RVA: 0x000BC86B File Offset: 0x000BAA6B
	public bool IsValid
	{
		get
		{
			return this.partitionerEntry.IsValid();
		}
	}

	// Token: 0x17000118 RID: 280
	// (get) Token: 0x060024F9 RID: 9465 RVA: 0x000BC878 File Offset: 0x000BAA78
	// (set) Token: 0x060024FA RID: 9466 RVA: 0x000BC880 File Offset: 0x000BAA80
	public float creationTime { get; private set; }

	// Token: 0x17000119 RID: 281
	// (get) Token: 0x060024FB RID: 9467 RVA: 0x000BC889 File Offset: 0x000BAA89
	public bool IsReacting
	{
		get
		{
			return this.reactor != null;
		}
	}

	// Token: 0x060024FC RID: 9468 RVA: 0x001D83F0 File Offset: 0x001D65F0
	public Reactable(GameObject gameObject, HashedString id, ChoreType chore_type, int range_width = 15, int range_height = 8, bool follow_transform = false, float globalCooldown = 0f, float localCooldown = 0f, float lifeSpan = float.PositiveInfinity, float max_initial_delay = 0f, ObjectLayer overrideLayer = ObjectLayer.NumLayers)
	{
		this.rangeHeight = range_height;
		this.rangeWidth = range_width;
		this.id = id;
		this.gameObject = gameObject;
		this.choreType = chore_type;
		this.globalCooldown = globalCooldown;
		this.localCooldown = localCooldown;
		this.lifeSpan = lifeSpan;
		this.initialDelay = ((max_initial_delay > 0f) ? UnityEngine.Random.Range(0f, max_initial_delay) : 0f);
		this.creationTime = GameClock.Instance.GetTime();
		ObjectLayer objectLayer = (overrideLayer == ObjectLayer.NumLayers) ? this.reactionLayer : overrideLayer;
		ReactionMonitor.Def def = gameObject.GetDef<ReactionMonitor.Def>();
		if (overrideLayer != objectLayer && def != null)
		{
			objectLayer = def.ReactionLayer;
		}
		this.reactionLayer = objectLayer;
		this.Initialize(follow_transform);
	}

	// Token: 0x060024FD RID: 9469 RVA: 0x000BC897 File Offset: 0x000BAA97
	public void Initialize(bool followTransform)
	{
		this.UpdateLocation();
		if (followTransform)
		{
			this.transformId = Singleton<CellChangeMonitor>.Instance.RegisterCellChangedHandler(this.gameObject.transform, new System.Action(this.UpdateLocation), "Reactable follow transform");
		}
	}

	// Token: 0x060024FE RID: 9470 RVA: 0x000BC8CE File Offset: 0x000BAACE
	public void Begin(GameObject reactor)
	{
		this.reactor = reactor;
		this.lastTriggerTime = GameClock.Instance.GetTime();
		this.InternalBegin();
	}

	// Token: 0x060024FF RID: 9471 RVA: 0x001D84CC File Offset: 0x001D66CC
	public void End()
	{
		this.InternalEnd();
		if (this.reactor != null)
		{
			GameObject gameObject = this.reactor;
			this.InternalEnd();
			this.reactor = null;
			if (gameObject != null)
			{
				ReactionMonitor.Instance smi = gameObject.GetSMI<ReactionMonitor.Instance>();
				if (smi != null)
				{
					smi.StopReaction();
				}
			}
		}
	}

	// Token: 0x06002500 RID: 9472 RVA: 0x001D851C File Offset: 0x001D671C
	public bool CanBegin(GameObject reactor, Navigator.ActiveTransition transition)
	{
		float time = GameClock.Instance.GetTime();
		float num = time - this.creationTime;
		float num2 = time - this.lastTriggerTime;
		if (num < this.initialDelay || num2 < this.globalCooldown)
		{
			return false;
		}
		ChoreConsumer component = reactor.GetComponent<ChoreConsumer>();
		Chore chore = (component != null) ? component.choreDriver.GetCurrentChore() : null;
		if (chore == null || this.choreType.priority <= chore.choreType.priority)
		{
			return false;
		}
		int num3 = 0;
		while (this.additionalPreconditions != null && num3 < this.additionalPreconditions.Count)
		{
			if (!this.additionalPreconditions[num3](reactor, transition))
			{
				return false;
			}
			num3++;
		}
		return this.InternalCanBegin(reactor, transition);
	}

	// Token: 0x06002501 RID: 9473 RVA: 0x000BC8ED File Offset: 0x000BAAED
	public bool IsExpired()
	{
		return GameClock.Instance.GetTime() - this.creationTime > this.lifeSpan;
	}

	// Token: 0x06002502 RID: 9474
	public abstract bool InternalCanBegin(GameObject reactor, Navigator.ActiveTransition transition);

	// Token: 0x06002503 RID: 9475
	public abstract void Update(float dt);

	// Token: 0x06002504 RID: 9476
	protected abstract void InternalBegin();

	// Token: 0x06002505 RID: 9477
	protected abstract void InternalEnd();

	// Token: 0x06002506 RID: 9478
	protected abstract void InternalCleanup();

	// Token: 0x06002507 RID: 9479 RVA: 0x001D85D8 File Offset: 0x001D67D8
	public void Cleanup()
	{
		this.End();
		this.InternalCleanup();
		if (this.transformId != -1)
		{
			Singleton<CellChangeMonitor>.Instance.UnregisterCellChangedHandler(this.transformId, new System.Action(this.UpdateLocation));
			this.transformId = -1;
		}
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
	}

	// Token: 0x06002508 RID: 9480 RVA: 0x001D8630 File Offset: 0x001D6830
	private void UpdateLocation()
	{
		GameScenePartitioner.Instance.Free(ref this.partitionerEntry);
		if (this.gameObject != null)
		{
			this.sourceCell = Grid.PosToCell(this.gameObject);
			Extents extents = new Extents(Grid.PosToXY(this.gameObject.transform.GetPosition()).x - this.rangeWidth / 2, Grid.PosToXY(this.gameObject.transform.GetPosition()).y - this.rangeHeight / 2, this.rangeWidth, this.rangeHeight);
			this.partitionerEntry = GameScenePartitioner.Instance.Add("Reactable", this, extents, GameScenePartitioner.Instance.objectLayers[(int)this.reactionLayer], null);
		}
	}

	// Token: 0x06002509 RID: 9481 RVA: 0x000BC908 File Offset: 0x000BAB08
	public Reactable AddPrecondition(Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		this.additionalPreconditions.Add(precondition);
		return this;
	}

	// Token: 0x0600250A RID: 9482 RVA: 0x000BC92A File Offset: 0x000BAB2A
	public void InsertPrecondition(int index, Reactable.ReactablePrecondition precondition)
	{
		if (this.additionalPreconditions == null)
		{
			this.additionalPreconditions = new List<Reactable.ReactablePrecondition>();
		}
		index = Math.Min(index, this.additionalPreconditions.Count);
		this.additionalPreconditions.Insert(index, precondition);
	}

	// Token: 0x04001985 RID: 6533
	private HandleVector<int>.Handle partitionerEntry;

	// Token: 0x04001986 RID: 6534
	protected GameObject gameObject;

	// Token: 0x04001987 RID: 6535
	public HashedString id;

	// Token: 0x04001988 RID: 6536
	public bool preventChoreInterruption = true;

	// Token: 0x04001989 RID: 6537
	public int sourceCell;

	// Token: 0x0400198A RID: 6538
	private int rangeWidth;

	// Token: 0x0400198B RID: 6539
	private int rangeHeight;

	// Token: 0x0400198C RID: 6540
	private int transformId = -1;

	// Token: 0x0400198D RID: 6541
	public float globalCooldown;

	// Token: 0x0400198E RID: 6542
	public float localCooldown;

	// Token: 0x0400198F RID: 6543
	public float lifeSpan = float.PositiveInfinity;

	// Token: 0x04001990 RID: 6544
	private float lastTriggerTime = -2.1474836E+09f;

	// Token: 0x04001991 RID: 6545
	private float initialDelay;

	// Token: 0x04001993 RID: 6547
	protected GameObject reactor;

	// Token: 0x04001994 RID: 6548
	private ChoreType choreType;

	// Token: 0x04001995 RID: 6549
	protected LoggerFSS log;

	// Token: 0x04001996 RID: 6550
	private List<Reactable.ReactablePrecondition> additionalPreconditions;

	// Token: 0x04001997 RID: 6551
	private ObjectLayer reactionLayer;

	// Token: 0x02000833 RID: 2099
	// (Invoke) Token: 0x0600250C RID: 9484
	public delegate bool ReactablePrecondition(GameObject go, Navigator.ActiveTransition transition);
}
