# Dragon Board 410c Windows 10 IoT Core with 96 Board Starter Kit sample
Dragon Board 410c Windows 10 IoT Core with 96 Board Starter Kit サンプル

このレポジトリでは DragonBoard 410c で Linker メザニンカードと各種センサー類を使う UWP サンプルを公開しています。他のサンプルと異なり、
極力多くのセンサーを同時に利用するようにしています。またオンボード LED および　GPS も利用します。

### DragonBoard 410c 
<img src="https://www.96boards.org/product/ce/dragonboard410c/images/DragonBoard-UpdatedImages-front.png" width="200">

詳細は[こちら](https://www.96boards.org/product/dragonboard410c)から

### Linker Mezzanine card starter kit for 96board
<img src="http://static.chip1stop.com/img/product/LINS/800px-Arrow3874.JPG" width="200">

詳細は[こちら](http://linksprite.com/wiki/index.php5?title=Linker_Mezzanine_card_starter_kit_for_96board)から

### DragonBoard 410c のピン配置とメザニンカードのダイアグラム
<p>
<img src="https://az835927.vo.msecnd.net/sites/iot/Resources/images/PinMappings/DB_Pinout.png" width="300">
<img src="http://linksprite.com/wiki/images/c/c7/1-4.jpg" width="500">
</p>

上記情報から、メザニンボードの各種デジタルインターフェースと GPIO ピンの対応は以下のようになります。
- D1 -> 36
- D2 -> 13
- D3 -> 115
- D4 -> 24

## DragonBoard 410c への Windows 10 IoT Core インストール方法
[こちら](https://developer.microsoft.com/en-us/windows/iot/getstarted)の手順に沿ってインストールしてください。

## センサーの接続
このサンプルでは、以下の組み合わせでセンサーをつなぐ想定です。

- タッチセンサー -> D1
- LED -> D2
- ボタン -> D3
- 傾きセンサー -> D4
- 温度センサー -> ADC1
- 照度センサー -> ADC2

## 利用しているセンサー 
#### 出力
- LED: 赤 LED および二つのオンボード緑 LED

#### 入力: スイッチ
- タッチセンサー: センサーにタッチすると、各種 LED が光り、センサーの値が表示されます。
- ボタン: ボタンを押すと、各種 LED が光り、センサーの値が表示されます。
- 傾きセンサー: センサーを一方向に傾けると、各種 LED が光り、センサーの値が表示されます。

#### 入力: センサー
- 温度センサー: 温度を測定
- 照度センサー: 照度を測定
- オンボード GPS: 位置を測定

## Lisence
MIT
