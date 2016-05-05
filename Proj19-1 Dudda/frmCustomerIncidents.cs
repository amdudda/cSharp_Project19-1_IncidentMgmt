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
                txtCustomerIdSearch.Focus();
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
            // if input IS present and IS NOT a postive integer, get cranky.
            // notice I don't use Validator.IsPresent - I'm OK with users leaving this blank if they decide they're not searching for a new customer.
            if (mySearchBox.Text != "" && !(Validator.IsPositiveInteger(mySearchBox, "Customer ID")))
            {
                mySearchBox.Focus();
            }
        }

        private void btnAddIncident_Click(object sender, EventArgs e)
        {
            if (customerIDTextBox.Text == "")
            {
                // tell users they need to pick a customer to work with
                string msg = "Please select a customer to work with.  You can find them by their " +
                    "Customer ID, or browse for them by state.";
                string caption = "No Customer Selected";
                MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCustomerIdSearch.Focus();
            }
            else
            {
                // open a form to create a new incident
                frmAddIncident addIncident = new frmAddIncident();
                addIncident.create(customerIDTextBox.Text, nameTextBox.Text);
                // then refresh the dataset
                FillIncidents();
                    
            }
        }

        private void FillIncidents()
        {
            try
            {
                this.incidentsTableAdapter.Fill(this.techSupport_DataDataSet.Incidents);
                
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
}
