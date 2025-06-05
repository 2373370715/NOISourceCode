using System;
using System.Collections.Generic;
using KSerialization;
using UnityEngine;

// Token: 0x02001E56 RID: 7766
[AddComponentMenu("KMonoBehaviour/scripts/Messenger")]
public class Messenger : KMonoBehaviour
{
	// Token: 0x17000A87 RID: 2695
	// (get) Token: 0x0600A29A RID: 41626 RVA: 0x0010E1D4 File Offset: 0x0010C3D4
	public int Count
	{
		get
		{
			return this.messages.Count;
		}
	}

	// Token: 0x0600A29B RID: 41627 RVA: 0x0010E1E1 File Offset: 0x0010C3E1
	public IEnumerator<Message> GetEnumerator()
	{
		return this.messages.GetEnumerator();
	}

	// Token: 0x0600A29C RID: 41628 RVA: 0x0010E1EE File Offset: 0x0010C3EE
	public static void DestroyInstance()
	{
		Messenger.Instance = null;
	}

	// Token: 0x17000A88 RID: 2696
	// (get) Token: 0x0600A29D RID: 41629 RVA: 0x0010E1F6 File Offset: 0x0010C3F6
	public SerializedList<Message> Messages
	{
		get
		{
			return this.messages;
		}
	}

	// Token: 0x0600A29E RID: 41630 RVA: 0x0010E1FE File Offset: 0x0010C3FE
	protected override void OnPrefabInit()
	{
		Messenger.Instance = this;
	}

	// Token: 0x0600A29F RID: 41631 RVA: 0x003EDF10 File Offset: 0x003EC110
	protected override void OnSpawn()
	{
		int i = 0;
		while (i < this.messages.Count)
		{
			if (this.messages[i].IsValid())
			{
				i++;
			}
			else
			{
				this.messages.RemoveAt(i);
			}
		}
		base.Trigger(-599791736, null);
	}

	// Token: 0x0600A2A0 RID: 41632 RVA: 0x0010E206 File Offset: 0x0010C406
	public void QueueMessage(Message message)
	{
		this.messages.Add(message);
		base.Trigger(1558809273, message);
	}

	// Token: 0x0600A2A1 RID: 41633 RVA: 0x003EDF60 File Offset: 0x003EC160
	public Message DequeueMessage()
	{
		Message result = null;
		if (this.messages.Count > 0)
		{
			result = this.messages[0];
			this.messages.RemoveAt(0);
		}
		return result;
	}

	// Token: 0x0600A2A2 RID: 41634 RVA: 0x003EDF98 File Offset: 0x003EC198
	public void ClearAllMessages()
	{
		for (int i = this.messages.Count - 1; i >= 0; i--)
		{
			this.messages.RemoveAt(i);
		}
	}

	// Token: 0x0600A2A3 RID: 41635 RVA: 0x0010E220 File Offset: 0x0010C420
	public void RemoveMessage(Message m)
	{
		this.messages.Remove(m);
		base.Trigger(-599791736, null);
	}

	// Token: 0x04007F4E RID: 32590
	[Serialize]
	private SerializedList<Message> messages = new SerializedList<Message>();

	// Token: 0x04007F4F RID: 32591
	public static Messenger Instance;
}
