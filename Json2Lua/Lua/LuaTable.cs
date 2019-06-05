using System;
using System.Collections.Generic;
using System.Text;

namespace Json2Lua.Lua
{
    public class LuaTable
    {
        private List<LuaObject> mItems_order = new List<LuaObject>(); //存放table中的有序部分
        private Dictionary<string, LuaObject> mItems_kv = new Dictionary<string, LuaObject>(); //存放table中的无序部分


        #region 有序数组添加

        /// <summary>
        /// [有序]添加string value
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(string value)
        {
            mItems_order.Add(new LuaObject(value));
        }

        /// <summary>
        /// [有序]num value
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(int value)
        {
            mItems_order.Add(new LuaObject(value));
        }

        /// <summary>
        /// [有序]num value
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(float value)
        {
            mItems_order.Add(new LuaObject(value));
        }
        /// <summary>
        /// [有序]bool value
        /// </summary>
        /// <param name="value"></param>
        public void AddItem(bool value)
        {
            mItems_order.Add(new LuaObject(value));
        }

        public void AddItem(LuaTable value)
        {
            mItems_order.Add(new LuaObject(value));

        }

        /// <summary>
        /// 加入nil
        /// </summary>
        public void AddItemNil()
        {
            mItems_order.Add(new LuaObject());

        }

        #endregion


        #region Key_Value添加

        public void AddItem(string key, string value)
        {
            //查重
            if (!mItems_kv.ContainsKey(key))
            {
                mItems_kv.Add(key, new LuaObject(value));
            }
        }

        public void AddItem(string key, int value)
        {
            //查重
            if (!mItems_kv.ContainsKey(key))
            {
                mItems_kv.Add(key, new LuaObject(value));
            }
        }
        public void AddItem(string key, float value)
        {
            //查重
            if (!mItems_kv.ContainsKey(key))
            {
                mItems_kv.Add(key, new LuaObject(value));
            }
        }
        public void AddItem(string key, bool value)
        {
            //查重
            if (!mItems_kv.ContainsKey(key))
            {
                mItems_kv.Add(key, new LuaObject(value));
            }
        }
        public void AddItem(string key, LuaTable value)
        {
            //查重
            if (!mItems_kv.ContainsKey(key))
            {
                mItems_kv.Add(key, new LuaObject(value));
            }
        }

        public void AddItemNil(string key)
        {
            //查重
            if (!mItems_kv.ContainsKey(key))
            {
                mItems_kv.Add(key, new LuaObject());
            }
        }

        #endregion



        public string GetString()
        {
            string str = "{";
            if(mItems_order.Count > 0)
            {
                foreach(var item in mItems_order)
                {
                    str += item.GetString();
                    str += ",";
                }
            }

            if(mItems_kv.Count > 0)
            {
                foreach (var item in mItems_kv)
                {
                    str += (item.Key + "=" + item.Value.GetString());
                    str += ",";
                }
            }

            str += "}";

            return str;
        }
        

        


    }
}
