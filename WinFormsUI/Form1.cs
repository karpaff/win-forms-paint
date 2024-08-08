using Timer = System.Windows.Forms.Timer;

namespace WinFormsUI
{
    public partial class Form1 : Form
    {
        private bool addPointsMode = false;
        private List<Point> pointsList = new List<Point>();

        int lineThickness = 10;
        int pointThickness = 10;
        Color lineColor = Color.Black;
        Color pointColor = Color.Red;

        private bool movingMode = false;
        private List<Point> initialDirections = new List<Point>();
        private Random random = new Random();
        private Timer movementTimer = new Timer();
        int moveDistance = 5;

        private bool drawingMode = false;
        private List<Point> drawingPath = new List<Point>();
        private Point startPoint;
        private bool isDragging = false;

        private bool curveApplied = false;
        private bool polygonApplied = false;
        private bool bezierApplied = false;
        private bool filledApplied = false;
        public Form1()
        {

            InitializeComponent();
            DoubleBufferedPanel panel1 = new DoubleBufferedPanel();
            movementTimer.Interval = 20; // Интервал таймера в миллисекундах
            movementTimer.Tick += movementTimer_Tick;

        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                case (Keys.Space):
                    movingMode = !movingMode;
                    if (movingMode) {
                        InitializeMovement();
                        movementTimer.Start();
                    }
                    else
                    {
                        movementTimer.Stop();
                    }
                    return true;
                case Keys.Up:
                    MoveFigure(0, -moveDistance);
                    return true;
                case Keys.Down:
                    MoveFigure(0, moveDistance);
                    return true;
                case Keys.Left:
                    MoveFigure(-moveDistance, 0);
                    return true;
                case Keys.Right:
                    MoveFigure(moveDistance, 0);
                    return true;;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Oemplus: 
                    // Увеличивает скорость движения точек
                    IncreaseSpeed();
                    e.Handled = true;
                    break;
                case Keys.OemMinus:
                    // Уменьшает скорость движения точек
                    DecreaseSpeed();
                    e.Handled = true;
                    break;
                case Keys.Escape:
                    // Очищает содержимое формы
                    ClearForm();
                    e.Handled = true;
                    break;
                default:
                    e.Handled = false;
                    break;
            }
        }

        private void MoveFigure(int deltaX, int deltaY)
        {
            // Смещаем координаты каждой точки на заданное расстояние
            for (int i = 0; i < pointsList.Count; i++)
            {
                pointsList[i] = new Point(pointsList[i].X + deltaX, pointsList[i].Y + deltaY);
            }

            if (curveApplied) { DrawCurve(); }
            if (polygonApplied) { DrawPolygon(); }
            if (bezierApplied) { DrawBezier(); }
            if (filledApplied) { DrawClosedCurve(); }

            // Обновляем отображение
            panel1.Invalidate();
        }

        private void IncreaseSpeed()
        {
            // Увеличивает скорость движения точек
            for (int i = 0; i < initialDirections.Count; i++)
            {
                initialDirections[i] = new Point(initialDirections[i].X * 2, initialDirections[i].Y * 2);
            }
        }

        private void DecreaseSpeed()
        {
            // Уменьшает скорость движения точек
            for (int i = 0; i < initialDirections.Count; i++)
            {
                initialDirections[i] = new Point(initialDirections[i].X / 2, initialDirections[i].Y / 2);
            }
        }

        private void ClearForm()
        {
            // Очищает содержимое формы
            pointsList.Clear();
            initialDirections.Clear();
            panel1.Invalidate();
        }

        private void movementTimer_Tick(object sender, EventArgs e)
        {
            MovePoints();
            panel1.Invalidate();
        }

        private void MovePoints()
        {
            for (int i = 0; i < pointsList.Count; i++)
            {
                pointsList[i] = new Point(pointsList[i].X + initialDirections[i].X, pointsList[i].Y + initialDirections[i].Y);

                // Отражение точек при достижении границ Panel
                if (pointsList[i].X < 0 || pointsList[i].X > panel1.Width)
                {
                    initialDirections[i] = new Point(-initialDirections[i].X, initialDirections[i].Y);
                }

                if (pointsList[i].Y < 0 || pointsList[i].Y > panel1.Height)
                {
                    initialDirections[i] = new Point(initialDirections[i].X, -initialDirections[i].Y);
                }
            }
        }

        private void InitializeMovement()
        {
            initialDirections.Clear();

            // Создаем начальные направления для каждой точки
            for (int i = 0; i < pointsList.Count; i++)
            {
                int randomSpeed = random.Next(1, 20);
                int directionX = random.Next(-randomSpeed, randomSpeed + 1);
                int directionY = random.Next(-randomSpeed, randomSpeed + 1);
                initialDirections.Add(new Point(directionX, directionY));
            }
        }

        private void pointsButton_Click(object sender, EventArgs e)
        {
            addPointsMode = !addPointsMode;
            // Отключаем режим рисования
            drawingMode = false;
            drawModeButton.Text = "Рисование";
            panel1.Cursor = Cursors.Default; // Возвращаем обычный курсор

            if (addPointsMode)
            {
                pointsList.Clear();
                MessageBox.Show("Режим добавления точек включен. Щелкните мышью для добавления точек.");
            }
            else
            {
                MessageBox.Show("Режим добавления точек выключен.");
            }
        }

        private void settingsButton_Click(object sender, EventArgs e)
        {
            using (Form2 optionsForm = new Form2())
            {
                if (optionsForm.ShowDialog() == DialogResult.OK)
                {
                    lineThickness = optionsForm.LineThickness;
                    pointThickness = optionsForm.PointThickness;

                    lineColor = optionsForm.LineColor;
                    pointColor = optionsForm.PointColor;


                    // После обработки параметров, принудительно вызываем событие Paint для обновления рисунка
                    panel1.Invalidate();
                }
            }
        }

        private void curveButton_Click(object sender, EventArgs e)
        {
            DrawCurve();
            curveApplied = true;
        }

        private void DrawCurve()
        {
            Pen pen = new Pen(color: lineColor, width: lineThickness);
            if (pointsList.Count >= 3) // Для построения кривой нужно минимум 3 точки
            {
                using (Graphics g = panel1.CreateGraphics())
                {
                    g.DrawClosedCurve(pen, pointsList.ToArray());
                }

            }
        }
        private void polygonButton_Click(object sender, EventArgs e)
        {
            DrawPolygon();
            polygonApplied = true;
        }
        private void DrawPolygon()
        {
            Pen pen = new Pen(color: lineColor, width: lineThickness);
            if (pointsList.Count >= 3) // Для построения полигона нужно минимум 3 точки
            {
                using (Graphics g = panel1.CreateGraphics())
                {
                    g.DrawPolygon(pen, pointsList.ToArray());
                }
            }
        }
        private void bezierButton_Click(object sender, EventArgs e)
        {
            DrawBezier();
            bezierApplied = true;
        }
        private void DrawBezier()
        {
            Pen pen = new Pen(color: lineColor, width: lineThickness);

            // Количество точек в массиве должно быть кратно трем плюс 1, например 4, 7 или 10.
            if ((pointsList.Count - 1) % 3 == 0)
            {
                using (Graphics g = panel1.CreateGraphics())
                {
                    g.DrawBeziers(pen, pointsList.ToArray());
                }
            }
        }
        private void closedCurveButton_Click(object sender, EventArgs e)
        {
            DrawClosedCurve();
            filledApplied = true;
        }

        private void DrawClosedCurve()
        {
            Brush brush = new SolidBrush(color: lineColor);
            if (pointsList.Count >= 3) // Для построения кривой нужно минимум 3 точки
            {
                using (Graphics g = panel1.CreateGraphics())
                {
                    g.FillClosedCurve(brush, pointsList.ToArray());
                }
            }
        }

        private void moveModeButton_Click(object sender, EventArgs e)
        {
            movingMode = !movingMode;

            if (movingMode)
            {
                InitializeMovement();
                movementTimer.Start();
            }
            else
            {
                movementTimer.Stop();
            }
        }

        private void drawingModeButton_Click(object sender, EventArgs e)
        {
            // Переключаемся между режимами "Рисование" и "Обычный"
            drawingMode = !drawingMode;
            addPointsMode = false;

            if (drawingMode)
            {
                pointsList.Clear();
                initialDirections.Clear();
                panel1.Invalidate();
                // Включаем режим рисования
                drawModeButton.Text = "Стоп";
                panel1.Cursor = Cursors.Cross; // Меняем курсор
            }
            else
            {
                // Отключаем режим рисования
                drawModeButton.Text = "Рисование";
                panel1.Cursor = Cursors.Default; // Возвращаем обычный курсор
            }
        }

        private void resetButton_Click(object sender, EventArgs e)
        {
            pointsList.Clear();
            initialDirections.Clear();
            drawingPath.Clear();
            panel1.Invalidate();
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Pen pen = new Pen(color: pointColor);
            Brush brush = new SolidBrush(color: pointColor);
            // Рисуем все эллипсы из списка точек
            if (addPointsMode)
            {
                using (Graphics g = e.Graphics)
                {
                    foreach (Point point in pointsList)
                    {
                        g.DrawEllipse(pen, point.X - pointThickness, point.Y - pointThickness, pointThickness, pointThickness);
                        g.FillEllipse(brush, point.X - pointThickness, point.Y - pointThickness, pointThickness, pointThickness);
                    }
                }
            }

            if (drawingMode && drawingPath.Count > 1)
            {
                e.Graphics.DrawLines(Pens.Black, drawingPath.ToArray());
            }
        }
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            // Если включен режим добавления точек и произошел щелчок мыши
            if (addPointsMode && e.Button == MouseButtons.Left)
            {
                // Проверяем, находится ли щелчок мыши внутри координат панели
                if (panel1.ClientRectangle.Contains(e.Location))
                {
                    pointsList.Add(e.Location);

                    // Принудительно вызываем событие Paint для обновления рисунка
                    panel1.Invalidate();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Установим фокус на panel1 при загрузке формы
            panel1.Focus();
            this.ActiveControl = panel1;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // Проверяем, нажата ли левая кнопка мыши
            if (e.Button == MouseButtons.Left)
            {
                // Находим ближайшую точку к месту щелчка
                foreach (Point point in pointsList)
                {
                    if (Math.Abs(e.X - point.X) <= 5 && Math.Abs(e.Y - point.Y) <= 5)
                    {
                        // Начинаем перетаскивание
                        isDragging = true;
                        startPoint = e.Location;
                        Cursor = Cursors.Hand; // Меняем курсор
                        break;
                    }
                }
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // Смещаем точку на величину изменения положения мыши
                for (int i = 0; i < pointsList.Count; i++)
                {
                    if (Math.Abs(startPoint.X - pointsList[i].X) <= 5 && Math.Abs(startPoint.Y - pointsList[i].Y) <= 5)
                    {
                        pointsList[i] = new Point(pointsList[i].X + (e.X - startPoint.X), pointsList[i].Y + (e.Y - startPoint.Y));
                        startPoint = e.Location;
                        panel1.Invalidate(); 
                        break;
                    }
                }
            }

            // Проверяем, зажата ли левая кнопка мыши и включен ли режим рисования
            if (e.Button == MouseButtons.Left && drawingMode)
            {
                // Добавляем текущую точку к пути
                drawingPath.Add(e.Location);
                panel1.Invalidate(); 
            }
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isDragging = false;
                Cursor = Cursors.Default; 
            }
        }

    }
    public class DoubleBufferedPanel : Panel
    {
        public DoubleBufferedPanel()
        {
            this.DoubleBuffered = true;
        }
    }

}
