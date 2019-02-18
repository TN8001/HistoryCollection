using System;
using System.Drawing;

namespace HistoryCollection
{
    // HistoryCollectionが本題なのだが System.Drawing.ColorTranslatorがいまいちすぎて。。。
    /// <summary><see cref="Color" />構造体と文字列の相互変換 アルファ値対応</summary>
    public class ColorTranslator
    {
        // 入力側は出来るだけ柔軟になるようにした
        /// <summary>カラー名や#F00等の文字列から<see cref="Color" />構造体を返します
        /// <para>#RGB #ARGB #RRGGBB #AARRGGBB</para></summary>
        /// <param name="htmlColor">カラー名や#F00等の文字列</param>
        /// <returns><see cref="Color" />構造体</returns>
        /// <exception cref="ArgumentNullException"><paramref name="htmlColor" />は<see langword="null" />です</exception>
        /// <exception cref="ArgumentException"><paramref name="htmlColor" />が不正です</exception>
        public static Color FromHtml(string htmlColor)
        {
            if(string.IsNullOrWhiteSpace(htmlColor)) throw new ArgumentNullException(nameof(htmlColor));

            try
            {
                if(!htmlColor.StartsWith("#")) // "Red"
                    return System.Drawing.ColorTranslator.FromHtml(htmlColor);

                var alpha = "";
                var color = "";

                switch(htmlColor.Length)
                {
                    case 9: // #88FF0000
                        alpha = $"0x{htmlColor.Substring(1, 2)}";
                        color = $"0x{htmlColor.Substring(3, 6)}";
                        break;
                    case 5: // #8F00
                        var s = htmlColor;
                        alpha = $"0x{s[1]}{s[1]}";
                        color = $"0x{s[2]}{s[2]}{s[3]}{s[3]}{s[4]}{s[4]}"; // 雑いｗ
                        break;
                    case 7: // #FF0000
                    case 4: // #F00
                        return System.Drawing.ColorTranslator.FromHtml(htmlColor);
                    default:
                        // System.Drawing.ColorTranslator.FromHtmlは
                        // #Fや#FFFFF等も通ってしまってわかりにくいので throw
                        throw new ArgumentException(nameof(htmlColor));
                }

                var a = Convert.ToInt32(alpha, 16);
                var c = Convert.ToInt32(color, 16);

                return Color.FromArgb(a, Color.FromArgb(c));
            }
            catch(Exception e)
            {
                throw new ArgumentException(nameof(htmlColor), e);
            }
        }

        // 出力側はあまりこだわらない
        /// <summary><see cref="Color" />構造体からカラー名または#FF0000等の文字列を返します</summary>
        /// <param name="color"><see cref="Color" />構造体</param>
        /// <returns>カラー名または#FF0000等の文字列</returns>
        public static string ToHtml(Color color)
        {
            // #88ff0000から#8f00への圧縮は省略
            return color.A == 255 ? System.Drawing.ColorTranslator.ToHtml(color)
                                  : $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
