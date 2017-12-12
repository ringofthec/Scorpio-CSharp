2017-12-12
fork 喜羊羊的项目，原因是在使用的过程中，自己很想修改一些东西，但是提交很不方便，只能打patch给他
干脆fork一个算了


# Scorpio-CSharp #
* author : while
* QQ群 : 245199668 [加群](http://shang.qq.com/wpa/qunwpa?idkey=8ef904955c52f7b3764403ab81602b9c08b856f040d284f7e2c1d05ed3428de8)

* VSCode 基础语法提示插件 https://marketplace.visualstudio.com/items?itemName=while.scorpio 或者 VSCode 直接搜索 scorpio
* 脚本教程 http://www.fengyuezhu.com/readme/
* Scorpio-CSharp语法 体验地址 http://www.fengyuezhu.com/project/Scorpio-CSharp/scriptconsole/
* 脚本实现Space Shooter 体验地址 http://www.fengyuezhu.com/project/Scorpio-CSharp/unitysample/
* 脚本实现Space Shooter 源码地址 https://github.com/qingfeng346/ScorpioUnitySample


* Sco脚本的Java实现 : https://github.com/qingfeng346/Scorpio-Java
* 网络协议,Excel表数据转换工具 : https://github.com/qingfeng346/ScorpioConversion
* 国内用户如果网比较慢可以在此链接下载 : http://git.oschina.net/qingfeng346/Scorpio-CSharp


## 此脚本是用作Unity游戏热更新使用的脚本,纯c#实现,最低支持.net3.5,兼容.net3.5以上所有平台,语法类似 javascript
* **脚本示例** 放在 [bin/Scripts](https://github.com/qingfeng346/Scorpio-CSharp/tree/master/bin/Scripts) 目录
* **语法测试** 直接运行 **bin/ScorpioDemo.exe**  左侧选中要测试的脚本,点击 **Run Script** 按钮即可
* **性能测试** (C#light,ulua,Scorpio-CSharp) https://github.com/qingfeng346/ScriptTestor

## 项目宏定义说明:
* **SCORPIO_NET_CORE** .net4.5平台以后使用(UWP平台 , dotnet core)
* **SCORPIO_DYNAMIC_DELEGATE** 动态创建Delegate对象 不适用的请自行实现一个继承 DelegateTypeFactory 的类,目前亲测只有android和windows(exe)平台可用

## 源码目录说明
* Script 文件是脚本的引擎对象
* Util 文件是一些常用的函数集合
* ScriptXXX 所有Script开头的类都是脚本内保存的对象
    * ScriptObject 所有脚本类都继承自此类
    * ScriptNull 空类型 null
    * ScriptBoolean bool 类型
    * ScriptNumber 数字类型 衍生出 ScriptNumberDouble，ScriptNumberInt，ScriptNumberLong 三个类
    * ScriptString 字符串类型
    * ScriptFunction 函数类型
    * ScriptArray 数组([])类型，相当于c#的List< ScriptObject >
    * ScriptTable table类型，相当于c#的Dictionary<object,ScriptObject>
    * ScriptEnum 处理所有c#内的枚举
    * ScriptUserdata 处理所有c#内的对象，衍生出所有Userdata目录下的类
* 子文件夹
    * CodeDom 此目录下全部都是解析脚本后生成的中间代码
    * Compiler 此目录下是脚本解释器
    * Exception 脚本引擎抛出的已知异常，例如解析失败，未支持语法等
    * Function 所有的函数类型，脚本函数，扩展函数等
    * Library 脚本内使用的库的源码，例如**json**,**array**,**table**,**string**,**math**库等，方便使用，初始化脚本时请调用 LoadLibrary 函数后方可使用
    * Runtime 此目录是运行 CodeDom 目录下的所有中间代码
    * Serialize 序列化字节码使用，把文本文件解析成二进制数据以及把二进制数据反序列化成文本文件
    * Userdata   此目录是根据c#代码内object的类型分别处理的代码，例如DefaultScriptUserdataDelegate是处理Delegate类型的对象，DefaultScriptUserdataObject是处理普通的c#对象，DefaultScriptUserdataEnum是处理枚举对象等
    * Variable 脚本内对象的差异化处理，例如ScriptNumberDouble，ScriptNumberInt，ScriptNumberLong三个类都是处理number类型，但是不同类型的处理方式不同

## 注意事项 ##
* 如果要使用 **Script.LoadFile** 函数，文件编码要改成 **utf8 without bom (无签名的utf8格式)**, 否则bom三个字节会解析失败
* 使用 **import_type import_extension**  前要确认是否已经添加该类的程序集(Assembly),例如要使用 **UnityEngine.dll** 中的 **UnityEngine.GameObject** 类,要先再c#中调用 **script.PushAssembly(typeof(GameObject).GetTypeInfo().Assembly)** 压入程序集,然后**UnityEngine.dll** 中的类就都可以使用了,也就是 **每个dll文件的程序集** 都要添加一次
* 脚本内所有c#变量(除了number,string等基础类型)**均为引用**,struct变量也一样
* c#重载**[]**运算符后脚本里不能直接使用[],请使用 **get_Item set_Item** 函数
* c#数组对象获取元素不能直接使用**[]**,请使用 **GetValue SetValue** 函数, 具体参数可以参考 **Array** 类的 **GetValue SetValue** 函数
* c#中event对象+= -=操作可以使用函数 **add_[event变量名] remove_[event变量名]** 代替
* 同类中静态函数和实例函数不要重名,否则会调用失败 例如 static void Test(object a); void Test(object a, object b); 两个函数不一定会当静态还是实例函数处理
* 同类中重载的函数相同参数不要是继承关系,否则可能调用失败,例如 void Test(object a); void Test(string a); 两个Test函数都可以传入string,但是调用时不一定会调用哪一个
* c#类的变量不能类似 **+= -= \*= /= 等赋值计算操作(只有event可以使用 += -=)** 请使用 **变量 = 变量 + XXX**
* 如果要使用c#扩展函数,请调用一次 **import_extension("实现扩展函数的类的全路径")** , 注意：这个函数一定要在调用扩展函数之前调用,否则会找不到
* IL2CPP生成后,好多Unity的类的函数反射回调用不到,遇到这种情况请自行包一层函数,自己写的c#代码不会有这种情况
* UWP平台master配置下 **generic_method** 函数会出问题,可能是因为UWP屏蔽了此函数 报错: PlatformNotSupported_NoTypeHandleForOpenTypes. For more information, visit http://go.microsoft.com/fwlink/?LinkId=623485
* UWP平台master配置下 **generic_type** 函数也会出问题
* 不能使用 **SCORPIO_DYNAMIC_DELEGATE** 的平台,要实现一个继承 DelegateTypeFactory 的类,然后调用ScriptUserdataDelegateType.SetFactory函数设置一下 例如:

```csharp
public class MyDelegateFactory : DelegateTypeFactory {
	public Delegate CreateDelegate(Script script, Type type, ScriptFunction func) {
		if (type == typeof(UnityAction))						//UnityAction委托类型
			return new UnityAction (() => { func.call (); });
		else if (type == typeof(Application.LogCallback)) 		//Application.LogCallback 委托类型
			return new Application.LogCallback((arg1, arg2, arg3) => { func.call(arg1, arg2, arg3); });
		else if (type == typeof(Comparison<Transform>))			//Comparison<Transform> List排序委托类型, 带返回值
			return new Comparison<Transform>((arg1, arg2) => { return Util.ToInt32(((ScriptObject)func.call(arg1, arg2)).ObjectValue); });
		//自己可能用到的委托类型请自行添加
		Debug.Log ("Delegate Type is not found : " + type + "  func : " + func);
        return null;
	}
}
///然后实现完以后 调用 ScriptUserdataDelegateType.SetFactory(new MyDelegateFactory()); 设置一下
```
* 也可以使用**ScorpioReflect**项目中的**GenerateScorpioDelegate**类可以自动生成一个 DelegateTypeFactory 类  

```csharp
var generate = new Scorpio.ScorpioReflect.GenerateScorpioDelegate();
generate.AddType(typeof(Action<bool>));     //使用AddType 添加所有需要用到的 Delegate , 模板函数 要单独添加
generate.AddType(typeof(Action<int>));
System.IO.File.WriteAllText(Application.dataPath + "/Scripts/ScorpioDelegate/" + gen.ClassName + ".cs", gen.Generate());        //调用Generate函数生成内容

/***********下面是生成文件示例***********************
using System;
using Scorpio;
using Scorpio.Userdata;
namespace ScorpioDelegate {
    public class ScorpioDelegateFactory : DelegateTypeFactory {
        public static void Initialize() {
            ScriptUserdataDelegateType.SetFactory(new ScorpioDelegateFactory());
        }
        public Delegate CreateDelegate(Script script, Type type, ScriptFunction func) {
            if (type == typeof(System.Action<System.Boolean>))
                return new System.Action<System.Boolean>((arg0) => { func.call(arg0); });
            if (type == typeof(System.Action<System.Int32>))
                return new System.Action<System.Int32>((arg0) => { func.call(arg0); });
            throw new Exception("Delegate Type is not found : " + type + "  func : " + func);
        }
    }
}
生成文件后 调用 Initialize 函数即可
*/
```

## 使用去反射功能注意事项 ##
* 不能调用模板函数
* 不能调用含有ref和out参数的函数

## Unity3d发布平台支持(亲测):
- [x] Web Player
- [x] PC, Mac & Linux Standalone
- [x] iOS(包括IL2CPP)
- [x] Android
- [x] BlackBerry
- [x] Windows Phone 8
- [x] Windows 10 (Universal Windows Platform) (请添加 SCORPIO_NET_CORE 宏定义)
- [x] WebGL
- [x] Tizen
- [ ] 理论上可以支持所有平台

## 反射调用运算符重载函数
*	\+    op_Addition
*	\-    op_Subtraction
*	\*    op_Multiply
*	/    op_Division
*	%    op_Modulus
*	|    op_BitwiseOr
*	&    op_BitwiseAnd
*	^    op_ExclusiveOr
*	\>    op_GreaterThan
*	<    op_LessThan
*	==   op_Equality
*	!=   op_Inequality
*	[]   get_Item(获取变量)
*	[]   set_Item(设置变量)


## 源码目录说明:
* **Scorpio** 脚本引擎项目,平常使用只需导入此目录即可
* **ScorpioDemo** Scorpio例子程序
* **ScorpioExec** 跟lua.exe一样,命令行调用Scorpio脚本
* **ScorpioMaker** 把Scorpio脚本序列化成二进制文件,把二进制文件反序列化成文本文件
* **ScorpioTest** Unity内使用Scorpio脚本例子
* **ScorpioLibrary** ScorpioExec脚本使用的第三方库,可以直接运行一个脚本然后当作类似python一样的脚本去处理一些简单的任务
* **ScorpioReflect** Scorpio脚本去反射机制的实现

## Unity导入Scorpio-CSharp:
* 第一种方法(建议) : 把 [Scorpio/src](https://github.com/qingfeng346/Scorpio-CSharp/tree/master/Scorpio/src) 整个文件夹复制到项目即可
* 第二种方法 : 用VS打开Scorpio.sln编译一下项目 生成Scorpio.dll文件 然后复制到Unity项目Plugins目录下 (ps:源码的项目文件自带SCORPIO_DYNAMIC_DELEGATE编译符号,请删除后编译)

## Scorpio脚本Hello World函数 (c#控制台项目):
```csharp
using System;
using Scorpio;
namespace HelloWorld {
    public delegate void TestDelegate1(int a, int b);
    public class Test {
        public static TestDelegate1 dele;
        private int a = 100;
        public Test(int a) {
            this.a = a;
        }
        public void Func() {
            Console.WriteLine("Func " + a);
        }
        public static void StaticFunc() {
            Console.WriteLine("StaticFunc");
        }
        public static void Call() {
            if (dele != null) dele(100, 200);
        }
    }
    public enum TestEnum {
        Test1,
        Test2,
        Test3,
    }
    public class MyDelegateFactory : Scorpio.Userdata.DelegateTypeFactory {
        public Delegate CreateDelegate(Script script, Type type, ScriptFunction func) {
            if (type == typeof(TestDelegate1))                        //UnityAction委托类型
                return new TestDelegate1((arg1, arg2) => { func.call(arg1, arg2); });
            //自己可能用到的委托类型请自行添加
            return null;
        }
    }
    class MainClass {
        public static void Main(string[] args) {
            Scorpio.Userdata.ScriptUserdataDelegateType.SetFactory(new MyDelegateFactory());        //设置委托生成器
            Script script = new Script();                           //new一个Script对象
            script.LoadLibrary();                                   //加载所有Scorpio的库，源码在Library目录下
            script.PushAssembly(typeof(MainClass).Assembly);            //添加当前程序的程序集
            script.SetObject("CTest", script.CreateObject(new Test(300)));  //SetObject可以设置一个c#对象到脚本里
                                                                            //LoadString 解析一段字符串,LoadString传入的参数就是热更新的文本文件内容
            script.LoadString(@"
print(""hello world"")
");
            //Scorpio脚本调用c#函数
            script.LoadString(@"
MyTest = import_type('HelloWorld.Test')             //import_type 要写入类的全路径 要加上命名空间 否则找不到此类,然后赋值给 MyTest 对象
MyTest.StaticFunc()                                 //调用c#类的静态函数
var t = MyTest(200)                                 //new 一个Test对象, 括号里面是构造函数的参数
t.Func()                                            //调用c#的内部函数
CTest.Func()                                        //调用c#的内部函数 CTest是通过 script.SetObject 函数设置
MyTest.dele = function(a, b) {                      //设置委托类型
    print(a + '   ' + b)
}
MyTest.Call()
TestEnum = import_type('HelloWorld.TestEnum')       //引入枚举
print(TestEnum.Test1)                               //直接使用枚举
");
            Console.ReadKey();
        }
    }
}
```

## c#去反射类使用 ##
* 把**ScorpioReflect**项目中的**GenerateScorpioClass**文件夹下的所有cs文件复制到项目工程,放到**Editor**目录即可,此类只用作生成中间代码,后期不会使用,使用示例: 

```csharp
//就拿UnityEngine.GameObject类为例
//先生成中间代码
//使用GenerateScorpioClass生成代码,ScorpioClassName变量是中间类名称,Generate()返回生成后的代码
//下面两句代码是生成UnityEngine.GameObject的中间类到工程目录
var gen = new GenerateScorpioClass(typeof(UnityEngine.GameObject));
System.IO.File.WriteAllText(Application.dataPath + "/Scripts/ScorpioClass/" + gen.ScorpioClassName + ".cs", gen.Generate());


//然后调用Script类的PushFastReflectClass函数把typeof(UnityEngine.GameObject)和生成后的类ScorpioClass_UnityEngine_GameObject关联上
Script script = new Script();
script.LoadLibrary();
script.PushFastReflectClass(typeof(UnityEngine.GameObject), new ScorpioClass_UnityEngine_GameObject(script));
```

## master版本更新和修改内容 ##

(2017-11-02)
-----------
* 修改table设置一个key为null 则table的map会删除此key

(2017-10-24)
-----------
* 支持 **finally** 关键字

(2017-08-22)
-----------
* 增加字符串内部变量功能，例如：

```javascript
var a = 1
var b = 2
print("a${a}a${b+1}a")
//输出 a1a3a
//字符串内添加 ${变量} 就可以使用
```

(2017-07-05)
-----------
* 修复一个运算符重载的BUG

(2017-07-04)
-----------
* 修复一个table function 中 赋值 this 的 bug

(2017-06-21)
-----------
* 添加userdata库，只有一个 rename 函数，可以重命名 c# 函数名称，例如：

```javascript
ListType = import_type("System.Collections.Generic.List`1")
StringType = import_type("System.String")
ListString = generic_type( ListType ,  StringType)      //载入List<String>
var lt = ListString();                                  //创建一个对象
//rename 函数第一个参数可以传入c# 类对象，也可以传入 实例对象
//rename 函数只需调用一次，所有实例类就都可以使用
userdata.rename(lt, "Add", "testAdd1")                  //重命名Add函数为testAdd1，Add还可以继续使用
userdata.rename(ListString, "Add", "testAdd2")          //重命名Add函数为testAdd2，
userdata.rename(ListString, "Capacity", "testCapacity")
lt.testAdd1("111")
lt.testAdd2("222")
print(lt.testCapacity)
lt.testCapacity = 15000
print(lt.testCapacity)
```

(2017-06-20)
-----------
* 提升调用c#函数性能
* 修复一个调用c#函数BUG

(2017-06-19)
-----------
* table的function变量全部支持this关键字，例如

```javascript
tab = {
    a : 100,
    b : function() {
        //此处可以使用this
        print(this.a)
    }
    function d () {
        //此处可以使用this
        print(this.a)
    }
}
tab.c = function() {
    //此处不可以使用 this ，只有在table内部声明才可以使用this
    print(this.a)
}
```

(2017-05-27)
-----------
* **string**库添加**char2ascii**函数
* **string**库添加**ascii2char**函数
* **print**支持直接输出**array**和**table**变量

(2017-05-11)
-----------
* 修复去反射生成工具，过滤器不能过滤 property( get,set ) 函数的bug

(2017-04-20)
-----------
* 修复array初始化时会传入对象引用的问题，例如  

```javascript
var num = 0
var arr = [num]     //修改前传入的num是引用，num本身改变会应用arr[0]的值
num += 1
print(arr[0])       //修改前会输出1，修改后会输出0
```

(2017-03-24)
-----------
* string库增加join函数

(2017-02-14)
-----------
* 增加 **long** 和 **int** 类型的 **[~]取反操作** ,多谢 **[福灵心至]** 同学的建议

(2016-12-30)
-----------
* 初始化 **table** 时，后申请的变量可以使用先申请的变量，例如

```javascript
a = {
    a1 = 100,
    a2 = a1 + 100,
    a3 = func1(),
    function func1() {      //函数优先初始化
        return 100
    }
}
```
(2016-12-27)
-----------
* 修改 **if** 简洁写法最后加 **;** 会导致后面的 **else** 和 **else if** 解析错误的问题 (感谢 **[小卒 北京]** 同学的反馈)

```javascript
if (true)
    return;             //修改前此行就解析出错
else
    print("false")
```

(2016-12-06)
-----------
* 修复生成去反射工具某些情况下过滤条件判断错误的问题

(2016-11-25)
-----------
* 申请table变量可以不赋值,默认为null

```javascript
var t = {
    a,      //修改前此处会报解析错误，修改后a会默认为null，相当于 a = null,
}
```

(2016-10-20)
-----------
* 增加支持c#的扩展函数方法 具体方法请查看注意事项

(2016-8-15)
-----------
* 支持c#函数默认参数

(2016-8-4)
-----------
* ScorpioExec 项目的生成改为 sco.exe ， 把 ScorpioExec\bin\Debug 加入环境变量可以使用 [sco 文件路径] 命令直接运行一个脚本文件

(2016-8-2)
-----------
* 增加 tosbyte tobyte toshort toushort touint toulong tofloat 函数, 可以传入制定类型的数字参数

(2016-6-20)
-----------
* 优化堆栈输出内容

(2016-6-17)
-----------
* 增加报错字段名输出
    * 修改前错误输出格式 : 文件名:行数:错误信息
    * 修改后错误输出格式 : 文件名:行数[字段名]:错误信息

(2016-6-13)
-----------
* **ScorpioReflect**项目增加**GenerateScorpioDelegate**类，可以自动生成**DelegateTypeFactory**类,具体方法请查看**注意事项**

(2016-6-6)
-----------
* 修改源码代码最低支持.net3.5,之前的.net版本不再兼容
* 支持 .net core
* 优化脚本执行性能
* UWP平台宏定义改为 SCORPIO_NET_CORE

(2016-6-1)
-----------
* 去反射类过滤不生成的函数改为 实现一个继承自 Scorpio.ScorpioReflect.ClassFilter 的类, 然后调用 GenerateScorpioClass 的 SetClassFilter 设置
* 去反射生成的变量,属性,事件,函数加入名字排序,不会导致每次生成有可能导致文件发生改变,也方便查找

(2016-5-31)
-----------
* 提升脚本执行性能 5% - 10%

(2016-5-31)
-----------
* 类型null支持当作table的key值
* 类型bool可以直接传入类型当作table的key值,修改前只可以传入变量

```javascript
var a = {
    true = 100,
    null = 100,
}
print(a[true])
print(a[null])
a[true] = 200
a[null] = 200
print(a[true])
print(a[null])
```

(2016-5-20)
-----------
* 增加func库(只对脚本函数有效)
	* func.count() 返回函数参数个数
	* func.isparams() 返回函数是否是不定参函数
	* func.isstatic() 返回函数是否是静态函数(不是table内部函数)
	* func.getparams() 返回函数参数名字数组
    
(2016-4-20)
-----------
* 优化去反射工具生成代码
	* 所有函数信息直接生成代码,不会运行时获取反射,il2cpp下获取反射函数会获取不全
	* 可以使用反射函数名调用运算符重载
	* 可是使用反射函数名操作property和event
	* GenerateScorpioClass 增加 AddExclude 函数,可以在生成时去除不想生成的函数

(2016-4-12)
-----------
* table 支持 + += 操作,用此操作可以实现伪继承 示例:

```javascript
var base = {
	value1 = 100,
	function hello1() { print("hello1 " + this.value1) }
	function hello2() { print("default hello2") }
}
var a = base + {
	value2 = 200,
	function hello2() { print("hello2 " + this.value2) }
}
var b = base + {
}
a.value1 = 300
a.hello1()		//输出 hello1 300
a.hello2()		//输出 hello2 200
b.hello1()		//输出 hello1 100
b.hello2()		//输出 default hello2
//用 + += 可以实现伪继承 相加的数据都是clone的,所以相互的数据不会共享
```
(2016-4-9)
-----------
* 增加宏定义判断,用法同c#
	* 支持关键字 **#define #if #ifndef #elseif #elif #endif**
	* **#elseif**和**#elif**功能一样

```javascript
#define TEST
#if TEST
	print("1")
	#if TEST1
		print("6")
	#endif
	print("2")
#elseif TEST2
	print("3")
	#if TEST
		print("4")
		#if TEST
			print("6")
		#endif
	#endif
	print("5")
#endif
```
* 增加函数 push_search 增加一个 require 的目录
* 增加函数 push_define 增加一个宏定义
	* 注:此函数是运行时添加 #define 函数是编译时增加

(2016-4-7)
-----------
* 优化脚本array类型操作,示例 

```javascript
var a = []
a[10] = 10
var c = a[20]
////////////////赋值操作/////////////////
//修改前 a[10] 会直接报错越界 
//修改后 a 长度会自动扩充成 11 长度

////////////////取值操作/////////////////
//修改前 a[20] 会直接报错越界
//修改后 会返回null 但是 a 长度不会自动扩充
```
* 增加array库resize函数
* 修改c#类多次调用重载运算符 + - * / 会报错的BUG

(2016-3-24)
-----------
* 优化脚本执行性能,大约提升 10%-15%

(2016-3-21)
-----------
* 增加c#类去反射机制

(2016-3-9)
-----------
* c#类重载运算符,可以在脚本里直接使用 + - * / 

(2016-2-26)
-----------
* switch 语法 支持 case 变量 , case 暂时不支持return语法 只支持break

(2016-2-18)
-----------
* 修复UWP平台master配置下枚举类型会出错的BUG
* IScriptExtensions 改为 保存在 ScriptExtensions 类的 静态变量

(2016-1-27)
-----------
* string库增加split函数
* Delegate类型增加 + - 操作
* event类型增加 += -= 操作
* Delegate类型和event类型可是用 变量.Type 获取Delegate类型

(2016-1-6)
-----------
* 优化运算符,逻辑运算,赋值运算的逻辑
* 修改逻辑运算判断, 除了false和null,其他类型逻辑判断全部都是true

(2015-12-24)
-----------
* math库增加 sqrt 和 pow 函数
* 增加解析语法出错可以报出文件的功能

(2015-12-2)
-----------
* 修复foreach for循环堆栈内临时变量会变的BUG
* 优化部分错误提示

(2015-11-9)
-----------
* 实现UWP平台下阉割的功能
* Script添加IScriptExtensions(扩展函数)对象, 如果遇到某平台下不能使用的函数, 可以自行实现一个继承IScriptExtensions的对象

(2015-11-2)
-----------
* math库增加clamp函数

(2015-10-29)
-----------
* 修复table内基础(number string)变量当作函数参数传入后 然后通过自运算会影响 堆栈数据的BUG

(2015-10-28)
-----------
* 修复变长函数的参数 多次调用会覆盖参数的BUG
* 修复临时函数内不能修改父级堆栈变量的BUG

(2015-10-22)
-----------
* UWP(Universal Windows Platform) 应用去除Script.LoadFile函数,UWP平台下请自行实现

(2015-9-15)
-----------
* math库增加 floor 函数

(2015-9-14)
-----------
* 修复 functin 递归调用会导致数据错误的BUG

(2015-8-5)
-----------
* 增加ScriptNumberInt类 (可以通用 toint 函数转换成 int 类型 调用一些函数的时候 可以强制执行int类型时使用 默认传入int值还是会使用double表示  只有通用toint函数才能生成int类型)
* 基础库增加 toint is_int 函数
* 修复 long 类型 在脚本里面使用 [-]执行负值的时候 会转换成double类型的BUG

(2015-8-1)
-----------
* 适配Windows通用平台UWP(Universal Windows Platform)

(2015-7-14)
-----------
* 修复三目运算符条件判断优先级问题

(2015-7-10)
-----------
* table库增加 keys values 函数

(2015-7-7)
-----------
* array库增加 sort 函数
* 修复table array类型相等比较会报错的BUG

(2015-7-1)
-----------
* 修复某些特殊情况下 function(){} 这种类似 lambda 表达式的作用域以及值问题
* 修改bool类型 跟其他不是bool类型相等比较一律返回false

(2015-6-29)
-----------
* array库增加 popfirst safepopfirst poplast safepoplast 函数

(2015-6-27)
-----------
* ScorpioMaker转成二进制文件 第一个字节写入一个null 用来区别字符串文件
* Script类LoadFile函数支持直接载入二进制文件

(2015-6-9)
-----------
* 修复 continue 导致跳出循环的BUG

(2015-6-2)
-----------
* 修复 function(){} 这种类似 lambda 表达式的作用域以及值问题

(2015-5-26)
-----------
* ScorpioMaker工具修复 Deserialize 函数行数会多一行的问题
* 修复ScorpioMaker.Deserialize关键字null读取错误的问题
* 增加toenum函数

(2015-5-5)
-----------
* ScorpioMaker工具修复 普通字符串 "\n" 会当作回车处理的问题
* 修复三目运算符的优先级问题

(2015-4-10)
-----------
* ScorpioMaker工具修复 @"" @'' 字符串的支持

(2015-4-9)
-----------
* 修改解析文本[回车]判断 由原来的[\r\n]修改为[\n]
* 完善ScorpioMaker工具 可以由文本sco文件转换为二进制文件 也可以由二进制文件转换回文本sco文件

(2015-4-7)
-----------
* 修复返回function类型 父区域的值会变化的BUG 例如:

```javascript
function test(data) { 
	return function() {
		print(data)
	}
}
var b = test(100)
b()
test(200)
b()
/*
(修改前)上述代码会输出(b的data值会随test函数调用改变)
100
200
(修改后)上述代码会输出(b的data值不会随test函数调用改变)
100
100
*/
```
(2015-4-1)
-----------
* string库增加indexof lastindexof startswith endswith contains函数
* 修改运行时发生异常 错误输出会加上 文件行信息 例如:

```javascript
	print(null.a)
	//修改前报错会输出 类型[Null]不支持获取变量
	//修改后报错会输出 test.sco:1 : 类型[Null]不支持获取变量
```
(2015-3-31)
-----------
* 增加function类型内部变量 例如:

```javascript
function test() { print(str) }
test.str = "hello world"
/*上述代码会输出(str就相当于 test函数的变量)
hello world 
*/
test = function() { print(str) }
test.str = "hello world"
/*上述代码会输出(str就相当于 test函数的变量)
hello world 
*/
```
(2015-3-7)
-----------
* 增加c#委托和脚本function类型无缝切换 例如:

```csharp
    public delegate void Action();
    public class Test {
        public static void Func(Action action);
    }
```

```javascript
    //修改前代码要写成这样:
    Test.Func(Action( function() { } ) )
    //修改后可以去掉Action 程序会自动检测并转换
    Test.Func(function() {} )
```
(2015-3-6)
-----------
* array库增加 safepop 函数(如果array长度小于0默认返回null)
* string库增加 isnullorempty 函数
* Script类增加 ClearStackInfo 函数
* 修复某些语法情况下出错报不出堆栈的问题
* 修复相同名字相同参数类型函数泛型和非泛型判断错误的问题 例如(修改前):

```csharp
    public class Test {
        public static void Func<T>(int args) {}
        public static void Func(int args) {}
    }
//如果在脚本里面调用 Test.Func(100) 按顺序查找会先找到泛型函数 从而调用Func函数失败
//注:泛型函数声明在非泛型函数之后不会有问题
```
(2015-3-5)
-----------
* array库增加 pop 函数
* 修复循环continue会导致跳出循环的BUG (多谢[**过期**,**丶守望灬稻田**]同学提供反馈)
* 修复相同常量自运算的问题 例如(修改前) (多谢[**过期**]同学提供反馈):

```javascript
    var a = []
    for (var i=0;i<2;++i)
        array.add(a, 0)
    a[0]++
    for (var i=0;i<array.count(a);++i)
        print(i + " , " + a[i])
    /*上面代码会输出
    0 , 1
    1 , 1
    */
```
(2015-3-4)
-----------
* array库增加 remove removeat contains indexof lastindexof 函数
* table库增加 remove containskey clear 函数
* 全局函数增加 is_null is_bool is_number is_double is_long is_string is_function is_array is_table is_enum is_userdata函数
* 全局函数type函数 返回值由枚举Scorpio.ObjectType改为int型
* 增加单句执行语法  例如(修改后):

```javascript
    if (true) { 
        print("hello world ")
    }
    //上面的代码可以写成
    if (true)
        print("hello world")
	/*注:如果是 没有返回值的return
		if(true) return
		请在return后面加上[;] 否则会解析失败
		if(true) return;
	*/
```
* 修复调用c#变长参数的函数 某些情况判断错误的问题
* 修复()内区域变量[!][-]修饰符会失效的BUG 例如(修改前)(多谢[**he110world**]同学提供反馈): 

```javascript
    print((-1)) 
    //上面代码会输出 1
```

(2015-2-11)
-----------
* 增加调用c#函数 找不到合适函数的错误输出
* 修复[%]运算解析错误的问题
* 修复 for while循环 return 后没有跳出循环的BUG

(2014-12-17)
-----------
* 增加16进制表达式 16进制表达式会保存成long型 示例：print(0xffff)
* 增加位运算(| & ^ >> <<) 位运算只支持long型  示例：var a = 0L print(a |= 1)
* 增加单引号字符串声明 示例 print('hello world')
* 增加json库 json.encode  json.decode
* Script类增加LoadTokens函数
* 增加require函数 可以加载一个文件 搜索目录为 _G["searchpath"]
* 增加generic_method函数 可以声明泛型函数 示例： 

```csharp
    //c#代码
    public class Test {  
        public static T Func<T>() {  
            return default(T);  
        }  
    }  
```

```javascript
    //sco代码
    var func = generic_method(import_type("Test").Func, import_type("System.Int32"))  
    print(func())  
```
* 发布ScorpioMaker工具 可以把脚本编译成二进制数据
* 修改table类型Key值 可以为任意变量
* 修改string类型可以用 []表达式 获取指定位置的字符
* 修改 解析Array类型 最后一个值加逗号会解析失败的问题
* 修复 负值常量 多次运行 值会修改的BUG
* 增加新增功能的示例
* 发布ScorpioMessage项目 可以热更新网络协议 传送门 https://github.com/qingfeng346/ScorpioMessage

(2014-11-25)
-----------
* 增加声明泛型类的函数 示例： ListInt 就相当于c#的List<int>

```javascript
    List = import_type("System.Collections.Generic.List`1")  
    ListInt = generic_type(List, import_type("System.Int32"))   
```
* 大幅优化与c#交互效率 具体测试结果请参考 (https://github.com/qingfeng346/ScriptTestor)

(2014-11-14)
-----------
* 适配Unity3d WebGL (Unity5.0.0b1测试通过 WebGL示例地址 http://yunpan.cn/cAVkfYdGbgFug  提取码 2df5)
* 修复Unity下Delegate动态委托出错的BUG
* 修复赋值操作（如 = += -= 等）出错报不出堆栈的问题
* 优化数字和字符串的执行效率
* 同步发布Scorpio-Java 地址:https://github.com/qingfeng346/Scorpio-Java

(2014-11-4)
-----------
* 增加table声明语法  支持 Key 用 数字和字符串声明 示例：

```javascript
    var a = { 
        1 = 1, 
        "a" = a, 
        b = b
    }
```
* 增加elseif语法 现支持三种 elseif,elif,else if 都可以当作 else if 语法
* 修改 不同类型之间 做 ==  != 比较报错的问题  改成  不同类型之间==比较 直接返回false
* 增加switch语句 只支持 number和string 并且 case 只支持常量 不支持变量
* 支持try catch 语法 示例： 

```javascript
    try { 
        throw "error" 
    } catch (e) { 
        print(e) 
    }
```
* 支持脚本调用 c# 变长参数(params) 的函数
* 增加 switch trycatch import_type 示例

(2014-10-27)
-----------
* 增加赋值操作返回值  示例: 

```javascript
    if ((a = true) == true) { }
    var a = 100
    if ((a += 100) == 200) { }
```
* 修复对Unity3d Windows Phone 8 版本的兼容问题  （亲测支持wp版本）

(2014-10-18)
-----------
* 增加对Delegate动态委托的支持 示例：

```csharp
    //c#代码
    namespace Scropio {  
        public class Hello {  
            public delegate void Test(int a, int b);  
            public static Test t;  
        }  
    }  
```

```javascript
    //sco代码
    function test(a,b) {   
        print(a)  
        print(b)  
    }  
    Hello.t = Hello.Test(test)  
    Hello.t(100,200)
``` 

(2014-10-13)
-----------
* 修复已知BUG
* 增加对不定参的支持 示例：(args会传入一个Array)

```javascript
    function hello(a,...args) { }  
```
* 增加 eval函数 可以动态执行一段代码
* 删除对ulong类型的支持
* 增加基础for循环 for(i=begin,finished(包含此值),step) 示例：  

```csharp
    for (i=0,10000) {  
    }  
    for (i=0,10000,2) {  
    }
```
* 统一scriptobject产出函数 方便以后加入对象池
* 增加Unity例子 (亲测支持pc web android ios wp(需要修改一些基础函数调用,应该不影响功能使用,稍后会发布一个版本支持wp))