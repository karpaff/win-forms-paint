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
            movementTimer.Interval = 20; // �������� ������� � �������������
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
                    // ����������� �������� �������� �����
                    IncreaseSpeed();
                    e.Handled = true;
                    break;
                case Keys.OemMinus:
                    // ��������� �������� �������� �����
                    DecreaseSpeed();
                    e.Handled = true;
                    break;
                case Keys.Escape:
                    // ������� ���������� �����
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
            // ������� ���������� ������ ����� �� �������� ����������
            for (int i = 0; i < pointsList.Count; i++)
            {
                pointsList[i] = new Point(pointsList[i].X + deltaX, pointsList[i].Y + deltaY);
            }

            if (curveApplied) { DrawCurve(); }
            if (polygonApplied) { DrawPolygon(); }
            if (bezierApplied) { DrawBezier(); }
            if (filledApplied) { DrawClosedCurve(); }

            // ��������� �����������
            panel1.Invalidate();
        }

        private void IncreaseSpeed()
        {
            // ����������� �������� �������� �����
            for (int i = 0; i < initialDirections.Count; i++)
            {
                initialDirections[i] = new Point(initialDirections[i].X * 2, initialDirections[i].Y * 2);
            }
        }

        private void DecreaseSpeed()
        {
            // ��������� �������� �������� �����
            for (int i = 0; i < initialDirections.Count; i++)
            {
                initialDirections[i] = new Point(initialDirections[i].X / 2, initialDirections[i].Y / 2);
            }
        }

        private void ClearForm()
        {
            // ������� ���������� �����
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

                // ��������� ����� ��� ���������� ������ Panel
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

            // ������� ��������� ����������� ��� ������ �����
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
            // ��������� ����� ���������
            drawingMode = false;
            drawModeButton.Text = "���������";
            panel1.Cursor = Cursors.Default; // ���������� ������� ������

            if (addPointsMode)
            {
                pointsList.Clear();
                MessageBox.Show("����� ���������� ����� �������. �������� ����� ��� ���������� �����.");
            }
            else
            {
                MessageBox.Show("����� ���������� ����� ��������.");
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


                    // ����� ��������� ����������, ������������� �������� ������� Paint ��� ���������� �������
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
            if (pointsList.Count >= 3) // ��� ���������� ������ ����� ������� 3 �����
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
            if (pointsList.Count >= 3) // ��� ���������� �������� ����� ������� 3 �����
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

            // ���������� ����� � ������� ������ ���� ������ ���� ���� 1, �������� 4, 7 ��� 10.
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
            if (pointsList.Count >= 3) // ��� ���������� ������ ����� ������� 3 �����
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
            // ������������� ����� �������� "���������" � "�������"
            drawingMode = !drawingMode;
            addPointsMode = false;

            if (drawingMode)
            {
                pointsList.Clear();
                initialDirections.Clear();
                panel1.Invalidate();
                // �������� ����� ���������
                drawModeButton.Text = "����";
                panel1.Cursor = Cursors.Cross; // ������ ������
            }
            else
            {
                // ��������� ����� ���������
                drawModeButton.Text = "���������";
                panel1.Cursor = Cursors.Default; // ���������� ������� ������
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
            // ������ ��� ������� �� ������ �����
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
            // ���� ������� ����� ���������� ����� � ��������� ������ ����
            if (addPointsMode && e.Button == MouseButtons.Left)
            {
                // ���������, ��������� �� ������ ���� ������ ��������� ������
                if (panel1.ClientRectangle.Contains(e.Location))
                {
                    pointsList.Add(e.Location);

                    // ������������� �������� ������� Paint ��� ���������� �������
                    panel1.Invalidate();
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // ��������� ����� �� panel1 ��� �������� �����
            panel1.Focus();
            this.ActiveControl = panel1;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            // ���������, ������ �� ����� ������ ����
            if (e.Button == MouseButtons.Left)
            {
                // ������� ��������� ����� � ����� ������
                foreach (Point point in pointsList)
                {
                    if (Math.Abs(e.X - point.X) <= 5 && Math.Abs(e.Y - point.Y) <= 5)
                    {
                        // �������� ��������������
                        isDragging = true;
                        startPoint = e.Location;
                        Cursor = Cursors.Hand; // ������ ������
                        break;
                    }
                }
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                // ������� ����� �� �������� ��������� ��������� ����
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

            // ���������, ������ �� ����� ������ ���� � ������� �� ����� ���������
            if (e.Button == MouseButtons.Left && drawingMode)
            {
                // ��������� ������� ����� � ����
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
