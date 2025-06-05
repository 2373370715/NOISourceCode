using System;
using KSerialization;

// Token: 0x02000E7A RID: 3706
[SerializationConfig(MemberSerialization.OptIn)]
public class LogicElementSensor : Switch, ISaveLoadable, ISim200ms
{
	// Token: 0x060048EF RID: 18671 RVA: 0x000D3BBA File Offset: 0x000D1DBA
	protected override void OnPrefabInit()
	{
		base.OnPrefabInit();
		base.GetComponent<Filterable>().onFilterChanged += this.OnElementSelected;
	}

	// Token: 0x060048F0 RID: 18672 RVA: 0x00265470 File Offset: 0x00263670
	protected override void OnSpawn()
	{
		base.OnSpawn();
		base.OnToggle += this.OnSwitchToggled;
		this.UpdateLogicCircuit();
		this.UpdateVisualState(true);
		this.wasOn = this.switchedOn;
		base.Subscribe<LogicElementSensor>(-592767678, LogicElementSensor.OnOperationalChangedDelegate);
	}

	// Token: 0x060048F1 RID: 18673 RVA: 0x002654C0 File Offset: 0x002636C0
	public void Sim200ms(float dt)
	{
		int i = Grid.PosToCell(this);
		if (this.sampleIdx < 8)
		{
			this.samples[this.sampleIdx] = (Grid.ElementIdx[i] == this.desiredElementIdx);
			this.sampleIdx++;
			return;
		}
		this.sampleIdx = 0;
		bool flag = true;
		bool[] array = this.samples;
		for (int j = 0; j < array.Length; j++)
		{
			flag = (array[j] && flag);
		}
		if (base.IsSwitchedOn != flag)
		{
			this.Toggle();
		}
	}

	// Token: 0x060048F2 RID: 18674 RVA: 0x000D3BD9 File Offset: 0x000D1DD9
	private void OnSwitchToggled(bool toggled_on)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060048F3 RID: 18675 RVA: 0x00265540 File Offset: 0x00263740
	private void UpdateLogicCircuit()
	{
		bool flag = this.switchedOn && base.GetComponent<Operational>().IsOperational;
		base.GetComponent<LogicPorts>().SendSignal(LogicSwitch.PORT_ID, flag ? 1 : 0);
	}

	// Token: 0x060048F4 RID: 18676 RVA: 0x0026557C File Offset: 0x0026377C
	private void UpdateVisualState(bool force = false)
	{
		if (this.wasOn != this.switchedOn || force)
		{
			this.wasOn = this.switchedOn;
			KBatchedAnimController component = base.GetComponent<KBatchedAnimController>();
			component.Play(this.switchedOn ? "on_pre" : "on_pst", KAnim.PlayMode.Once, 1f, 0f);
			component.Queue(this.switchedOn ? "on" : "off", KAnim.PlayMode.Once, 1f, 0f);
		}
	}

	// Token: 0x060048F5 RID: 18677 RVA: 0x00265604 File Offset: 0x00263804
	private void OnElementSelected(Tag element_tag)
	{
		if (!element_tag.IsValid)
		{
			return;
		}
		Element element = ElementLoader.GetElement(element_tag);
		bool on = true;
		if (element != null)
		{
			this.desiredElementIdx = ElementLoader.GetElementIndex(element.id);
			on = (element.id == SimHashes.Void || element.id == SimHashes.Vacuum);
		}
		base.GetComponent<KSelectable>().ToggleStatusItem(Db.Get().BuildingStatusItems.NoFilterElementSelected, on, null);
	}

	// Token: 0x060048F6 RID: 18678 RVA: 0x000D3BD9 File Offset: 0x000D1DD9
	private void OnOperationalChanged(object data)
	{
		this.UpdateLogicCircuit();
		this.UpdateVisualState(false);
	}

	// Token: 0x060048F7 RID: 18679 RVA: 0x0026473C File Offset: 0x0026293C
	protected override void UpdateSwitchStatus()
	{
		StatusItem status_item = this.switchedOn ? Db.Get().BuildingStatusItems.LogicSensorStatusActive : Db.Get().BuildingStatusItems.LogicSensorStatusInactive;
		base.GetComponent<KSelectable>().SetStatusItem(Db.Get().StatusItemCategories.Power, status_item, null);
	}

	// Token: 0x04003323 RID: 13091
	private bool wasOn;

	// Token: 0x04003324 RID: 13092
	public Element.State desiredState = Element.State.Gas;

	// Token: 0x04003325 RID: 13093
	private const int WINDOW_SIZE = 8;

	// Token: 0x04003326 RID: 13094
	private bool[] samples = new bool[8];

	// Token: 0x04003327 RID: 13095
	private int sampleIdx;

	// Token: 0x04003328 RID: 13096
	private ushort desiredElementIdx = ushort.MaxValue;

	// Token: 0x04003329 RID: 13097
	private static readonly EventSystem.IntraObjectHandler<LogicElementSensor> OnOperationalChangedDelegate = new EventSystem.IntraObjectHandler<LogicElementSensor>(delegate(LogicElementSensor component, object data)
	{
		component.OnOperationalChanged(data);
	});
}
