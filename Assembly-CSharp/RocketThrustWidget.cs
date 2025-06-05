using System;
using STRINGS;
using TUNING;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// Token: 0x02001F45 RID: 8005
[AddComponentMenu("KMonoBehaviour/scripts/RocketThrustWidget")]
public class RocketThrustWidget : KMonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	// Token: 0x0600A8C3 RID: 43203 RVA: 0x000AA038 File Offset: 0x000A8238
	protected override void OnPrefabInit()
	{
	}

	// Token: 0x0600A8C4 RID: 43204 RVA: 0x0040DDFC File Offset: 0x0040BFFC
	public void Draw(CommandModule commandModule)
	{
		if (this.rectTransform == null)
		{
			this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
		}
		this.commandModule = commandModule;
		this.totalWidth = this.rectTransform.rect.width;
		this.UpdateGraphDotPos(commandModule);
	}

	// Token: 0x0600A8C5 RID: 43205 RVA: 0x0040DE54 File Offset: 0x0040C054
	private void UpdateGraphDotPos(CommandModule rocket)
	{
		this.totalWidth = this.rectTransform.rect.width;
		float num = Mathf.Lerp(0f, this.totalWidth, rocket.rocketStats.GetTotalMass() / this.maxMass);
		num = Mathf.Clamp(num, 0f, this.totalWidth);
		this.graphDot.rectTransform.SetLocalPosition(new Vector3(num, 0f, 0f));
		this.graphDotText.text = "-" + Util.FormatWholeNumber(rocket.rocketStats.GetTotalThrust() - rocket.rocketStats.GetRocketMaxDistance()) + "km";
	}

	// Token: 0x0600A8C6 RID: 43206 RVA: 0x0040DF08 File Offset: 0x0040C108
	private void Update()
	{
		if (this.mouseOver)
		{
			if (this.rectTransform == null)
			{
				this.rectTransform = this.graphBar.gameObject.GetComponent<RectTransform>();
			}
			Vector3 position = this.rectTransform.GetPosition();
			Vector2 size = this.rectTransform.rect.size;
			float num = KInputManager.GetMousePos().x - position.x + size.x / 2f;
			num = Mathf.Clamp(num, 0f, this.totalWidth);
			this.hoverMarker.rectTransform.SetLocalPosition(new Vector3(num, 0f, 0f));
			float num2 = Mathf.Lerp(0f, this.maxMass, num / this.totalWidth);
			float totalThrust = this.commandModule.rocketStats.GetTotalThrust();
			float rocketMaxDistance = this.commandModule.rocketStats.GetRocketMaxDistance();
			this.hoverTooltip.SetSimpleTooltip(string.Concat(new string[]
			{
				UI.STARMAP.ROCKETWEIGHT.MASS,
				GameUtil.GetFormattedMass(num2, GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"),
				"\n",
				UI.STARMAP.ROCKETWEIGHT.MASSPENALTY,
				Util.FormatWholeNumber(ROCKETRY.CalculateMassWithPenalty(num2)),
				UI.UNITSUFFIXES.DISTANCE.KILOMETER,
				"\n\n",
				UI.STARMAP.ROCKETWEIGHT.CURRENTMASS,
				GameUtil.GetFormattedMass(this.commandModule.rocketStats.GetTotalMass(), GameUtil.TimeSlice.None, GameUtil.MetricMassFormat.Kilogram, true, "{0:0.#}"),
				"\n",
				UI.STARMAP.ROCKETWEIGHT.CURRENTMASSPENALTY,
				Util.FormatWholeNumber(totalThrust - rocketMaxDistance),
				UI.UNITSUFFIXES.DISTANCE.KILOMETER
			}));
		}
	}

	// Token: 0x0600A8C7 RID: 43207 RVA: 0x00111DF0 File Offset: 0x0010FFF0
	public void OnPointerEnter(PointerEventData eventData)
	{
		this.mouseOver = true;
		this.hoverMarker.SetAlpha(1f);
	}

	// Token: 0x0600A8C8 RID: 43208 RVA: 0x00111E09 File Offset: 0x00110009
	public void OnPointerExit(PointerEventData eventData)
	{
		this.mouseOver = false;
		this.hoverMarker.SetAlpha(0f);
	}

	// Token: 0x040084D2 RID: 34002
	public Image graphBar;

	// Token: 0x040084D3 RID: 34003
	public Image graphDot;

	// Token: 0x040084D4 RID: 34004
	public LocText graphDotText;

	// Token: 0x040084D5 RID: 34005
	public Image hoverMarker;

	// Token: 0x040084D6 RID: 34006
	public ToolTip hoverTooltip;

	// Token: 0x040084D7 RID: 34007
	public RectTransform markersContainer;

	// Token: 0x040084D8 RID: 34008
	public Image markerTemplate;

	// Token: 0x040084D9 RID: 34009
	private RectTransform rectTransform;

	// Token: 0x040084DA RID: 34010
	private float maxMass = 20000f;

	// Token: 0x040084DB RID: 34011
	private float totalWidth = 5f;

	// Token: 0x040084DC RID: 34012
	private bool mouseOver;

	// Token: 0x040084DD RID: 34013
	public CommandModule commandModule;
}
