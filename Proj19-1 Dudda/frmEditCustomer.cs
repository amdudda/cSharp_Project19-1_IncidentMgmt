﻿using System;
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

        public void AddCustomer()
        {
            // loads form 
            this.ShowDialog();
        }

        private void nameTextBox_Leave(object sender, EventArgs e)
        {
            TextBox myBox = (TextBox)sender;
            string myName = myBox.Tag.ToString();
            if (!(Validator.IsPresent(myBox, myName)))
            {
                myBox.Focus();
            }
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
                Validator.IsPresent(zipCodeTextBox,zipCodeTextBox.Tag.ToString());
            return isvalid;
        }
    }
}
