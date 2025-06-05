using System;

namespace EventSystem2Syntax
{
	// Token: 0x0200214A RID: 8522
	internal class OldExample : KMonoBehaviour2
	{
		// Token: 0x0600B59A RID: 46490 RVA: 0x0045545C File Offset: 0x0045365C
		protected override void OnPrefabInit()
		{
			base.OnPrefabInit();
			base.Subscribe(0, new Action<object>(this.OnObjectDestroyed));
			bool flag = false;
			base.Trigger(0, flag);
		}

		// Token: 0x0600B59B RID: 46491 RVA: 0x0011A659 File Offset: 0x00118859
		private void OnObjectDestroyed(object data)
		{
			Debug.Log((bool)data);
		}
	}
}
