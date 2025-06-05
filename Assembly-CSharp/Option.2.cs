using System;

// Token: 0x0200061E RID: 1566
public static class Option
{
	// Token: 0x06001BE7 RID: 7143 RVA: 0x000B6B36 File Offset: 0x000B4D36
	public static Option<T> Some<T>(T value)
	{
		return new Option<T>(value);
	}

	// Token: 0x06001BE8 RID: 7144 RVA: 0x001B8148 File Offset: 0x001B6348
	public static Option<T> Maybe<T>(T value)
	{
		if (value.IsNullOrDestroyed())
		{
			return default(Option<T>);
		}
		return new Option<T>(value);
	}

	// Token: 0x170000AB RID: 171
	// (get) Token: 0x06001BE9 RID: 7145 RVA: 0x001B8174 File Offset: 0x001B6374
	public static Option.Internal.Value_None None
	{
		get
		{
			return default(Option.Internal.Value_None);
		}
	}

	// Token: 0x06001BEA RID: 7146 RVA: 0x001B818C File Offset: 0x001B638C
	public static bool AllHaveValues(params Option.Internal.Value_HasValue[] options)
	{
		if (options == null || options.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < options.Length; i++)
		{
			if (!options[i].HasValue)
			{
				return false;
			}
		}
		return true;
	}

	// Token: 0x0200061F RID: 1567
	public static class Internal
	{
		// Token: 0x02000620 RID: 1568
		public readonly struct Value_None
		{
		}

		// Token: 0x02000621 RID: 1569
		public readonly struct Value_HasValue
		{
			// Token: 0x06001BEB RID: 7147 RVA: 0x000B6B3E File Offset: 0x000B4D3E
			public Value_HasValue(bool hasValue)
			{
				this.HasValue = hasValue;
			}

			// Token: 0x040011CE RID: 4558
			public readonly bool HasValue;
		}
	}
}
