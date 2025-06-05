using System;
using System.Collections.Generic;

// Token: 0x020012D6 RID: 4822
public class EnergySim
{
	// Token: 0x17000621 RID: 1569
	// (get) Token: 0x060062EB RID: 25323 RVA: 0x000E4FAA File Offset: 0x000E31AA
	public HashSet<Generator> Generators
	{
		get
		{
			return this.generators;
		}
	}

	// Token: 0x060062EC RID: 25324 RVA: 0x000E4FB2 File Offset: 0x000E31B2
	public void AddGenerator(Generator generator)
	{
		this.generators.Add(generator);
	}

	// Token: 0x060062ED RID: 25325 RVA: 0x000E4FC1 File Offset: 0x000E31C1
	public void RemoveGenerator(Generator generator)
	{
		this.generators.Remove(generator);
	}

	// Token: 0x060062EE RID: 25326 RVA: 0x000E4FD0 File Offset: 0x000E31D0
	public void AddManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Add(manual_generator);
	}

	// Token: 0x060062EF RID: 25327 RVA: 0x000E4FDF File Offset: 0x000E31DF
	public void RemoveManualGenerator(ManualGenerator manual_generator)
	{
		this.manualGenerators.Remove(manual_generator);
	}

	// Token: 0x060062F0 RID: 25328 RVA: 0x000E4FEE File Offset: 0x000E31EE
	public void AddBattery(Battery battery)
	{
		this.batteries.Add(battery);
	}

	// Token: 0x060062F1 RID: 25329 RVA: 0x000E4FFD File Offset: 0x000E31FD
	public void RemoveBattery(Battery battery)
	{
		this.batteries.Remove(battery);
	}

	// Token: 0x060062F2 RID: 25330 RVA: 0x000E500C File Offset: 0x000E320C
	public void AddEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Add(energy_consumer);
	}

	// Token: 0x060062F3 RID: 25331 RVA: 0x000E501B File Offset: 0x000E321B
	public void RemoveEnergyConsumer(EnergyConsumer energy_consumer)
	{
		this.energyConsumers.Remove(energy_consumer);
	}

	// Token: 0x060062F4 RID: 25332 RVA: 0x002C67C8 File Offset: 0x002C49C8
	public void EnergySim200ms(float dt)
	{
		foreach (Generator generator in this.generators)
		{
			generator.EnergySim200ms(dt);
		}
		foreach (ManualGenerator manualGenerator in this.manualGenerators)
		{
			manualGenerator.EnergySim200ms(dt);
		}
		foreach (Battery battery in this.batteries)
		{
			battery.EnergySim200ms(dt);
		}
		foreach (EnergyConsumer energyConsumer in this.energyConsumers)
		{
			energyConsumer.EnergySim200ms(dt);
		}
	}

	// Token: 0x040046E8 RID: 18152
	private HashSet<Generator> generators = new HashSet<Generator>();

	// Token: 0x040046E9 RID: 18153
	private HashSet<ManualGenerator> manualGenerators = new HashSet<ManualGenerator>();

	// Token: 0x040046EA RID: 18154
	private HashSet<Battery> batteries = new HashSet<Battery>();

	// Token: 0x040046EB RID: 18155
	private HashSet<EnergyConsumer> energyConsumers = new HashSet<EnergyConsumer>();
}
