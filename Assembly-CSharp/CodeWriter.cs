using System;
using System.Collections.Generic;
using System.IO;

// Token: 0x020005F0 RID: 1520
public class CodeWriter
{
	// Token: 0x06001AA5 RID: 6821 RVA: 0x000B5C61 File Offset: 0x000B3E61
	public CodeWriter(string path)
	{
		this.Path = path;
	}

	// Token: 0x06001AA6 RID: 6822 RVA: 0x000B5C7B File Offset: 0x000B3E7B
	public void Comment(string text)
	{
		this.Lines.Add("// " + text);
	}

	// Token: 0x06001AA7 RID: 6823 RVA: 0x001B48EC File Offset: 0x001B2AEC
	public void BeginPartialClass(string class_name, string parent_name = null)
	{
		string text = "public partial class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AA8 RID: 6824 RVA: 0x001B4938 File Offset: 0x001B2B38
	public void BeginClass(string class_name, string parent_name = null)
	{
		string text = "public class " + class_name;
		if (parent_name != null)
		{
			text = text + " : " + parent_name;
		}
		this.Line(text);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AA9 RID: 6825 RVA: 0x000B5C93 File Offset: 0x000B3E93
	public void EndClass()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001AAA RID: 6826 RVA: 0x000B5CAE File Offset: 0x000B3EAE
	public void BeginNameSpace(string name)
	{
		this.Line("namespace " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AAB RID: 6827 RVA: 0x000B5C93 File Offset: 0x000B3E93
	public void EndNameSpace()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001AAC RID: 6828 RVA: 0x000B5CDA File Offset: 0x000B3EDA
	public void BeginArrayStructureInitialization(string name)
	{
		this.Line("new " + name);
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AAD RID: 6829 RVA: 0x000B5D06 File Offset: 0x000B3F06
	public void EndArrayStructureInitialization(bool last_item)
	{
		this.Indent--;
		if (!last_item)
		{
			this.Line("},");
			return;
		}
		this.Line("}");
	}

	// Token: 0x06001AAE RID: 6830 RVA: 0x000B5D30 File Offset: 0x000B3F30
	public void BeginArraArrayInitialization(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AAF RID: 6831 RVA: 0x000B5D62 File Offset: 0x000B3F62
	public void EndArrayArrayInitialization(bool last_item)
	{
		this.Indent--;
		if (last_item)
		{
			this.Line("}");
			return;
		}
		this.Line("},");
	}

	// Token: 0x06001AB0 RID: 6832 RVA: 0x000B5D8C File Offset: 0x000B3F8C
	public void BeginConstructor(string name)
	{
		this.Line("public " + name + "()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AB1 RID: 6833 RVA: 0x000B5C93 File Offset: 0x000B3E93
	public void EndConstructor()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001AB2 RID: 6834 RVA: 0x000B5D30 File Offset: 0x000B3F30
	public void BeginArrayAssignment(string array_type, string array_name)
	{
		this.Line(array_name + " = new " + array_type + "[]");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AB3 RID: 6835 RVA: 0x000B5DBD File Offset: 0x000B3FBD
	public void EndArrayAssignment()
	{
		this.Indent--;
		this.Line("};");
	}

	// Token: 0x06001AB4 RID: 6836 RVA: 0x000B5DD8 File Offset: 0x000B3FD8
	public void FieldAssignment(string field_name, string value)
	{
		this.Line(field_name + " = " + value + ";");
	}

	// Token: 0x06001AB5 RID: 6837 RVA: 0x000B5DF1 File Offset: 0x000B3FF1
	public void BeginStructureDelegateFieldInitializer(string name)
	{
		this.Line(name + "=delegate()");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AB6 RID: 6838 RVA: 0x000B5E1D File Offset: 0x000B401D
	public void EndStructureDelegateFieldInitializer()
	{
		this.Indent--;
		this.Line("},");
	}

	// Token: 0x06001AB7 RID: 6839 RVA: 0x000B5E38 File Offset: 0x000B4038
	public void BeginIf(string condition)
	{
		this.Line("if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AB8 RID: 6840 RVA: 0x001B4984 File Offset: 0x001B2B84
	public void BeginElseIf(string condition)
	{
		this.Indent--;
		this.Line("}");
		this.Line("else if(" + condition + ")");
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001AB9 RID: 6841 RVA: 0x000B5C93 File Offset: 0x000B3E93
	public void EndIf()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001ABA RID: 6842 RVA: 0x001B49DC File Offset: 0x001B2BDC
	public void BeginFunctionDeclaration(string name, string parameter, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"(",
			parameter,
			")"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001ABB RID: 6843 RVA: 0x001B4A40 File Offset: 0x001B2C40
	public void BeginFunctionDeclaration(string name, string return_type)
	{
		this.Line(string.Concat(new string[]
		{
			"public ",
			return_type,
			" ",
			name,
			"()"
		}));
		this.Line("{");
		this.Indent++;
	}

	// Token: 0x06001ABC RID: 6844 RVA: 0x000B5C93 File Offset: 0x000B3E93
	public void EndFunctionDeclaration()
	{
		this.Indent--;
		this.Line("}");
	}

	// Token: 0x06001ABD RID: 6845 RVA: 0x001B4A98 File Offset: 0x001B2C98
	private void InternalNamedParameter(string name, string value, bool last_parameter)
	{
		string str = "";
		if (!last_parameter)
		{
			str = ",";
		}
		this.Line(name + ":" + value + str);
	}

	// Token: 0x06001ABE RID: 6846 RVA: 0x000B5E69 File Offset: 0x000B4069
	public void NamedParameterBool(string name, bool value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString().ToLower(), last_parameter);
	}

	// Token: 0x06001ABF RID: 6847 RVA: 0x000B5E7F File Offset: 0x000B407F
	public void NamedParameterInt(string name, int value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString(), last_parameter);
	}

	// Token: 0x06001AC0 RID: 6848 RVA: 0x000B5E90 File Offset: 0x000B4090
	public void NamedParameterFloat(string name, float value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value.ToString() + "f", last_parameter);
	}

	// Token: 0x06001AC1 RID: 6849 RVA: 0x000B5EAB File Offset: 0x000B40AB
	public void NamedParameterString(string name, string value, bool last_parameter = false)
	{
		this.InternalNamedParameter(name, value, last_parameter);
	}

	// Token: 0x06001AC2 RID: 6850 RVA: 0x000B5EB6 File Offset: 0x000B40B6
	public void BeginFunctionCall(string name)
	{
		this.Line(name);
		this.Line("(");
		this.Indent++;
	}

	// Token: 0x06001AC3 RID: 6851 RVA: 0x000B5ED8 File Offset: 0x000B40D8
	public void EndFunctionCall()
	{
		this.Indent--;
		this.Line(");");
	}

	// Token: 0x06001AC4 RID: 6852 RVA: 0x001B4AC8 File Offset: 0x001B2CC8
	public void FunctionCall(string function_name, params string[] parameters)
	{
		string str = function_name + "(";
		for (int i = 0; i < parameters.Length; i++)
		{
			str += parameters[i];
			if (i != parameters.Length - 1)
			{
				str += ", ";
			}
		}
		this.Line(str + ");");
	}

	// Token: 0x06001AC5 RID: 6853 RVA: 0x000B5EF3 File Offset: 0x000B40F3
	public void StructureFieldInitializer(string field, string value)
	{
		this.Line(field + " = " + value + ",");
	}

	// Token: 0x06001AC6 RID: 6854 RVA: 0x001B4B20 File Offset: 0x001B2D20
	public void StructureArrayFieldInitializer(string field, string field_type, params string[] values)
	{
		string text = field + " = new " + field_type + "[]{ ";
		for (int i = 0; i < values.Length; i++)
		{
			text += values[i];
			if (i < values.Length - 1)
			{
				text += ", ";
			}
		}
		text += " },";
		this.Line(text);
	}

	// Token: 0x06001AC7 RID: 6855 RVA: 0x001B4B80 File Offset: 0x001B2D80
	public void Line(string text = "")
	{
		for (int i = 0; i < this.Indent; i++)
		{
			text = "\t" + text;
		}
		this.Lines.Add(text);
	}

	// Token: 0x06001AC8 RID: 6856 RVA: 0x000B5F0C File Offset: 0x000B410C
	public void Flush()
	{
		File.WriteAllLines(this.Path, this.Lines.ToArray());
	}

	// Token: 0x04001131 RID: 4401
	private List<string> Lines = new List<string>();

	// Token: 0x04001132 RID: 4402
	private string Path;

	// Token: 0x04001133 RID: 4403
	private int Indent;
}
