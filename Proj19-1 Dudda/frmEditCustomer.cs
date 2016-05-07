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

        private void frmEditCustomer_Load(object sender, EventArgs e)
        {
            try
            {
                // fill the dataset and then move to a new record.
                this.customersTableAdapter.Fill(this.techSupport_DataDataSet.Customers);
                customersBindingSource.AddNew();
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

        private void customersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            // only try update if the required fields have data in them
            if (IsValidData())
            {
                // error trapping
                try
                {
                    this.Validate();
                    this.customersBindingSource.EndEdit();
                    this.tableAdapterManager.UpdateAll(this.techSupport_DataDataSet);
                }
                catch (DBConcurrencyException)
                {
                    // alert user to error
                    string msg = "A concurrency error occurred.  Some records were not saved.";
                    string caption = "Concurrency Error";
                    MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // then fetch fresh data
                    this.customersTableAdapter.Fill(this.techSupport_DataDataSet.Customers);
                }
                catch (DataException de)
                {
                    // alert user to error
                    string msg = "A data exception error occurred:\n\n" + de.Message;
                    string caption = de.GetType().ToString();
                    MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    // then cancel the edit(s) and let the user try again.
                    this.customersBindingSource.CancelEdit();
                }
                catch (Exception ex)
                {
                    string msg = "An unexpected error occurred:\n" + ex.Message;
                    string caption = ex.GetType().ToString();
                    MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void AddCustomer()
        {
            // loads form 
            this.ShowDialog();
        }

        private void btnSaveExit_Click(object sender, EventArgs e)
        {
            customersBindingNavigatorSaveItem_Click(sender, e);
            this.Close();
        }

        private void btnExitAbandonChanges_Click(object sender, EventArgs e)
        {
            DialogResult okToLeave = MessageBox.Show("This will undo all unsaved changes to records.\nOK to proceed?", "Changes will not be saved!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (okToLeave == DialogResult.Yes)
            {
                // if they say yes, abandom changes and exit.
                customersBindingSource.CancelEdit();
                this.Close();
            } // otherwise, do nothing, code sends user back to data entry form.
        }

        private bool IsValidData()
        {
            // checks that all required fields have been filled in
            bool isvalid = Validator.IsPresent(nameTextBox,nameTextBox.Tag.ToString()) &&
                Validator.IsPresent(addressTextBox,addressTextBox.Tag.ToString()) &&
                Validator.IsPresent(cityTextBox,cityTextBox.Tag.ToString()) &&
                Validator.IsPresent(stateTextBox,stateTextBox.Tag.ToString()) &&
                Validator.IsPresent(zipCodeTextBox,zipCodeTextBox.Tag.ToString()) &&
                Validator.IsEmail(emailTextBox,emailTextBox.Tag.ToString());
            return isvalid;
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            // validates required fields to ensure they contain data
            // TODO: turn this back on when I figure out how to stop it triggering when 
            // buttons that trigger canceledit events are clicked.
            /*
            TextBox myBox = (TextBox)sender;
            string myName = myBox.Tag.ToString();
            if (!(Validator.IsPresent(myBox, myName)))
            {
                myBox.Focus();
            }
             * */
        }

        private void emailTextBox_Leave(object sender, EventArgs e)
        {
            // validate the field if the user tries to tab away.
            Validator.IsEmail(emailTextBox, emailTextBox.Tag.ToString());
        }

        private void btnCancelEdit_Click(object sender, EventArgs e)
        {
            this.customersBindingSource.CancelEdit();
        }

        private void fillByNameToolStripButton_Click(object sender, EventArgs e)
        {
            // this method allows a user to find a customer by name
            DialogResult okToProceed;
            // TODO determine whether unsaved changes exist and only prompt if such exist.
            okToProceed = MessageBox.Show("This will delete any unsaved changes to your data!  Do you wish to proceed?", "Unsaved Changes will be Lost", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (okToProceed == DialogResult.Yes)
            {
                try
                {
                    // abandon changes to the current record
                    this.customersBindingSource.CancelEdit();
                    // then find the requested customer
                    this.customersTableAdapter.FillByName(this.techSupport_DataDataSet.Customers, nameToolStripTextBox.Text);
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
}
