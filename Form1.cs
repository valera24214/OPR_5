using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace OPR_5
{
    public partial class Form1 : Form
    {
        List<int> it1 = new List<int>(2) { 2, 31 };
        List<int> it2 = new List<int>(2) { 3, 47 };
        List<int> it3 = new List<int>(2) { 1, 14 };

        int capacity = 0;
        public Form1()
        {
            InitializeComponent();
            DGV.ColumnCount = 2;
            DGV.RowCount = 3;
            DGV.Columns[0].HeaderCell.Value = "Wi";
            DGV.Columns[1].HeaderCell.Value = "Ri";
            for (int i = 1; i <= 2; i++)
            {
                DGV.Rows[0].Cells[i - 1].Value = it1[i - 1].ToString();
                DGV.Rows[1].Cells[i - 1].Value = it2[i - 1].ToString();
                DGV.Rows[2].Cells[i - 1].Value = it3[i - 1].ToString();
            }
            for (int i = 0; i <= 2; i++)
                DGV.Rows[i].HeaderCell.Value = (i + 1).ToString();
            DGV.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }
        private bool CheckData()
        {
            for (int i = 0; i < 3; i++)
                for (int j = 0; j < 2; j++)
                {
                    if (!double.TryParse(DGV.Rows[i].Cells[j].Value.ToString(), out _))
                    {
                        MessageBox.Show($"В ячейке [{i + 1};{j + 1}] содержится не число!", "Ошибка данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    if (Math.Sign(Convert.ToDouble(DGV.Rows[i].Cells[j].Value)) != 1)
                    {
                        MessageBox.Show($"В ячейке [{i + 1};{j + 1}] содержится не положительное число!", "Ошибка данных", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
            return true;
        }
        private void GetData(bool flag)
        {
            if (!CheckData()) return;
            if (!flag)
            {
                for (int i = 0; i < DGV.Columns.Count; i++)
                {
                    it1[i] = Convert.ToInt32(DGV.Rows[0].Cells[i].Value);
                    it2[i] = Convert.ToInt32(DGV.Rows[1].Cells[i].Value);
                    it3[i] = Convert.ToInt32(DGV.Rows[2].Cells[i].Value);
                }
            }
            else
            {
                for (int i = 0; i < DGV.Columns.Count; i++)
                {
                    it1[i] = Convert.ToInt32(DGV.Rows[2].Cells[i].Value);
                    it2[i] = Convert.ToInt32(DGV.Rows[1].Cells[i].Value);
                    it3[i] = Convert.ToInt32(DGV.Rows[0].Cells[i].Value);
                }
            }
            capacity = Convert.ToInt32(numericUpDown1.Value);
        }
        private void ClearAll()
        {
            DGVStep1_1.Rows.Clear();
            DGVStep1_2.Rows.Clear();
            DGVStep2_1.Rows.Clear();
            DGVStep2_2.Rows.Clear();
            DGVStep3_1.Rows.Clear();
            DGVStep3_2.Rows.Clear();
            DGVlast.Rows.Clear();
        }
        private void button1_Click(object sender, System.EventArgs e)
        {
            GetData(false);
            ClearAll();
            Step(it1, 1, DGVStep1_1, DGVStep1_2, DGVStep1_2, 1, 1, textBoxStep1);
            Step(it2, 2, DGVStep2_1, DGVStep2_2, DGVStep1_2, 0, 1, textBoxStep2);
            Step(it3, 3, DGVStep3_1, DGVStep3_2, DGVStep2_2, 0, 1, textBoxStep3);
            Finals(1);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            GetData(true);
            ClearAll();
            Step(it1, 3, DGVStep1_1, DGVStep1_2, DGVStep1_2, 1, 0, textBoxStep1);
            Step(it2, 2, DGVStep2_1, DGVStep2_2, DGVStep1_2, 0, 0, textBoxStep2);
            Step(it3, 1, DGVStep3_1, DGVStep3_2, DGVStep2_2, 0, 0, textBoxStep3);
            Finals(0);
        }
        private void Step(List<int> currentList,
            int num,
            DataGridView DGVbig,
            DataGridView DGVsmall,
            DataGridView DGVtoTake,
            int type,
            int direction,
            TextBox tb)
        {
            DGVbig.ColumnCount = (capacity / currentList[0]) + 1;
            DGVbig.RowCount = capacity + 1;
            DGVsmall.ColumnCount = 2;
            DGVsmall.RowCount = capacity + 1;

            for (int i = 0; i < DGVbig.ColumnCount; i++)
            {
                DGVbig.Columns[i].HeaderCell.Value = $"m{num}={i}";
                for (int j = 0; j <= capacity; j++)
                {
                    DGVbig.Rows[j].HeaderCell.Value = j.ToString();

                    if (Math.Sign(j - Convert.ToInt32(DGV.Rows[num - 1].Cells[0].Value) * i) == -1)
                        DGVbig.Rows[j].Cells[i].Value = "-";
                    else
                    {
                        if (type == 1)
                            DGVbig.Rows[j].Cells[i].Value = Convert.ToInt32(DGV.Rows[num - 1].Cells[1].Value) * i;
                        else
                            DGVbig.Rows[j].Cells[i].Value = Convert.ToInt32(DGV.Rows[num - 1].Cells[1].Value) * i + Convert.ToInt32(DGVtoTake.Rows[j - i * currentList[0]].Cells[0].Value);
                    }
                }
            }

            if (type == 1)
                tb.Text = $"f{num}(x{num})=max[{DGV.Rows[num - 1].Cells[1].Value}*m{num}*(Math.Sign(x{num}-{DGV.Rows[num - 1].Cells[0].Value}*m{num})], max[m{num}]=[{capacity}/{DGV.Rows[num - 1].Cells[0].Value}]={capacity / Convert.ToInt32(DGV.Rows[num - 1].Cells[0].Value)}";
            else
            {
                if (direction == 1)
                    tb.Text = $"f{num}(x{num})=max[{DGV.Rows[num - 1].Cells[1].Value}*m{num}+(f{num - 1}(x{num}-{DGV.Rows[num - 1].Cells[0].Value}*m{num})], max[m{num}]=[{capacity}/{DGV.Rows[num - 1].Cells[0].Value}]={capacity / Convert.ToInt32(DGV.Rows[num - 1].Cells[0].Value)}";
                else
                    tb.Text = $"f{num}(x{num})=max[{DGV.Rows[num - 1].Cells[1].Value}*m{num}+(f{num + 1}(x{num}-{DGV.Rows[num - 1].Cells[0].Value}*m{num})], max[m{num}]=[{capacity}/{DGV.Rows[num - 1].Cells[0].Value}]={capacity / Convert.ToInt32(DGV.Rows[num - 1].Cells[0].Value)}";
            }
            DGVsmall.Columns[0].HeaderCell.Value = $"f{num}(x{num})";
            DGVsmall.Columns[1].HeaderCell.Value = $"m{num}";

            List<int> tempList = new List<int>();
            for (int j = 0; j <= capacity; j++)
            {
                for (int i = 0; i < DGVbig.ColumnCount; i++)
                {
                    if (DGVbig.Rows[j].Cells[i].Value.ToString() != "-")
                        tempList.Add(Convert.ToInt32(DGVbig.Rows[j].Cells[i].Value));
                    DGVsmall.Rows[j].Cells[0].Value = tempList.Max();
                    DGVsmall.Rows[j].Cells[1].Value = tempList.IndexOf(tempList.Max());
                }
                tempList.Clear();
            }

            DGVbig.CurrentCell.Selected = false;
            DGVsmall.CurrentCell.Selected = false;
        }
        private void Finals(int type)
        {
            Application.DoEvents();
            DGVlast.ColumnCount = 3;
            DGVlast.RowCount = 4;

            DGVlast.Columns[0].HeaderCell.Value = "i";
            DGVlast.Columns[1].HeaderCell.Value = "Wi";
            DGVlast.Columns[2].HeaderCell.Value = "Ri";

            for (int i = 1; i < 4; i++)
            {
                DGVlast.Rows[i - 1].Cells[0].Value = i.ToString();
                DGVlast.Rows[3].Cells[i - 1].Style.BackColor = Color.LightGray;
            }
            DGVlast.Rows[3].Cells[0].Value = "Итого";

            List<DataGridView> listDGV = new List<DataGridView>() { DGVStep3_2, DGVStep2_2, DGVStep1_2 };
            int ind = 0, remainingSpace = capacity;

            if (type == 1) //прямой, да-да, не удивляйтесь с i
            {
                for (int i = 2, temp = 0; i >= 0; i--, temp++)
                {
                    for (int j = capacity; j >= 0; j--)
                    {
                        Application.DoEvents();
                        Thread.Sleep(200);
                        ind = 0;
                        listDGV[temp].Rows[j].Cells[0].Style.BackColor = Color.LightCoral;
                        listDGV[temp].Rows[j].Cells[1].Style.BackColor = Color.LightCoral;
                        if (Convert.ToInt32(listDGV[temp].Rows[j].Cells[1].Value) * Convert.ToInt32(DGV.Rows[i].Cells[0].Value) <= remainingSpace)
                        {
                            ind = Convert.ToInt32(listDGV[temp].Rows[j].Cells[1].Value);
                            listDGV[temp].Rows[j].Cells[0].Style.BackColor = Color.LightSkyBlue;
                            listDGV[temp].Rows[j].Cells[1].Style.BackColor = Color.LightSkyBlue;
                            break;
                        }
                    }
                    DGVlast.Rows[i].Cells[1].Value = $"{ind}*{Convert.ToInt32(DGV.Rows[i].Cells[0].Value)}={ind * Convert.ToInt32(DGV.Rows[i].Cells[0].Value)}";
                    DGVlast.Rows[i].Cells[2].Value = $"{ind}*{Convert.ToInt32(DGV.Rows[i].Cells[1].Value)}={ind * Convert.ToInt32(DGV.Rows[i].Cells[1].Value)}";
                    remainingSpace -= ind * Convert.ToInt32(DGV.Rows[i].Cells[0].Value);
                }
            }
            else //обратный
            {
                for (int i = 0; i < 3; i++)
                {
                    for (int j = capacity; j >= 0; j--)
                    {
                        Application.DoEvents();
                        Thread.Sleep(200);
                        ind = 0;
                        listDGV[i].Rows[j].Cells[0].Style.BackColor = Color.LightCoral;
                        listDGV[i].Rows[j].Cells[1].Style.BackColor = Color.LightCoral;
                        if (Convert.ToInt32(listDGV[i].Rows[j].Cells[1].Value) * Convert.ToInt32(DGV.Rows[i].Cells[0].Value) <= remainingSpace)
                        {
                            ind = Convert.ToInt32(listDGV[i].Rows[j].Cells[1].Value);
                            listDGV[i].Rows[j].Cells[0].Style.BackColor = Color.LightSkyBlue;
                            listDGV[i].Rows[j].Cells[1].Style.BackColor = Color.LightSkyBlue;
                            break;
                        }
                    }
                    DGVlast.Rows[i].Cells[1].Value = $"{ind}*{Convert.ToInt32(DGV.Rows[i].Cells[0].Value)}={ind * Convert.ToInt32(DGV.Rows[i].Cells[0].Value)}";
                    DGVlast.Rows[i].Cells[2].Value = $"{ind}*{Convert.ToInt32(DGV.Rows[i].Cells[1].Value)}={ind * Convert.ToInt32(DGV.Rows[i].Cells[1].Value)}";
                    remainingSpace -= ind * Convert.ToInt32(DGV.Rows[i].Cells[0].Value);
                }
            }

            int weight = 0, sum = 0;
            for (int i = 0; i < 3; i++)
            {
                string[] strs1 = DGVlast.Rows[i].Cells[1].Value.ToString().Split('=');
                string[] strs2 = DGVlast.Rows[i].Cells[2].Value.ToString().Split('=');
                weight += Convert.ToInt32(strs1[1]);
                sum += Convert.ToInt32(strs2[1]);
            }
            DGVlast.Rows[3].Cells[1].Value = weight;
            DGVlast.Rows[3].Cells[2].Value = sum;
        }
    }
}