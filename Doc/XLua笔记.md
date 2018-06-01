##XLua
###1.Lua文件加载
建议的加载Lua脚本方式是：整个程序就一个DoString("require 'main'")，然后在main.lua加载其它脚本（类似lua脚本的命令行执行：lua main.lua）。
###2.C#访问Lua
####2.1 访问全局变量
	luaEvent.Global.Get<T>("变量名")
####2.2 访问全局table
#####by value 值传递
1. 映射到class/struct 		需要定义定义相应字段的类 自动创建实例
2. 映射到List/Dictionary  	无需定义类 更加轻量级

#####by ref 引用传递
1. 映射到interface	自动生成代码 创建实例 **推荐使用这种方法** 
2. 映射到luaTable	无需生成代码 但速度比映射到interface慢一个数量级

####2.3 访问全局function
1. 映射到delegate 	**（推荐方法）** 
- 性能高效 类型安全 但需要生成代码	
- 多返回值从左到右映射到C#的输出参数（返回值/out 参数/ref 参数）
2. 映射到LuaFunction 	
- 无需生成代码 但效率低 类型不安全
- 使用Call方法调用，可传入任意参数，返回一个object数组

###3.Lua访问C#
#### 创建C#对象
    local tmepGameObject = CS.UnityEngine.GameObject("name")
####访问C#静态属性，方法
	local Time = CS.UnityEngine.Time
	print(Time.deltaTime)
	Time.timeScale = 0.5

	local GameObject = CS.UnityEngine.GameObject
	temp = GameObject.Find("temp")
####访问C#成员属性，方法
- xlua支持（通过派生类）访问基类的静态属性，静态方法，（通过派生类实例）访问基类的成员属性，成员方法 
	