using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Calculator
{
    public partial class FrmCalc : Form
    {
        bool Complete = false;
        object ans;

        const int ellipseSize = 50;

        public FrmCalc()
        {
            InitializeComponent();
        }

        #region Buttons

        private void BtnEquate_Click(object sender, EventArgs e)
        {
            //TODO - Special Method
            UpdateLine("");
            ClearInput();
            Calculate();
        }

        private void BtnMultiply_Click(object sender, EventArgs e)
        {
            //TODO - Special Method
            UpdateLine("*");
            ClearInput();
        }

        private void BtnSubtract_Click(object sender, EventArgs e)
        {
            //TODO - Special Method
            UpdateLine("-");
            ClearInput();
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            //TODO - Special Method
            UpdateLine("+");
            ClearInput();
        }

        private void BtnDivide_Click(object sender, EventArgs e)
        {
            //TODO - Special Method
            UpdateLine("/");
            ClearInput();
        }

        private void BtnInputDot_Click(object sender, EventArgs e)
        {
            //TODO - Special Method
            TbxInput.Text += ".";
        }

        private void BtnCancelThis_Click(object sender, EventArgs e)
        {
            ClearCalc();
            ClearInput();
        }

        private void BtnCancelCalc_Click(object sender, EventArgs e)
        {
            ClearInput();
        }

        private void BtnInput3_Click(object sender, EventArgs e)
        {
            Input(3);
        }

        private void BtnInput6_Click(object sender, EventArgs e)
        {
            Input(6);
        }

        private void BtnInput8_Click(object sender, EventArgs e)
        {
            Input(8);
        }

        private void BtnInput5_Click(object sender, EventArgs e)
        {
            Input(5);
        }

        private void BtnInput4_Click(object sender, EventArgs e)
        {
            Input(4);
        }

        private void BtnInput7_Click(object sender, EventArgs e)
        {
            Input(7);
        }

        private void BtnInput1_Click(object sender, EventArgs e)
        {
            Input(1);
        }

        private void BtnInput2_Click(object sender, EventArgs e)
        {
            Input(2);
        }

        private void BtnInput0_Click(object sender, EventArgs e)
        {
            Input(0);
        }

        private void BtnInput9_Click(object sender, EventArgs e)
        {
            Input(9);
        }

        #endregion

        #region Operations
        
        private void FrmCalc_Load(object sender, EventArgs e)
        {
            CheckState();
        }

        private void TbxInput_TextChanged(object sender, EventArgs e)
        {
            TbxInput.Focus();
            TbxInput.Select(TbxInput.Text.Count(), 0);
        }

        private void LBHist_DoubleClick(object sender, EventArgs e)
        {
            TbxInput.Text = Convert.ToString(LbHist.SelectedItem);
        }

        private void TbxInput_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!PnlVenn.Visible)
            {
                e.Handled = (char.IsLetter(e.KeyChar));
            }

            if (e.KeyChar == '+')
            {
                e.Handled = true;
                BtnAdd.PerformClick();
            }
            else if (e.KeyChar == '-')
            {
                e.Handled = true;
                BtnSubtract.PerformClick();
            }
            else if (e.KeyChar == '*')
            {
                e.Handled = true;
                BtnMultiply.PerformClick();
            }
            else if (e.KeyChar == '/')
            {
                e.Handled = true;
                BtnDivide.PerformClick();
            }
            else if (e.KeyChar == '=' || e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                e.Handled = true;
                BtnEquate.PerformClick();
            }
        }

        private void Input(float val)
        {
            TbxInput.Text += val;
        }

        private void UpdateHist()
        {
            LbHist.Items.Add(TbxCalc.Text);
        }

        private void UpdateLine(string val)
        {
            if (TbxInput.TextLength > 0 && !Complete)
            {
                TbxCalc.Text += " " + TbxInput.Text;
            }
            else if (Complete == true)
            {
                ClearCalc();
                TbxCalc.Text = (string)ans;
                Complete = false;
            }


            TbxCalc.Text += " " + val;
        }

        private void ClearInput()
        {
            TbxInput.Text = string.Empty;
        }

        private void ClearCalc()
        {
            TbxCalc.Text = string.Empty;
        }

        private void Calculate()
        {
            if (!TsmiVennDiagram.Checked || !TbxCalc.Text.Any(c => char.IsUpper(c)))
            {
                var result = new DataTable().Compute(TbxCalc.Text, null);
                
                Complete = true;
                UpdateHist();

                TbxInput.Text = Convert.ToString(result);
                ans = TbxInput.Text;

                if (TsmiMultiples.Checked)
                {
                    FindMultiples();
                }

                if (TsmiFactorise.Checked)
                {
                    Factorise();
                }
                
                TbxCalc.Text = string.Empty;
            }
            else
            {
                UpdateHist();
                ResetView();
            }


        }

        private void CheckState()
        {
            this.Height = 390;
            PnlDispay.Height = 70;
            this.Width = 235;
            if (LbHist.Visible)
            {
                TsmiShowHistory.Checked = true;
                this.Height += LbHist.Height;
                PnlDispay.Height += LbHist.Height;
            }
            else
            {
                TsmiShowHistory.Checked = false;
            }

            if (PnlMultiply.Visible)
            {
                TsmiMultiples.Checked = true;
                this.Height += PnlMultiply.Height;
            }
            else
            {
                TsmiMultiples.Checked = false;
            }

            if (PnlFactorise.Visible)
            {
                TsmiFactorise.Checked = true;
                this.Height += PnlFactorise.Height;
            }
            else
            {
                TsmiFactorise.Checked = false;
            }

            if (PnlVenn.Visible)
            {
                TsmiVennDiagram.Checked = true;
                this.Width += 490;
            }
        }

        private void ResetView()
        {
            //this.Hide();
            this.PnlVennView.Refresh();
            //this.Show();
        }

        private void FindMultiples()
        {
            int highest = 0;
            for (int i = 1; i < 11; i++)
            {
                var res = new DataTable().Compute("(" + Convert.ToString(TbxCalc.Text) + ") * " + i, null);

                string value = ans + " * " + i + " = " + Convert.ToString(res);
                LbMultiply.Items.Add(value);

                if (value.Length > highest)
                {
                    highest = value.Length;
                }
            }

            LbMultiply.ColumnWidth = highest * 6;
        }

        private void Factorise()
        {
            int highest = 0;
            for (int i = 1; i < 11; i++)
            {
                var res = new DataTable().Compute("(" + Convert.ToString(TbxCalc.Text) + ") / " + i, null);

                string value = ans + " / " + i + " = " + Convert.ToString(res);
                LbFactorise.Items.Add(value);
                
                if (value.Length > highest)
                {
                    highest = value.Length;
                }
            }
            LbFactorise.ColumnWidth = highest * 6;
        }

        #endregion

        #region Menu Items

        private void showHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LbHist.Visible = TsmiShowHistory.Checked;
            CheckState();
        }

        private void clearHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LbHist.Items.Clear();
        }

        private void TsmiMultiples_Click(object sender, EventArgs e)
        {
            PnlMultiply.Visible = TsmiMultiples.Checked;
            CheckState();
        }

        private void TsmiFactorise_Click(object sender, EventArgs e)
        {
            PnlFactorise.Visible = TsmiFactorise.Checked;
            CheckState();
        }

        private void TsmiVennDiagram_Click(object sender, EventArgs e)
        {
            PnlVenn.Visible = TsmiVennDiagram.Checked;
            CheckState();
        }
        #endregion

        #region Venn Diagram
        
        private void PnlVennView_Paint(object sender, PaintEventArgs e)
        {
            int paintWidth = PnlVennView.ClientRectangle.Size.Width;
            int paintHeight = PnlVennView.ClientRectangle.Size.Height;

            Graphics g = e.Graphics;

            string value = TbxCalc.Text;
            
            int count = 0;
            for (int i = 0; i < value.Length; i++)
            {
                if (char.IsUpper(value[i]) && !string.IsNullOrWhiteSpace(value))
                {
                    count++;
                    g.DrawEllipse(new Pen(Color.Black),
                        (paintWidth / 3) + (count * (((ellipseSize / 2) / 2) * 3)), (paintHeight / 3), ellipseSize, ellipseSize);
                    g.DrawString(Convert.ToString(value[i]), new Font("Segoe UI", 10), new SolidBrush(Color.Black),
                        (paintWidth / 3) + (count * (((ellipseSize / 2) / 2) * 3)) + (ellipseSize / 2) - Font.Size, (paintHeight / 3) + (ellipseSize / 2) - Font.Size);
                }
            }
        }

        #endregion

    }
}
