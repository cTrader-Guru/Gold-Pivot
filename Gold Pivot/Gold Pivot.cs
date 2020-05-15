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

        // --> Eventuali enumeratori li mettiamo qui

        #endregion

        #region Identity

        /// <summary>
        /// Nome del prodotto, identificativo, da modificare con il nome della propria creazione
        /// </summary>
        public const string NAME = "Gold Pivot";

        /// <summary>
        /// La versione del prodotto, progressivo, utilie per controllare gli aggiornamenti se viene reso disponibile sul sito ctrader.guru
        /// </summary>
        public const string VERSION = "1.0.0";

        #endregion

        #region Params

        /// <summary>
        /// Identità del prodotto nel contesto di ctrader.guru
        /// </summary>
        [Parameter(NAME + " " + VERSION, Group = "Identity", DefaultValue = "https://ctrader.guru/product/gold-pivot/")]
        public string ProductInfo { get; set; }

        /// <summary>
        /// Il numero di giorni da visualizzare
        /// </summary>
        [Parameter("Time Frame", Group = "Params", DefaultValue = 8, Step = 1)]
        public TimeFrame CandleTimeFrame { get; set; }

        /// <summary>
        /// Il numero di giorni da visualizzare
        /// </summary>
        [Parameter("Candle Show", Group = "Params", DefaultValue = 1, MinValue = 1, MaxValue = 50, Step = 1)]
        public int CandleShow { get; set; }

        /// <summary>
        /// La zona da identificare
        /// </summary>
        [Parameter("Show Label ?", Group = "Params", DefaultValue = true)]
        public bool ShowLabel { get; set; }

        /// <summary>
        /// Il Box, lo stile del bordo
        /// </summary>
        [Parameter("Line Style", Group = "Styles", DefaultValue = LineStyle.DotsRare)]
        public LineStyle LineStyleBox { get; set; }

        /// <summary>
        /// Il Box, lo spessore del bordo
        /// </summary>
        [Parameter("Tickness", Group = "Styles", DefaultValue = 1, MaxValue = 5, MinValue = 1, Step = 1)]
        public int TicknessBox { get; set; }

        /// <summary>
        /// Il Box, il colore del minimo
        /// </summary>
        [Parameter("Pivot Color", Group = "Styles", DefaultValue = "Gray")]
        public string ColorPivot { get; set; }

        /// <summary>
        /// Il Box, il colore del massimo
        /// </summary>
        [Parameter("Bullish Color", Group = "Styles", DefaultValue = "DodgerBlue")]
        public string ColorBull { get; set; }

        /// <summary>
        /// Il Box, il colore del minimo
        /// </summary>
        [Parameter("Bearish Color", Group = "Styles", DefaultValue = "Red")]
        public string ColorBear { get; set; }

        #endregion

        #region Property

        #endregion

        #region Indicator Events

        /// <summary>
        /// Viene generato all'avvio dell'indicatore, si inizializza l'indicatore
        /// </summary>
        protected override void Initialize()
        {

            // --> Stampo nei log la versione corrente
            Print("{0} : {1}", NAME, VERSION);

            // --> Se il timeframe è superiore o uguale al corrente devo uscire
            if (_canDraw() && TimeFrame >= CandleTimeFrame)
                Chart.DrawStaticText("Alert", string.Format("{0} : USE THIS INDICATOR ON TIMEFRAME LOWER {1}", NAME.ToUpper(), CandleTimeFrame.ToString().ToUpper()), VerticalAlignment.Center, HorizontalAlignment.Center, Color.Red);

        }

        /// <summary>
        /// Generato ad ogni tick, vengono effettuati i calcoli dell'indicatore
        /// </summary>
        /// <param name="index">L'indice della candela in elaborazione</param>
        public override void Calculate(int index)
        {

            // --> Non esiste ancora un metodo per rimuovere l'indicatore dal grafico, quindi ci limitiamo a uscire
            if (TimeFrame >= CandleTimeFrame)
                return;

            // --> Disegnamo il Pivot
            _drawPivot();


        }

        #endregion

        #region Private Methods

        private bool _canDraw()
        {

            return RunningMode == RunningMode.RealTime || RunningMode == RunningMode.VisualBacktesting;

        }

        private int _getTimeFrameCandleInMinutes(TimeFrame MyCandle)
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

        /// <summary>
        /// Parto dalle ultime candele personalizzate e le disegno ogni volta
        /// </summary>
        /// <param name="index"></param>
        private void _drawPivot()
        {

            // --> Prelevo le candele di riferimento
            Bars barsCustom = MarketData.GetBars(CandleTimeFrame);
            int minimumBars = 3;

            int index = barsCustom.Count - 1;

            // --> Potrei non avere un numero sufficiente di candele
            if (index < CandleShow || index < minimumBars)
                return;

            // --> eseguo un ciclo aretroso per disegnare le ultime candele
            for (int i = 0; i < CandleShow; i++)
            {

                // --> Il numero di candele da visualizzare potrebbero essere troppe
                try
                {

                    DateTime thisCandle = barsCustom[index - i].OpenTime;
                    DateTime nextCandle = (i == 0) ? thisCandle.AddMinutes(_getTimeFrameCandleInMinutes(CandleTimeFrame)) : barsCustom[index - i + 1].OpenTime;

                    // --> Prelevo il range della candela precedente, su di esso costruisco il pivot
                    double pivotRange = Math.Round(barsCustom[index - i - 1].High - barsCustom[index - i - 1].Low, Symbol.Digits);

                    // --> Devo sapere se bullish o bearish, bullish se flat
                    bool lastIsBearish = barsCustom[index - i - 1].Close < barsCustom[index - i - 1].Open;

                    double pivot618 = Math.Round((pivotRange / 100) * 61.8, Symbol.Digits);

                    double goldPivot = (lastIsBearish) ? barsCustom[index - i - 1].Low + pivot618 : barsCustom[index - i - 1].High - pivot618;
                    double lastLevel = (lastIsBearish) ? barsCustom[index - i - 1].Low + pivotRange : barsCustom[index - i - 1].High - pivotRange;
                    double oppositeLastLevel = (!lastIsBearish) ? barsCustom[index - i - 1].Low + pivotRange : barsCustom[index - i - 1].High - pivotRange;

                    string rangeFlag = thisCandle.ToString();
                    string lastLevelColor = (lastLevel < goldPivot) ? ColorBear : ColorBull;
                    string oppositeLastLevelColor = (oppositeLastLevel < goldPivot) ? ColorBear : ColorBull;

                    if (_canDraw()) {

                        Chart.DrawTrendLine("GoldPivot" + rangeFlag, thisCandle, goldPivot, nextCandle, goldPivot, Color.FromName(ColorPivot), TicknessBox, LineStyleBox);
                        Chart.DrawTrendLine("LastLevel" + rangeFlag, thisCandle, lastLevel, nextCandle, lastLevel, Color.FromName(lastLevelColor), TicknessBox, LineStyleBox);
                        Chart.DrawTrendLine("OppositeLastLevel" + rangeFlag, thisCandle, oppositeLastLevel, nextCandle, oppositeLastLevel, Color.FromName(oppositeLastLevelColor), TicknessBox, LineStyleBox);
                                                
                        if(ShowLabel && i == 0)
                        {

                            double bullishLevel = (lastLevel > oppositeLastLevel) ? lastLevel : oppositeLastLevel;
                            double bearishLevel = (lastLevel < oppositeLastLevel) ? lastLevel : oppositeLastLevel;

                            double midBuyDistance = (bullishLevel - goldPivot) / 2;
                            double midSellDistance = (goldPivot - bearishLevel) / 2;

                            Chart.DrawText("BuyZone", string.Format("Buy Zone ( {0} )", CandleTimeFrame), nextCandle, goldPivot + midBuyDistance, Color.FromName(ColorBull));
                            Chart.DrawText("SellZone", string.Format("Sell Zone ( {0} )", CandleTimeFrame), nextCandle, goldPivot - midSellDistance, Color.FromName(ColorBear));
                            
                            Chart.DrawText("Bullish", string.Format("Bullish ( {0} {1} )", bullishLevel, CandleTimeFrame), nextCandle, bullishLevel, Color.FromName(ColorBull));
                            Chart.DrawText("Bearish", string.Format("Bearish ( {0} {1} )", bearishLevel, CandleTimeFrame), nextCandle, bearishLevel, Color.FromName(ColorBear));
                            Chart.DrawText("Pivot", string.Format( "Pivot ( {0} {1} )", goldPivot, CandleTimeFrame ), nextCandle, goldPivot, Color.FromName(ColorPivot));

                        }
                        

                    }

                }
                catch
                {


                }

            }

        }

        #endregion

    }

}

