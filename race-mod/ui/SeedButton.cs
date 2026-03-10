using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace racemod.race_mod.ui
{
    public partial class SeedButton : Button
    {
        private Label seedLabel;
        public LineEdit input;
        public string currentSeed = "";

        public override void _Ready()
        {
            SetupButton();
            SetupSeedLabel();
        }

        private void SetupButton()
        {
            Name = "SeedButton";
            Text = "设定种子";

            // 按钮尺寸
            CustomMinimumSize = new Vector2(90, 36);

            // 左下角
            //AnchorLeft = 0;
            //AnchorBottom = 1;
            //OffsetLeft = 20;
            //OffsetBottom = -20;

            Position = new Vector2(100, 1000);

            // 无外框
            Flat = true;

            // 字体大小
            AddThemeFontSizeOverride("font_size", 36);

            // 默认黄色
            AddThemeColorOverride("font_color", new Color(1f, 0.85f, 0.2f));

            // Hover 绿色
            AddThemeColorOverride("font_hover_color", new Color(0.4f, 1f, 0.4f));

            // 按下
            AddThemeColorOverride("font_pressed_color", new Color(0.3f, 0.9f, 0.3f));

            // 黑色描边
            AddThemeConstantOverride("outline_size", 2);
            AddThemeColorOverride("font_outline_color", new Color(0, 0, 0));
            FocusMode = FocusModeEnum.None;
            Pressed += OnPressed;

            input = new LineEdit();
            input.PlaceholderText = "输入种子";
            AddChild(input);

            input.Position = new Vector2(0, -50);
            input.SetSize(new Vector2(200,50));
            input.AddThemeFontSizeOverride("font_size", 24);
            input.Hide();
        }

        private void SetupSeedLabel()
        {
            seedLabel = new Label();

            seedLabel.Text = "";
            seedLabel.Position = new Vector2(145, 6);

            seedLabel.AddThemeFontSizeOverride("font_size", 36);
            seedLabel.AddThemeColorOverride("font_color", new Color(0.4f, 0.7f, 1f));

            AddChild(seedLabel);
        }

        private void OnPressed()
        {
            if(!input.Visible)
                ShowSeedPopup();
            else
            {
                currentSeed = input.Text;
                seedLabel.Text = input.Text;

                input.Hide();
            }
        }

        private void ShowSeedPopup()
        {
            input.Show();

            if (!string.IsNullOrEmpty(currentSeed))
                input.Text = currentSeed;
            input.GrabFocus();


            input.TextSubmitted += (text) =>
            {
                currentSeed = text;
                seedLabel.Text = text;

                input.Hide();
            };
        }

    }
}
