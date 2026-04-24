## 26.4.24
1、调研HybridCLR热更模块；
2、为游戏逻辑添加一个asmdef文件，用于热更处理；
3、

## 26.4.23
1、改进：改进LuBan的模板，使用Lazy模式，只在需要时才会加载数据表；
2、新增：新增LuBan的组件功能，在二进制数据加载完毕后，对它进行初始化处理；
    访问方式：GameEntry.LuBan.Tables.Tbbullets.Get(1001);

## 26.4.22
1、改进LuBan升级二进制文件的合并逻辑，避免合并后原始文件被删除。（点击LuBan > Build Combined Binary）可自动执行LuBan编译流程，然后再将其合并到一个二进制文件中。
2、新增LuBanFuture加载器，实现自定义的LuBan的资源包加载支持；
    - 通过AssetsManager.Instance.LoadLuBan("Assets/AllConfigs.bytes")一次性加载所有LuBan数据资源；
    - 内部会自动解析LuBan数据，生成对应的表格数据结构；二进制结构为（名字|数据长度|数据）
3、新增SpriteAtlas资源加载的支持；
    - 新增GetSprite方法，用于获得SpriteAtlas中的Sprite资源；
    - 新增GetSprites方法，用于获得SpriteAtlas中的多个Sprite资源；
4、为子弹实现数据读取逻辑，支持不同的ID，渲染对应的子弹形象；
5、新增子弹发射控制器，支持不同的子弹ID，实现数据的发射间隔；

## 26.4.20
1、新增：为`FairyUIFormLogic`组件添加了一个自动绑定字段的方法，用于自动绑定FairyGUI的字段，比如布局里存在一个btn_start按钮，自动绑定到对应逻辑类的`btn_start`字段上；
2、新增：新增【敌人】死亡加分事件逻辑；
3、学习：学习如何使用Luban，命令行生成JSON数据以及代码，熟悉Luban的表格数据结构；

## 26.4.17
1、调研了摄像机Base/Overlay配置处理相关的内容，重叠渲染处理；
2、新增FairyGUIFuture加载器，实现自定义的FairyGUI的资源包加载支持；          AssetsManager.Instance.LoadFairyUI("Assets/FGUI/Package1");
3、实现FairyGUI的初始化流程处理；
4、测试了界面展示：
GObject startView = UIPackage.CreateObject("Package1", "StartView");
GRoot.inst.AddChild(startView);
（明天调研一下如何通过GameEntry.UI.OpenUIForm打开FairyGUI的界面）

## 26.4.12~26.4.16
1. 修复：修复【底层加载】的资源完成、失败流程，避免游戏资源发生阻塞的问题；
2. 调研：调研【飞机】的资源动画处理，学习如何使用ShowEntity进行游戏逻辑构造处理；
    - 学习如何使用ShowEntity进行游戏逻辑构造处理，包括动画播放、事件触发等；
    - 通过AssetsManager加载所有资源，进入场景进行游戏；
    - 学习如何使用像素摄像头支持；
    - 学习如何控制飞机；
    - 学习anim动画以及其动画控制器，来控制飞机的左倾斜、右倾以及待机等基础动画；
    - 学习Unity的镜头坐标转换处理，像素坐标与世界空间坐标之间的关系；（用来处理飞机的边界、子弹、怪物出界处理）
    - 实现背景无限循环处理；
    - 实现子弹、敌人基础类，之间的战斗逻辑实现，并实现子弹的击中效果、敌机爆炸效果；
    - 实现敌人的动画播放处理；
    - 学习如何使用z控制层级，避免显示对象发生闪烁问题；
    - 学习如何使用Procedure进行流程控制；（开始界面 跳转 战斗界面）
    - 学习如何使用Canvas制作UI界面（开始界面）；
3. 新增：为`EntityComponent`组件添加了一个ShowEntity返回ID值的方法，ID会自动递增，用于唯一标识每个实体；

# 26.4.11
1. 改进：改进BytesFuture，支持加载YooAssets的字节资源；
2. 新增：新增Texture2DData，用于管理YooAssets加载的Texture2D资源。例如使用YooAssets加载的资源则通过AssetsHanlder进行释放资源，常规加载的Texture2D则使用Object.Destroy进行释放资源；
3. 新增：新增AssetsManager的Dispose方法，用于释放所有加载的资源；

（上述方法无法支持资源数量统计，脱离了GameFramework的资源管理机制，下述尝试更改GameFramework支持YooAssets的资源管理机制）
4. 框架结构变更：删除GameFramework.dll，替换为源码式GameFramework支持；
    - 需要添加GameFramework.asmdef文件；
    - 需要在对应的asmdef中添加GameFramework.asmdef的引用；
    - 需要删除GameFramework.dll文件；
    - 需要保留ICSharpCode.SharpZipLib.dll文件；
5. 变更【LoadResourceAgent】，支持加载YooAssets的资源；
6. 新增【ResourceObject】，用于管理YooAssets加载的handle资源，管理handle释放；
7. 支持【GameEntity.Resource】加载YooAssets的资源；