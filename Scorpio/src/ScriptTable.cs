using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Scorpio.Function;
using Scorpio.Compiler;
using Scorpio.Exception;
namespace Scorpio {
    //脚本table类型
    public class ScriptTable : ScriptObject {
        public override ObjectType Type { get { return ObjectType.Table; } }
        private Dictionary<object, ScriptObject> m_listObject = new Dictionary<object, ScriptObject>();  //所有的数据(函数和数据都在一个数组)
        public ScriptTable(Script script) : base(script) { }
        private ScriptTable metaTable; // 元表
        public void setMetaTable(ScriptTable meta) { this.metaTable = meta; }
        public ScriptTable getMetaTable() { return metaTable; }
        public override void SetValue(object key, ScriptObject value) {
            if (value is ScriptNull) {
                m_listObject.Remove(key);
            } else {
                if (this.metaTable == null)
                {
                    m_listObject[key] = value.Assign();
                }
                else
                {
                    if (m_listObject.ContainsKey(key))
                        m_listObject[key] = value.Assign();
                    else
                    {
                        ScriptObject idx_obj = this.metaTable.GetValue(ScorpioMetaMethod.NEWINDEX);
                        if (idx_obj == null)
                        {
                            m_listObject[key] = value.Assign();
                            return;
                        }

                        if (idx_obj.IsFunction)
                        {
                            ScriptObject[] paramsArr = { this, m_Script.CreateObject(key), value };
                            m_Script.CreateObject(idx_obj.Call(paramsArr));
                        }
                        else if (idx_obj.IsTable)
                        {
                            idx_obj.SetValue(key, value);
                        }
                    }
                }
            }
        }
        public override ScriptObject GetValue(object key) {
            if (this.metaTable == null)
                return m_listObject.ContainsKey(key) ? m_listObject[key] : m_Script.Null;
            else
            {
                if (m_listObject.ContainsKey(key))
                    return m_listObject[key];

                ScriptObject idx_obj = this.metaTable.GetValue(ScorpioMetaMethod.INDEX);
                if (idx_obj == null)
                    return m_Script.Null;

                if (idx_obj.IsFunction)
                {
                    ScriptObject[] paramsArr = { this, m_Script.CreateObject(key)};
                    return m_Script.CreateObject( idx_obj.Call(paramsArr) );
                } else if (idx_obj.IsTable)
                {
                    return idx_obj.GetValue(key);
                } else
                {
                    return m_Script.Null;
                }
            }
        }
        public override ScriptObject AssignCompute(TokenType type, ScriptObject value) {
            if (type != TokenType.AssignPlus) { return base.AssignCompute(type, value); }
            ScriptTable table = value as ScriptTable;
            if (table == null) throw new ExecutionException(m_Script, this, "table [+=] 操作只限两个[table]之间,传入数据类型:" + value.Type);
            ScriptObject obj = null;
            foreach (KeyValuePair<object, ScriptObject> pair in table.m_listObject) {
                obj = pair.Value.Clone();
                m_listObject[pair.Key] = obj;
            }
            return this;
        }
        public override ScriptObject Compute(TokenType type, ScriptObject value) {
            if (type != TokenType.Plus) { return base.Compute(type, value); }
            ScriptTable table = value as ScriptTable;
            if (table == null) throw new ExecutionException(m_Script, this, "table [+] 操作只限两个[table]之间,传入数据类型:" + value.Type);
            ScriptTable ret = m_Script.CreateTable();
            ScriptObject obj = null;
            foreach (KeyValuePair<object, ScriptObject> pair in m_listObject) {
                obj = pair.Value.Clone();
                ret.m_listObject[pair.Key] = obj;
            }
            foreach (KeyValuePair<object, ScriptObject> pair in table.m_listObject) {
                obj = pair.Value.Clone();
                ret.m_listObject[pair.Key] = obj;
            }
            return ret;
        }
        public bool HasValue(object key) {
            if (key == null) return false;
            return m_listObject.ContainsKey(key);
        }
        public int Count() {
            return m_listObject.Count;
        }
        public void Clear() {
            m_listObject.Clear();
        }
        public void Remove(object key) {
            m_listObject.Remove(key);
        }
        public ScriptArray GetKeys() {
            ScriptArray ret = m_Script.CreateArray();
            foreach (KeyValuePair<object, ScriptObject> pair in m_listObject) {
                ret.Add(m_Script.CreateObject(pair.Key));
            }
            return ret;
        }
        public ScriptArray GetValues() {
            ScriptArray ret = m_Script.CreateArray();
            foreach (KeyValuePair<object, ScriptObject> pair in m_listObject) {
                ret.Add(pair.Value.Assign());
            }
            return ret;
        }
        public Dictionary<object, ScriptObject>.Enumerator GetIterator() {
            return m_listObject.GetEnumerator();
        }
        public override ScriptObject Clone() {
            ScriptTable ret = m_Script.CreateTable();
            ScriptObject obj = null;
            foreach (KeyValuePair<object, ScriptObject> pair in m_listObject) {
                if (pair.Value == this) {
                    ret.m_listObject[pair.Key] = ret;
                } else {
                    obj = pair.Value.Clone();
                    ret.m_listObject[pair.Key] = obj;
                }
            }
            return ret;
        }
        public override string ToString() { return ToJson(); }
        public override string ToJson() {
            StringBuilder builder = new StringBuilder();
            builder.Append("{");
            bool first = true;
            foreach (KeyValuePair<object, ScriptObject> pair in m_listObject) {
                if (pair.Value is ScriptFunction) { continue; }
                if (first) { first = false; } else { builder.Append(","); }
                builder.Append("\"");
                builder.Append(pair.Key);
                builder.Append("\":");
                builder.Append(pair.Value.ToJson());
            }
            builder.Append("}");
            return builder.ToString();
        }
    }
}
