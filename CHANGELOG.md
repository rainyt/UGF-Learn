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