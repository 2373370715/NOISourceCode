using System;
using FMOD.Studio;
using UnityEngine;

// Token: 0x02000AB4 RID: 2740
public abstract class LoopingSoundParameterUpdater
{
	// Token: 0x17000206 RID: 518
	// (get) Token: 0x06003211 RID: 12817 RVA: 0x000C4FDC File Offset: 0x000C31DC
	// (set) Token: 0x06003212 RID: 12818 RVA: 0x000C4FE4 File Offset: 0x000C31E4
	public HashedString parameter { get; private set; }

	// Token: 0x06003213 RID: 12819 RVA: 0x000C4FED File Offset: 0x000C31ED
	public LoopingSoundParameterUpdater(HashedString parameter)
	{
		this.parameter = parameter;
	}

	// Token: 0x06003214 RID: 12820
	public abstract void Add(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x06003215 RID: 12821
	public abstract void Update(float dt);

	// Token: 0x06003216 RID: 12822
	public abstract void Remove(LoopingSoundParameterUpdater.Sound sound);

	// Token: 0x02000AB5 RID: 2741
	public struct Sound
	{
		// Token: 0x0400223E RID: 8766
		public EventInstance ev;

		// Token: 0x0400223F RID: 8767
		public HashedString path;

		// Token: 0x04002240 RID: 8768
		public Transform transform;

		// Token: 0x04002241 RID: 8769
		public SoundDescription description;

		// Token: 0x04002242 RID: 8770
		public bool objectIsSelectedAndVisible;
	}
}
