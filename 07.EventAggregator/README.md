# モジュール間の通信をするEventAggregator

Prismで大規模なアプリケーションを組んでいくと、複数Moduleでアプリケーションを作ることがあります。
複数Moduleでアプリケーションを組んでいると、Module間でデータのやり取りなどを行いたいケースがおきます。
例えば、データの登録をするModuleがあって、結果を表示するModuleがあるとします。
そのとき、データの登録したら結果を表示するといったことはやりたくなると思います。
Moduleを分けると簡単にはデータのやり取りやイベントによる結合が行えなくなります。
そんなときに使えるのがIEventAggregatorになります。

IEventAggregatorは、名前の通りイベントの仲介者です。
PrismのUnityBootstrapperで作成されたIUnityContainer内には、シングルトンでIEventAggregatorのインスタンスが管理されています。
このIEventAggregatorをModule間で共有することでイベントのやり取りが疎結合に行えるようになります。

## IEventAggregatorの簡単な使い方

IEventAggregator自体は、T GetEvent<T>()というメソッドを持つだけのシンプルなインターフェースです。
GetEventの型引数で指定したイベントが同じ型の場合は同じインスタンスを返すようになっています。
GetEventの型引数には、普通に使う場合は、PubSubEvent<TPayload>型の派生クラスを使用します。
以下のようなクラスを定義します。

```cs
using Prism.Events;

namespace EventAggregatorSample.Infrastructure
{
    public class InputValueEvent : PubSubEvent<InputValue>
    {
    }

    public class InputValue
    {
        public int Value { get; set; }
    }
}
```

このクラスをキーにして、Module間でイベントのやり取りをします。

### イベントの発行

このイベントの発行の仕方は以下のようになります。

- UnityのコンテナからインジェクションしてもらったIEventAggregatorに対して、GetEventでイベントを取得
- PublishメソッドでInputValueを渡す

コードにすると以下のようになります。

```cs
using EventAggregatorSample.Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;

namespace EventAggregatorSample.Input.Models
{
    public class ValuePublisher : IValuePublisher
    {
        [Dependency]
        public IEventAggregator EventAggregator { get; set; }

        private Random Random { get; } = new Random();
        
        public void Publish()
        {
            this.EventAggregator
                .GetEvent<InputValueEvent>()
                .Publish(new InputValue { Value = this.Random.Next(100) });
        }        
    }
}
```

0～99までのランダムな数字を発行しています。

### イベントの購読

イベントの受信側は、以下のような流れになります。

- UnityのコンテナからインジェクションしてもらたIEventAggregatorに対して、GetEventでイベントを取得
- Subscribeメソッドでイベント受信時の処理を渡す。
	- オプションとして引数で、どのスレッドで実行するかを指定可能
- Subscribeメソッドの戻り値に対してDisposeメソッドを呼び出すことでイベントの受信を中止する

このとき注意する点は、Subscribeで渡したデリゲートは弱参照で管理されているという点です。
フィールドを参照してないようなラムダ式を渡すとGCなどのタイミングで処理が実行されなくなります。
フィールドを参照しているラムダ式か、インスタンスメソッドの参照を渡すようにしたほうが意図しないタイミングで
処理が実行されなくなったりといったことがなくて安全です。

先ほどのInputValueEventを購読するコードは以下のようになります。

```cs
using EventAggregatorSample.Infrastructure;
using Prism.Events;
using Prism.Mvvm;

namespace EventAggregatorSample.Output1.Models
{
    public class SingleValueProvider : BindableBase, ISingleValueProvider
    {
        private int value;

        public int Value
        {
            get { return this.value; }
            private set { this.SetProperty(ref this.value, value); }
        }

        public SingleValueProvider(IEventAggregator eventAggregator)
        {
            eventAggregator
                .GetEvent<InputValueEvent>()
                .Subscribe(x => this.Value = x.Value, ThreadOption.UIThread);
        }
    }
}
```

ThreadOptionでUIスレッド上で実行されるように指定しています。
この他にも、BackgroundThread（名前の通りバックグラウンドで実行する）やPublisherThread（名前のとおりイベント発行者と同じスレッドで実行する）が指定できます。

## サンプルプログラムの解説

このフォルダに格納されているサンプルプログラムは以下のようなプロジェクトで構成されています。

- EventAggregatorSampleApp
	- ShellとBootstrapperを持ったプロジェクト
	- ShellはInputRegionとOutputRegionに分かれてる
- EventAggregatorSample.Infrastructure
	- InputValueやInputValueEventなど、複数Module間で共有するクラスを定義している
- EventAggregatorSample.Input
	- InputValueEventを発行するModule
- EventAggregatorSample.Output1
	- InputValueEventを購読して値を表示するModule
- EventAggregatorSample.Output2
	- InputValueEventを購読して値の履歴を表示するModule
