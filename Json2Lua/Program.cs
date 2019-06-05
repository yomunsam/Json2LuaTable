using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Json2Lua.Lua;

namespace Json2Lua
{
    class Program
    {
        static void Main(string[] args)
        {
            //args = new string[] { @"E:\DevWork\Corala\VectorWarProject\VectorWar_Client_SVN\trunk\Assets\Game\Data\dict\CN\dict_lv.json", @"E:\DevWork\Corala\VectorWarProject\VectorWar_PD\配表\配表_CN\test.json" };
            if (args.Length < 1)
            {
                Console.WriteLine("要附带参数呀,否则我也不知道要干嘛啊，\n传入格式：\n  参数1：json文件路径（必须）\n  参数2：输出文件路径[可空]");
                Console.ReadKey();
            }
            else
            {
                var json_path = args[0];
                string lua_path;
                if (args.Length >= 2)
                {
                    lua_path = args[1];
                }
                else
                {
                    lua_path = Path.Combine(Directory.GetParent(json_path).ToString(), Path.GetFileNameWithoutExtension(json_path)) + ".lua";
                    Console.WriteLine("未指定路径，默认为:" + lua_path);
                }

                //判空
                if (!File.Exists(json_path))
                {
                    Console.WriteLine("出错：指定的Json路径不存在：" + json_path);
                    Console.ReadLine();
                }

                var json_str = File.ReadAllText(json_path);
                string lua_str = StartCovert(json_str);


                if (File.Exists(lua_path))
                {
                    File.Delete(lua_path);
                }
                var _dirPath = Directory.GetParent(lua_path).ToString();
                if (!Directory.Exists(_dirPath))
                {
                    Directory.CreateDirectory(_dirPath);
                }

                try
                {
                    File.WriteAllText(lua_path, lua_str);
                    Console.WriteLine("导出完成:" + lua_path);
                }catch(Exception exc)
                {
                    Console.WriteLine("出错：{0}", exc);
                }
                


            }
        }

        static string StartCovert(string json_str)
        {
            var jsonBase = JObject.Parse(json_str);
            //var luaBase = new LuaTable();

            var luaBaseTable = JsonObject2LuaTable(jsonBase);
            var str = "return " + luaBaseTable.GetString();
            return str;
            //Console.WriteLine("test:读取到的Lua:\n" + luaBaseTable.GetString());

        }

        /// <summary>
        /// 递归方法
        /// </summary>
        static LuaTable JsonObject2LuaTable(JObject jsonObj)
        {
            var curLuaTable = new LuaTable();

            if(jsonObj.Type == JTokenType.Array)
            {
                return JsonArray2LuaTable(jsonObj.ToObject<JArray>());
            }else if(jsonObj.Type == JTokenType.Object)
            {
                //无序
                foreach(var item in jsonObj)
                {
                    switch (item.Value.Type)
                    {
                        case JTokenType.Boolean:
                            curLuaTable.AddItem(item.Key,(bool)item.Value);
                            break;
                        case JTokenType.Array:
                            curLuaTable.AddItem(item.Key, JsonArray2LuaTable(item.Value.ToObject<JArray>()));
                            break;
                        case JTokenType.String:
                            curLuaTable.AddItem(item.Key, (string)item.Value);
                            break;
                        case JTokenType.Date: //转成string
                            curLuaTable.AddItem(item.Key, (string)item.Value);
                            break;
                        case JTokenType.Float:
                            curLuaTable.AddItem(item.Key, (float)item.Value);
                            break;
                        case JTokenType.Integer:
                            curLuaTable.AddItem(item.Key, (int)item.Value);
                            break;
                        case JTokenType.None:
                            curLuaTable.AddItemNil(item.Key);
                            break;
                        case JTokenType.Null:
                            curLuaTable.AddItemNil(item.Key);
                            break;
                        case JTokenType.Object:
                            curLuaTable.AddItem(item.Key, JsonObject2LuaTable(item.Value.ToObject<JObject>()));
                            break;
                        case JTokenType.TimeSpan:
                            curLuaTable.AddItem(item.Key, (float)item.Value);
                            break;
                    }
                }
            } 


            return curLuaTable;
        }

        static LuaTable JsonArray2LuaTable(JArray json_arr)
        {
            var curLuaTable = new LuaTable();
            //往luaTable里面扔有序数组
            foreach (var item in json_arr)
            {
                //检查子项类型

                switch (item.Type)
                {
                    case JTokenType.Boolean:
                        curLuaTable.AddItem((bool)item);
                        break;
                    case JTokenType.Array:
                        curLuaTable.AddItem(JsonArray2LuaTable(item.ToObject<JArray>()));
                        break;
                    case JTokenType.String:
                        curLuaTable.AddItem((string)item);
                        break;
                    case JTokenType.Object:
                        curLuaTable.AddItem(JsonObject2LuaTable(item.ToObject<JObject>()));
                        break;
                    case JTokenType.Date:
                        curLuaTable.AddItem((string)item);
                        break;
                    case JTokenType.Float:
                        curLuaTable.AddItem((float)item);
                        break;
                    case JTokenType.Integer:
                        curLuaTable.AddItem((int)item);
                        break;
                    case JTokenType.None:
                        curLuaTable.AddItemNil();
                        break;
                    case JTokenType.Null:
                        curLuaTable.AddItemNil();
                        break;
                    case JTokenType.TimeSpan:
                        curLuaTable.AddItem((float)item);
                        break;
                }


            }

            return curLuaTable;
        }

        //下面这些是昨天在Github看到的，后来有bug就不用了，自己撸了一套，哼唧


        //static string ConvertLua(string jsonStr)
        //{
        //    jsonStr = jsonStr.Replace(" ", string.Empty);//去掉所有空格

        //    string lua = "return";

        //    lua += ConvertJsonType(jsonStr);

        //    return lua;
        //}

        //static string ConvertJsonType(string jsonStr)
        //{
        //    string tempStr = jsonStr.Replace("\n", "").Replace("\r", "");
        //    string firstChar = "";
        //    try
        //    {
        //        firstChar = tempStr.Substring(0, 2);
        //    }
        //    catch (System.Exception)
        //    {

        //        Console.WriteLine(tempStr);
        //    }

        //    if (firstChar == "[{")
        //    {
        //        return ConvertJsonArray(jsonStr);
        //    }
        //    else if (firstChar[0] == '{')
        //    {
        //        return ConvertJsonArray(jsonStr);
        //    }
        //    else
        //    {
        //        return ConvertJsonArrayNoKey(jsonStr);
        //    }

        //}

        ///// <summary>
        ///// 没有key的 例如[1,2,3]
        ///// </summary>
        ///// <returns></returns>
        //static string ConvertJsonArrayNoKey(string jsonStr)
        //{
        //    string lastJsonStr = jsonStr.Replace("[", "{").Replace("]", "}");
        //    return lastJsonStr;
        //}

        //static string ConvertJsonArray(string jsonStr)
        //{
        //    string lastJsonStr = "";
        //    var separatorIndex = jsonStr.IndexOf(':');//通过:取得所有对象
        //    while (separatorIndex >= 0)
        //    {
        //        separatorIndex += 1;//加上冒号
        //        string cutStr = jsonStr.Substring(0, separatorIndex);
        //        string tempKey = "";
        //        string tempValue = "";
        //        for (int i = 0; i < cutStr.Length; i++)
        //        {
        //            char c = cutStr[i];
        //            if (c == '[')
        //            {
        //                c = '{';
        //            }
        //            else if (c == '"')
        //            {
        //                continue;
        //            }
        //            else if (c == ':')
        //            {
        //                c = '=';
        //            }
        //            tempKey += c;

        //        }
        //        jsonStr = jsonStr.Substring(separatorIndex);
        //        int index = 0;
        //        for (int i = 0; i < jsonStr.Length; i++)
        //        {

        //            char c = jsonStr[i];

        //            if (c == ',')
        //            {
        //                break;
        //            }
        //            else if (c == '{')
        //            {
        //                //新对象的开始
        //                string surplusStr = jsonStr.Substring(index);
        //                int bracketNum = 0;
        //                for (int j = 0; j < surplusStr.Length; j++)
        //                {
        //                    if (surplusStr[j] == '{')
        //                    {
        //                        bracketNum++;
        //                    }
        //                    else if (surplusStr[j] == '}')
        //                    {
        //                        if (bracketNum == 1)
        //                        {
        //                            string tempStr = jsonStr.Substring(index, index + j + 1);
        //                            string strResult = ConvertJsonType(tempStr);
        //                            tempValue += strResult;
        //                            index = index + j;
        //                            break;
        //                        }
        //                        bracketNum--;
        //                    }
        //                }
        //                i = index;
        //                continue;
        //            }
        //            else if (c == '[')
        //            {
        //                string surplusStr = jsonStr.Substring(index);
        //                int bracketNum = 0;
        //                for (int j = 0; j < surplusStr.Length; j++)
        //                {
        //                    if (surplusStr[j] == '[')
        //                    {
        //                        bracketNum++;
        //                    }
        //                    else if (surplusStr[j] == ']')
        //                    {
        //                        if (bracketNum == 1)
        //                        {
        //                            string tempStr = jsonStr.Substring(index, index + j + 1);
        //                            string strResult = ConvertJsonType(tempStr);
        //                            tempValue += strResult;
        //                            index = index + j;
        //                            break;
        //                        }
        //                        bracketNum--;
        //                    }
        //                }
        //                i = index;
        //                continue;
        //            }
        //            else if (c == ']')
        //            {
        //                c = '}';
        //            }
        //            index = i;
        //            tempValue += c;
        //        }
        //        lastJsonStr += tempKey + tempValue;
        //        jsonStr = jsonStr.Substring(index + 1);
        //        separatorIndex = jsonStr.IndexOf(':');
        //    }
        //    return lastJsonStr;
        //}
    }
}
