# ModuleCatalogの構成

ModuleCatalogは、これまでModuleCatalogのAPIを使って構成してきました。
ここでは、それに加えてApp.configで公正を行う方法について紹介します。

## App.configによる構成

App.configによるModuleCatalogを構成するには、以下のようにApp.cinfigファイルを編集します。

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <configSections>
    <section name="modules" type="Prism.Modularity.ModulesConfigurationSection, Prism.Wpf" />
  </configSections>
  <startup>
    <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5.2" />
  </startup>
  <modules>
    <module assemblyFile="ModuleA.dll" moduleType="ModuleA.ModuleAModule, ModuleA, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" moduleName="ModuleAModule" startupLoaded="True" />
    <module assemblyFile="ModuleB.dll" moduleType="ModuleB.ModuleBModule, ModuleB, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" moduleName="ModuleBModule" startupLoaded="True">
      <dependencies>
        <dependency moduleName="ModuleAModule" />
      </dependencies>
    </module>
    <module assemblyFile="ModuleC.dll" moduleType="ModuleC.ModuleCModule, ModuleC, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null" moduleName="ModuleCModule" startupLoaded="False" />
  </modules>
</configuration>
```

sectionタグでmodulesタグを定義しています。
modulesタグにはmoduleタグを定義できて、このタグにassemblyFileでモジュールのあるアセンブリ名、moduleTypeでモジュールの型名、moduleNameでモジュールの名前、startupLoadedで起動時にモジュールを読み込むかどうか指定します。

モジュールが別モジュールに依存している場合はmoduleタグ内にdependenciesタグを書いて、その中にdependencyタグでmoduleNameを使って依存するモジュールを定義します。
上記例では、ModuleBModuleがModuleAModuleに依存しているようにしています。

## App.configを読み込むようにする

デフォルトでは、Prism.WpfのBootstrapperはApp.configから構成を読み込みません。
この動作を変えるにはBootstrapperクラスのCreateModuleCatalogメソッドをオーバーライドしてConfigurationModuleCatalogを返すようにします。
こうすることで、App.configに定義したモジュールの構成を読み込むようになります。

```cs
using Microsoft.Practices.Unity;
using Prism.Unity;
using ConfigureModuleCatalogApp.Views;
using System.Windows;
using Prism.Modularity;

namespace ConfigureModuleCatalogApp
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ConfigurationModuleCatalog();
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}

```

