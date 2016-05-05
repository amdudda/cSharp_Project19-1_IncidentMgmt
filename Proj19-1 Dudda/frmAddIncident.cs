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
    public partial class frmAddIncident : Form
    {
        public frmAddIncident()
        {
            InitializeComponent();
        }

        private void frmAddIncident_Load(object sender, EventArgs e)
        {
            
            try
            {// TODO: This line of code loads data into the 'techSupport_DataDataSet.Incidents' table. You can move, or remove it, as needed.
                // this.incidentsTableAdapter.Fill(this.techSupport_DataDataSet.Incidents);
                //  This line of code loads data into the 'techSupport_DataDataSet.Products' table. You can move, or remove it, as needed.
                //this.productsTableAdapter.Fill(this.techSupport_DataDataSet.Products);
                // the line below restricts the dropdown to ONLY registered products.  Client 1010 demonstrates we need to create incidents for products where a user cannot register it.
                 this.productsTableAdapter.FillByRegistrationsCustomerID(this.techSupport_DataDataSet.Products, Convert.ToInt32(txtCustomerID.Text));
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

        public void create(string myCustID, string myName)
        {
            this.txtCustomerID.Text = myCustID;
            this.txtCustomerName.Text = myName;
            this.ShowDialog();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (Validator.IsPresent(txtTitle, "Title") && Validator.IsPresent(txtDescription, "Description")) 
            {
                /* got help from https://msdn.microsoft.com/en-us/library/ms233812.aspx */
                // set up a new record to be added to the database
                TechSupport_DataDataSet.IncidentsRow newrow;
                newrow = techSupport_DataDataSet.Incidents.NewIncidentsRow();
                newrow.CustomerID = Convert.ToInt32(txtCustomerID.Text);
                newrow.ProductCode = cboProductCode.SelectedValue.ToString();
                newrow.Title = txtTitle.Text;
                newrow.Description = txtDescription.Text;
                newrow.DateOpened = DateTime.Now;
                // create the new record
                this.techSupport_DataDataSet.Incidents.Rows.Add(newrow);
                // and save it
                this.incidentsTableAdapter.Update(this.techSupport_DataDataSet.Incidents);
                this.Close();
            }

        }

        private void txtTitle_Leave(object sender, EventArgs e)
        {
            TextBox mybox = (TextBox) sender;
            // don't let user leave the field empty.
            if (!(Validator.IsPresent(mybox, mybox.Tag.ToString())))
            {
                mybox.Focus();
            }
        }

    }
}
