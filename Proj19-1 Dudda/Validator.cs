using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proj19_1_Dudda
{
    public static class Validator
    {
        // this class enables field validation for the ProductMtce form.
        public static bool IsPresent(Control control, string name)
        {
            // checks that the control is not empty
            if (control.Text == "")
            {
                MessageBox.Show("The " + name + " field is required.  Please enter data.");
                control.Focus();
                return false;
            }
            else return true;
        }

        public static bool IsDecimal(Control control, string name)
        {
            // checks that control is a decimal value
            string myVal = control.Text;
            decimal ans;
            if (Decimal.TryParse(myVal, out ans))
            {
                return true;
            }
            else
            {
                MessageBox.Show("The " + name + " field is not a decimal number.  Please enter a decimal value such as '2' or '3.1415'.");
                control.Focus();
                return false;
            }
        }

        public static bool IsPositiveInteger(Control control, string name)
        {
            // checks that control is positive integer
            string myVal = control.Text;
            int ans;
            if (Int32.TryParse(myVal, out ans) && ans > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("The " + name + " field is not a postive integer.  Please enter a whole number such as '1234'.");
                control.Focus();
                return false;
            }
        }

        public static bool IsDate(Control control, string name)
        {
            // checks that the control contains a datelike value
            string myDate = control.Text;
            DateTime ans;
            if (DateTime.TryParse(myDate, out ans))
            {
                return true;
            }
            else
            {
                MessageBox.Show("The " + name + " field must be a date.  Please select a date.");
                control.Focus();
                return false;
            }
        }

        public static bool IsEmail(Control control, string name)
        {
            // email can be empty, but if it has text, it must have an @ sign in it somewhere after the first character
            if (control.Text != "" && control.Text.IndexOf("@") < 1)
            {
                // alert user & return false
                MessageBox.Show("The " + name + " field does not appear to be a valid email address.  Please enter a valid value.");
                control.Focus();
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
