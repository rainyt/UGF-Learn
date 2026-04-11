# 26.4.11
1. 改进：改进BytesFuture，支持加载YooAssets的字节资源；
2. 新增：新增Texture2DData，用于管理YooAssets加载的Texture2D资源。例如使用YooAssets加载的资源则通过AssetsHanlder进行释放资源，常规加载的Texture2D则使用Object.Destroy进行释放资源。