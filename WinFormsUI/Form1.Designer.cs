namespace WinFormsUI
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            pointsButton = new Button();
            settingsButton = new Button();
            curveButton = new Button();
            polygonButton = new Button();
            bezierButton = new Button();
            closedCurveButton = new Button();
            moveModeButton = new Button();
            resetButton = new Button();
            drawModeButton = new Button();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Location = new Point(278, 41);
            panel1.Name = "panel1";
            panel1.Size = new Size(801, 627);
            panel1.TabIndex = 1;
            panel1.Paint += panel1_Paint;
            panel1.MouseClick += panel1_MouseClick;
            panel1.MouseDown += panel1_MouseDown;
            panel1.MouseMove += panel1_MouseMove;
            panel1.MouseUp += panel1_MouseUp;
            // 
            // pointsButton
            // 
            pointsButton.Location = new Point(26, 36);
            pointsButton.Name = "pointsButton";
            pointsButton.Size = new Size(207, 51);
            pointsButton.TabIndex = 2;
            pointsButton.TabStop = false;
            pointsButton.Text = "Точки";
            pointsButton.UseVisualStyleBackColor = true;
            pointsButton.Click += pointsButton_Click;
            // 
            // settingsButton
            // 
            settingsButton.Location = new Point(26, 104);
            settingsButton.Name = "settingsButton";
            settingsButton.Size = new Size(207, 51);
            settingsButton.TabIndex = 3;
            settingsButton.TabStop = false;
            settingsButton.Text = "Параметры";
            settingsButton.UseVisualStyleBackColor = true;
            settingsButton.Click += settingsButton_Click;
            // 
            // curveButton
            // 
            curveButton.Location = new Point(26, 174);
            curveButton.Name = "curveButton";
            curveButton.Size = new Size(207, 51);
            curveButton.TabIndex = 4;
            curveButton.TabStop = false;
            curveButton.Text = "Кривая";
            curveButton.UseVisualStyleBackColor = true;
            curveButton.Click += curveButton_Click;
            // 
            // polygonButton
            // 
            polygonButton.Location = new Point(26, 247);
            polygonButton.Name = "polygonButton";
            polygonButton.Size = new Size(207, 51);
            polygonButton.TabIndex = 5;
            polygonButton.TabStop = false;
            polygonButton.Text = "Ломанная";
            polygonButton.UseVisualStyleBackColor = true;
            polygonButton.Click += polygonButton_Click;
            // 
            // bezierButton
            // 
            bezierButton.Location = new Point(26, 318);
            bezierButton.Name = "bezierButton";
            bezierButton.Size = new Size(207, 51);
            bezierButton.TabIndex = 6;
            bezierButton.TabStop = false;
            bezierButton.Text = "Безье";
            bezierButton.UseVisualStyleBackColor = true;
            bezierButton.Click += bezierButton_Click;
            // 
            // closedCurveButton
            // 
            closedCurveButton.Location = new Point(26, 390);
            closedCurveButton.Name = "closedCurveButton";
            closedCurveButton.Size = new Size(207, 51);
            closedCurveButton.TabIndex = 7;
            closedCurveButton.TabStop = false;
            closedCurveButton.Text = "Заполненная";
            closedCurveButton.UseVisualStyleBackColor = true;
            closedCurveButton.Click += closedCurveButton_Click;
            // 
            // moveModeButton
            // 
            moveModeButton.Location = new Point(26, 464);
            moveModeButton.Name = "moveModeButton";
            moveModeButton.Size = new Size(207, 51);
            moveModeButton.TabIndex = 8;
            moveModeButton.TabStop = false;
            moveModeButton.Text = "Движение";
            moveModeButton.UseVisualStyleBackColor = true;
            moveModeButton.Click += moveModeButton_Click;
            // 
            // resetButton
            // 
            resetButton.Location = new Point(26, 617);
            resetButton.Name = "resetButton";
            resetButton.Size = new Size(207, 51);
            resetButton.TabIndex = 9;
            resetButton.TabStop = false;
            resetButton.Text = "Очистить";
            resetButton.UseVisualStyleBackColor = true;
            resetButton.Click += resetButton_Click;
            // 
            // drawModeButton
            // 
            drawModeButton.Location = new Point(26, 539);
            drawModeButton.Name = "drawModeButton";
            drawModeButton.Size = new Size(207, 51);
            drawModeButton.TabIndex = 10;
            drawModeButton.TabStop = false;
            drawModeButton.Text = "Рисование";
            drawModeButton.UseVisualStyleBackColor = true;
            drawModeButton.Click += drawingModeButton_Click;
            // 
            // Form1
            // 
            ClientSize = new Size(1174, 721);
            Controls.Add(drawModeButton);
            Controls.Add(resetButton);
            Controls.Add(moveModeButton);
            Controls.Add(closedCurveButton);
            Controls.Add(bezierButton);
            Controls.Add(polygonButton);
            Controls.Add(curveButton);
            Controls.Add(settingsButton);
            Controls.Add(pointsButton);
            Controls.Add(panel1);
            DoubleBuffered = true;
            KeyPreview = true;
            MaximumSize = new Size(1200, 900);
            MinimumSize = new Size(400, 300);
            Name = "Form1";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "2D графика";
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Button pointsButton;
        private Button settingsButton;
        private Button curveButton;
        private Button polygonButton;
        private Button bezierButton;
        private Button closedCurveButton;
        private Button moveModeButton;
        private Button resetButton;
        private Button drawModeButton;
    }
}
