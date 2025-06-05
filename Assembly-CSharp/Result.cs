using System;

// Token: 0x02000630 RID: 1584
public readonly struct Result<TSuccess, TError>
{
	// Token: 0x06001C33 RID: 7219 RVA: 0x000B6F3D File Offset: 0x000B513D
	private Result(TSuccess successValue, TError errorValue)
	{
		this.successValue = successValue;
		this.errorValue = errorValue;
	}

	// Token: 0x06001C34 RID: 7220 RVA: 0x000B6F57 File Offset: 0x000B5157
	public bool IsOk()
	{
		return this.successValue.IsSome();
	}

	// Token: 0x06001C35 RID: 7221 RVA: 0x000B6F64 File Offset: 0x000B5164
	public bool IsErr()
	{
		return this.errorValue.IsSome() || this.successValue.IsNone();
	}

	// Token: 0x06001C36 RID: 7222 RVA: 0x000B6F80 File Offset: 0x000B5180
	public TSuccess Unwrap()
	{
		if (this.successValue.IsSome())
		{
			return this.successValue.Unwrap();
		}
		if (this.errorValue.IsSome())
		{
			throw new Exception("Tried to unwrap result that is an Err()");
		}
		throw new Exception("Tried to unwrap result that isn't initialized with an Err() or Ok() value");
	}

	// Token: 0x06001C37 RID: 7223 RVA: 0x000B6FBD File Offset: 0x000B51BD
	public Option<TSuccess> Ok()
	{
		return this.successValue;
	}

	// Token: 0x06001C38 RID: 7224 RVA: 0x000B6FC5 File Offset: 0x000B51C5
	public Option<TError> Err()
	{
		return this.errorValue;
	}

	// Token: 0x06001C39 RID: 7225 RVA: 0x001B8600 File Offset: 0x001B6800
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Ok<TSuccess> value)
	{
		return new Result<TSuccess, TError>(value.value, default(TError));
	}

	// Token: 0x06001C3A RID: 7226 RVA: 0x001B8624 File Offset: 0x001B6824
	public static implicit operator Result<TSuccess, TError>(Result.Internal.Value_Err<TError> value)
	{
		return new Result<TSuccess, TError>(default(TSuccess), value.value);
	}

	// Token: 0x040011E9 RID: 4585
	private readonly Option<TSuccess> successValue;

	// Token: 0x040011EA RID: 4586
	private readonly Option<TError> errorValue;
}
