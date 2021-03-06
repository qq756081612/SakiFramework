### 基于Unity3D Lua侧的UI框架搭建
- 先流水账的记录一下 方便以后整理  


#### 拟解决问题及方案
1. **Q：**多个场景会反复出现相同的“UI窗体”，造成多个场景中反复加载相同的UI窗体。  
**A：**“UI框架” 需要缓存项目（例游戏项目）中常用的“UI窗体"。使用对象池。关闭的页面不销毁，缓存在UIManager的字典中，用的时候先看池中有没有缓存，有的话直接显示,没有才加载。

2. **Q：**开发商业复杂项目时，各个UI(UI脚本)之间传值，容易出现“紧耦合”的情况，容易导致项目的“可复用性”降低。  
**A：**希望建立能和C#测共用的消息派发机制。

3. **Q：**卡牌、RPG等游戏类型项目，很多情况下会出现多个“弹出窗体” 叠加的现象，开发人员需要“手工”维护窗体中间的层级关系。
**A：**设计UI框架系统，使用“栈”的数据结构，保存与控制当前所有需要显示的“UI窗体”的层级关系。

#### 一些思路
1. 将UI分为固定/弹出/全屏三种分别做处理，弹出菜单需要靠栈维护层级关系，全屏菜单弹出时，隐藏掉其余所有UI
2. 国际化资源技术，字符串都采用读表的形式获取，将来翻译这个表即可
3. 采用专门的层级管理特效
4. 蒙版层所有弹出窗体共用 维护蒙版的层级 透明度状态等

#### 草稿
- 并在UIBaseView实例中增加控制变量，控制在什么状态销毁物体释放内存（如：不常用的页面定时销毁，切换场景销毁之类的）