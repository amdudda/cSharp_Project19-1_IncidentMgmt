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
    public partial class frm_CustomerIncidents : Form
    {
        public frm_CustomerIncidents()
        {
            InitializeComponent();
        }

        private void frm_CustomerIncidents_Load(object sender, EventArgs e)
        {
            // set focus on cutomer ID field to prompt user to search
            // TODO - not focusing!
            txtCustomerIdSearch.Focus();
        }

        private void fillByCustomerIDToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                int custId = Convert.ToInt32(txtCustomerIdSearch.Text);
                this.customersTableAdapter.FillByCustomerID(this.techSupport_DataDataSet.Customers,
                    custId); 
                this.incidentsTableAdapter.Fill(this.techSupport_DataDataSet.Incidents);
                if (customersBindingSource.Count == 0)
                {
                    MessageBox.Show("No customers with that ID found.  Please try another number.",
                        "ID not found");
                    txtCustomerIdSearch.Focus();
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

        private void findByStateToolStripButton_Click(object sender, EventArgs e)
        {
            frmFindCustomer myfrm = new frmFindCustomer();
            int custByState = myfrm.selectCustomer();

            // only update the screen if a valid customer number is returned
            if (custByState != -1)
            {
                // update the search box display
                txtCustomerIdSearch.Text = custByState.ToString();
                // now display only the selected customer
                try
                {
                    int custId = custByState; //Convert.ToInt32(customerIDToolStripTextBox.Text);
                    this.customersTableAdapter.FillByCustomerID(this.techSupport_DataDataSet.Customers,
                        custId);
                    this.incidentsTableAdapter.Fill(this.techSupport_DataDataSet.Incidents);
                    if (customersBindingSource.Count == 0)
                    {
                        MessageBox.Show("No customers with that ID found.  Please try another number.",
                            "ID not found");
                        txtCustomerIdSearch.Focus();
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
        }

        private void txtCustomerIdSearch_Leave(object sender, EventArgs e)
        {
            Control mySearchBox = txtCustomerIdSearch.Control;
            //MessageBox.Show(mySearchBox.Text); //.ToString());
            // if input IS present and IS NOT a postive integer, get cranky.
            if (mySearchBox.Text != "" && !(Validator.IsPositiveInteger(mySearchBox, "Customer ID")))
            {
                mySearchBox.Focus();
            }
        }

    }
}
