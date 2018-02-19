/* Augusto Serrao
   Deepti Aggarwal
   Hartaj Dhillon
   Gagandeep Singh
   Shoaib Hassan
*/

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Timers;

namespace MassWeightConvertion
{
    class MassWeightConvertionVM : INotifyPropertyChanged
    {
        private const float GRAVITATIONAL_ACCELERATION = 9.8f;
        private const int WEIGHT_TOO_HEAVY_LIMIT = 1000;
        private const int WEIGHT_TOO_LIGHT_LIMIT = 10;

        private const string TOO_LIGHT_STRING_COLOR_DISABLE = "#600000FF";
        private const string TOO_LIGHT_STRING_COLOR_ENABLE = "#0000FF";
        private const string TOO_HEAVY_STRING_COLOR_DISABLE = "#50FF0000";
        private const string TOO_HEAVY_STRING_COLOR_ENABLE = "#FF0000";
        private const string GOOD_STRING_COLOR_DISABLE = "#5000FF00";
        private const string GOOD_STRING_COLOR_ENABLE = "#00FF00";

        private const string PROGRESS_BAR_BACKGROUND_NORMAL = "#FFFFFF";
        private const string PROGRESS_BAR_BACKGROUND_TOO_LIGHT = "#0000FF";
        private const string PROGRESS_BAR_BACKGROUND_TOO_HEAVY = "#FF0000";

        private const int STRINGS_FONT_SIZE_DISABLE = 15;
        private const int STRINGS_FONT_SIZE_ENABLE = 20;
        private const int PROGRESS_BAR_INIT_VALUE = 0;
        private const int PROGRESS_BAR_TOO_LIGHT_EFFECT_VALUE = 50;
        private const int PROGRESS_BAR_MAX_VALUE = 100;

        enum tooLight_stateMachine
        {
            TOO_LIGHT_MS_INCREASE_PG,
            TOO_LIGHT_MS_DECREASE_PG,
            TOO_LIGHT_MS_SHOW_MESSAGE
        }

        tooLight_stateMachine tooLightSM;

        Timer timer = new Timer(10);

        float massValue;
        float weightValue;
        string tooLightStringColor = TOO_LIGHT_STRING_COLOR_DISABLE;
        string tooHeavyStringColor = TOO_HEAVY_STRING_COLOR_DISABLE;
        string goodStringColor = GOOD_STRING_COLOR_DISABLE;
        int tooLightStringFontSize = STRINGS_FONT_SIZE_DISABLE;
        int tooHeavyStringFontSize = STRINGS_FONT_SIZE_DISABLE;
        int goodStringFontSize = STRINGS_FONT_SIZE_DISABLE;
        int progressBarValue = PROGRESS_BAR_INIT_VALUE;
        string progressBarBackground = PROGRESS_BAR_BACKGROUND_NORMAL;
        
        public float MassValue
        {
            get { return massValue; }
            set { massValue = value; propertyChanged(); }
        }

        public float WeightValue
        {
            get { return weightValue; }
            set { weightValue = value; propertyChanged(); }
        }

        public string TooLightStringColor
        {
            get { return tooLightStringColor; }
            set { tooLightStringColor = value; propertyChanged(); }
        }

        public string TooHeavyStringColor
        {
            get { return tooHeavyStringColor; }
            set { tooHeavyStringColor = value; propertyChanged(); }
        }

        public string GoodStringColor
        {
            get { return goodStringColor; }
            set { goodStringColor = value; propertyChanged(); }
        }

        public int TooLightStringFontSize
        {
            get { return tooLightStringFontSize; }
            set { tooLightStringFontSize = value; propertyChanged(); }
        }

        public int TooHeavyStringFontSize
        {
            get { return tooHeavyStringFontSize; }
            set { tooHeavyStringFontSize = value; propertyChanged(); }
        }

        public int GoodStringFontSize
        {
            get { return goodStringFontSize; }
            set { goodStringFontSize = value; propertyChanged(); }
        }

        public string ProgressBarBackground
        {
            get { return progressBarBackground; }
            set { progressBarBackground = value; propertyChanged(); }
        }

        public int ProgressBarValue
        {
            get { return progressBarValue; }
            set { progressBarValue = value; propertyChanged(); }
        }

        public void ConvertMassIntoWeight()
        {
            timer.AutoReset = true;
            
            // Reset current UI to change it again
            reset();

            WeightValue = GRAVITATIONAL_ACCELERATION * MassValue;

            if (WeightValue > WEIGHT_TOO_HEAVY_LIMIT)
            {
                timer.Elapsed += timerHandler_TooHeavy;
            }
            else if (WeightValue < WEIGHT_TOO_LIGHT_LIMIT)
            {
                timer.Elapsed += timerHandler_TooLight;
            }
            else
            {
                timer.Elapsed += timerHandler_Good;
            }

            timer.Start();
        }

        // Reset all parameters
        private void reset()
        {
            TooLightStringColor = TOO_LIGHT_STRING_COLOR_DISABLE;
            TooHeavyStringColor = TOO_HEAVY_STRING_COLOR_DISABLE;
            GoodStringColor = GOOD_STRING_COLOR_DISABLE;
            TooLightStringFontSize = STRINGS_FONT_SIZE_DISABLE;
            TooHeavyStringFontSize = STRINGS_FONT_SIZE_DISABLE;
            GoodStringFontSize = STRINGS_FONT_SIZE_DISABLE;
            ProgressBarValue = PROGRESS_BAR_INIT_VALUE;
            ProgressBarBackground = PROGRESS_BAR_BACKGROUND_NORMAL;

            // Reset timer handlers so that they don't overlap each other
            timer.Elapsed -= timerHandler_TooHeavy;
            timer.Elapsed -= timerHandler_TooLight;
            timer.Elapsed -= timerHandler_Good;
        }

        // Handler for timer when weight is too light. It has a state machine in order to provide the effect
        // of progress bar increasing, then decreasing, and finally changing font color and size and progress bar background
        private void timerHandler_TooLight(Object source, System.Timers.ElapsedEventArgs e)
        {
            switch (tooLightSM)
            {
                case tooLight_stateMachine.TOO_LIGHT_MS_INCREASE_PG:
                    if (ProgressBarValue < PROGRESS_BAR_TOO_LIGHT_EFFECT_VALUE)
                    {
                        ProgressBarValue += 2;
                    }
                    else
                    {
                        tooLightSM = tooLight_stateMachine.TOO_LIGHT_MS_DECREASE_PG;
                    }
                    break;
                case tooLight_stateMachine.TOO_LIGHT_MS_DECREASE_PG:
                    if (ProgressBarValue > PROGRESS_BAR_INIT_VALUE)
                    {
                        ProgressBarValue -= 2;
                    }
                    else   
                    {
                        tooLightSM = tooLight_stateMachine.TOO_LIGHT_MS_SHOW_MESSAGE;
                    }
                    break;
                case tooLight_stateMachine.TOO_LIGHT_MS_SHOW_MESSAGE:
                    TooLightStringColor = TOO_LIGHT_STRING_COLOR_ENABLE;
                    TooLightStringFontSize = STRINGS_FONT_SIZE_ENABLE;
                    ProgressBarBackground = PROGRESS_BAR_BACKGROUND_TOO_LIGHT;
                    tooLightSM = tooLight_stateMachine.TOO_LIGHT_MS_INCREASE_PG;
                    timer.Stop();
                    break;
                default:
                    break;
            }
        }

        // Handler for timer when weight is too heavy. It increases progress bar until end and changes colors and sizes
        private void timerHandler_TooHeavy(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (ProgressBarValue < PROGRESS_BAR_MAX_VALUE)
            {
                ProgressBarValue += 2;
            }
            else
            {
                ProgressBarValue = 0;
                ProgressBarBackground = PROGRESS_BAR_BACKGROUND_TOO_HEAVY;
                TooHeavyStringColor = TOO_HEAVY_STRING_COLOR_ENABLE;
                TooHeavyStringFontSize = STRINGS_FONT_SIZE_ENABLE;
                timer.Stop();
            }
        }

        // Handler for timer when weight is good. It increases progress bar until the right proportion and changes colors and sizes
        private void timerHandler_Good(Object source, System.Timers.ElapsedEventArgs e)
        {
            if (ProgressBarValue < ((WeightValue - WEIGHT_TOO_LIGHT_LIMIT) * 100) / (WEIGHT_TOO_HEAVY_LIMIT - WEIGHT_TOO_LIGHT_LIMIT))
            {
                ProgressBarValue += 2;
            }
            else
            {
                GoodStringColor = GOOD_STRING_COLOR_ENABLE;
                GoodStringFontSize = STRINGS_FONT_SIZE_ENABLE;
                timer.Stop();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void propertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
