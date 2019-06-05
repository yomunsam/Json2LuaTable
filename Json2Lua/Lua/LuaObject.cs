using System;
using System.Collections.Generic;
using System.Text;

namespace Json2Lua.Lua
{
    public class LuaObject
    {
        public ELuaItemType type;

        public float value_num;
        public string value_str;
        public bool value_boolean;
        public LuaTable value_table;



        public LuaObject(string value)
        {
            type = ELuaItemType.String;
            value_str = value;
        }

        public LuaObject(float value)
        {
            type = ELuaItemType.Num;
            value_num = value;
        }

        public LuaObject(int value)
        {
            type = ELuaItemType.Num;
            value_num = value;
        }

        public LuaObject(bool value)
        {
            type = ELuaItemType.Boolean;
            value_boolean = value;
        }

        public LuaObject(LuaTable value)
        {
            type = ELuaItemType.Table;
            value_table = value;
        }

        public LuaObject()
        {
            type = ELuaItemType.nil;
        }

        public string GetString()
        {
            switch (type)
            {
                case ELuaItemType.Table:
                    return value_table.GetString();
                    //break;
                case ELuaItemType.Boolean:
                    if (value_boolean)
                    {
                        return "true";
                    }
                    else
                    {
                        return "false";
                    }
                    //break;
                case ELuaItemType.Num:
                    return value_num.ToString();
                //break;
                case ELuaItemType.String:
                    return "\""+ value_str+"\"";
                case ELuaItemType.nil:
                    return "nil";
                default:
                    return "";
            }
        }

    }
}
