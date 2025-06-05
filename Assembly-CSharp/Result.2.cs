using System;

// Token: 0x02000631 RID: 1585
public static class Result
{
	// Token: 0x06001C3B RID: 7227 RVA: 0x000B6FCD File Offset: 0x000B51CD
	public static Result.Internal.Value_Ok<T> Ok<T>(T value)
	{
		return new Result.Internal.Value_Ok<T>(value);
	}

	// Token: 0x06001C3C RID: 7228 RVA: 0x000B6FD5 File Offset: 0x000B51D5
	public static Result.Internal.Value_Err<T> Err<T>(T value)
	{
		return new Result.Internal.Value_Err<T>(value);
	}

	// Token: 0x02000632 RID: 1586
	public static class Internal
	{
		// Token: 0x02000633 RID: 1587
		public readonly struct Value_Ok<T>
		{
			// Token: 0x06001C3D RID: 7229 RVA: 0x000B6FDD File Offset: 0x000B51DD
			public Value_Ok(T value)
			{
				this.value = value;
			}

			// Token: 0x040011EB RID: 4587
			public readonly T value;
		}

		// Token: 0x02000634 RID: 1588
		public readonly struct Value_Err<T>
		{
			// Token: 0x06001C3E RID: 7230 RVA: 0x000B6FE6 File Offset: 0x000B51E6
			public Value_Err(T value)
			{
				this.value = value;
			}

			// Token: 0x040011EC RID: 4588
			public readonly T value;
		}
	}
}
