using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

// Token: 0x020014DC RID: 5340
[Serializable]
public class LocString
{
	// Token: 0x17000712 RID: 1810
	// (get) Token: 0x06006EDF RID: 28383 RVA: 0x000ED1F2 File Offset: 0x000EB3F2
	public string text
	{
		get
		{
			return this._text;
		}
	}

	// Token: 0x17000713 RID: 1811
	// (get) Token: 0x06006EE0 RID: 28384 RVA: 0x000ED1FA File Offset: 0x000EB3FA
	public StringKey key
	{
		get
		{
			return this._key;
		}
	}

	// Token: 0x06006EE1 RID: 28385 RVA: 0x000ED202 File Offset: 0x000EB402
	public LocString(string text)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x06006EE2 RID: 28386 RVA: 0x000ED21D File Offset: 0x000EB41D
	public LocString(string text, string keystring)
	{
		this._text = text;
		this._key = new StringKey(keystring);
	}

	// Token: 0x06006EE3 RID: 28387 RVA: 0x000ED202 File Offset: 0x000EB402
	public LocString(string text, bool isLocalized)
	{
		this._text = text;
		this._key = default(StringKey);
	}

	// Token: 0x06006EE4 RID: 28388 RVA: 0x000ED238 File Offset: 0x000EB438
	public static implicit operator LocString(string text)
	{
		return new LocString(text);
	}

	// Token: 0x06006EE5 RID: 28389 RVA: 0x000ED240 File Offset: 0x000EB440
	public static implicit operator string(LocString loc_string)
	{
		return loc_string.text;
	}

	// Token: 0x06006EE6 RID: 28390 RVA: 0x000ED248 File Offset: 0x000EB448
	public override string ToString()
	{
		return Strings.Get(this.key).String;
	}

	// Token: 0x06006EE7 RID: 28391 RVA: 0x000ED25A File Offset: 0x000EB45A
	public void SetKey(string key_name)
	{
		this._key = new StringKey(key_name);
	}

	// Token: 0x06006EE8 RID: 28392 RVA: 0x000ED268 File Offset: 0x000EB468
	public void SetKey(StringKey key)
	{
		this._key = key;
	}

	// Token: 0x06006EE9 RID: 28393 RVA: 0x000ED271 File Offset: 0x000EB471
	public string Replace(string search, string replacement)
	{
		return this.ToString().Replace(search, replacement);
	}

	// Token: 0x06006EEA RID: 28394 RVA: 0x002FEC28 File Offset: 0x002FCE28
	public static void CreateLocStringKeys(Type type, string parent_path = "STRINGS.")
	{
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		string text = parent_path;
		if (text == null)
		{
			text = "";
		}
		text = text + type.Name + ".";
		foreach (FieldInfo fieldInfo in fields)
		{
			if (!(fieldInfo.FieldType != typeof(LocString)))
			{
				if (!fieldInfo.IsStatic)
				{
					DebugUtil.DevLogError("LocString fields must be static, skipping. " + parent_path);
				}
				else
				{
					string text2 = text + fieldInfo.Name;
					LocString locString = (LocString)fieldInfo.GetValue(null);
					locString.SetKey(text2);
					string text3 = locString.text;
					Strings.Add(new string[]
					{
						text2,
						text3
					});
					fieldInfo.SetValue(null, locString);
				}
			}
		}
		Type[] nestedTypes = type.GetNestedTypes(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < nestedTypes.Length; i++)
		{
			LocString.CreateLocStringKeys(nestedTypes[i], text);
		}
	}

	// Token: 0x06006EEB RID: 28395 RVA: 0x002FED14 File Offset: 0x002FCF14
	public static string[] GetStrings(Type type)
	{
		List<string> list = new List<string>();
		FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy);
		for (int i = 0; i < fields.Length; i++)
		{
			LocString locString = (LocString)fields[i].GetValue(null);
			list.Add(locString.text);
		}
		return list.ToArray();
	}

	// Token: 0x04005375 RID: 21365
	[SerializeField]
	private string _text;

	// Token: 0x04005376 RID: 21366
	[SerializeField]
	private StringKey _key;

	// Token: 0x04005377 RID: 21367
	public const BindingFlags data_member_fields = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy;
}
