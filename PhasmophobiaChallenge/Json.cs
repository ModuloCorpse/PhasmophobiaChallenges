using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Json;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace PhasmophobiaChallenge
{
    public class Json
    {
        private readonly JsonObject m_Data;

        public static Json LoadFromFile(string path)
        {
            try
            {
                return new Json(File.ReadAllText(path, Encoding.UTF8));
            } catch(Exception)
            {
                return new Json();
            }
        }

        public static List<Json> LoadFromConcatenatedContent(string content)
        {
            List<Json> jsons = new List<Json>();
            string[] strings = Regex.Split(content.Trim(), "}\\s*\\{");
            if (strings.Length == 0)
                return jsons;
            if (strings.Length == 1)
            {
                jsons.Add(new Json(content));
                return jsons;
            }
            int idx = 0;
            foreach (string jsonContent in strings)
            {
                Json json;
                if (idx == 0)
                    json = new Json(jsonContent + "}");
                else if (idx == strings.Length - 1)
                    json = new Json("{" + jsonContent);
                else
                    json = new Json("{" + jsonContent + "}");
                jsons.Add(json);
                ++idx;
            }
            return jsons;
        }

        public static Json Copy(Json from)
        {
            return new Json(from.m_Data);
        }

        public Json()
        {
            m_Data = new JsonObject();
        }

        private Json(JsonObject value)
        {
            m_Data = value;
        }

        public Json(string value)
        {
            JsonValue jValue = JsonValue.Parse(value);
            if (jValue.JsonType == JsonType.Object)
                m_Data = jValue as JsonObject;
        }

        public Json(byte[] byteArray): this(Encoding.UTF8.GetString(byteArray).Substring(1)) {} //Substring(1) is here to remove the ? added by UTF8 encoding

        public override bool Equals(object obj)
        {
            if (obj is Json other)
                return m_Data.Equals(other.m_Data);
            return false;
        }

        public override int GetHashCode()
        {
            return 1768953197 + EqualityComparer<JsonObject>.Default.GetHashCode(m_Data);
        }

        private void WriteToFile(string path, int indent, bool async)
        {
            if (async)
            {
                Thread thread = new Thread(() => { File.WriteAllText(path, ToString(indent), Encoding.UTF8); });
                thread.Start();
            }
            else
                File.WriteAllText(path, ToString(indent), Encoding.Unicode);
        }

        public void ToFile(string path)
        {
            WriteToFile(path, 4, false);
        }

        public void ToAsyncFile(string path)
        {
            WriteToFile(path, 4, true);
        }

        public void ToFile(string path, int indent)
        {
            WriteToFile(path, indent, false);
        }

        public void ToAsyncFile(string path, int indent)
        {
            WriteToFile(path, indent, true);
        }

        public void Merge(Json json)
        {
            List<string> keys = json.GetKeys();
            foreach (string key in keys)
            {
                object ret = json.Get<object>(key);
                object current = Get<object>(key);
                if (current == null)
                    Set(key, ret, true);
                else if (current.GetType() == ret.GetType())
                {
                    if (ret is List<object> list)
                        ((List<object>)current).AddRange(list);
                    else if (ret is Json retJson)
                        ((Json)current).Merge(retJson);
                    Set(key, current, true);
                }
            }
        }

        public override string ToString()
        {
            return ToString("", m_Data, 0, -1, false);
        }

        public string ToString(int indentFactor)
        {
            return ToString("", m_Data, indentFactor, -1, true);
        }

        private string ToString(string nodeName, JsonValue node, int indentFactor, int depth, bool lineEnd)
        {
            if (node == null)
                return "";
            string tabs = "";
            string indent = "";
            for (int i = 0; i != indentFactor; ++i)
                indent += ' ';
            for (uint n = 0; n != (depth + 1); ++n)
                tabs += indent;
            string content = "";
            if (nodeName.Length > 0)
                content += string.Format("{0}\"{1}\": ", tabs, nodeName);
            else
                content += tabs;
            if (node is JsonArray array)
            {
                content += '[';
                if (lineEnd)
                    content += '\n';
                int idx = 0;
                foreach (JsonValue value in array)
                {
                    content += ToString("", value, indentFactor, depth + 1, lineEnd);
                    ++idx;
                    if (idx != node.Count)
                        content += ',';
                    if (lineEnd)
                        content += '\n';
                    else if (idx != node.Count)
                        content += ' ';
                }
                content += string.Format("{0}]", tabs);
            }
            else if (node is JsonObject obj)
            {
                content += '{';
                if (lineEnd)
                    content += '\n';
                int idx = 0;
                foreach (KeyValuePair<string, JsonValue> pair in obj)
                {
                    content += ToString(pair.Key, pair.Value, indentFactor, depth + 1, lineEnd);
                    ++idx;
                    if (idx != node.Count)
                        content += ',';
                    if (lineEnd)
                        content += '\n';
                    else if (idx != node.Count)
                        content += ' ';
                }
                content += string.Format("{0}{1}", tabs, '}');
            }
            else
                content += node.ToString();
            return content;

        }

        public Dictionary<string, object> ToMap()
        {
            Dictionary<string, object> ret = new Dictionary<string, object>();
            foreach (string key in m_Data.Keys)
                ret[key] = ConvertJsonValue<object>(m_Data[key]);
            return ret;
        }

        private object GetData<T>(string key)
        {
            string search = key.Split('.')[0];
            JsonValue ret;
            try
            {
                ret = m_Data[search];
            }
            catch (Exception)
            {
                return null;
            }
            if (key == search)
            {
                if (ret == null)
                    return null;
                return ConvertJsonValue<T>(ret);
            }
            return new Json((JsonObject)ret).GetData<T>(key.Substring(search.Length + 1));
        }

        public T Get<T>(string key)
        {
            object data = GetData<T>(key);
            if (data != null && data is T result)
                return result;
            return (T)(null as object);
        }

        public T Get<T>(string key, T defaultValue)
        {
            object data = GetData<T>(key);
            if (data != null && data is T result)
                return result;
            return defaultValue;
        }

        public bool Find(string key)
        {
            string search = key.Split('.')[0];
            if (key == search)
            {
                try
                {
                    if (m_Data[search] == null)
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
                return true;
            }
            return Find(key.Substring(search.Length + 1));
        }

        public List<T> GetArray<T>(string key)
        {
            List<T> ret = new List<T>();
            object data = GetData<T>(key);
            if (data != null && data is List<object> result)
            {
                foreach (object elem in result)
                {
                    if (elem is T val)
                        ret.Add(val);
                }
            }
            return ret;
        }

        public Dictionary<string, T> GetMap<T>(string key)
        {
            Dictionary<string, T> ret = new Dictionary<string, T>();
            object data = GetData<T>(key);
            if (data != null && data is Json result)
            {
                foreach (string keyToMap in result.m_Data.Keys)
                {
                    T tmp = result.Get<T>(keyToMap);
                    if (tmp != null)
                        ret[keyToMap] = tmp;
                }
            }
            return ret;
        }

        private object ConvertJsonValue<T>(JsonValue jValue)
        {
            if (jValue is JsonValue)
            {
                if (jValue is JsonPrimitive primitive)
                {
                    Type genericType = typeof(T);
                    if (genericType == typeof(bool))
                        return (bool)primitive;
                    else if (genericType == typeof(char))
                        return (char)primitive;
                    else if (genericType == typeof(byte))
                        return (byte)primitive;
                    else if (genericType == typeof(int))
                        return (int)primitive;
                    else if (genericType == typeof(uint))
                        return (uint)primitive;
                    else if (genericType == typeof(DateTime))
                        return (DateTime)primitive;
                    else if (genericType == typeof(DateTimeOffset))
                        return (DateTimeOffset)primitive;
                    else if (genericType == typeof(decimal))
                        return (decimal)primitive;
                    else if (genericType == typeof(double))
                        return (double)primitive;
                    else if (genericType == typeof(float))
                        return (float)primitive;
                    else if (genericType == typeof(Guid))
                        return (Guid)primitive;
                    else if (genericType == typeof(long))
                        return (long)primitive;
                    else if (genericType == typeof(sbyte))
                        return (sbyte)primitive;
                    else if (genericType == typeof(short))
                        return (short)primitive;
                    else if (genericType == typeof(string))
                        return (string)primitive;
                    else if (genericType == typeof(TimeSpan))
                        return (TimeSpan)primitive;
                    else if (genericType == typeof(ulong))
                        return (ulong)primitive;
                    else if (genericType == typeof(Uri))
                        return (Uri)primitive;
                    else if (genericType == typeof(ushort))
                        return (ushort)primitive;
                    else if (genericType == typeof(object))
                        return primitive;
                    else
                        return null;
                }
                else if (jValue is JsonObject jObj)
                {
                    Json tmp = new Json(jObj);
                    if (typeof(IJsonable).IsAssignableFrom(typeof(T)))
                    {
                        MethodInfo deserializeMethod = typeof(T).GetMethod("Deserialize");
                        if (deserializeMethod != null && deserializeMethod.IsStatic)
                            return deserializeMethod.Invoke(null, new object[] { tmp });
                    }
                    return tmp;
                }
                else if (jValue is JsonArray array)
                {
                    List<object> list = new List<object>();
                    foreach (JsonValue elem in array)
                        list.Add(ConvertJsonValue<T>(elem));
                    return list;
                }
                return null;
            }
            else
                return jValue;
        }

        private JsonValue ConvertObject(object obj)
        {
            if (obj is JsonValue jsonVal)
                return jsonVal;
            else if (obj is Json json)
                return json.m_Data;
            else if (obj is IDictionary dictionary)
            {
                JsonObject ret = new JsonObject();
                foreach (DictionaryEntry item in dictionary)
                    ret[item.Key.ToString()] = ConvertObject(item.Value);
                return ret;
            }
            else if (obj is IList list)
            {
                JsonArray ret = new JsonArray();
                foreach (var item in list)
                    ret.Add(ConvertObject(item));
                return ret;
            }
            else if (obj is IJsonable jsonable)
                return jsonable.Serialize().m_Data;
            else if (obj is bool @bool)
                return new JsonPrimitive(@bool);
            else if (obj is char @char)
                return new JsonPrimitive(@char);
            else if (obj is byte @byte)
                return new JsonPrimitive(@byte);
            else if (obj is int @int)
                return new JsonPrimitive(@int);
            else if (obj is uint @uint)
                return new JsonPrimitive(@uint);
            else if (obj is DateTime dateTime)
                return new JsonPrimitive(dateTime);
            else if (obj is DateTimeOffset dateTimeOffset)
                return new JsonPrimitive(dateTimeOffset);
            else if (obj is decimal @decimal)
                return new JsonPrimitive(@decimal);
            else if (obj is double @double)
                return new JsonPrimitive(@double);
            else if (obj is float @float)
                return new JsonPrimitive(@float);
            else if (obj is Guid guid)
                return new JsonPrimitive(guid);
            else if (obj is long @long)
                return new JsonPrimitive(@long);
            else if (obj is sbyte @sbyte)
                return new JsonPrimitive(@sbyte);
            else if (obj is short @short)
                return new JsonPrimitive(@short);
            else if (obj is string @string)
                return new JsonPrimitive(@string);
            else if (obj is TimeSpan timeSpan)
                return new JsonPrimitive(timeSpan);
            else if (obj is ulong @ulong)
                return new JsonPrimitive(@ulong);
            else if (obj is Uri uri)
                return new JsonPrimitive(uri);
            else if (obj is ushort @ushort)
                return new JsonPrimitive(@ushort);
            else
                return null;
        }

        public bool Add(string key, object valueToAdd)
        {
            return Set(key, valueToAdd, false);
        }

        public bool Set(string key, object valueToAdd)
        {
            return Set(key, valueToAdd, true);
        }

        private bool Set(string key, object valueToAdd, bool crush)
        {
            object value;
            string[] keys = key.Split('.');
            JsonObject map = m_Data;
            for (int n = 0; n != (keys.Length - 1); ++n)
            {
                try
                {
                    value = map[keys[n]];
                    if (value is JsonObject valueJObj)
                        map = valueJObj;
                    else
                        return false;
                } catch (Exception)
                {
                    JsonObject newSection = new JsonObject();
                    map.Add(keys[n], newSection);
                    map = newSection;
                }
            }
            if (valueToAdd is Json valueToAddJson)
                valueToAdd = valueToAddJson.m_Data;
            else if (valueToAdd is List<object> valueToAddList)
                valueToAdd = ConvertObject(valueToAddList);
            if (!crush)
            {
                try
                {
                    value = map[keys[keys.Length - 1]];
                    if (value is JsonArray array)
				    {
                        if (valueToAdd is JsonArray secondArray)
                        {
                            foreach (JsonValue item in secondArray)
                                array.Add(item);
                        }
                        else
                            array.Add(ConvertObject(valueToAdd));
                        valueToAdd = array;
                    }
                    else if (value.GetType() == valueToAdd.GetType())
                    {
                        JsonArray tmp = new JsonArray
                        {
                            ConvertObject(value),
                            ConvertObject(valueToAdd)
                        };
                        valueToAdd = tmp;
                    }
                }
                catch (Exception) {}
            }
            map[keys[keys.Length - 1]] = ConvertObject(valueToAdd);
            return true;
        }

        public List<string> GetKeys()
        {
            List<string> ret = new List<string>();
            ret.AddRange(m_Data.Keys);
            return ret;
        }
    }
}
