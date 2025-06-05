using System;

// Token: 0x02000BC1 RID: 3009
public class OrbitalData : Resource
{
	// Token: 0x060038E3 RID: 14563 RVA: 0x00229ED0 File Offset: 0x002280D0
	public OrbitalData(string id, ResourceSet parent, string animFile = "earth_kanim", string initialAnim = "", OrbitalData.OrbitalType orbitalType = OrbitalData.OrbitalType.poi, float periodInCycles = 1f, float xGridPercent = 0.5f, float yGridPercent = 0.5f, float minAngle = -350f, float maxAngle = 350f, float radiusScale = 1.05f, bool rotatesBehind = true, float behindZ = 0.05f, float distance = 25f, float renderZ = 1f) : base(id, parent, null)
	{
		this.animFile = animFile;
		this.initialAnim = initialAnim;
		this.orbitalType = orbitalType;
		this.periodInCycles = periodInCycles;
		this.xGridPercent = xGridPercent;
		this.yGridPercent = yGridPercent;
		this.minAngle = minAngle;
		this.maxAngle = maxAngle;
		this.radiusScale = radiusScale;
		this.rotatesBehind = rotatesBehind;
		this.behindZ = behindZ;
		this.distance = distance;
		this.renderZ = renderZ;
	}

	// Token: 0x0400275C RID: 10076
	public string animFile;

	// Token: 0x0400275D RID: 10077
	public string initialAnim;

	// Token: 0x0400275E RID: 10078
	public float periodInCycles;

	// Token: 0x0400275F RID: 10079
	public float xGridPercent;

	// Token: 0x04002760 RID: 10080
	public float yGridPercent;

	// Token: 0x04002761 RID: 10081
	public float minAngle;

	// Token: 0x04002762 RID: 10082
	public float maxAngle;

	// Token: 0x04002763 RID: 10083
	public float radiusScale;

	// Token: 0x04002764 RID: 10084
	public bool rotatesBehind;

	// Token: 0x04002765 RID: 10085
	public float behindZ;

	// Token: 0x04002766 RID: 10086
	public float distance;

	// Token: 0x04002767 RID: 10087
	public float renderZ;

	// Token: 0x04002768 RID: 10088
	public OrbitalData.OrbitalType orbitalType;

	// Token: 0x04002769 RID: 10089
	public Func<float> GetRenderZ;

	// Token: 0x02000BC2 RID: 3010
	public enum OrbitalType
	{
		// Token: 0x0400276B RID: 10091
		world,
		// Token: 0x0400276C RID: 10092
		poi,
		// Token: 0x0400276D RID: 10093
		inOrbit,
		// Token: 0x0400276E RID: 10094
		landed
	}
}
