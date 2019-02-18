using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace HistoryCollection
{
    // 使用例 軽い気持ちでColorでやったら案外大掛かりに。。ｗ
    internal class Program
    {
        private class RecentColors : HistoryCollection<Color>
        {
            public RecentColors() { }
            public RecentColors(int limit) : base(limit) { }
            public RecentColors(string colors) : base(Split(colors)) { }
            public RecentColors(int limit, string colors) : base(limit, Split(colors)) { }

            public override string ToString()
                => string.Join(", ", this.Select(x => ColorTranslator.ToHtml(x)));

            // ユーザー編集が想定されるので不正な値は無視する
            private static IEnumerable<Color> Split(string colors)
            {
                if(colors == null) throw new ArgumentNullException(nameof(colors));

                return Core();

                IEnumerable<Color> Core()
                {
                    foreach(var s in colors.Split(','))
                    {
                        var c = GetColor(s.Trim());
                        if(c != null) yield return (Color)c;
                    }
                }
                Color? GetColor(string s)
                {
                    try
                    {
                        return ColorTranslator.FromHtml(s.Trim());
                    }
                    catch { }
                    return null;
                }
            }
        }


        private static void Main(string[] args)
        {
            // Inline Color Picker
            // https://marketplace.visualstudio.com/items?itemName=NikolaMSFT.InlineColorPicker
            // を入れるとカラフルな上 変更も簡単で超おすすめ
            // xaml用だと思ってたらどこでも効くっていう


            // 空で生成
            var c = new RecentColors();
            Console.WriteLine(c); // string.Empty

            // サイズ指定＆空で生成
            c = new RecentColors(2);
            Console.WriteLine(c); // string.Empty

            // 文字列から生成
            c = new RecentColors("Red, #00F");
            Console.WriteLine(c); // Red, #0000FF

            // サイズ指定＆文字列から生成
            c = new RecentColors(1, "Red, #00F");
            Console.WriteLine(c); // #0000FF

            // コレクション初期化子で生成  
            // 注)名前付きとHexは別扱い（Colorの等値判定に準ずる）
            c = new RecentColors
            {
                Color.Red,
                Color.Red,
                ColorTranslator.FromHtml("#f00"),
                ColorTranslator.FromHtml("#ff0000"),
            };
            Console.WriteLine(c); // Red, #FF0000

            // サイズ指定＆コレクション初期化子で生成
            c = new RecentColors(1)
            {
                Color.Red,
                ColorTranslator.FromHtml("#f00"),
            };
            Console.WriteLine(c); // #FF0000    


            var r = new RecentColors("#000, #F00, #0F0");
            Console.WriteLine(r); // #000000, #FF0000, #00FF00

            // 追加
            c.Add(ColorTranslator.FromHtml("#00F"));
            Console.WriteLine(r); // #000000, #FF0000, #00FF00, #0000FF

            // リミット変更
            r.Limit = 2;
            Console.WriteLine(r); // #00FF00, #0000FF

            // 同値の追加
            r.Add(ColorTranslator.FromHtml("#0F0"));
            Console.WriteLine(r); // #0000FF, #00FF00

            // 不正文字列はスルーする
            c = new RecentColors("Reed, #00F");
            Console.WriteLine(c); // #0000FF

            Console.ReadKey();
        }
    }
}
