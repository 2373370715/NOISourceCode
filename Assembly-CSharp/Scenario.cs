using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Klei.AI;
using ProcGenGame;
using UnityEngine;

// Token: 0x0200187B RID: 6267
[AddComponentMenu("KMonoBehaviour/scripts/Scenario")]
public class Scenario : KMonoBehaviour
{
	// Token: 0x1700083F RID: 2111
	// (get) Token: 0x06008130 RID: 33072 RVA: 0x000F990E File Offset: 0x000F7B0E
	// (set) Token: 0x06008131 RID: 33073 RVA: 0x000F9916 File Offset: 0x000F7B16
	public bool[] ReplaceElementMask { get; set; }

	// Token: 0x06008132 RID: 33074 RVA: 0x000F991F File Offset: 0x000F7B1F
	public static void DestroyInstance()
	{
		Scenario.Instance = null;
	}

	// Token: 0x06008133 RID: 33075 RVA: 0x000F9927 File Offset: 0x000F7B27
	protected override void OnPrefabInit()
	{
		Scenario.Instance = this;
		SaveLoader instance = SaveLoader.Instance;
		instance.OnWorldGenComplete = (Action<Cluster>)Delegate.Combine(instance.OnWorldGenComplete, new Action<Cluster>(this.OnWorldGenComplete));
	}

	// Token: 0x06008134 RID: 33076 RVA: 0x000F9955 File Offset: 0x000F7B55
	private void OnWorldGenComplete(Cluster clusterLayout)
	{
		this.Init();
	}

	// Token: 0x06008135 RID: 33077 RVA: 0x00344E2C File Offset: 0x0034302C
	private void Init()
	{
		this.Bot = Grid.HeightInCells / 4;
		this.Left = 150;
		this.RootCell = Grid.OffsetCell(0, this.Left, this.Bot);
		this.ReplaceElementMask = new bool[Grid.CellCount];
	}

	// Token: 0x06008136 RID: 33078 RVA: 0x00344E7C File Offset: 0x0034307C
	private void DigHole(int x, int y, int width, int height)
	{
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y + j), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 200f, -1f, byte.MaxValue, 0, -1);
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + j), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
				SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + width, y + j), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			}
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y - 1), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x + i, y + height), SimHashes.Ice, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
		}
	}

	// Token: 0x06008137 RID: 33079 RVA: 0x000F995D File Offset: 0x000F7B5D
	private void Fill(int x, int y, SimHashes id = SimHashes.Ice)
	{
		SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), id, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
	}

	// Token: 0x06008138 RID: 33080 RVA: 0x00344FBC File Offset: 0x003431BC
	private void PlaceColumn(int x, int y, int height)
	{
		for (int i = 0; i < height; i++)
		{
			SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y + i), SimHashes.Ice, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
		}
	}

	// Token: 0x06008139 RID: 33081 RVA: 0x0034500C File Offset: 0x0034320C
	private void PlaceTileX(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(left + i, bot, "Tile", SimHashes.Cuprite);
		}
	}

	// Token: 0x0600813A RID: 33082 RVA: 0x0034503C File Offset: 0x0034323C
	private void PlaceTileY(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(left, bot + i, "Tile", SimHashes.Cuprite);
		}
	}

	// Token: 0x0600813B RID: 33083 RVA: 0x000F998D File Offset: 0x000F7B8D
	private void Clear(int x, int y)
	{
		SimMessages.ReplaceElement(Grid.OffsetCell(this.RootCell, x, y), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 10000f, -1f, byte.MaxValue, 0, -1);
	}

	// Token: 0x0600813C RID: 33084 RVA: 0x0034506C File Offset: 0x0034326C
	private void PlacerLadder(int x, int y, int amount)
	{
		int num = 1;
		if (amount < 0)
		{
			amount = -amount;
			num = -1;
		}
		for (int i = 0; i < amount; i++)
		{
			this.PlaceBuilding(x, y + i * num, "Ladder", SimHashes.Cuprite);
		}
	}

	// Token: 0x0600813D RID: 33085 RVA: 0x003450A8 File Offset: 0x003432A8
	private void PlaceBuildings(int left, int bot)
	{
		this.PlaceBuilding(++left, bot, "ManualGenerator", SimHashes.Iron);
		this.PlaceBuilding(left += 2, bot, "OxygenMachine", SimHashes.Steel);
		this.PlaceBuilding(left += 2, bot, "SpaceHeater", SimHashes.Steel);
		this.PlaceBuilding(++left, bot, "Electrolyzer", SimHashes.Steel);
		this.PlaceBuilding(++left, bot, "Smelter", SimHashes.Steel);
		this.SpawnOre(left, bot + 1, SimHashes.Ice);
	}

	// Token: 0x0600813E RID: 33086 RVA: 0x000F99C1 File Offset: 0x000F7BC1
	private IEnumerator TurnOn(GameObject go)
	{
		yield return null;
		yield return null;
		go.GetComponent<BuildingEnabledButton>().IsEnabled = true;
		yield break;
	}

	// Token: 0x0600813F RID: 33087 RVA: 0x0034513C File Offset: 0x0034333C
	private void SetupPlacerTest(Scenario.Builder b, Element element)
	{
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (buildingDef.Name != "Excavator")
			{
				b.Placer(buildingDef.PrefabID, element);
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008140 RID: 33088 RVA: 0x003451B8 File Offset: 0x003433B8
	private void SetupBuildingTest(Scenario.RowLayout row_layout, bool is_powered, bool break_building)
	{
		Scenario.Builder builder = null;
		int num = 0;
		foreach (BuildingDef buildingDef in Assets.BuildingDefs)
		{
			if (builder == null)
			{
				builder = row_layout.NextRow();
				num = this.Left;
				if (is_powered)
				{
					builder.Minion(null);
					builder.Minion(null);
				}
			}
			if (buildingDef.Name != "Excavator")
			{
				GameObject gameObject = builder.Building(buildingDef.PrefabID);
				if (break_building)
				{
					BuildingHP component = gameObject.GetComponent<BuildingHP>();
					if (component != null)
					{
						component.DoDamage(int.MaxValue);
					}
				}
			}
			if (builder.Left > num + 100)
			{
				builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
				builder = null;
			}
		}
		builder.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008141 RID: 33089 RVA: 0x000F99D0 File Offset: 0x000F7BD0
	private IEnumerator RunAfterNextUpdateRoutine(System.Action action)
	{
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		yield return null;
		action();
		yield break;
	}

	// Token: 0x06008142 RID: 33090 RVA: 0x000F99DF File Offset: 0x000F7BDF
	private void RunAfterNextUpdate(System.Action action)
	{
		base.StartCoroutine(this.RunAfterNextUpdateRoutine(action));
	}

	// Token: 0x06008143 RID: 33091 RVA: 0x003452A4 File Offset: 0x003434A4
	public void SetupFabricatorTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("ManualGenerator");
		b.Ore(3, SimHashes.Cuprite);
		b.Minion(null);
		b.Building("Masonry");
		b.InAndOuts();
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008144 RID: 33092 RVA: 0x000F99EF File Offset: 0x000F7BEF
	public void SetupDoorTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(1, 0);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008145 RID: 33093 RVA: 0x000F9A29 File Offset: 0x000F7C29
	public void SetupHatchTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008146 RID: 33094 RVA: 0x000F9A5B File Offset: 0x000F7C5B
	public void SetupPropaneGeneratorTest(Scenario.Builder b)
	{
		b.Building("PropaneGenerator");
		b.Building("OxygenMachine");
		b.FinalizeRoom(SimHashes.Propane, SimHashes.Steel);
	}

	// Token: 0x06008147 RID: 33095 RVA: 0x003452FC File Offset: 0x003434FC
	public void SetupAirLockTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(1, 0);
		b.Minion(null);
		b.Jump(1, 0);
		b.Building("PoweredAirlock");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008148 RID: 33096 RVA: 0x00345350 File Offset: 0x00343550
	public void SetupBedTest(Scenario.Builder b)
	{
		b.Minion(delegate(GameObject go)
		{
			go.GetAmounts().SetValue("Stamina", 10f);
		});
		b.Building("ManualGenerator");
		b.Minion(delegate(GameObject go)
		{
			go.GetAmounts().SetValue("Stamina", 10f);
		});
		b.Building("ComfyBed");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008149 RID: 33097 RVA: 0x000F9A86 File Offset: 0x000F7C86
	public void SetupHexapedTest(Scenario.Builder b)
	{
		b.Fill(4, 4, SimHashes.Oxygen);
		b.Prefab("Hexaped", null);
		b.Jump(2, 0);
		b.Ore(1, SimHashes.Iron);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0600814A RID: 33098 RVA: 0x003453D0 File Offset: 0x003435D0
	public void SetupElectrolyzerTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Building("ManualGenerator");
		b.Ore(3, SimHashes.Ice);
		b.Minion(null);
		b.Building("Electrolyzer");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0600814B RID: 33099 RVA: 0x00345420 File Offset: 0x00343620
	public void SetupOrePerformanceTest(Scenario.Builder b)
	{
		int num = 20;
		int num2 = 20;
		int left = b.Left;
		int bot = b.Bot;
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j += 2)
			{
				b.Jump(i, j);
				b.Ore(1, SimHashes.Cuprite);
				b.JumpTo(left, bot);
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0600814C RID: 33100 RVA: 0x00345490 File Offset: 0x00343690
	public void SetupFeedingTest(Scenario.Builder b)
	{
		b.FillOffsets(SimHashes.IgneousRock, new int[]
		{
			1,
			0,
			3,
			0,
			3,
			1,
			5,
			0,
			5,
			1,
			5,
			2,
			7,
			0,
			7,
			1,
			7,
			2,
			9,
			0,
			9,
			1,
			11,
			0
		});
		b.PrefabOffsets("Hexaped", new int[]
		{
			0,
			0,
			2,
			0,
			4,
			0,
			7,
			3,
			9,
			2,
			11,
			1
		});
		b.OreOffsets(1, SimHashes.IronOre, new int[]
		{
			1,
			1,
			3,
			2,
			5,
			3,
			8,
			0,
			10,
			0,
			12,
			0
		});
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0600814D RID: 33101 RVA: 0x00345508 File Offset: 0x00343708
	public void SetupLiquifierTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Minion(null);
		b.Ore(2, SimHashes.Ice);
		b.Building("ManualGenerator");
		b.Building("Liquifier");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0600814E RID: 33102 RVA: 0x00345558 File Offset: 0x00343758
	public void SetupFallTest(Scenario.Builder b)
	{
		b.Jump(0, 5);
		b.Minion(null);
		b.Jump(0, -1);
		b.Building("Tile");
		b.Building("Tile");
		b.Building("Tile");
		b.Jump(-1, 1);
		b.Minion(null);
		b.Jump(2, 0);
		b.Minion(null);
		b.Jump(0, -1);
		b.Building("Tile");
		b.Jump(2, 1);
		b.Minion(null);
		b.Building("Ladder");
		b.Jump(-1, -1);
		b.Building("Tile");
		b.Jump(-1, -3);
		b.Building("Ladder");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0600814F RID: 33103 RVA: 0x00345628 File Offset: 0x00343828
	public void SetupClimbTest(int left, int bot)
	{
		this.DigHole(left, bot, 13, 5);
		this.SpawnPrefab(left + 1, bot + 1, "Minion", Grid.SceneLayer.Ore);
		int num = left + 2;
		this.Clear(num++, bot - 1);
		num++;
		this.Fill(num++, bot, SimHashes.Ice);
		num++;
		this.Clear(num, bot - 1);
		this.Clear(num++, bot - 2);
		this.Fill(num++, bot, SimHashes.Ice);
		this.Clear(num, bot - 1);
		this.Clear(num++, bot - 2);
		num++;
		this.Fill(num, bot, SimHashes.Ice);
		this.Fill(num, bot + 1, SimHashes.Ice);
	}

	// Token: 0x06008150 RID: 33104 RVA: 0x003456E0 File Offset: 0x003438E0
	private void SetupSuitRechargeTest(Scenario.Builder b)
	{
		b.Prefab("PressureSuit", delegate(GameObject go)
		{
			go.GetComponent<SuitTank>().Empty();
		});
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("SuitRecharger");
		b.Minion(null);
		b.Building("GasVent");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008151 RID: 33105 RVA: 0x0034575C File Offset: 0x0034395C
	private void SetupSuitTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Prefab("PressureSuit", null);
		b.Jump(1, 2);
		b.Building("Tile");
		b.Jump(-1, -2);
		b.Building("Door");
		b.Building("ManualGenerator");
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008152 RID: 33106 RVA: 0x003457C4 File Offset: 0x003439C4
	private void SetupTwoKelvinsOneSuitTest(Scenario.Builder b)
	{
		b.Minion(null);
		b.Jump(2, 0);
		b.Building("Door");
		b.Jump(2, 0);
		b.Minion(null);
		b.Prefab("PressureSuit", null);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008153 RID: 33107 RVA: 0x00345818 File Offset: 0x00343A18
	public void Clear()
	{
		foreach (Brain brain in Components.Brains.Items)
		{
			UnityEngine.Object.Destroy(brain.gameObject);
		}
		foreach (Pickupable pickupable in Components.Pickupables.Items)
		{
			UnityEngine.Object.Destroy(pickupable.gameObject);
		}
		foreach (BuildingComplete buildingComplete in Components.BuildingCompletes.Items)
		{
			UnityEngine.Object.Destroy(buildingComplete.gameObject);
		}
	}

	// Token: 0x06008154 RID: 33108 RVA: 0x00345904 File Offset: 0x00343B04
	public void SetupGameplayTest()
	{
		this.Init();
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		if (this.ClearExistingScene)
		{
			this.Clear();
		}
		Scenario.RowLayout rowLayout = new Scenario.RowLayout(0, 0);
		if (this.CementMixerTest)
		{
			this.SetupCementMixerTest(rowLayout.NextRow());
		}
		if (this.RockCrusherTest)
		{
			this.SetupRockCrusherTest(rowLayout.NextRow());
		}
		if (this.PropaneGeneratorTest)
		{
			this.SetupPropaneGeneratorTest(rowLayout.NextRow());
		}
		if (this.DoorTest)
		{
			this.SetupDoorTest(rowLayout.NextRow());
		}
		if (this.HatchTest)
		{
			this.SetupHatchTest(rowLayout.NextRow());
		}
		if (this.AirLockTest)
		{
			this.SetupAirLockTest(rowLayout.NextRow());
		}
		if (this.BedTest)
		{
			this.SetupBedTest(rowLayout.NextRow());
		}
		if (this.LiquifierTest)
		{
			this.SetupLiquifierTest(rowLayout.NextRow());
		}
		if (this.SuitTest)
		{
			this.SetupSuitTest(rowLayout.NextRow());
		}
		if (this.SuitRechargeTest)
		{
			this.SetupSuitRechargeTest(rowLayout.NextRow());
		}
		if (this.TwoKelvinsOneSuitTest)
		{
			this.SetupTwoKelvinsOneSuitTest(rowLayout.NextRow());
		}
		if (this.FabricatorTest)
		{
			this.SetupFabricatorTest(rowLayout.NextRow());
		}
		if (this.ElectrolyzerTest)
		{
			this.SetupElectrolyzerTest(rowLayout.NextRow());
		}
		if (this.HexapedTest)
		{
			this.SetupHexapedTest(rowLayout.NextRow());
		}
		if (this.FallTest)
		{
			this.SetupFallTest(rowLayout.NextRow());
		}
		if (this.FeedingTest)
		{
			this.SetupFeedingTest(rowLayout.NextRow());
		}
		if (this.OrePerformanceTest)
		{
			this.SetupOrePerformanceTest(rowLayout.NextRow());
		}
		if (this.KilnTest)
		{
			this.SetupKilnTest(rowLayout.NextRow());
		}
	}

	// Token: 0x06008155 RID: 33109 RVA: 0x000F9AC6 File Offset: 0x000F7CC6
	private GameObject SpawnMinion(int x, int y)
	{
		return this.SpawnPrefab(x, y, "Minion", Grid.SceneLayer.Move);
	}

	// Token: 0x06008156 RID: 33110 RVA: 0x00345AB0 File Offset: 0x00343CB0
	private void SetupLadderTest(int left, int bot)
	{
		int num = 5;
		this.DigHole(left, bot, 13, num);
		this.SpawnMinion(left + 1, bot);
		int x = left + 1;
		this.PlacerLadder(x++, bot, num);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x, bot);
		this.PlacerLadder(x++, bot + 1, num - 1);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot, num);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot + 1, num - 1);
		this.PlaceColumn(x++, bot, num);
		this.SpawnMinion(x++, bot);
		this.PlacerLadder(x++, bot - 1, -num);
	}

	// Token: 0x06008157 RID: 33111 RVA: 0x00345B8C File Offset: 0x00343D8C
	public void PlaceUtilitiesX(int left, int bot, int amount)
	{
		for (int i = 0; i < amount; i++)
		{
			this.PlaceUtilities(left + i, bot);
		}
	}

	// Token: 0x06008158 RID: 33112 RVA: 0x000F9AD7 File Offset: 0x000F7CD7
	public void PlaceUtilities(int left, int bot)
	{
		this.PlaceBuilding(left, bot, "Wire", SimHashes.Cuprite);
		this.PlaceBuilding(left, bot, "GasConduit", SimHashes.Cuprite);
	}

	// Token: 0x06008159 RID: 33113 RVA: 0x00345BB0 File Offset: 0x00343DB0
	public void SetupVisualTest()
	{
		this.Init();
		Scenario.RowLayout row_layout = new Scenario.RowLayout(this.Left, this.Bot);
		this.SetupBuildingTest(row_layout, false, false);
	}

	// Token: 0x0600815A RID: 33114 RVA: 0x00345BE0 File Offset: 0x00343DE0
	private void SpawnMaterialTest(Scenario.Builder b)
	{
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsSolid)
			{
				b.Element = element.id;
				b.Building("Generator");
			}
		}
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x0600815B RID: 33115 RVA: 0x000F9AFF File Offset: 0x000F7CFF
	public GameObject PlaceBuilding(int x, int y, string prefab_id, SimHashes element = SimHashes.Cuprite)
	{
		return Scenario.PlaceBuilding(this.RootCell, x, y, prefab_id, element);
	}

	// Token: 0x0600815C RID: 33116 RVA: 0x00345C5C File Offset: 0x00343E5C
	public static GameObject PlaceBuilding(int root_cell, int x, int y, string prefab_id, SimHashes element = SimHashes.Cuprite)
	{
		int cell = Grid.OffsetCell(root_cell, x, y);
		BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
		if (buildingDef == null || buildingDef.PlacementOffsets == null)
		{
			DebugUtil.LogErrorArgs(new object[]
			{
				"Missing def for",
				prefab_id
			});
		}
		Element element2 = ElementLoader.FindElementByHash(element);
		global::Debug.Assert(element2 != null, string.Concat(new string[]
		{
			"Missing primary element '",
			Enum.GetName(typeof(SimHashes), element),
			"' in '",
			prefab_id,
			"'"
		}));
		GameObject gameObject = buildingDef.Build(buildingDef.GetBuildingCell(cell), Orientation.Neutral, null, new Tag[]
		{
			element2.tag,
			ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
		}, 293.15f, false, -1f);
		PrimaryElement component = gameObject.GetComponent<PrimaryElement>();
		component.InternalTemperature = 300f;
		component.Temperature = 300f;
		return gameObject;
	}

	// Token: 0x0600815D RID: 33117 RVA: 0x00345D50 File Offset: 0x00343F50
	private void SpawnOre(int x, int y, SimHashes element = SimHashes.Cuprite)
	{
		this.RunAfterNextUpdate(delegate
		{
			Vector3 position = Grid.CellToPosCCC(Grid.OffsetCell(this.RootCell, x, y), Grid.SceneLayer.Ore);
			position.x += UnityEngine.Random.Range(-0.1f, 0.1f);
			ElementLoader.FindElementByHash(element).substance.SpawnResource(position, 4000f, 293f, byte.MaxValue, 0, false, false, false);
		});
	}

	// Token: 0x0600815E RID: 33118 RVA: 0x000F9B11 File Offset: 0x000F7D11
	public GameObject SpawnPrefab(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		return Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer);
	}

	// Token: 0x0600815F RID: 33119 RVA: 0x00345D94 File Offset: 0x00343F94
	public void SpawnPrefabLate(int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		this.RunAfterNextUpdate(delegate
		{
			Scenario.SpawnPrefab(this.RootCell, x, y, name, scene_layer);
		});
	}

	// Token: 0x06008160 RID: 33120 RVA: 0x00345DE0 File Offset: 0x00343FE0
	public static GameObject SpawnPrefab(int RootCell, int x, int y, string name, Grid.SceneLayer scene_layer = Grid.SceneLayer.Ore)
	{
		int cell = Grid.OffsetCell(RootCell, x, y);
		GameObject prefab = Assets.GetPrefab(TagManager.Create(name));
		if (prefab == null)
		{
			return null;
		}
		return GameUtil.KInstantiate(prefab, Grid.CellToPosCBC(cell, scene_layer), scene_layer, null, 0);
	}

	// Token: 0x06008161 RID: 33121 RVA: 0x00345E20 File Offset: 0x00344020
	public void SetupElementTest()
	{
		this.Init();
		PropertyTextures.FogOfWarScale = 1f;
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		this.Clear();
		Scenario.Builder builder = new Scenario.RowLayout(0, 0).NextRow();
		HashSet<Element> elements = new HashSet<Element>();
		int bot = builder.Bot;
		foreach (Element element5 in (from element in ElementLoader.elements
		where element.IsSolid
		orderby element.highTempTransitionTarget
		select element).ToList<Element>())
		{
			if (element5.IsSolid)
			{
				Element element2 = element5;
				int left = builder.Left;
				bool hasTransitionUp;
				do
				{
					elements.Add(element2);
					builder.Hole(2, 3);
					builder.Fill(2, 2, element2.id);
					builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
					builder = new Scenario.Builder(left, builder.Bot + 4, SimHashes.Copper);
					hasTransitionUp = element2.HasTransitionUp;
					if (hasTransitionUp)
					{
						element2 = element2.highTempTransition;
					}
				}
				while (hasTransitionUp);
				builder = new Scenario.Builder(left + 3, bot, SimHashes.Copper);
			}
		}
		foreach (Element element3 in (from element in ElementLoader.elements
		where element.IsLiquid && !elements.Contains(element)
		orderby element.highTempTransitionTarget
		select element).ToList<Element>())
		{
			int left2 = builder.Left;
			bool hasTransitionUp2;
			do
			{
				elements.Add(element3);
				builder.Hole(2, 3);
				builder.Fill(2, 2, element3.id);
				builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
				builder = new Scenario.Builder(left2, builder.Bot + 4, SimHashes.Copper);
				hasTransitionUp2 = element3.HasTransitionUp;
				if (hasTransitionUp2)
				{
					element3 = element3.highTempTransition;
				}
			}
			while (hasTransitionUp2);
			builder = new Scenario.Builder(left2 + 3, bot, SimHashes.Copper);
		}
		foreach (Element element4 in (from element in ElementLoader.elements
		where element.state == Element.State.Gas && !elements.Contains(element)
		select element).ToList<Element>())
		{
			int left3 = builder.Left;
			builder.Hole(2, 3);
			builder.Fill(2, 2, element4.id);
			builder.FinalizeRoom(SimHashes.Vacuum, SimHashes.Unobtanium);
			builder = new Scenario.Builder(left3, builder.Bot + 4, SimHashes.Copper);
			builder = new Scenario.Builder(left3 + 3, bot, SimHashes.Copper);
		}
	}

	// Token: 0x06008162 RID: 33122 RVA: 0x00346144 File Offset: 0x00344344
	private void InitDebugScenario()
	{
		this.Init();
		PropertyTextures.FogOfWarScale = 1f;
		Vector3 pos = Grid.CellToPosCCC(this.RootCell, Grid.SceneLayer.Background);
		CameraController.Instance.SnapTo(pos);
		this.Clear();
	}

	// Token: 0x06008163 RID: 33123 RVA: 0x00346180 File Offset: 0x00344380
	public void SetupTileTest()
	{
		this.InitDebugScenario();
		for (int i = 0; i < Grid.HeightInCells; i++)
		{
			for (int j = 0; j < Grid.WidthInCells; j++)
			{
				SimMessages.ReplaceElement(Grid.XYToCell(j, i), SimHashes.Oxygen, CellEventLogger.Instance.Scenario, 100f, -1f, byte.MaxValue, 0, -1);
			}
		}
		Scenario.Builder builder = new Scenario.RowLayout(0, 0).NextRow();
		for (int k = 0; k < 16; k++)
		{
			builder.Jump(0, 0);
			builder.Fill(1, 1, ((k & 1) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(1, 0);
			builder.Fill(1, 1, ((k & 2) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(-1, 1);
			builder.Fill(1, 1, ((k & 4) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(1, 0);
			builder.Fill(1, 1, ((k & 8) != 0) ? SimHashes.Copper : SimHashes.Diamond);
			builder.Jump(2, -1);
		}
	}

	// Token: 0x06008164 RID: 33124 RVA: 0x0034628C File Offset: 0x0034448C
	public void SetupRiverTest()
	{
		this.InitDebugScenario();
		int num = Mathf.Min(64, Grid.WidthInCells);
		int num2 = Mathf.Min(64, Grid.HeightInCells);
		List<Element> list = new List<Element>();
		foreach (Element element in ElementLoader.elements)
		{
			if (element.IsLiquid)
			{
				list.Add(element);
			}
		}
		for (int i = 0; i < num2; i++)
		{
			for (int j = 0; j < num; j++)
			{
				SimHashes new_element = (i == 0) ? SimHashes.Unobtanium : SimHashes.Oxygen;
				SimMessages.ReplaceElement(Grid.XYToCell(j, i), new_element, CellEventLogger.Instance.Scenario, 1000f, -1f, byte.MaxValue, 0, -1);
			}
		}
	}

	// Token: 0x06008165 RID: 33125 RVA: 0x000F9B23 File Offset: 0x000F7D23
	public void SetupRockCrusherTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("Crusher");
		b.Minion(null);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008166 RID: 33126 RVA: 0x0034636C File Offset: 0x0034456C
	public void SetupCementMixerTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("Generator");
		b.Minion(null);
		b.Building("Crusher");
		b.Minion(null);
		b.Minion(null);
		b.Building("Mixer");
		b.Ore(20, SimHashes.SedimentaryRock);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x06008167 RID: 33127 RVA: 0x003463D8 File Offset: 0x003445D8
	public void SetupKilnTest(Scenario.Builder b)
	{
		this.InitDebugScenario();
		b.Building("ManualGenerator");
		b.Minion(null);
		b.Building("Kiln");
		b.Minion(null);
		b.Ore(20, SimHashes.SandCement);
		b.FinalizeRoom(SimHashes.Oxygen, SimHashes.Steel);
	}

	// Token: 0x04006253 RID: 25171
	private int Bot;

	// Token: 0x04006254 RID: 25172
	private int Left;

	// Token: 0x04006255 RID: 25173
	public int RootCell;

	// Token: 0x04006257 RID: 25175
	public static Scenario Instance;

	// Token: 0x04006258 RID: 25176
	public bool PropaneGeneratorTest = true;

	// Token: 0x04006259 RID: 25177
	public bool HatchTest = true;

	// Token: 0x0400625A RID: 25178
	public bool DoorTest = true;

	// Token: 0x0400625B RID: 25179
	public bool AirLockTest = true;

	// Token: 0x0400625C RID: 25180
	public bool BedTest = true;

	// Token: 0x0400625D RID: 25181
	public bool SuitTest = true;

	// Token: 0x0400625E RID: 25182
	public bool SuitRechargeTest = true;

	// Token: 0x0400625F RID: 25183
	public bool FabricatorTest = true;

	// Token: 0x04006260 RID: 25184
	public bool ElectrolyzerTest = true;

	// Token: 0x04006261 RID: 25185
	public bool HexapedTest = true;

	// Token: 0x04006262 RID: 25186
	public bool FallTest = true;

	// Token: 0x04006263 RID: 25187
	public bool FeedingTest = true;

	// Token: 0x04006264 RID: 25188
	public bool OrePerformanceTest = true;

	// Token: 0x04006265 RID: 25189
	public bool TwoKelvinsOneSuitTest = true;

	// Token: 0x04006266 RID: 25190
	public bool LiquifierTest = true;

	// Token: 0x04006267 RID: 25191
	public bool RockCrusherTest = true;

	// Token: 0x04006268 RID: 25192
	public bool CementMixerTest = true;

	// Token: 0x04006269 RID: 25193
	public bool KilnTest = true;

	// Token: 0x0400626A RID: 25194
	public bool ClearExistingScene = true;

	// Token: 0x0200187C RID: 6268
	public class RowLayout
	{
		// Token: 0x06008169 RID: 33129 RVA: 0x000F9B62 File Offset: 0x000F7D62
		public RowLayout(int left, int bot)
		{
			this.Left = left;
			this.Bot = bot;
		}

		// Token: 0x0600816A RID: 33130 RVA: 0x003464C8 File Offset: 0x003446C8
		public Scenario.Builder NextRow()
		{
			if (this.Builder != null)
			{
				this.Bot = this.Builder.Max.y + 1;
			}
			this.Builder = new Scenario.Builder(this.Left, this.Bot, SimHashes.Copper);
			return this.Builder;
		}

		// Token: 0x0400626B RID: 25195
		public int Left;

		// Token: 0x0400626C RID: 25196
		public int Bot;

		// Token: 0x0400626D RID: 25197
		public Scenario.Builder Builder;
	}

	// Token: 0x0200187D RID: 6269
	public class Builder
	{
		// Token: 0x0600816B RID: 33131 RVA: 0x00346518 File Offset: 0x00344718
		public Builder(int left, int bot, SimHashes element = SimHashes.Copper)
		{
			this.Left = left;
			this.Bot = bot;
			this.Element = element;
			this.Scenario = Scenario.Instance;
			this.PlaceUtilities = true;
			this.Min = new Vector2I(left, bot);
			this.Max = new Vector2I(left, bot);
		}

		// Token: 0x0600816C RID: 33132 RVA: 0x0034656C File Offset: 0x0034476C
		private void UpdateMinMax(int x, int y)
		{
			this.Min.x = Math.Min(x, this.Min.x);
			this.Min.y = Math.Min(y, this.Min.y);
			this.Max.x = Math.Max(x + 1, this.Max.x);
			this.Max.y = Math.Max(y + 1, this.Max.y);
		}

		// Token: 0x0600816D RID: 33133 RVA: 0x003465F0 File Offset: 0x003447F0
		public void Utilities(int count)
		{
			for (int i = 0; i < count; i++)
			{
				this.Scenario.PlaceUtilities(this.Left, this.Bot);
				this.Left++;
			}
		}

		// Token: 0x0600816E RID: 33134 RVA: 0x00346630 File Offset: 0x00344830
		public void BuildingOffsets(string prefab_id, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Building(prefab_id);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600816F RID: 33135 RVA: 0x00346680 File Offset: 0x00344880
		public void Placer(string prefab_id, Element element)
		{
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			int buildingCell = buildingDef.GetBuildingCell(Grid.OffsetCell(Scenario.Instance.RootCell, this.Left, this.Bot));
			Vector3 pos = Grid.CellToPosCBC(buildingCell, Grid.SceneLayer.Building);
			this.UpdateMinMax(this.Left, this.Bot);
			this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
			this.Left += buildingDef.WidthInCells;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				Assets.GetBuildingDef(prefab_id).TryPlace(null, pos, Orientation.Neutral, new Tag[]
				{
					element.tag,
					ElementLoader.FindElementByHash(SimHashes.SedimentaryRock).tag
				}, 0);
			});
		}

		// Token: 0x06008170 RID: 33136 RVA: 0x00346740 File Offset: 0x00344940
		public GameObject Building(string prefab_id)
		{
			GameObject result = this.Scenario.PlaceBuilding(this.Left, this.Bot, prefab_id, this.Element);
			BuildingDef buildingDef = Assets.GetBuildingDef(prefab_id);
			this.UpdateMinMax(this.Left, this.Bot);
			this.UpdateMinMax(this.Left + buildingDef.WidthInCells - 1, this.Bot + buildingDef.HeightInCells - 1);
			if (this.PlaceUtilities)
			{
				for (int i = 0; i < buildingDef.WidthInCells; i++)
				{
					this.UpdateMinMax(this.Left + i, this.Bot);
					this.Scenario.PlaceUtilities(this.Left + i, this.Bot);
				}
			}
			this.Left += buildingDef.WidthInCells;
			return result;
		}

		// Token: 0x06008171 RID: 33137 RVA: 0x00346804 File Offset: 0x00344A04
		public void Minion(Action<GameObject> on_spawn = null)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			int left = this.Left;
			int bot = this.Bot;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				GameObject obj = this.Scenario.SpawnMinion(left, bot);
				if (on_spawn != null)
				{
					on_spawn(obj);
				}
			});
		}

		// Token: 0x06008172 RID: 33138 RVA: 0x000F9B78 File Offset: 0x000F7D78
		private GameObject Hexaped()
		{
			return this.Scenario.SpawnPrefab(this.Left, this.Bot, "Hexaped", Grid.SceneLayer.Front);
		}

		// Token: 0x06008173 RID: 33139 RVA: 0x00346868 File Offset: 0x00344A68
		public void OreOffsets(int count, SimHashes element, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Ore(count, element);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x06008174 RID: 33140 RVA: 0x003468B8 File Offset: 0x00344AB8
		public void Ore(int count = 1, SimHashes element = SimHashes.Cuprite)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			for (int i = 0; i < count; i++)
			{
				this.Scenario.SpawnOre(this.Left, this.Bot, element);
			}
		}

		// Token: 0x06008175 RID: 33141 RVA: 0x003468FC File Offset: 0x00344AFC
		public void PrefabOffsets(string prefab_id, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Prefab(prefab_id, null);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x06008176 RID: 33142 RVA: 0x0034694C File Offset: 0x00344B4C
		public void Prefab(string prefab_id, Action<GameObject> on_spawn = null)
		{
			this.UpdateMinMax(this.Left, this.Bot);
			int left = this.Left;
			int bot = this.Bot;
			this.Scenario.RunAfterNextUpdate(delegate
			{
				GameObject obj = this.Scenario.SpawnPrefab(left, bot, prefab_id, Grid.SceneLayer.Ore);
				if (on_spawn != null)
				{
					on_spawn(obj);
				}
			});
		}

		// Token: 0x06008177 RID: 33143 RVA: 0x003469B8 File Offset: 0x00344BB8
		public void Wall(int height)
		{
			for (int i = 0; i < height; i++)
			{
				this.Scenario.PlaceBuilding(this.Left, this.Bot + i, "Tile", SimHashes.Cuprite);
				this.UpdateMinMax(this.Left, this.Bot + i);
				if (this.PlaceUtilities)
				{
					this.Scenario.PlaceUtilities(this.Left, this.Bot + i);
				}
			}
			this.Left++;
		}

		// Token: 0x06008178 RID: 33144 RVA: 0x000F9B98 File Offset: 0x000F7D98
		public void Jump(int x = 0, int y = 0)
		{
			this.Left += x;
			this.Bot += y;
		}

		// Token: 0x06008179 RID: 33145 RVA: 0x000F9BB6 File Offset: 0x000F7DB6
		public void JumpTo(int left, int bot)
		{
			this.Left = left;
			this.Bot = bot;
		}

		// Token: 0x0600817A RID: 33146 RVA: 0x00346A38 File Offset: 0x00344C38
		public void Hole(int width, int height)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num = Grid.OffsetCell(this.Scenario.RootCell, this.Left + i, this.Bot + j);
					this.UpdateMinMax(this.Left + i, this.Bot + j);
					SimMessages.ReplaceElement(num, SimHashes.Vacuum, CellEventLogger.Instance.Scenario, 0f, -1f, byte.MaxValue, 0, -1);
					this.Scenario.ReplaceElementMask[num] = true;
				}
			}
		}

		// Token: 0x0600817B RID: 33147 RVA: 0x00346AC8 File Offset: 0x00344CC8
		public void FillOffsets(SimHashes element, params int[] offsets)
		{
			int left = this.Left;
			int bot = this.Bot;
			for (int i = 0; i < offsets.Length / 2; i++)
			{
				this.Jump(offsets[i * 2], offsets[i * 2 + 1]);
				this.Fill(1, 1, element);
				this.JumpTo(left, bot);
			}
		}

		// Token: 0x0600817C RID: 33148 RVA: 0x00346B18 File Offset: 0x00344D18
		public void Fill(int width, int height, SimHashes element)
		{
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					int num = Grid.OffsetCell(this.Scenario.RootCell, this.Left + i, this.Bot + j);
					this.UpdateMinMax(this.Left + i, this.Bot + j);
					SimMessages.ReplaceElement(num, element, CellEventLogger.Instance.Scenario, 5000f, -1f, byte.MaxValue, 0, -1);
					this.Scenario.ReplaceElementMask[num] = true;
				}
			}
		}

		// Token: 0x0600817D RID: 33149 RVA: 0x00346BA4 File Offset: 0x00344DA4
		public void InAndOuts()
		{
			this.Wall(3);
			this.Building("GasVent");
			this.Hole(3, 3);
			this.Utilities(2);
			this.Wall(3);
			this.Building("LiquidVent");
			this.Hole(3, 3);
			this.Utilities(2);
			this.Wall(3);
			this.Fill(3, 3, SimHashes.Water);
			this.Utilities(2);
			GameObject pump = this.Building("Pump");
			this.Scenario.RunAfterNextUpdate(delegate
			{
				pump.GetComponent<BuildingEnabledButton>().IsEnabled = true;
			});
		}

		// Token: 0x0600817E RID: 33150 RVA: 0x00346C40 File Offset: 0x00344E40
		public Scenario.Builder FinalizeRoom(SimHashes element = SimHashes.Oxygen, SimHashes tileElement = SimHashes.Steel)
		{
			for (int i = this.Min.x - 1; i <= this.Max.x; i++)
			{
				if (i == this.Min.x - 1 || i == this.Max.x)
				{
					for (int j = this.Min.y - 1; j <= this.Max.y; j++)
					{
						this.Scenario.PlaceBuilding(i, j, "Tile", tileElement);
					}
				}
				else
				{
					int num = 500;
					if (element == SimHashes.Void)
					{
						num = 0;
					}
					for (int k = this.Min.y; k < this.Max.y; k++)
					{
						int num2 = Grid.OffsetCell(this.Scenario.RootCell, i, k);
						if (!this.Scenario.ReplaceElementMask[num2])
						{
							SimMessages.ReplaceElement(num2, element, CellEventLogger.Instance.Scenario, (float)num, -1f, byte.MaxValue, 0, -1);
						}
					}
				}
				this.Scenario.PlaceBuilding(i, this.Min.y - 1, "Tile", tileElement);
				this.Scenario.PlaceBuilding(i, this.Max.y, "Tile", tileElement);
			}
			return new Scenario.Builder(this.Max.x + 1, this.Min.y, SimHashes.Copper);
		}

		// Token: 0x0400626E RID: 25198
		public bool PlaceUtilities;

		// Token: 0x0400626F RID: 25199
		public int Left;

		// Token: 0x04006270 RID: 25200
		public int Bot;

		// Token: 0x04006271 RID: 25201
		public Vector2I Min;

		// Token: 0x04006272 RID: 25202
		public Vector2I Max;

		// Token: 0x04006273 RID: 25203
		public SimHashes Element;

		// Token: 0x04006274 RID: 25204
		private Scenario Scenario;
	}
}
