# Moduleの読み込み処理

PrismのModuleは読み込み順序を指定したり、必要になるまで読み込みを遅延させる機能があります。
起動時に、全Moduleを読み込むと起動が遅くなったり、あるModuleを読み込むまえに読み込んでおかなければならないModuleなどを適切に管理できます。

管理の仕方は、いくつかありますが一番簡単な方法はModuleの登録時に行う方法です。

```cs
// このModuleを読み込む前に読み込まないといけないModuleを指定しつつModuleを登録する
AddModule(Type moduleType, params string[] dependsOn)

// 初期化方法を指定してModuleを登録する
AddModule(Type moduleType, InitializingMode initializingMode, params string[] dependsOn)
```

例えばオンデマンドに読み込むBModuleと、CommonModuleに依存したAModuleを登録する場合は以下のようになります。

```cs
protected override void ConfigureModuleCatalog()
{
    base.ConfigureModuleCatalog();

    var c = (ModuleCatalog)this.ModuleCatalog;
    // ondemand
    c.AddModule(typeof(BModule), InitializationMode.OnDemand);
    // depend CommonModule
    c.AddModule(typeof(AModule), nameof(CommonModule));
    c.AddModule(typeof(CommonModule));
}
```

このようにすると、アプリケーション起動時にCommonModule → AModuleの順番に初期化されます。
BModuleは起動時には初期化されません。

BModuleを読み込むにはIModuleManagerを使用します。
IModuleManagerのLoadModuleで読み込むModule名を指定することで読み込ませることが出来ます。
BModuleを読み込ませるコード例は以下のようになります。


```cs
[Dependency]
public IModuleManager ModuleManager { get; set; }

private void ButtonLoadModuleB_Click(object sender, RoutedEventArgs e)
{
    this.ModuleManager.LoadModule("BModule");
}
```

