using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

// Token: 0x020018AF RID: 6319
public static class SerializableOutfitData
{
	// Token: 0x0600828A RID: 33418 RVA: 0x0034AB88 File Offset: 0x00348D88
	public static int GetVersionFrom(JObject jsonData)
	{
		int result;
		if (jsonData["Version"] == null)
		{
			result = 1;
		}
		else
		{
			result = jsonData.Value<int>("Version");
			jsonData.Remove("Version");
		}
		return result;
	}

	// Token: 0x0600828B RID: 33419 RVA: 0x0034ABC0 File Offset: 0x00348DC0
	public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
	{
		int versionFrom = SerializableOutfitData.GetVersionFrom(jsonData);
		if (versionFrom == 1)
		{
			return SerializableOutfitData.Version2.FromVersion1(SerializableOutfitData.Version1.FromJson(jsonData));
		}
		if (versionFrom != 2)
		{
			DebugUtil.DevAssert(false, string.Format("Version {0} of OutfitData is not supported", versionFrom), null);
			return new SerializableOutfitData.Version2();
		}
		return SerializableOutfitData.Version2.FromJson(jsonData);
	}

	// Token: 0x0600828C RID: 33420 RVA: 0x000FA5B2 File Offset: 0x000F87B2
	public static JObject ToJson(SerializableOutfitData.Version2 data)
	{
		return SerializableOutfitData.Version2.ToJson(data);
	}

	// Token: 0x0600828D RID: 33421 RVA: 0x0034AC10 File Offset: 0x00348E10
	public static string ToJsonString(JObject data)
	{
		string result;
		using (StringWriter stringWriter = new StringWriter())
		{
			using (JsonTextWriter jsonTextWriter = new JsonTextWriter(stringWriter))
			{
				data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
				result = stringWriter.ToString();
			}
		}
		return result;
	}

	// Token: 0x0600828E RID: 33422 RVA: 0x0034AC70 File Offset: 0x00348E70
	public static void ToJsonString(JObject data, TextWriter textWriter)
	{
		using (JsonTextWriter jsonTextWriter = new JsonTextWriter(textWriter))
		{
			data.WriteTo(jsonTextWriter, Array.Empty<JsonConverter>());
		}
	}

	// Token: 0x04006358 RID: 25432
	public const string VERSION_KEY = "Version";

	// Token: 0x020018B0 RID: 6320
	public class Version2
	{
		// Token: 0x0600828F RID: 33423 RVA: 0x0034ACAC File Offset: 0x00348EAC
		public static SerializableOutfitData.Version2 FromVersion1(SerializableOutfitData.Version1 data)
		{
			Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> dictionary = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();
			foreach (KeyValuePair<string, string[]> keyValuePair in data.CustomOutfits)
			{
				string text;
				string[] array;
				keyValuePair.Deconstruct(out text, out array);
				string key = text;
				string[] itemIds = array;
				dictionary.Add(key, new SerializableOutfitData.Version2.CustomTemplateOutfitEntry
				{
					outfitType = "Clothing",
					itemIds = itemIds
				});
			}
			Dictionary<string, Dictionary<string, string>> dictionary2 = new Dictionary<string, Dictionary<string, string>>();
			foreach (KeyValuePair<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> keyValuePair2 in data.DuplicantOutfits)
			{
				string text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary3;
				keyValuePair2.Deconstruct(out text, out dictionary3);
				string key2 = text;
				Dictionary<ClothingOutfitUtility.OutfitType, string> dictionary4 = dictionary3;
				Dictionary<string, string> dictionary5 = new Dictionary<string, string>();
				dictionary2[key2] = dictionary5;
				foreach (KeyValuePair<ClothingOutfitUtility.OutfitType, string> keyValuePair3 in dictionary4)
				{
					ClothingOutfitUtility.OutfitType outfitType;
					keyValuePair3.Deconstruct(out outfitType, out text);
					ClothingOutfitUtility.OutfitType outfitType2 = outfitType;
					string value = text;
					dictionary5.Add(Enum.GetName(typeof(ClothingOutfitUtility.OutfitType), outfitType2), value);
				}
			}
			return new SerializableOutfitData.Version2
			{
				PersonalityIdToAssignedOutfits = dictionary2,
				OutfitIdToUserAuthoredTemplateOutfit = dictionary
			};
		}

		// Token: 0x06008290 RID: 33424 RVA: 0x000FA5BA File Offset: 0x000F87BA
		public static SerializableOutfitData.Version2 FromJson(JObject jsonData)
		{
			return jsonData.ToObject<SerializableOutfitData.Version2>(SerializableOutfitData.Version2.GetSerializer());
		}

		// Token: 0x06008291 RID: 33425 RVA: 0x000FA5C7 File Offset: 0x000F87C7
		public static JObject ToJson(SerializableOutfitData.Version2 data)
		{
			JObject jobject = JObject.FromObject(data, SerializableOutfitData.Version2.GetSerializer());
			jobject.AddFirst(new JProperty("Version", 2));
			return jobject;
		}

		// Token: 0x06008292 RID: 33426 RVA: 0x000FA5EA File Offset: 0x000F87EA
		public static JsonSerializer GetSerializer()
		{
			if (SerializableOutfitData.Version2.s_serializer != null)
			{
				return SerializableOutfitData.Version2.s_serializer;
			}
			SerializableOutfitData.Version2.s_serializer = JsonSerializer.CreateDefault();
			SerializableOutfitData.Version2.s_serializer.Converters.Add(new StringEnumConverter());
			return SerializableOutfitData.Version2.s_serializer;
		}

		// Token: 0x04006359 RID: 25433
		public Dictionary<string, Dictionary<string, string>> PersonalityIdToAssignedOutfits = new Dictionary<string, Dictionary<string, string>>();

		// Token: 0x0400635A RID: 25434
		public Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry> OutfitIdToUserAuthoredTemplateOutfit = new Dictionary<string, SerializableOutfitData.Version2.CustomTemplateOutfitEntry>();

		// Token: 0x0400635B RID: 25435
		private static JsonSerializer s_serializer;

		// Token: 0x020018B1 RID: 6321
		public class CustomTemplateOutfitEntry
		{
			// Token: 0x0400635C RID: 25436
			public string outfitType;

			// Token: 0x0400635D RID: 25437
			public string[] itemIds;
		}
	}

	// Token: 0x020018B2 RID: 6322
	public class Version1
	{
		// Token: 0x06008295 RID: 33429 RVA: 0x000FA63A File Offset: 0x000F883A
		public static JObject ToJson(SerializableOutfitData.Version1 data)
		{
			return JObject.FromObject(data);
		}

		// Token: 0x06008296 RID: 33430 RVA: 0x0034AE18 File Offset: 0x00349018
		public static SerializableOutfitData.Version1 FromJson(JObject jsonData)
		{
			SerializableOutfitData.Version1 version = new SerializableOutfitData.Version1();
			SerializableOutfitData.Version1 result;
			using (JsonReader jsonReader = jsonData.CreateReader())
			{
				string a = null;
				string b = "DuplicantOutfits";
				string b2 = "CustomOutfits";
				while (jsonReader.Read())
				{
					JsonToken tokenType = jsonReader.TokenType;
					if (tokenType == JsonToken.PropertyName)
					{
						a = jsonReader.Value.ToString();
					}
					if (tokenType == JsonToken.StartObject && a == b)
					{
						ClothingOutfitUtility.OutfitType outfitType = ClothingOutfitUtility.OutfitType.LENGTH;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								string key = jsonReader.Value.ToString();
								while (jsonReader.Read())
								{
									tokenType = jsonReader.TokenType;
									if (tokenType == JsonToken.EndObject)
									{
										break;
									}
									if (tokenType == JsonToken.PropertyName)
									{
										Enum.TryParse<ClothingOutfitUtility.OutfitType>(jsonReader.Value.ToString(), out outfitType);
										while (jsonReader.Read())
										{
											tokenType = jsonReader.TokenType;
											if (tokenType == JsonToken.String)
											{
												string value = jsonReader.Value.ToString();
												if (outfitType != ClothingOutfitUtility.OutfitType.LENGTH)
												{
													if (!version.DuplicantOutfits.ContainsKey(key))
													{
														version.DuplicantOutfits.Add(key, new Dictionary<ClothingOutfitUtility.OutfitType, string>());
													}
													version.DuplicantOutfits[key][outfitType] = value;
													break;
												}
												break;
											}
										}
									}
								}
							}
						}
					}
					else if (a == b2)
					{
						string text = null;
						while (jsonReader.Read())
						{
							tokenType = jsonReader.TokenType;
							if (tokenType == JsonToken.EndObject)
							{
								break;
							}
							if (tokenType == JsonToken.PropertyName)
							{
								text = jsonReader.Value.ToString();
							}
							if (tokenType == JsonToken.StartArray)
							{
								JArray jarray = JArray.Load(jsonReader);
								if (jarray != null)
								{
									string[] array = new string[jarray.Count];
									for (int i = 0; i < jarray.Count; i++)
									{
										array[i] = jarray[i].ToString();
									}
									if (text != null)
									{
										version.CustomOutfits[text] = array;
									}
								}
							}
						}
					}
				}
				result = version;
			}
			return result;
		}

		// Token: 0x0400635E RID: 25438
		public Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>> DuplicantOutfits = new Dictionary<string, Dictionary<ClothingOutfitUtility.OutfitType, string>>();

		// Token: 0x0400635F RID: 25439
		public Dictionary<string, string[]> CustomOutfits = new Dictionary<string, string[]>();
	}
}
