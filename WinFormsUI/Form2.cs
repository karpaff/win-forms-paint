using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsUI
{
    public partial class Form2 : Form
    {
        public int LineThickness { get { return (int)numericUpDown1.Value; } }
        public int PointThickness { get { return (int)numericUpDown2.Value; } }
        public Color LineColor { get { return GetSelectedColor(comboBox1); } }
        public Color PointColor { get { return GetSelectedColor(comboBox2); } }

        public Form2()
        {
            InitializeComponent();
        }

        private void buttonApply_Click(object sender, EventArgs e)
        {
            // Здесь происходит применение изменений
            DialogResult = DialogResult.OK; // Устанавливаем DialogResult в OK, чтобы указать, что изменения были применены
            Close(); // Закрываем форму после применения изменений
        }

        private Color GetSelectedColor(ComboBox combox)
        {
            string? selectedColorName = combox.SelectedItem as string;

            if (!string.IsNullOrEmpty(selectedColorName))
            {
                return Color.FromName(selectedColorName);
            }

            // По умолчанию возвращаем черный цвет или другое значение по умолчанию
            return Color.Black;
        }
    }
}
