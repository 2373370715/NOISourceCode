using System;

// Token: 0x02001FBD RID: 8125
public interface IConfigurableConsumer
{
	// Token: 0x0600ABB9 RID: 43961
	IConfigurableConsumerOption[] GetSettingOptions();

	// Token: 0x0600ABBA RID: 43962
	IConfigurableConsumerOption GetSelectedOption();

	// Token: 0x0600ABBB RID: 43963
	void SetSelectedOption(IConfigurableConsumerOption option);
}
