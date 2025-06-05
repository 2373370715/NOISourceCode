using System;
using UnityEngine;

// Token: 0x02001955 RID: 6485
public interface ILaunchableRocket
{
	// Token: 0x170008D0 RID: 2256
	// (get) Token: 0x060086F3 RID: 34547
	LaunchableRocketRegisterType registerType { get; }

	// Token: 0x170008D1 RID: 2257
	// (get) Token: 0x060086F4 RID: 34548
	GameObject LaunchableGameObject { get; }

	// Token: 0x170008D2 RID: 2258
	// (get) Token: 0x060086F5 RID: 34549
	float rocketSpeed { get; }

	// Token: 0x170008D3 RID: 2259
	// (get) Token: 0x060086F6 RID: 34550
	bool isLanding { get; }
}
