# Prism.Windowsとは

Prism.Windowsは、Universal Windows Platformアプリ（UWPアプリ）向けのPrismになります。
UWPアプリを作成するために、よく作るようなクラスがある程度実装されていたり、XAML系プラットフォームでは鉄板の設計方法であるMVVMパターンを支援する機能が各種盛り込まれています。

Prism.Windowsを導入することで得られるメリットを以下に示します。

- MVVMの基本クラス
	- INotifyPropertyChangedの実装クラス(BindableBase)
	- ICommandの実装クラス(DelegateCommand)
	- 検証機能つきINotifyPropertyChangedの実装クラス(ValidatableBindableBase)
	- 画面遷移時のコールバックに対応したViewModelの基本クラス(ViewModelBase)
	- クラス間の疎結合なメッセージベースの連携機能（IEventAggregator）
- UWP固有の機能への対応
	- 戻る処理への対応
		- タイトルバーへの戻るボタンの表示・非表示
		- 戻るボタンへの対応
	- 中断処理への対応
		- 中断時に指定したViewModelのプロパティを保存する機能
		- ViewModel, Modelで使用可能な一時保存データ用のISessionStateServiceインターフェースの提供
		- 中断時に画面遷移履歴の保存と復帰時に画面遷移履歴の復元
	- 名前ベースの画面遷移処理
		- Views/xxxxPageという命名規約に従っていればxxxxという文字列の指定で画面遷移が可能
			- 命名規約のカスタマイズに対応
	- ViewとViewModelの自動紐づけ機能
		- Views/xxxxPageに対してViewModels/xxxxPageViewModelクラスという命名規約で自動的にDataContextにViewModelを設定する機能
			- 命名規約のカスタマイズに対応
- DIコンテナへの対応（Prism.Unity.Windows導入時）
	- Prismの各種サービスを提供するインターフェースを自動的にコンテナに登録する機能
		- これによりModelやViewModelからシームレスにPrismの機能をDIして使うことができるようになる
	- DIコンテナによるViewModelの生成機能
		- これによりViewModelでDIが使用可能になる

