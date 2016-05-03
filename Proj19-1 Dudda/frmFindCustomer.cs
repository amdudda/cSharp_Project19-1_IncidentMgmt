using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proj19_1_Dudda
{
    public partial class frmFindCustomer : Form
    {
        public int customerID;  // eventually return this to main form

        public frmFindCustomer()
        {
            InitializeComponent();
        }

        private void customersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.customersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.techSupport_DataDataSet);

        }

        private void frmFindCustomer_Load(object sender, EventArgs e)
        {
            try 
            {
                // This line of code loads data into the 'techSupport_DataDataSet.Customers' table. You can move, or remove it, as needed.
                this.customersTableAdapter.Fill(this.techSupport_DataDataSet.Customers);
                txtState.Focus();
            }
            catch (SqlException sqle)
            {
                string msg = "Database error # " + sqle.Number + ":\n" + sqle.Message;
                string caption = sqle.GetType().ToString();
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                string state = txtState.Text;
                this.customersTableAdapter.FillByState(this.techSupport_DataDataSet.Customers, state);
                if (customersBindingSource.Count == 0)
                {
                    MessageBox.Show("No customers found in " + state.ToUpper() + ".  Please try another state.",
                        "No Customers Found");
                    txtState.Focus();
                }
            }
            catch (SqlException sqle)
            {
                string msg = "Database error # " + sqle.Number + ":\n" + sqle.Message;
                string caption = sqle.GetType().ToString();
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (System.Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }

        private void customersDataGridView_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        public int selectCustomer()
        {
            this.ShowDialog();
            return customerID;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(customerID.ToString());
            this.Close();
        }

        private void customersDataGridView_SelectionChanged(object sender, EventArgs e)
        {  
            if (customersDataGridView.SelectedRows.Count > 0)
            {
                string selCustomer = customersDataGridView.SelectedRows[0].Cells["custID"].Value.ToString();
                customerID = Convert.ToInt32(selCustomer);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            customerID = -1;  // an obviously fake number
            this.Close();
        }

        private void customersDataGridView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            // when a user double clicks on a record, return that value to the main form.
            string selCustomer = customersDataGridView.SelectedRows[0].Cells["custID"].Value.ToString();
            customerID = Convert.ToInt32(selCustomer);
            this.Close();
        }
    }
}
