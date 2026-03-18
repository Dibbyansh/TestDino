using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestDino.Locators
{
    public class SignUpLocators
    {
        public static By Title_CreateAccount => By.CssSelector("h2[ data-testid = 'signup-title']");

        public static By Input_firstname => By.Name("firstname");
        public static By Input_lastname => By.Name("lastname");
        public static By Input_email => By.Name("email");
        public static By Input_password => By.Name("password");
      
        public static By Btn_CreateAccount => By.CssSelector("button[data-testid = 'signup-submit-button']");
        
        public static By AccountCreated_ToastMessage => By.CssSelector("div#_rht_toaster div[role='status']");
    }

    public class SignInLocators
    {
        public static By Title_SignIn => By.CssSelector("h2[data-testid='login-title']");
       
        public static By Input_email => By.Name("email");
        public static By Input_password => By.Name("password");
        
        public static By Btn_SignIn => By.CssSelector("button[data-testid='login-submit-button']");
        
        public static By SignIn_ToastMessage => By.CssSelector("div#_rht_toaster div[role='status']");
    }
}
