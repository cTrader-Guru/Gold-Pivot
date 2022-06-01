/*  CTRADER GURU --> Indicator Template 1.0.6

    Homepage    : https://ctrader.guru/
    Telegram    : https://t.me/ctraderguru
    Twitter     : https://twitter.com/cTraderGURU/
    Facebook    : https://www.facebook.com/ctrader.guru/
    YouTube     : https://www.youtube.com/channel/UCKkgbw09Fifj65W5t5lHeCQ
    GitHub      : https://github.com/ctrader-guru

*/

using System;
using cAlgo.API;

namespace cAlgo
{

    [Indicator(IsOverlay = true, TimeZone = TimeZones.UTC, AccessRights = AccessRights.None)]
    public class GoldPivot : Indicator
    {

        #region Enums


        public enum MyColors
        {

            AliceBlue,
            AntiqueWhite,
            Aqua,
            Aquamarine,
            Azure,
            Beige,
            Bisque,
            Black,
            BlanchedAlmond,
            Blue,
            BlueViolet,
            Brown,
            BurlyWood,
            CadetBlue,
            Chartreuse,
            Chocolate,
            Coral,
            CornflowerBlue,
            Cornsilk,
            Crimson,
            Cyan,
            DarkBlue,
            DarkCyan,
            DarkGoldenrod,
            DarkGray,
            DarkGreen,
            DarkKhaki,
            DarkMagenta,
            DarkOliveGreen,
            DarkOrange,
            DarkOrchid,
            DarkRed,
            DarkSalmon,
            DarkSeaGreen,
            DarkSlateBlue,
            DarkSlateGray,
            DarkTurquoise,
            DarkViolet,
            DeepPink,
            DeepSkyBlue,
            DimGray,
            DodgerBlue,
            Firebrick,
            FloralWhite,
            ForestGreen,
            Fuchsia,
            Gainsboro,
            GhostWhite,
            Gold,
            Goldenrod,
            Gray,
            Green,
            GreenYellow,
            Honeydew,
            HotPink,
            IndianRed,
            Indigo,
            Ivory,
            Khaki,
            Lavender,
            LavenderBlush,
            LawnGreen,
            LemonChiffon,
            LightBlue,
            LightCoral,
            LightCyan,
            LightGoldenrodYellow,
            LightGray,
            LightGreen,
            LightPink,
            LightSalmon,
            LightSeaGreen,
            LightSkyBlue,
            LightSlateGray,
            LightSteelBlue,
            LightYellow,
            Lime,
            LimeGreen,
            Linen,
            Magenta,
            Maroon,
            MediumAquamarine,
            MediumBlue,
            MediumOrchid,
            MediumPurple,
            MediumSeaGreen,
            MediumSlateBlue,
            MediumSpringGreen,
            MediumTurquoise,
            MediumVioletRed,
            MidnightBlue,
            MintCream,
            MistyRose,
            Moccasin,
            NavajoWhite,
            Navy,
            OldLace,
            Olive,
            OliveDrab,
            Orange,
            OrangeRed,
            Orchid,
            PaleGoldenrod,
            PaleGreen,
            PaleTurquoise,
            PaleVioletRed,
            PapayaWhip,
            PeachPuff,
            Peru,
            Pink,
            Plum,
            PowderBlue,
            Purple,
            Red,
            RosyBrown,
            RoyalBlue,
            SaddleBrown,
            Salmon,
            SandyBrown,
            SeaGreen,
            SeaShell,
            Sienna,
            Silver,
            SkyBlue,
            SlateBlue,
            SlateGray,
            Snow,
            SpringGreen,
            SteelBlue,
            Tan,
            Teal,
            Thistle,
            Tomato,
            Transparent,
            Turquoise,
            Violet,
            Wheat,
            White,
            WhiteSmoke,
            Yellow,
            YellowGreen

        }

        #endregion

        #region Identity

        public const string NAME = "Gold Pivot";

        public const string VERSION = "1.0.0";

        #endregion

        #region Params

        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://www.google.com/search?q=ctrader+guru+gold+pivot")]
        public string ProductInfo { get; set; }

        [Parameter("Time Frame", Group = "Params", DefaultValue = 8, Step = 1)]
        public TimeFrame CandleTimeFrame { get; set; }

        [Parameter("Candle Show", Group = "Params", DefaultValue = 1, MinValue = 1, MaxValue = 50, Step = 1)]
        public int CandleShow { get; set; }

        [Parameter("Show Label ?", Group = "Params", DefaultValue = true)]
        public bool ShowLabel { get; set; }

        [Parameter("Line Style", Group = "Styles", DefaultValue = LineStyle.DotsRare)]
        public LineStyle LineStyleBox { get; set; }

        [Parameter("Tickness", Group = "Styles", DefaultValue = 1, MaxValue = 5, MinValue = 1, Step = 1)]
        public int TicknessBox { get; set; }

        [Parameter("Pivot Color", Group = "Styles", DefaultValue = MyColors.Gray)]
        public MyColors ColorPivot { get; set; }

        [Parameter("Bullish Color", Group = "Styles", DefaultValue = MyColors.DodgerBlue)]
        public MyColors ColorBull { get; set; }

        [Parameter("Bearish Color", Group = "Styles", DefaultValue = MyColors.Red)]
        public MyColors ColorBear { get; set; }

        #endregion

        #region Indicator Events

        protected override void Initialize()
        {

            Print("{0} : {1}", NAME, VERSION);

            if (CanDraw() && TimeFrame >= CandleTimeFrame)
                Chart.DrawStaticText("Alert", string.Format("{0} : USE THIS INDICATOR ON TIMEFRAME LOWER {1}", NAME.ToUpper(), CandleTimeFrame.ToString().ToUpper()), VerticalAlignment.Center, HorizontalAlignment.Center, Color.Red);

        }

        public override void Calculate(int index)
        {

            if (TimeFrame >= CandleTimeFrame)
                return;

            DrawPivot();


        }

        #endregion

        #region Private Methods

        private bool CanDraw()
        {

            return RunningMode == RunningMode.RealTime || RunningMode == RunningMode.VisualBacktesting;

        }

        private int GetTimeFrameCandleInMinutes(TimeFrame MyCandle)
        {

            if (MyCandle == TimeFrame.Daily)
                return 60 * 24;
            if (MyCandle == TimeFrame.Day2)
                return 60 * 24 * 2;
            if (MyCandle == TimeFrame.Day3)
                return 60 * 24 * 3;
            if (MyCandle == TimeFrame.Hour)
                return 60;
            if (MyCandle == TimeFrame.Hour12)
                return 60 * 12;
            if (MyCandle == TimeFrame.Hour2)
                return 60 * 2;
            if (MyCandle == TimeFrame.Hour3)
                return 60 * 3;
            if (MyCandle == TimeFrame.Hour4)
                return 60 * 4;
            if (MyCandle == TimeFrame.Hour6)
                return 60 * 6;
            if (MyCandle == TimeFrame.Hour8)
                return 60 * 8;
            if (MyCandle == TimeFrame.Minute)
                return 1;
            if (MyCandle == TimeFrame.Minute10)
                return 10;
            if (MyCandle == TimeFrame.Minute15)
                return 15;
            if (MyCandle == TimeFrame.Minute2)
                return 2;
            if (MyCandle == TimeFrame.Minute20)
                return 20;
            if (MyCandle == TimeFrame.Minute3)
                return 3;
            if (MyCandle == TimeFrame.Minute30)
                return 30;
            if (MyCandle == TimeFrame.Minute4)
                return 4;
            if (MyCandle == TimeFrame.Minute45)
                return 45;
            if (MyCandle == TimeFrame.Minute5)
                return 5;
            if (MyCandle == TimeFrame.Minute6)
                return 6;
            if (MyCandle == TimeFrame.Minute7)
                return 7;
            if (MyCandle == TimeFrame.Minute8)
                return 8;
            if (MyCandle == TimeFrame.Minute9)
                return 9;
            if (MyCandle == TimeFrame.Monthly)
                return 60 * 24 * 30;
            if (MyCandle == TimeFrame.Weekly)
                return 60 * 24 * 7;

            return 0;

        }

        private void DrawPivot()
        {

            Bars barsCustom = MarketData.GetBars(CandleTimeFrame);
            int minimumBars = 3;

            int index = barsCustom.Count - 1;

            if (index < CandleShow || index < minimumBars)
                return;

            for (int i = 0; i < CandleShow; i++)
            {

                try
                {

                    DateTime thisCandle = barsCustom[index - i].OpenTime;
                    DateTime nextCandle = (i == 0) ? thisCandle.AddMinutes(GetTimeFrameCandleInMinutes(CandleTimeFrame)) : barsCustom[index - i + 1].OpenTime;

                    double pivotRange = Math.Round(barsCustom[index - i - 1].High - barsCustom[index - i - 1].Low, Symbol.Digits);

                    bool lastIsBearish = barsCustom[index - i - 1].Close < barsCustom[index - i - 1].Open;

                    double pivot618 = Math.Round((pivotRange / 100) * 61.8, Symbol.Digits);

                    double goldPivot = (lastIsBearish) ? barsCustom[index - i - 1].Low + pivot618 : barsCustom[index - i - 1].High - pivot618;
                    double lastLevel = (lastIsBearish) ? barsCustom[index - i - 1].Low + pivotRange : barsCustom[index - i - 1].High - pivotRange;
                    double oppositeLastLevel = (!lastIsBearish) ? barsCustom[index - i - 1].Low + pivotRange : barsCustom[index - i - 1].High - pivotRange;

                    string rangeFlag = thisCandle.ToString();
                    Color lastLevelColor = (lastLevel < goldPivot) ? Color.FromName(ColorBear.ToString("G")) : Color.FromName(ColorBull.ToString("G"));
                    Color oppositeLastLevelColor = (oppositeLastLevel < goldPivot) ? Color.FromName(ColorBear.ToString("G")) : Color.FromName(ColorBull.ToString("G"));

                    if (CanDraw())
                    {

                        Chart.DrawTrendLine("GoldPivot" + rangeFlag, thisCandle, goldPivot, nextCandle, goldPivot, Color.FromName(ColorPivot.ToString("G")), TicknessBox, LineStyleBox);
                        Chart.DrawTrendLine("LastLevel" + rangeFlag, thisCandle, lastLevel, nextCandle, lastLevel, lastLevelColor, TicknessBox, LineStyleBox);
                        Chart.DrawTrendLine("OppositeLastLevel" + rangeFlag, thisCandle, oppositeLastLevel, nextCandle, oppositeLastLevel, oppositeLastLevelColor, TicknessBox, LineStyleBox);

                        if (ShowLabel && i == 0)
                        {

                            double bullishLevel = (lastLevel > oppositeLastLevel) ? lastLevel : oppositeLastLevel;
                            double bearishLevel = (lastLevel < oppositeLastLevel) ? lastLevel : oppositeLastLevel;

                            double midBuyDistance = (bullishLevel - goldPivot) / 2;
                            double midSellDistance = (goldPivot - bearishLevel) / 2;

                            Chart.DrawText("BuyZone", string.Format("Buy Zone ( {0} )", CandleTimeFrame), nextCandle, goldPivot + midBuyDistance, Color.FromName(ColorBull.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;
                            Chart.DrawText("SellZone", string.Format("Sell Zone ( {0} )", CandleTimeFrame), nextCandle, goldPivot - midSellDistance, Color.FromName(ColorBear.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;

                            Chart.DrawText("Bullish", string.Format("Bullish ( {0} {1} )", bullishLevel, CandleTimeFrame), nextCandle, bullishLevel, Color.FromName(ColorBull.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;
                            Chart.DrawText("Bearish", string.Format("Bearish ( {0} {1} )", bearishLevel, CandleTimeFrame), nextCandle, bearishLevel, Color.FromName(ColorBear.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;
                            Chart.DrawText("Pivot", string.Format("Pivot ( {0} {1} )", goldPivot, CandleTimeFrame), nextCandle, goldPivot, Color.FromName(ColorPivot.ToString("G"))).VerticalAlignment = VerticalAlignment.Center;

                        }


                    }

                } catch
                {


                }

            }

        }

        #endregion

    }

}

