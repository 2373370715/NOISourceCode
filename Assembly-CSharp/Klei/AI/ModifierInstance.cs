using System;
using UnityEngine;

namespace Klei.AI
{
	// Token: 0x02003CDF RID: 15583
	public class ModifierInstance<ModifierType> : IStateMachineTarget
	{
		// Token: 0x17000C66 RID: 3174
		// (get) Token: 0x0600EF28 RID: 61224 RVA: 0x00144F3D File Offset: 0x0014313D
		// (set) Token: 0x0600EF29 RID: 61225 RVA: 0x00144F45 File Offset: 0x00143145
		public GameObject gameObject { get; private set; }

		// Token: 0x0600EF2A RID: 61226 RVA: 0x00144F4E File Offset: 0x0014314E
		public ModifierInstance(GameObject game_object, ModifierType modifier)
		{
			this.gameObject = game_object;
			this.modifier = modifier;
		}

		// Token: 0x0600EF2B RID: 61227 RVA: 0x00144F64 File Offset: 0x00143164
		public ComponentType GetComponent<ComponentType>()
		{
			return this.gameObject.GetComponent<ComponentType>();
		}

		// Token: 0x0600EF2C RID: 61228 RVA: 0x00144F71 File Offset: 0x00143171
		public int Subscribe(int hash, Action<object> handler)
		{
			return this.gameObject.GetComponent<KMonoBehaviour>().Subscribe(hash, handler);
		}

		// Token: 0x0600EF2D RID: 61229 RVA: 0x00144F85 File Offset: 0x00143185
		public void Unsubscribe(int hash, Action<object> handler)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(hash, handler);
		}

		// Token: 0x0600EF2E RID: 61230 RVA: 0x00144F99 File Offset: 0x00143199
		public void Unsubscribe(int id)
		{
			this.gameObject.GetComponent<KMonoBehaviour>().Unsubscribe(id);
		}

		// Token: 0x0600EF2F RID: 61231 RVA: 0x00144FAC File Offset: 0x001431AC
		public void Trigger(int hash, object data = null)
		{
			this.gameObject.GetComponent<KPrefabID>().Trigger(hash, data);
		}

		// Token: 0x17000C67 RID: 3175
		// (get) Token: 0x0600EF30 RID: 61232 RVA: 0x00144FC0 File Offset: 0x001431C0
		public Transform transform
		{
			get
			{
				return this.gameObject.transform;
			}
		}

		// Token: 0x17000C68 RID: 3176
		// (get) Token: 0x0600EF31 RID: 61233 RVA: 0x00144FCD File Offset: 0x001431CD
		public bool isNull
		{
			get
			{
				return this.gameObject == null;
			}
		}

		// Token: 0x17000C69 RID: 3177
		// (get) Token: 0x0600EF32 RID: 61234 RVA: 0x00144FDB File Offset: 0x001431DB
		public string name
		{
			get
			{
				return this.gameObject.name;
			}
		}

		// Token: 0x0600EF33 RID: 61235 RVA: 0x000AA038 File Offset: 0x000A8238
		public virtual void OnCleanUp()
		{
		}

		// Token: 0x0400EAD2 RID: 60114
		public ModifierType modifier;
	}
}
