using System;
using System.Collections.Generic;
using System.Diagnostics;
using KSerialization;

// Token: 0x0200061D RID: 1565
[DebuggerDisplay("has_value={hasValue} {value}")]
[Serializable]
public readonly struct Option<T> : IEquatable<Option<T>>, IEquatable<T>
{
	// Token: 0x06001BCA RID: 7114 RVA: 0x000B6914 File Offset: 0x000B4B14
	public Option(T value)
	{
		this.value = value;
		this.hasValue = true;
	}

	// Token: 0x170000A8 RID: 168
	// (get) Token: 0x06001BCB RID: 7115 RVA: 0x000B6924 File Offset: 0x000B4B24
	public bool HasValue
	{
		get
		{
			return this.hasValue;
		}
	}

	// Token: 0x170000A9 RID: 169
	// (get) Token: 0x06001BCC RID: 7116 RVA: 0x000B692C File Offset: 0x000B4B2C
	public T Value
	{
		get
		{
			return this.Unwrap();
		}
	}

	// Token: 0x06001BCD RID: 7117 RVA: 0x000B6934 File Offset: 0x000B4B34
	public T Unwrap()
	{
		if (!this.hasValue)
		{
			throw new Exception("Tried to get a value for a Option<" + typeof(T).FullName + ">, but hasValue is false");
		}
		return this.value;
	}

	// Token: 0x06001BCE RID: 7118 RVA: 0x000B6968 File Offset: 0x000B4B68
	public T UnwrapOr(T fallback_value, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return fallback_value;
		}
		return this.value;
	}

	// Token: 0x06001BCF RID: 7119 RVA: 0x000B69A3 File Offset: 0x000B4BA3
	public T UnwrapOrElse(Func<T> get_fallback_value_fn, string warn_on_fallback = null)
	{
		if (!this.hasValue)
		{
			if (warn_on_fallback != null)
			{
				DebugUtil.DevAssert(false, "Failed to unwrap a Option<" + typeof(T).FullName + ">: " + warn_on_fallback, null);
			}
			return get_fallback_value_fn();
		}
		return this.value;
	}

	// Token: 0x06001BD0 RID: 7120 RVA: 0x001B80A0 File Offset: 0x001B62A0
	public T UnwrapOrDefault()
	{
		if (!this.hasValue)
		{
			return default(T);
		}
		return this.value;
	}

	// Token: 0x06001BD1 RID: 7121 RVA: 0x000B69E3 File Offset: 0x000B4BE3
	public T Expect(string msg_on_fail)
	{
		if (!this.hasValue)
		{
			throw new Exception(msg_on_fail);
		}
		return this.value;
	}

	// Token: 0x06001BD2 RID: 7122 RVA: 0x000B6924 File Offset: 0x000B4B24
	public bool IsSome()
	{
		return this.hasValue;
	}

	// Token: 0x06001BD3 RID: 7123 RVA: 0x000B69FA File Offset: 0x000B4BFA
	public bool IsNone()
	{
		return !this.hasValue;
	}

	// Token: 0x06001BD4 RID: 7124 RVA: 0x000B6A05 File Offset: 0x000B4C05
	public Option<U> AndThen<U>(Func<T, U> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return Option.Maybe<U>(fn(this.value));
	}

	// Token: 0x06001BD5 RID: 7125 RVA: 0x000B6A2B File Offset: 0x000B4C2B
	public Option<U> AndThen<U>(Func<T, Option<U>> fn)
	{
		if (this.IsNone())
		{
			return Option.None;
		}
		return fn(this.value);
	}

	// Token: 0x06001BD6 RID: 7126 RVA: 0x000B6A4C File Offset: 0x000B4C4C
	public static implicit operator Option<T>(T value)
	{
		return Option.Maybe<T>(value);
	}

	// Token: 0x06001BD7 RID: 7127 RVA: 0x000B6A54 File Offset: 0x000B4C54
	public static explicit operator T(Option<T> option)
	{
		return option.Unwrap();
	}

	// Token: 0x06001BD8 RID: 7128 RVA: 0x001B80C8 File Offset: 0x001B62C8
	public static implicit operator Option<T>(Option.Internal.Value_None value)
	{
		return default(Option<T>);
	}

	// Token: 0x06001BD9 RID: 7129 RVA: 0x000B6A5D File Offset: 0x000B4C5D
	public static implicit operator Option.Internal.Value_HasValue(Option<T> value)
	{
		return new Option.Internal.Value_HasValue(value.hasValue);
	}

	// Token: 0x06001BDA RID: 7130 RVA: 0x000B6A6A File Offset: 0x000B4C6A
	public void Deconstruct(out bool hasValue, out T value)
	{
		hasValue = this.hasValue;
		value = this.value;
	}

	// Token: 0x06001BDB RID: 7131 RVA: 0x000B6A80 File Offset: 0x000B4C80
	public bool Equals(Option<T> other)
	{
		return EqualityComparer<bool>.Default.Equals(this.hasValue, other.hasValue) && EqualityComparer<T>.Default.Equals(this.value, other.value);
	}

	// Token: 0x06001BDC RID: 7132 RVA: 0x001B80E0 File Offset: 0x001B62E0
	public override bool Equals(object obj)
	{
		if (obj is Option<T>)
		{
			Option<T> other = (Option<T>)obj;
			return this.Equals(other);
		}
		return false;
	}

	// Token: 0x06001BDD RID: 7133 RVA: 0x000B6AB2 File Offset: 0x000B4CB2
	public static bool operator ==(Option<T> lhs, Option<T> rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001BDE RID: 7134 RVA: 0x000B6ABC File Offset: 0x000B4CBC
	public static bool operator !=(Option<T> lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001BDF RID: 7135 RVA: 0x001B8108 File Offset: 0x001B6308
	public override int GetHashCode()
	{
		return (-363764631 * -1521134295 + this.hasValue.GetHashCode()) * -1521134295 + EqualityComparer<T>.Default.GetHashCode(this.value);
	}

	// Token: 0x06001BE0 RID: 7136 RVA: 0x000B6AC8 File Offset: 0x000B4CC8
	public override string ToString()
	{
		if (!this.hasValue)
		{
			return "None";
		}
		return string.Format("{0}", this.value);
	}

	// Token: 0x06001BE1 RID: 7137 RVA: 0x000B6AED File Offset: 0x000B4CED
	public static bool operator ==(Option<T> lhs, T rhs)
	{
		return lhs.Equals(rhs);
	}

	// Token: 0x06001BE2 RID: 7138 RVA: 0x000B6AF7 File Offset: 0x000B4CF7
	public static bool operator !=(Option<T> lhs, T rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001BE3 RID: 7139 RVA: 0x000B6B03 File Offset: 0x000B4D03
	public static bool operator ==(T lhs, Option<T> rhs)
	{
		return rhs.Equals(lhs);
	}

	// Token: 0x06001BE4 RID: 7140 RVA: 0x000B6B0D File Offset: 0x000B4D0D
	public static bool operator !=(T lhs, Option<T> rhs)
	{
		return !(lhs == rhs);
	}

	// Token: 0x06001BE5 RID: 7141 RVA: 0x000B6B19 File Offset: 0x000B4D19
	public bool Equals(T other)
	{
		return this.HasValue && EqualityComparer<T>.Default.Equals(this.value, other);
	}

	// Token: 0x170000AA RID: 170
	// (get) Token: 0x06001BE6 RID: 7142 RVA: 0x001B80C8 File Offset: 0x001B62C8
	public static Option<T> None
	{
		get
		{
			return default(Option<T>);
		}
	}

	// Token: 0x040011CC RID: 4556
	[Serialize]
	private readonly bool hasValue;

	// Token: 0x040011CD RID: 4557
	[Serialize]
	private readonly T value;
}
