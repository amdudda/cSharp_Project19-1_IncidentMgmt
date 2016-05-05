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
            // TODO: This line of code loads data into the 'techSupport_DataDataSet.Incidents' table. You can move, or remove it, as needed.
            this.incidentsTableAdapter.Fill(this.techSupport_DataDataSet.Incidents);
            try
            {
                //  This line of code loads data into the 'techSupport_DataDataSet.Products' table. You can move, or remove it, as needed.
                this.productsTableAdapter.Fill(this.techSupport_DataDataSet.Products);
                // the line below restricts the dropdown to ONLY registered products.  Client 1010 demonstrates we need to create incidents for products where a user cannot register it.
                // this.productsTableAdapter.FillByRegistrationsCustomerID(this.techSupport_DataDataSet.Products, Convert.ToInt32(txtCustomerID.Text));
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
            string msg = this.txtCustomerID.Text + ", " + this.txtCustomerName.Text + ", " +
                cboProductCode.ValueMember.ToString() + ", " + txtTitle.Text + ", " + txtDescription.Text;
            MessageBox.Show(msg);
        }

    }
}
