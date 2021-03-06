﻿using System;
using System.Collections.Generic;
using Scorpio;
using Scorpio.Runtime;
using Scorpio.Variable;
using Scorpio.Exception;
namespace Scorpio.Function {
    public class ScriptScriptFunction : ScriptFunction {
        private ScorpioScriptFunction m_ScriptFunction;                         //脚本函数
        private ScriptContext m_ParentContext;                                  //父级堆栈
        private Dictionary<String, ScriptObject> m_stackObject = new Dictionary<String, ScriptObject>();    //函数变量
        public override string ToString() { return "ScriptFunction(" + Name + ")" + "_" + this.funId; }
        public override string ToJson() { return "\"ScriptFunction\""; }
        private static int funCount = 0;
        private int funId;
        private bool m_isCloure = false;
        internal ScriptScriptFunction(Script script, String name, ScorpioScriptFunction function) : base(script, name)
        {
            this.funId = funCount++;
            this.m_ScriptFunction = function;
        }
        public bool isCloure { get { return m_isCloure; } set { m_isCloure = value; } }
        public override int GetParamCount() { return m_ScriptFunction.GetParameterCount(); }
        public override bool IsParams() { return m_ScriptFunction.IsParams(); }
        public override ScriptArray GetParams() { return m_ScriptFunction.GetParameters(); }
        public override void SetValue(object key, ScriptObject value) {
            if (!(key is string)) throw new ExecutionException(this.m_Script, this, "Function SetValue只支持String类型 key值为:" + key);
            m_stackObject[(string)key] = value;
        }
        public override ScriptObject GetValue(object key) {
            if (!(key is string)) throw new ExecutionException(this.m_Script, this, "Function GetValue只支持String类型 key值为:" + key);
            string skey = (string)key;
            return m_stackObject.ContainsKey(skey) ? m_stackObject[skey] : m_Script.Null;
        }
        public void SetTable(ScriptTable table)
        {
            m_stackObject["this"] = table;
            m_stackObject["self"] = table;
        }
        public ScriptScriptFunction SetParentContext(ScriptContext context) {
            m_ParentContext = context;
            return this;
        }
        public ScriptScriptFunction Create() {
            ScriptScriptFunction ret = new ScriptScriptFunction(m_Script, Name, m_ScriptFunction);
            return ret;
        }
        public override object Call(ScriptObject[] parameters) {
            return m_ScriptFunction.Call(m_ParentContext, m_stackObject, parameters);
        }
        public override ScriptObject Clone() {
            return Create();
        }
    }
}
