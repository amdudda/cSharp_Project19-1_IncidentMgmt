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
    public partial class frmEditCustomer : Form
    {
        public frmEditCustomer()
        {
            InitializeComponent();
        }

        private void customersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.customersBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.techSupport_DataDataSet);

        }

        private void frmEditCustomer_Load(object sender, EventArgs e)
        {
            try 
            {
            // This line of code loads data into the 'techSupport_DataDataSet.Customers' table. You can move, or remove it, as needed.
            this.customersTableAdapter.Fill(this.techSupport_DataDataSet.Customers);
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

        public void AddCustomer()
        {
            // loads form and moves to new record
            this.ShowDialog();
            customersBindingSource.AddNew();
            
            //customersBindingSource.MoveLast();
            //customersBindingSource.MoveNext();
            
            /*
            TechSupport_DataDataSet.CustomersRow newrow;
            newrow = techSupport_DataDataSet.Customers.NewCustomersRow();
            customersBindingSource.Add(newrow);
            customersTableAdapter.Update(techSupport_DataDataSet.Customers);
            customersBindingSource.MoveLast();
             * */
            
        }
    }
}
